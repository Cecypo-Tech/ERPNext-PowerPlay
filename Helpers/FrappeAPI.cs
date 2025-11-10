using DevExpress.DataAccess.DataFederation;
using DevExpress.DataAccess.Native.Json;
using DevExpress.XtraReports.UI.CrossTab;
using ERPNext_PowerPlay.Models;
using Serilog;
using SQLitePCL;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ERPNext_PowerPlay.Helpers
{
    public class FrappeAPI
    {
        public async Task<string> GetAsString(string api_endpoint, string api_filter)
        {   //With API Token
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format("{0}/{1}{2}", Program.FrappeURL, api_endpoint, api_filter));
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

        public async Task<HttpResponseMessage> GetAsReponse(string api_endpoint, string api_filter)
        {   //With API Token
            try
            {
                if (api_endpoint.StartsWith("/")) api_endpoint = api_endpoint.Substring(1, api_endpoint.Length - 1);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format("{0}/{1}{2}", Program.FrappeURL, api_endpoint, api_filter));
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
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format("{0}/{1}", Program.FrappeURL, FilterStr));
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


                                //Set fields for object
                                var title = JsonHelper.GetJsonElementValue(jsonElementTitle);
                                var owner = JsonHelper.GetJsonElementValue(jsonElementOwner);
                                if (owner == null) owner = "";
                                var originalSrcString = JsonHelper.GetJsonElementValue(jsonElementDate);
                                var grandTot = JsonHelper.GetJsonElementValue(jsonElementTot);
                                var printCount = JsonHelper.GetJsonElementValue(jsonElementPrintCount);
                                var status = JsonHelper.GetJsonElementValue(jsonElementStatus);

                                docList.data.Add(new Frappe_DocList.data()
                                {
                                    Date = Convert.ToDateTime(originalSrcString),
                                    DocType = ps.DocType,
                                    Grand_Total = Convert.ToDouble(grandTot),
                                    custom_print_count = Convert.ToInt16(printCount),
                                    Name = docname.ToString(),
                                    Status = status.ToString(),
                                    Title = title.ToString(),
                                    Owner = owner.ToString()
                                });
                            }
                            catch (Exception exJsonElement)
                            {
                                Log.Error(exJsonElement, "Error in Element {0}", nameCursor);
                            }
                        }
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

        public async Task<bool> UpdateCount(string api_endpoint, Frappe_DocList.data doc)
        {   //With API Token
            try
            {
                if (!api_endpoint.StartsWith("/")) api_endpoint = "/" + api_endpoint;
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, string.Format("{0}{1}/{2}", Program.FrappeURL, api_endpoint, doc.Name));
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
