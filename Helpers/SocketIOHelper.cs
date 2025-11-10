using ERPNext_PowerPlay.Models;
using Serilog;
using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ERPNext_PowerPlay.Helpers
{
    public class SocketIOHelper
    {
        private SocketIOClient.SocketIO _client;
        private AppDbContext _db;
        private bool _isConnected = false;

        public bool IsConnected => _isConnected;

        public SocketIOHelper(AppDbContext dbContext)
        {
            _db = dbContext;
        }

        /// <summary>
        /// Connect to Frappe's socket.io server and subscribe to Sales Invoice submission events
        /// Uses API Token for authentication instead of cookies
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
                var socketUrl = $"{uri.Scheme}://{uri.Host}:{uri.Port}";

                Log.Information("Connecting to Socket.IO at: {0}", socketUrl);

                // Create socket.io client with API Token authentication
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
                        // Add any required query parameters for Frappe authentication
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
                // Subscribe to Sales Invoice submission event
                // Frappe typically emits events in the format: "doc_update" or specific doctype events
                // Event names may vary based on Frappe version - common patterns:
                // - "doc_update" (general document update)
                // - "Sales Invoice:on_submit" (specific to Sales Invoice submission)
                // - "eval_js" (for realtime events)

                // Listen for generic document updates
                _client.On("doc_update", response =>
                {
                    HandleDocumentUpdate(response);
                });

                // Listen for Sales Invoice specific events
                _client.On("Sales Invoice:on_submit", response =>
                {
                    HandleSalesInvoiceSubmit(response);
                });

                // Listen for general realtime events
                _client.On("eval_js", response =>
                {
                    HandleRealtimeEvent(response);
                });

                // Emit subscription to specific document type
                await _client.EmitAsync("doctype_subscribe", "Sales Invoice");

                Log.Information("Subscribed to Sales Invoice submission events");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error subscribing to events: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Handle generic document update events
        /// </summary>
        private void HandleDocumentUpdate(SocketIOResponse response)
        {
            try
            {
                var data = response.GetValue<JsonElement>();
                Log.Debug("Document update received: {0}", data.ToString());

                // Check if it's a Sales Invoice
                if (data.TryGetProperty("doctype", out var doctype))
                {
                    if (doctype.GetString() == "Sales Invoice")
                    {
                        if (data.TryGetProperty("docstatus", out var docstatus) && docstatus.GetInt32() == 1)
                        {
                            // Document is submitted (docstatus = 1)
                            TriggerPrintJob(data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error handling document update: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Handle Sales Invoice submission events
        /// </summary>
        private void HandleSalesInvoiceSubmit(SocketIOResponse response)
        {
            try
            {
                var data = response.GetValue<JsonElement>();
                Log.Information("Sales Invoice submitted via Socket.IO: {0}", data.ToString());
                TriggerPrintJob(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error handling Sales Invoice submit: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Handle realtime events
        /// </summary>
        private void HandleRealtimeEvent(SocketIOResponse response)
        {
            try
            {
                var data = response.GetValue<JsonElement>();
                Log.Debug("Realtime event received: {0}", data.ToString());

                // Parse the event data to check if it's a Sales Invoice submission
                // Frappe realtime events may contain JavaScript code to execute
                // We need to parse and check for Sales Invoice related events
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error handling realtime event: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Trigger a print job for the document received via socket.io
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
                Log.Information("Triggering print job for Sales Invoice: {0}", docName);

                // Run print job in a separate STA thread (required for COM/printing operations)
                System.Threading.Thread thread = new System.Threading.Thread((System.Threading.ThreadStart)(() =>
                {
                    RunPrintJobForDocument(docName);
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
