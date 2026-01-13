using ERPNext_PowerPlay.Models;
using Serilog;
using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ERPNext_PowerPlay.Helpers
{
    public class SocketIOHelper
    {
        private SocketIOClient.SocketIO _client;
        private AppDbContext _db;
        private bool _isConnected = false;
        private string _siteName;
        private string _currentUser;

        // Deduplication lock to prevent duplicate prints from timer + Socket.IO
        private static readonly HashSet<string> _processingDocuments = new HashSet<string>();
        private static readonly object _lockObject = new object();

        public bool IsConnected => _isConnected;

        public SocketIOHelper(AppDbContext dbContext)
        {
            _db = dbContext;
        }

        /// <summary>
        /// Connect to Frappe's socket.io server and subscribe to Sales Invoice submission events
        /// Frappe uses namespaced connections: /{sitename}
        /// </summary>
        public async Task<bool> ConnectToSocketIO()
        {
            try
            {
                if (_client != null && _isConnected)
                {
                    Log.Information("Socket.IO already connected");
                    return true;
                }

                // Parse the Frappe URL to get the socket.io endpoint
                var uri = new Uri(Program.FrappeURL);

                // Extract site name from URL (e.g., "erp.example.com" from "https://erp.example.com")
                _siteName = uri.Host;

                // Try multiple ports:
                // 1. Port 9000 (Frappe default socketio_port)
                // 2. Same port as web server (nginx proxy setup)
                // 3. Port 443/80 if using standard ports
                var socketUrls = new List<string>();

                // Add port 9000 first (default Frappe Socket.IO port)
                socketUrls.Add($"{uri.Scheme}://{uri.Host}:9000");

                // Add the original URL port (for nginx proxy setups)
                if (uri.Port != 9000 && uri.Port != -1)
                {
                    socketUrls.Add($"{uri.Scheme}://{uri.Host}:{uri.Port}");
                }
                else if (uri.Port == -1)
                {
                    // Default ports - try without explicit port
                    socketUrls.Add($"{uri.Scheme}://{uri.Host}");
                }

                Log.Information("Will try Socket.IO on these URLs: {0}", string.Join(", ", socketUrls));
                Log.Information("Site namespace: /{0}", _siteName);

                string socketUrl = socketUrls[0]; // Start with first option

                // Get session info for authentication
                var (sessionCookie, userName) = await GetSessionInfo();
                _currentUser = userName;

                if (string.IsNullOrEmpty(sessionCookie))
                {
                    Log.Warning("Could not obtain session cookie for Socket.IO auth - trying with API token only");
                }
                else
                {
                    Log.Debug("Obtained session cookie: {0}... for user: {1}",
                        sessionCookie.Length > 10 ? sessionCookie.Substring(0, 10) : sessionCookie,
                        userName);
                }

                // Build authentication headers
                var headers = GetAuthenticationHeaders(sessionCookie);

                Log.Information("Auth headers configured: {0}", string.Join(", ", headers.Keys));

                // Try connecting to each URL until one works
                bool connected = false;
                Exception lastException = null;

                foreach (var tryUrl in socketUrls)
                {
                    // Try with namespace first, then without
                    var urlsToTry = new[] { $"{tryUrl}/{_siteName}", tryUrl };

                    foreach (var currentUrl in urlsToTry)
                    {
                        try
                        {
                            Log.Information("Trying Socket.IO connection to: {0}", currentUrl);

                            _client?.Dispose();
                            _client = new SocketIOClient.SocketIO(currentUrl, new SocketIOOptions
                            {
                                // Start with Polling - it's more reliable for initial connection
                                Transport = SocketIOClient.Transport.TransportProtocol.Polling,
                                Path = "/socket.io",
                                Reconnection = true,
                                ReconnectionAttempts = 3,
                                ReconnectionDelay = 2000,
                                ConnectionTimeout = TimeSpan.FromSeconds(10),
                                ExtraHeaders = headers
                            });

                            SetupEventHandlers();

                            // Use a cancellation token to timeout faster
                            using var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(10));
                            await _client.ConnectAsync(cts.Token);

                            // Wait a bit to see if we get connected
                            await Task.Delay(1000);

                            if (_isConnected)
                            {
                                Log.Information("Socket.IO CONNECTED successfully to: {0}", currentUrl);
                                connected = true;
                                break;
                            }
                            else
                            {
                                Log.Warning("ConnectAsync completed but not connected. Trying next URL...");
                            }
                        }
                        catch (OperationCanceledException)
                        {
                            Log.Warning("Socket.IO connection timed out for: {0}", currentUrl);
                        }
                        catch (Exception ex)
                        {
                            Log.Warning("Socket.IO connection failed for {0}: {1}", currentUrl, ex.Message);
                            lastException = ex;
                        }
                    }

                    if (connected) break;
                }

                if (!connected)
                {
                    Log.Error("Failed to connect to Socket.IO on any URL");
                    if (lastException != null)
                    {
                        Log.Error("Last exception: {0}", lastException.Message);
                    }
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error connecting to Socket.IO: {0}", ex.Message);
                _isConnected = false;
                return false;
            }
        }

        /// <summary>
        /// Get session info from Frappe by making a REST API call
        /// Returns both the session cookie (sid) and the logged-in user name
        /// </summary>
        private async Task<(string sessionCookie, string userName)> GetSessionInfo()
        {
            try
            {
                var handler = new HttpClientHandler
                {
                    CookieContainer = new CookieContainer(),
                    UseCookies = true
                };

                using var client = new HttpClient(handler);

                var request = new HttpRequestMessage(HttpMethod.Get,
                    $"{Program.FrappeURL}/api/method/frappe.auth.get_logged_user");
                request.Headers.Add("Authorization", $"token {Program.ApiToken}");

                Log.Debug("Fetching session info from: {0}/api/method/frappe.auth.get_logged_user", Program.FrappeURL);

                var response = await client.SendAsync(request);
                string userName = null;

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response to get the user name
                    var content = await response.Content.ReadAsStringAsync();
                    Log.Debug("get_logged_user response: {0}", content);

                    try
                    {
                        var json = JsonDocument.Parse(content);
                        if (json.RootElement.TryGetProperty("message", out var messageElement))
                        {
                            userName = messageElement.GetString();
                        }
                    }
                    catch (Exception parseEx)
                    {
                        Log.Warning("Could not parse user response: {0}", parseEx.Message);
                    }

                    var cookies = handler.CookieContainer.GetCookies(new Uri(Program.FrappeURL));
                    var sidCookie = cookies["sid"];
                    if (sidCookie != null)
                    {
                        Log.Debug("Found SID cookie in response");
                        return (sidCookie.Value, userName);
                    }
                    else
                    {
                        Log.Warning("No SID cookie found in response. Available cookies: {0}",
                            string.Join(", ", cookies.Cast<Cookie>().Select(c => c.Name)));
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Log.Warning("get_logged_user failed with status {0}: {1}",
                        response.StatusCode, errorContent);
                }

                return (null, userName);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Could not obtain session info: {0}", ex.Message);
                return (null, null);
            }
        }

        /// <summary>
        /// Get authentication headers for Socket.IO connection
        /// Frappe validates connections using either Cookie header or Authorization header
        /// </summary>
        private Dictionary<string, string> GetAuthenticationHeaders(string sessionCookie)
        {
            var headers = new Dictionary<string, string>();

            try
            {
                // Method 1: Cookie-based auth (preferred for Socket.IO)
                if (!string.IsNullOrEmpty(sessionCookie))
                {
                    headers.Add("Cookie", $"sid={sessionCookie}");
                    Log.Debug("Added SID cookie to Socket.IO headers");
                }

                // Method 2: API Token as backup/additional auth
                if (!string.IsNullOrEmpty(Program.ApiToken))
                {
                    headers.Add("Authorization", $"token {Program.ApiToken}");
                    Log.Debug("Added API Token to Socket.IO headers");
                }
                else
                {
                    Log.Warning("No API Token found for Socket.IO authentication");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating authentication headers for Socket.IO: {0}", ex.Message);
            }

            return headers;
        }

        /// <summary>
        /// Setup all socket.io event handlers
        /// </summary>
        private void SetupEventHandlers()
        {
            // Connection established
            _client.OnConnected += async (sender, e) =>
            {
                _isConnected = true;
                Log.Information("Socket.IO CONNECTED successfully to server");
                Log.Information("Socket ID: {0}", _client.Id);
                await SubscribeToEvents();
            };

            // Connection error
            _client.OnError += (sender, e) =>
            {
                _isConnected = false;
                Log.Error("Socket.IO ERROR: {0}", e);
            };

            // Disconnected
            _client.OnDisconnected += (sender, e) =>
            {
                _isConnected = false;
                Log.Warning("Socket.IO DISCONNECTED: {0}", e);
            };

            // Reconnection attempt
            _client.OnReconnectAttempt += (sender, attempt) =>
            {
                Log.Information("Socket.IO reconnection attempt #{0}", attempt);
            };

            // Reconnected
            _client.OnReconnected += async (sender, e) =>
            {
                _isConnected = true;
                Log.Information("Socket.IO RECONNECTED successfully");
                await SubscribeToEvents();
            };

            // Additional debugging: Handle ping/pong
            _client.OnPing += (sender, e) =>
            {
                Log.Debug("Socket.IO PING received");
            };

            _client.OnPong += (sender, e) =>
            {
                Log.Debug("Socket.IO PONG received (latency: {0}ms)", e.TotalMilliseconds);
            };
        }

        /// <summary>
        /// Subscribe to Frappe events
        /// Frappe uses room-based subscriptions for different event types
        /// </summary>
        private async Task SubscribeToEvents()
        {
            try
            {
                // DEBUG: Log ALL events received from the server
                _client.OnAny((eventName, response) =>
                {
                    Log.Information("Socket.IO EVENT [{0}]: {1}", eventName, response.ToString());
                });

                // Listen for our custom PowerPlay print event
                // This event is published by Server Script/Client Script using frappe.publish_realtime()
                _client.On("powerplay_print_invoice", response =>
                {
                    Log.Information("Received powerplay_print_invoice event!");
                    HandlePrintInvoiceEvent(response);
                });

                // Also listen for the generic "publish" event that Frappe uses
                // frappe.publish_realtime wraps events in a "publish" event
                _client.On("publish", response =>
                {
                    Log.Information("Socket.IO PUBLISH event received: {0}", response.ToString());
                    try
                    {
                        var data = response.GetValue<JsonElement>();
                        // Check if this is our powerplay event
                        if (data.TryGetProperty("event", out var eventElement))
                        {
                            var eventName = eventElement.GetString();
                            if (eventName == "powerplay_print_invoice")
                            {
                                Log.Information("Found powerplay_print_invoice inside publish event");
                                if (data.TryGetProperty("message", out var messageElement))
                                {
                                    HandlePrintInvoiceEventFromJson(messageElement);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Debug("Could not parse publish event: {0}", ex.Message);
                    }
                });

                // Frappe room subscription methods:
                // 1. "doctype_subscribe" - subscribe to doctype changes
                // 2. "doc_subscribe" - subscribe to specific document changes
                // 3. "task_subscribe" - subscribe to background task updates

                // Try multiple subscription approaches for compatibility

                // Approach 1: Subscribe to doctype changes for Sales Invoice
                try
                {
                    await _client.EmitAsync("doctype_subscribe", "Sales Invoice");
                    Log.Information("Subscribed to Sales Invoice doctype events");
                }
                catch (Exception ex)
                {
                    Log.Warning("doctype_subscribe failed: {0}", ex.Message);
                }

                // Approach 2: Subscribe to custom task/event channel
                try
                {
                    await _client.EmitAsync("task_subscribe", "powerplay_print_invoice");
                    Log.Information("Subscribed to powerplay_print_invoice task events");
                }
                catch (Exception ex)
                {
                    Log.Warning("task_subscribe failed: {0}", ex.Message);
                }

                // Approach 3: Subscribe to user-specific events if we have a user
                if (!string.IsNullOrEmpty(_currentUser))
                {
                    try
                    {
                        // Frappe sends user-specific events to user:{username} room
                        await _client.EmitAsync("frappe.subscribe", $"user:{_currentUser}");
                        Log.Information("Subscribed to user events for: {0}", _currentUser);
                    }
                    catch (Exception ex)
                    {
                        Log.Warning("User subscription failed: {0}", ex.Message);
                    }
                }

                Log.Information("Event subscription setup complete");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error subscribing to events: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Handle print invoice event from JSON data (from publish wrapper)
        /// </summary>
        private void HandlePrintInvoiceEventFromJson(JsonElement data)
        {
            try
            {
                Log.Information("Processing powerplay_print_invoice from JSON: {0}", data.ToString());

                if (data.TryGetProperty("name", out var nameElement))
                {
                    string docName = nameElement.GetString();

                    // Check custom_print_count
                    if (data.TryGetProperty("custom_print_count", out var printCountElement))
                    {
                        int printCount = printCountElement.GetInt32();
                        if (printCount > 0)
                        {
                            Log.Information("Document {0} already printed (count={1}), skipping", docName, printCount);
                            return;
                        }
                    }

                    TriggerPrintJob(data);
                }
                else
                {
                    Log.Warning("Print invoice event missing 'name' field");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error handling print invoice event from JSON: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Handle the custom print invoice event from ERPNext
        /// </summary>
        private void HandlePrintInvoiceEvent(SocketIOResponse response)
        {
            try
            {
                var data = response.GetValue<JsonElement>();
                Log.Information("Received powerplay_print_invoice event: {0}", data.ToString());

                // Extract document name
                if (data.TryGetProperty("name", out var nameElement))
                {
                    string docName = nameElement.GetString();

                    // Check custom_print_count to avoid reprinting already printed documents
                    if (data.TryGetProperty("custom_print_count", out var printCountElement))
                    {
                        int printCount = printCountElement.GetInt32();
                        if (printCount > 0)
                        {
                            Log.Information("Document {0} already printed (count={1}), skipping", docName, printCount);
                            return;
                        }
                    }

                    TriggerPrintJob(data);
                }
                else
                {
                    Log.Warning("Print invoice event missing 'name' field: {0}", data.ToString());
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error handling print invoice event: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Try to acquire a lock for processing a document (prevents duplicate prints)
        /// </summary>
        private bool TryAcquirePrintLock(string docName)
        {
            lock (_lockObject)
            {
                if (_processingDocuments.Contains(docName))
                    return false;
                _processingDocuments.Add(docName);
                return true;
            }
        }

        /// <summary>
        /// Release the processing lock for a document
        /// </summary>
        private void ReleasePrintLock(string docName)
        {
            lock (_lockObject)
            {
                _processingDocuments.Remove(docName);
            }
        }

        /// <summary>
        /// Trigger a print job for the document received via socket.io
        /// Uses deduplication lock to prevent duplicate prints from timer + Socket.IO
        /// </summary>
        private void TriggerPrintJob(JsonElement documentData)
        {
            try
            {
                // Extract document name
                if (!documentData.TryGetProperty("name", out var nameElement))
                {
                    Log.Warning("Document data missing 'name' field");
                    return;
                }

                string docName = nameElement.GetString();

                // Prevent duplicate processing
                if (!TryAcquirePrintLock(docName))
                {
                    Log.Information("Document {0} already being processed by another thread, skipping", docName);
                    return;
                }

                Log.Information("Triggering print job for Sales Invoice: {0} via Socket.IO", docName);

                // Run print job in a separate STA thread (required for COM/printing operations)
                System.Threading.Thread thread = new System.Threading.Thread((System.Threading.ThreadStart)(() =>
                {
                    try
                    {
                        RunPrintJobForDocument(docName);
                    }
                    finally
                    {
                        ReleasePrintLock(docName);
                    }
                }));

                thread.SetApartmentState(System.Threading.ApartmentState.STA);
                thread.Start();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error triggering print job: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Run print job for a specific Sales Invoice document
        /// </summary>
        [STAThread]
        private async void RunPrintJobForDocument(string documentName)
        {
            try
            {
                using var context = new AppDbContext();
                var printActions = new PrintActions();

                // Get all enabled printer settings for Sales Invoice
                var printerSettings = context.PrinterSetting
                    .Where(x => x.Enabled && x.DocType ==  DocType.SalesInvoice)
                    .ToList();

                foreach (var ps in printerSettings)
                {
                    try
                    {
                        // Fetch the specific document
                        var docList = await new FrappeAPI().GetDocs2Print(ps);

                        if (docList == null || docList.data == null)
                            continue;

                        // Find the specific document by name
                        var document = docList.data.FirstOrDefault(d => d.Name == documentName);

                        if (document != null)
                        {
                            Log.Information("Processing Sales Invoice {0} via Socket.IO trigger", documentName);

                            // Print the document
                            bool processed = await printActions.PrintDoc(document, ps);

                            if (processed)
                            {
                                // Update print count
                                await new FrappeAPI().UpdateCount("/api/resource/Sales Invoice", document);

                                // Save to job history
                                await SaveJobHistory(document);

                                Log.Information("Successfully printed Sales Invoice {0} via Socket.IO", documentName);
                            }
                            else
                            {
                                Log.Warning("Failed to print Sales Invoice {0} via Socket.IO", documentName);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Error processing print job for printer setting {0}: {1}", ps.ID, ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in RunPrintJobForDocument: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Save job to history database
        /// </summary>
        private async Task<bool> SaveJobHistory(Frappe_DocList.data doc)
        {
            using var context = new AppDbContext();
            await using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                doc.JobDate = DateTime.Now;
                context.JobHistory.Add(doc);
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error saving job history: {0}", ex.Message);
                await transaction.RollbackAsync();
                return false;
            }
        }

        /// <summary>
        /// Disconnect from socket.io server
        /// </summary>
        public async Task Disconnect()
        {
            try
            {
                if (_client != null && _isConnected)
                {
                    await _client.DisconnectAsync();
                    Log.Information("Socket.IO disconnected");
                }
                _isConnected = false;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error disconnecting from Socket.IO: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
