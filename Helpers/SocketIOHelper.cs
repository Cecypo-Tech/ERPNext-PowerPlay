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
        /// Uses cookie-based authentication via session ID
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

                // Frappe Socket.IO runs on port 9000 by default (configurable via socketio_port in site_config.json)
                int socketioPort = 9000;
                var socketUrl = $"{uri.Scheme}://{uri.Host}:{socketioPort}";

                Log.Information("Connecting to Socket.IO at: {0}", socketUrl);

                // Get session cookie for authentication
                string sessionCookie = await GetSessionCookie();

                if (string.IsNullOrEmpty(sessionCookie))
                {
                    Log.Warning("Could not obtain session cookie for Socket.IO auth - connection may fail");
                }
                else
                {
                    Log.Debug("Obtained session cookie for Socket.IO authentication");
                }

                // Create socket.io client with cookie-based authentication
                _client = new SocketIOClient.SocketIO(socketUrl, new SocketIOOptions
                {
                    Transport = SocketIOClient.Transport.TransportProtocol.WebSocket,
                    Path = "/socket.io",
                    Reconnection = true,
                    ReconnectionAttempts = int.MaxValue,
                    ReconnectionDelay = 5000,
                    ConnectionTimeout = TimeSpan.FromSeconds(30),
                    Query = new Dictionary<string, string>
                    {
                        { "sid", sessionCookie ?? "" }
                    },
                    ExtraHeaders = GetAuthenticationHeaders()
                });

                // Setup event handlers
                SetupEventHandlers();

                // Connect to the server
                await _client.ConnectAsync();

                Log.Information("Socket.IO connection initiated");
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
        /// Get session cookie from Frappe by making a REST API call
        /// </summary>
        private async Task<string> GetSessionCookie()
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

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var cookies = handler.CookieContainer.GetCookies(new Uri(Program.FrappeURL));
                    var sidCookie = cookies["sid"];
                    if (sidCookie != null)
                    {
                        return sidCookie.Value;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Could not obtain session cookie: {0}", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Get authentication headers using API Token instead of cookies
        /// </summary>
        private Dictionary<string, string> GetAuthenticationHeaders()
        {
            var headers = new Dictionary<string, string>();

            try
            {
                if (!string.IsNullOrEmpty(Program.ApiToken))
                {
                    // Use Authorization header with API token
                    headers.Add("Authorization", $"token {Program.ApiToken}");
                    Log.Debug("Added API Token to Socket.IO connection");
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
                Log.Information("Socket.IO connected successfully");
                await SubscribeToEvents();
            };

            // Connection error
            _client.OnError += (sender, e) =>
            {
                _isConnected = false;
                Log.Error("Socket.IO error: {0}", e);
            };

            // Disconnected
            _client.OnDisconnected += (sender, e) =>
            {
                _isConnected = false;
                Log.Warning("Socket.IO disconnected: {0}", e);
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
                Log.Information("Socket.IO reconnected successfully");
                await SubscribeToEvents();
            };
        }

        /// <summary>
        /// Subscribe to Frappe events
        /// </summary>
        private async Task SubscribeToEvents()
        {
            try
            {
                // Debug: Log all events received (useful for troubleshooting)
                _client.OnAny((eventName, response) =>
                {
                    Log.Debug("Socket.IO received event: {0}, data: {1}", eventName, response.ToString());
                });

                // Listen for our custom PowerPlay print event
                // This event is published by the Server Script in ERPNext when Sales Invoice is submitted
                _client.On("powerplay_print_invoice", response =>
                {
                    HandlePrintInvoiceEvent(response);
                });

                // Subscribe to the event channel
                // Frappe uses "task_subscribe" for custom realtime events
                await _client.EmitAsync("task_subscribe", "powerplay_print_invoice");

                Log.Information("Subscribed to powerplay_print_invoice events");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error subscribing to events: {0}", ex.Message);
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
