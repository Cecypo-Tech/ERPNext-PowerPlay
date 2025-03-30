using ERPNext_PowerPlay.Models;
using Serilog;
using System.Net.Http;
using System.Net.Http.Json;

namespace ERPNext_PowerPlay.Helpers
{
    public class FrappeAPIBase
    {

        public async Task<List<Frappe_DocList.data>> GetDocs2Print()
        {   //With cookies
            try
            {
                string FilterStr = string.Format("/api/resource/Sales Invoice?fields=[\"name\", \"customer\", \"posting_date\", \"docstatus\", \"status\", \"etr_invoice_number\"," +
                                                    "\"etr_invoice_number\", \"total_taxes_and_charges\", \"total\"]" +
                                                    "&filters=[" +
                                                    "[\"Sales Invoice\",\"docstatus\",\"=\",\"1\"]" +
                                                    ",[\"Sales Invoice\",\"etr_invoice_number\",\"!=\",\"\"]" + //Not empty
                                                    ",[\"Sales Invoice\",\"posting_date\",\">\",\"{0}\"]" +     //After Date
                                                    ",[\"Sales Invoice\",\"custom_print_count\",\"=\",\"0\"]" + //Print Count = 0
                                                    "]&limit_page_length={1}", new DateOnly(2025, 03, 01).ToString("yyyy-MM-dd"), 10);

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format("{0}/{1}", Program.FrappeURL, FilterStr));
                using (var handler = new HttpClientHandler() { CookieContainer = Program.Cookies })
                using (var client = new HttpClient(handler) { BaseAddress = new Uri(Program.FrappeURL) })
                {
                    HttpResponseMessage response_qr = await client.SendAsync(request);
                    response_qr.EnsureSuccessStatusCode();
                    List<Frappe_DocList.data> docList = await response_qr.Content.ReadFromJsonAsync<List<Frappe_DocList.data>>();
                    return docList;
                }

            }
            catch (Exception exSQL)
            {
                Log.Error(exSQL, exSQL.Message);
                return null;
            }
        }
    }
}