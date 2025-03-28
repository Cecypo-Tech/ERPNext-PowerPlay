using DevExpress.XtraReports.Templates;
using DevExpress.XtraRichEdit.Import.EPub;
using ERPNext_PowerPlay.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format("{0}/{1}?{2}", Program.FrappeURL,api_endpoint, api_filter));
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

        public async Task<HttpResponseMessage?> GetAsReponse(string api_endpoint, string api_filter)
        {   //With cookies
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format("{0}/{1}?{2}", Program.FrappeURL, api_endpoint, api_filter));
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


    }
}
