using DevExpress.DataAccess.DataFederation;
using DevExpress.DataAccess.Native.Json;
using DevExpress.XtraReports.UI.CrossTab;
using ERPNext_PowerPlay.Models;
using Serilog;
using SQLitePCL;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ERPNext_PowerPlay.Helpers
{
    public class FrappeAPI
    {
        /// <summary>
        /// Cleans and normalizes a URL to handle common issues like duplicate slashes,
        /// missing protocol, trailing slashes, etc.
        /// </summary>
        private static string CleanUrl(string baseUrl, string endpoint, string filter = "")
        {
            // Ensure base URL has protocol
            if (!baseUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !baseUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                baseUrl = "https://" + baseUrl;
            }

            // Remove trailing slashes from base URL
            baseUrl = baseUrl.TrimEnd('/');

            // Remove leading slashes from endpoint
            endpoint = endpoint.TrimStart('/');

            // Remove trailing slashes from endpoint (unless it's intentional for API)
            if (!endpoint.EndsWith("/") || endpoint.Count(c => c == '/') > 1)
                endpoint = endpoint.TrimEnd('/');

            // Build the URL
            string url = $"{baseUrl}/{endpoint}";

            // Add filter if present
            if (!string.IsNullOrEmpty(filter))
            {
                // Ensure filter starts correctly (with ? or without if endpoint has it)
                filter = filter.TrimStart('/');
                if (!filter.StartsWith("?") && !filter.StartsWith("&") && !url.Contains("?"))
                {
                    // Filter might be a path segment or query string
                    if (filter.Contains("="))
                        url = url + "?" + filter.TrimStart('?').TrimStart('&');
                    else
                        url = url + "/" + filter;
                }
                else
                {
                    url = url + filter;
                }
            }

            // Fix any double slashes (except in protocol)
            url = Regex.Replace(url, @"(?<!:)/{2,}", "/");

            // Fix double question marks or ampersands
            url = Regex.Replace(url, @"\?{2,}", "?");
            url = Regex.Replace(url, @"&{2,}", "&");
            url = Regex.Replace(url, @"\?&", "?");
            url = Regex.Replace(url, @"&\?", "&");

            return url;
        }

        public async Task<string> GetAsString(string api_endpoint, string api_filter)
        {   //With API Token
            try
            {
                // Strip leading "/" from endpoints starting with "/api"
                if (api_endpoint.StartsWith("/api", StringComparison.OrdinalIgnoreCase))
                {
                    api_endpoint = api_endpoint.TrimStart('/');
                }

                string cleanedUrl = CleanUrl(Program.FrappeURL, api_endpoint, api_filter);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, cleanedUrl);
                request.Headers.Add("Authorization", $"token {Program.ApiToken}");

                using (var client = new HttpClient() { BaseAddress = new Uri(Program.FrappeURL) })
                {
                    HttpResponseMessage response_qr = await client.SendAsync(request);
                    response_qr.EnsureSuccessStatusCode();

                    string result = await response_qr.Content.ReadAsStringAsync();
                    return result;
                }

            }
            catch (Exception exSQL)
            {
                Log.Error(exSQL, exSQL.Message + Environment.NewLine + "Failing Endpoint: " + api_endpoint);
                return "";
            }
        }

        /// <summary>
        /// Calls query_report.run endpoint with form data
        /// </summary>
        public async Task<string> GetQueryReport(string api_endpoint, string fieldListJson)
        {
            try
            {
                // Strip leading "/" from endpoints starting with "/api"
                if (api_endpoint.StartsWith("/api", StringComparison.OrdinalIgnoreCase))
                {
                    api_endpoint = api_endpoint.TrimStart('/');
                }

                string cleanedUrl = CleanUrl(Program.FrappeURL, api_endpoint, "");

                // Build query parameters from all properties in the JSON
                JsonElement fieldListDoc = JsonSerializer.Deserialize<JsonElement>(fieldListJson);
                var queryParams = new List<string>();
                foreach (JsonProperty prop in fieldListDoc.EnumerateObject())
                {
                    string value = prop.Value.ValueKind == JsonValueKind.String
                        ? prop.Value.GetString()
                        : prop.Value.GetRawText();
                    queryParams.Add($"{Uri.EscapeDataString(prop.Name)}={Uri.EscapeDataString(value)}");
                }
                cleanedUrl += "?" + string.Join("&", queryParams);

                Log.Information("Query Report URL: {0}", cleanedUrl);

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, cleanedUrl);
                request.Headers.Add("Authorization", $"token {Program.ApiToken}");

                using (var client = new HttpClient() { BaseAddress = new Uri(Program.FrappeURL) })
                {
                    HttpResponseMessage response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();

                    string result = await response.Content.ReadAsStringAsync();
                    return result;
                }
            }
            catch (Exception exSQL)
            {
                Log.Error(exSQL, exSQL.Message + Environment.NewLine + "Failing Endpoint: " + api_endpoint);
                return "";
            }
        }

        public async Task<HttpResponseMessage> GetAsReponse(string api_endpoint, string api_filter)
        {   //With API Token
            try
            {
                string cleanedUrl = CleanUrl(Program.FrappeURL, api_endpoint, api_filter);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, cleanedUrl);
                request.Headers.Add("Authorization", $"token {Program.ApiToken}");

                using (var client = new HttpClient() { BaseAddress = new Uri(Program.FrappeURL) })
                {
                    HttpResponseMessage response_qr = await client.SendAsync(request);
                    response_qr.EnsureSuccessStatusCode();

                    return response_qr;
                }

            }
            catch (Exception exSQL)
            {
                Log.Error(exSQL, exSQL.Message + Environment.NewLine + "Failing Endpoint: " + api_endpoint);
                return null;
            }
        }

        public async Task<Frappe_DocList.FrappeDocList> GetDocs2Print(PrinterSetting ps)
        {   //With API Token
            Frappe_DocList.FrappeDocList docList = new Frappe_DocList.FrappeDocList();
            docList.data = new List<Frappe_DocList.data>();
            string FilterStr = "";
            try
            {
                //for this ps.doctype, get all docs, with fileds and filters

                //  SAMPLE string.Format("/api/resource/Sales Invoice?fields=[\"name\", \"customer\", \"posting_date\", \"docstatus\", \"status\", \"etr_invoice_number\"," +
                //                                    "\"etr_invoice_number\", \"total_taxes_and_charges\", \"total\"]" +
                //                                    "&filters=[" +
                //                                    "[\"Sales Invoice\",\"docstatus\",\"=\",\"1\"]" +
                //                                    ",[\"Sales Invoice\",\"etr_invoice_number\",\"!=\",\"\"]" + //Not empty
                //                                    ",[\"Sales Invoice\",\"posting_date\",\">\",\"{0}\"]" +     //After Date
                //                                    ",[\"Sales Invoice\",\"custom_print_count\",\"=\",\"0\"]" + //Print Count = 0
                //                                    "]&limit_page_length={1}", new DateOnly(2025, 01, 01).ToString("yyyy-MM-dd"), 10);

                //Cleanup fields and filters
                string fields = Regex.Replace(ps.FieldList, @"\r\n?|\n", "");
                string filters = Regex.Replace(ps.FilterList, @"\r\n?|\n", "");
                string doctype = ps.DocType.GetAttributeOfType<DescriptionAttribute>().Description;
                List<string> UserList = new List<string>();
                string userFilter = "";

                if (!string.IsNullOrEmpty(ps.UserFilter))
                {
                    UserList = ps.UserFilter.Split(',').ToList();
                    foreach (string u in UserList)
                        userFilter += string.Format("\"{0}\",", u.Trim());
                    if (userFilter.EndsWith(",")) userFilter = userFilter.Substring(0, userFilter.Length - 1);
                    userFilter = string.Format(",[\"{0}\",\"owner\",\"IN\",[{1}]]", doctype, userFilter);
                    filters += userFilter;
                }

                FilterStr = string.Format("api/resource/{0}?fields={1}&filters=[{2}]&limit_page_length={3}", doctype, fields, filters, 2);
                //Get the docs
                string cleanedUrl = CleanUrl(Program.FrappeURL, FilterStr);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, cleanedUrl);
                request.Headers.Add("Authorization", $"token {Program.ApiToken}");

                using (var client = new HttpClient() { BaseAddress = new Uri(Program.FrappeURL) })
                {
                    HttpResponseMessage response_qr = await client.SendAsync(request);
                    if (!response_qr.IsSuccessStatusCode )
                    {
                        Log.Error("Failing Endpoint: " + string.Format("{0}/{1}", Program.FrappeURL, FilterStr));
                        return null;
                    }

                    //  //If preset fields;
                    //docList = await response_qr.Content.ReadFromJsonAsync<Frappe_DocList.FrappeDocList>();
                    //return docList;

                    //Dynamic list, because of customer_name, supplier_name, title.
                    string json = await response_qr.Content.ReadAsStringAsync();
                    Debug.WriteLine(json);

                    JsonElement doc = JsonSerializer.Deserialize<JsonElement>(json);
                    JsonElement docdata = JsonHelper.GetJsonElement(doc, "data");

                    if (docdata.ValueKind == JsonValueKind.Array)
                    {
                        //JsonElement edgesFirstElem =docdata.EnumerateArray().ElementAtOrDefault(index);
                        string nameCursor = "";
                        foreach (JsonElement jE in docdata.EnumerateArray())
                        {
                            try
                            {
                                //Name
                                JsonElement jsonElementName = JsonHelper.GetJsonElement(jE, "name");
                                var docname = JsonHelper.GetJsonElementValue(jsonElementName);
                                nameCursor = docname;

                                //Potential Total fields
                                JsonElement jsonElementTot = JsonHelper.GetJsonElement(jE, "grand_total");
                                if (jsonElementTot.ValueKind == JsonValueKind.Undefined) jsonElementTot = JsonHelper.GetJsonElement(jE, "total");
                                JsonElement jsonElementPrintCount = JsonHelper.GetJsonElement(jE, "custom_print_count");
                                JsonElement jsonElementStatus = JsonHelper.GetJsonElement(jE, "status");

                                //Potential Date Fields
                                JsonElement jsonElementDate = JsonHelper.GetJsonElement(jE, "date");
                                if (jsonElementDate.ValueKind == JsonValueKind.Undefined) jsonElementDate = JsonHelper.GetJsonElement(jE, "posting_date");
                                if (jsonElementDate.ValueKind == JsonValueKind.Undefined) jsonElementDate = JsonHelper.GetJsonElement(jE, "transaction_date");

                                //Potenntial Titles Fields (title as last option, used in picking list/stock transfers)
                                JsonElement jsonElementTitle = JsonHelper.GetJsonElement(jE, "customer");
                                if (jsonElementTitle.ValueKind == JsonValueKind.Undefined) jsonElementTitle = JsonHelper.GetJsonElement(jE, "supplier");
                                if (jsonElementTitle.ValueKind == JsonValueKind.Undefined) jsonElementTitle = JsonHelper.GetJsonElement(jE, "title");

                                //owner
                                JsonElement jsonElementOwner = JsonHelper.GetJsonElement(jE, "owner");

                                //Warehouse (set_warehouse field)
                                JsonElement jsonElementWarehouse = JsonHelper.GetJsonElement(jE, "set_warehouse");

                                //Set fields for object
                                var title = JsonHelper.GetJsonElementValue(jsonElementTitle);
                                var owner = JsonHelper.GetJsonElementValue(jsonElementOwner);
                                if (owner == null) owner = "";
                                var originalSrcString = JsonHelper.GetJsonElementValue(jsonElementDate);
                                var grandTot = JsonHelper.GetJsonElementValue(jsonElementTot);
                                var printCount = JsonHelper.GetJsonElementValue(jsonElementPrintCount);
                                var status = JsonHelper.GetJsonElementValue(jsonElementStatus);
                                var setWarehouse = JsonHelper.GetJsonElementValue(jsonElementWarehouse);
                                
                                docList.data.Add(new Frappe_DocList.data()
                                {
                                    Date = Convert.ToDateTime(originalSrcString),
                                    DocType = ps.DocType,
                                    Grand_Total = Convert.ToDouble(grandTot),
                                    custom_print_count = Convert.ToInt16(printCount),
                                    Name = docname.ToString(),
                                    Status = status.ToString(),
                                    Title = title.ToString(),
                                    Owner = owner.ToString(),
                                    Set_Warehouse = setWarehouse?.ToString() ?? ""
                                });
                            }
                            catch (Exception exJsonElement)
                            {
                                Log.Error(exJsonElement, "Error in Element {0}", nameCursor);
                            }
                        }
                    }

                    // Apply warehouse filter if set
                    if (!string.IsNullOrEmpty(ps.WarehouseFilter))
                    {
                        var warehouseList = ps.WarehouseFilter.Split(',').Select(w => w.Trim()).ToList();
                        var filteredDocs = docList.data.Where(d =>
                            !string.IsNullOrEmpty(d.Set_Warehouse) &&
                            warehouseList.Contains(d.Set_Warehouse)).ToList();

                        Log.Information("Warehouse filter applied: {0} documents matched out of {1}",
                            filteredDocs.Count, docList.data.Count);
                        docList.data = filteredDocs;
                    }

                    return docList;
                }
            }
            catch (Exception exSQL)
            {
                Log.Error(exSQL, exSQL.Message + Environment.NewLine + "Failing Endpoint: " + string.Format("{0}/{1}", Program.FrappeURL, FilterStr));
                return null;
            }
        }

        public async Task<string> GetLinkedPaymentEntries(string doctype, string docname)
    {
        // Query Payment Entry directly filtering by references child table
        // Payment Entry Reference child table is not directly accessible via API
        string filter = string.Format(
            "api/resource/Payment Entry?fields=[\"name\",\"posting_date\",\"paid_amount\",\"mode_of_payment\",\"reference_no\"]" +
            "&filters=[[\"Payment Entry\",\"references\",\"like\",\"%{0}%\"]]",
            docname);
        return await GetAsString(filter, "");
    }

    public async Task<string> GetPaymentEntryDetails(string paymentEntryName)
    {
        // Get Payment Entry details
        return await GetAsString("api/resource/Payment Entry/", paymentEntryName);
    }

    #region GetDocumentJson - Unified document fetch with enrichment

    /// <summary>
    /// Gets a document's full JSON and enriches it with linked payments and cleaned HTML.
    /// This is the preferred method for getting document data for printing/display.
    /// </summary>
    /// <param name="doctype">The DocType (e.g., "Sales Invoice", "Sales Order")</param>
    /// <param name="docname">The document name/ID</param>
    /// <param name="enrich">Whether to enrich the JSON with linked payments and strip HTML (default: true)</param>
    /// <returns>JSON string with the document data</returns>
    public async Task<string> GetDocumentJson(string doctype, string docname, bool enrich = true)
    {
        try
        {
            // Get the raw document JSON
            string jsonDoc = await GetAsString($"api/resource/{doctype}/", docname);

            if (string.IsNullOrEmpty(jsonDoc))
            {
                Log.Warning("GetDocumentJson: Empty response for {0}/{1}", doctype, docname);
                return jsonDoc;
            }

            // Enrich if requested
            if (enrich)
            {
                jsonDoc = await EnrichJsonDocument(jsonDoc, doctype, docname);
            }

            return jsonDoc;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "GetDocumentJson failed for {0}/{1}", doctype, docname);
            return "";
        }
    }

    /// <summary>
    /// Enriches a document JSON with linked payment information and strips HTML from item descriptions.
    /// </summary>
    private async Task<string> EnrichJsonDocument(string jsonDoc, string doctype, string docname)
    {
        try
        {
            using (JsonDocument document = JsonDocument.Parse(jsonDoc))
            {
                var root = document.RootElement;
                var dataElement = JsonHelper.GetJsonElement(root, "data");

                if (dataElement.ValueKind == JsonValueKind.Undefined)
                {
                    Log.Warning("EnrichJsonDocument: No 'data' element found in JSON");
                    return jsonDoc;
                }

                // Build a new JSON object with modifications
                var options = new JsonWriterOptions { Indented = false };
                using (var stream = new MemoryStream())
                {
                    using (var writer = new Utf8JsonWriter(stream, options))
                    {
                        writer.WriteStartObject();
                        writer.WritePropertyName("data");
                        writer.WriteStartObject();

                        // Copy all existing properties from data, modifying items as needed
                        foreach (var property in dataElement.EnumerateObject())
                        {
                            if (property.Name == "items" && property.Value.ValueKind == JsonValueKind.Array)
                            {
                                // Write items with HTML stripped from description
                                writer.WritePropertyName("items");
                                writer.WriteStartArray();
                                foreach (var item in property.Value.EnumerateArray())
                                {
                                    writer.WriteStartObject();

                                    // First pass: get item_name for comparison
                                    string itemName = null;
                                    if (item.TryGetProperty("item_name", out var itemNameProp) &&
                                        itemNameProp.ValueKind == JsonValueKind.String)
                                    {
                                        itemName = itemNameProp.GetString();
                                    }

                                    foreach (var itemProp in item.EnumerateObject())
                                    {
                                        if (itemProp.Name == "description" && itemProp.Value.ValueKind == JsonValueKind.String)
                                        {
                                            string stripped = StripHtml(itemProp.Value.GetString());
                                            // Blank out description if it matches item_name
                                            if (string.Equals(stripped, itemName, StringComparison.OrdinalIgnoreCase))
                                            {
                                                stripped = "";
                                            }
                                            writer.WriteString("description", stripped);
                                        }
                                        else
                                        {
                                            itemProp.WriteTo(writer);
                                        }
                                    }
                                    writer.WriteEndObject();
                                }
                                writer.WriteEndArray();
                            }
                            else
                            {
                                property.WriteTo(writer);
                            }
                        }

                        // Fetch and add linked payments
                        var linkedPayments = await GetLinkedPaymentsInternal(doctype, docname);
                        writer.WritePropertyName("linked_payments");
                        writer.WriteStartArray();
                        foreach (var payment in linkedPayments)
                        {
                            writer.WriteStartObject();
                            writer.WriteString("payment_entry", payment.PaymentEntry);
                            writer.WriteString("posting_date", payment.PostingDate);
                            writer.WriteNumber("paid_amount", payment.PaidAmount);
                            writer.WriteString("mode_of_payment", payment.ModeOfPayment);
                            writer.WriteString("reference_no", payment.ReferenceNo ?? "");
                            writer.WriteNumber("allocated_amount", payment.AllocatedAmount);
                            writer.WriteEndObject();
                        }
                        writer.WriteEndArray();

                        writer.WriteEndObject(); // end data
                        writer.WriteEndObject(); // end root
                    }

                    return Encoding.UTF8.GetString(stream.ToArray());
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "EnrichJsonDocument failed, returning original JSON");
            return jsonDoc;
        }
    }

    /// <summary>
    /// Strips HTML tags from a string
    /// </summary>
    private static string StripHtml(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        string stripped = Regex.Replace(input, "<.*?>", string.Empty);
        return System.Net.WebUtility.HtmlDecode(stripped).Trim();
    }

    /// <summary>
    /// Gets linked payment entries for a document
    /// </summary>
    private async Task<List<LinkedPayment>> GetLinkedPaymentsInternal(string doctype, string docname)
    {
        var payments = new List<LinkedPayment>();
        try
        {
            string peJson = await GetLinkedPaymentEntries(doctype, docname);

            if (string.IsNullOrEmpty(peJson)) return payments;

            using (JsonDocument peDoc = JsonDocument.Parse(peJson))
            {
                var dataArray = JsonHelper.GetJsonElement(peDoc.RootElement, "data");
                if (dataArray.ValueKind != JsonValueKind.Array) return payments;

                foreach (var peEntry in dataArray.EnumerateArray())
                {
                    var nameElement = JsonHelper.GetJsonElement(peEntry, "name");
                    var postingDateElement = JsonHelper.GetJsonElement(peEntry, "posting_date");
                    var paidAmountElement = JsonHelper.GetJsonElement(peEntry, "paid_amount");
                    var modeOfPaymentElement = JsonHelper.GetJsonElement(peEntry, "mode_of_payment");
                    var referenceNoElement = JsonHelper.GetJsonElement(peEntry, "reference_no");

                    string peName = JsonHelper.GetJsonElementValue(nameElement);
                    if (string.IsNullOrEmpty(peName)) continue;

                    double paidAmount = 0;
                    if (paidAmountElement.ValueKind == JsonValueKind.Number)
                        paidAmount = paidAmountElement.GetDouble();

                    payments.Add(new LinkedPayment
                    {
                        PaymentEntry = peName,
                        PostingDate = JsonHelper.GetJsonElementValue(postingDateElement) ?? "",
                        PaidAmount = paidAmount,
                        ModeOfPayment = JsonHelper.GetJsonElementValue(modeOfPaymentElement) ?? "",
                        ReferenceNo = JsonHelper.GetJsonElementValue(referenceNoElement),
                        AllocatedAmount = paidAmount // Using paid_amount as allocated since we can't get per-reference allocation easily
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "GetLinkedPaymentsInternal failed for {0}/{1}", doctype, docname);
        }
        return payments;
    }

    /// <summary>
    /// Internal class for linked payment data
    /// </summary>
    private class LinkedPayment
    {
        public string PaymentEntry { get; set; }
        public string PostingDate { get; set; }
        public double PaidAmount { get; set; }
        public string ModeOfPayment { get; set; }
        public string ReferenceNo { get; set; }
        public double AllocatedAmount { get; set; }
    }

    #endregion

    public async Task<bool> UpdateCount(string api_endpoint, Frappe_DocList.data doc)
        {   //With API Token
            try
            {
                string cleanedUrl = CleanUrl(Program.FrappeURL, api_endpoint, doc.Name);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, cleanedUrl);
                request.Headers.Add("Authorization", $"token {Program.ApiToken}");

                using (var client = new HttpClient() { BaseAddress = new Uri(Program.FrappeURL) })
                {
                    var content = new MultipartFormDataContent();
                    int newCount = doc.custom_print_count + 1;
                    content.Add(new StringContent(newCount.ToString()), "custom_print_count");
                    request.Content = content;
                    HttpResponseMessage response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    Log.Information("Updated {0} Print Count from {1} to {2}", doc.Name, doc.custom_print_count, newCount);
                    return true;
                }
            }
            catch (Exception exSQL)
            {
                Log.Error(exSQL, exSQL.Message);
                return false;
            }
        }

    }
}
