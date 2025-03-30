using DevExpress.DataAccess.Native.Web;
using DevExpress.XtraEditors.Filtering.Templates;
using DevExpress.XtraReports.Templates;
using DevExpress.XtraRichEdit.Import.EPub;
using ERPNext_PowerPlay.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ERPNext_PowerPlay.Helpers
{
    public class FrappeAPI
    {
        public async Task<string> GetAsString(string api_endpoint, string api_filter)
        {   //With cookies
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format("{0}/{1}{2}", Program.FrappeURL, api_endpoint, api_filter));
                using (var handler = new HttpClientHandler() { CookieContainer = Program.Cookies })
                using (var client = new HttpClient(handler) { BaseAddress = new Uri(Program.FrappeURL) })
                {
                    HttpResponseMessage response_qr = await client.SendAsync(request);
                    response_qr.EnsureSuccessStatusCode();

                    string result = await response_qr.Content.ReadAsStringAsync();
                    return result;
                }

            }
            catch (Exception exSQL)
            {
                Log.Error(exSQL, exSQL.Message);
                return "";
            }
        }

        public async Task<HttpResponseMessage> GetAsReponse(string api_endpoint, string api_filter)
        {   //With cookies
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format("{0}/{1}{2}", Program.FrappeURL, api_endpoint, api_filter));
                using (var handler = new HttpClientHandler() { CookieContainer = Program.Cookies })
                using (var client = new HttpClient(handler) { BaseAddress = new Uri(Program.FrappeURL) })
                {
                    HttpResponseMessage response_qr = await client.SendAsync(request);
                    response_qr.EnsureSuccessStatusCode();

                    return response_qr;
                }

            }
            catch (Exception exSQL)
            {
                Log.Error(exSQL, exSQL.Message);
                return null;
            }
        }

        public async Task<Frappe_DocList.FrappeDocList> GetDocs2Print(PrinterSetting ps)
        {   //With cookies
            try
            {
               //for this ps.doctype, get all docs, with filters
               //fields + filters
                string FilterStr = string.Format("/api/resource/Sales Invoice?fields=[\"name\", \"customer\", \"posting_date\", \"docstatus\", \"status\", \"etr_invoice_number\"," +
                                                    "\"etr_invoice_number\", \"total_taxes_and_charges\", \"total\"]" +
                                                    "&filters=[" +
                                                    "[\"Sales Invoice\",\"docstatus\",\"=\",\"1\"]" +
                                                    ",[\"Sales Invoice\",\"etr_invoice_number\",\"!=\",\"\"]" + //Not empty
                                                    ",[\"Sales Invoice\",\"posting_date\",\">\",\"{0}\"]" +     //After Date
                                                    ",[\"Sales Invoice\",\"custom_print_count\",\"=\",\"0\"]" + //Print Count = 0
                                                    "]&limit_page_length={1}", new DateOnly(2025, 01, 01).ToString("yyyy-MM-dd"), 10);

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format("{0}/{1}", Program.FrappeURL, FilterStr));
                using (var handler = new HttpClientHandler() { CookieContainer = Program.Cookies })
                using (var client = new HttpClient(handler) { BaseAddress = new Uri(Program.FrappeURL) })
                {
                    HttpResponseMessage response_qr = await client.SendAsync(request);
                    response_qr.EnsureSuccessStatusCode();
                    Frappe_DocList.FrappeDocList docList = new Frappe_DocList.FrappeDocList();
                    docList = await response_qr.Content.ReadFromJsonAsync<Frappe_DocList.FrappeDocList>();
                    return docList;
                }
            }
            catch (Exception exSQL)
            {
                Log.Error(exSQL, exSQL.Message);
                return null;
            }
        }

        public async Task<bool> UpdateCount(string api_endpoint, Frappe_DocList.data doc)
        {   //With cookies
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, string.Format("{0}/{1}/{2}", Program.FrappeURL, api_endpoint, doc.name));
                using (var handler = new HttpClientHandler() { CookieContainer = Program.Cookies })
                using (var client = new HttpClient(handler) { BaseAddress = new Uri(Program.FrappeURL) })
                {
                    var content = new MultipartFormDataContent();
                    int newCount = doc.custom_print_count + 1;
                    content.Add(new StringContent(newCount.ToString()), "custom_print_count");
                    request.Content = content;
                    HttpResponseMessage response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
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
