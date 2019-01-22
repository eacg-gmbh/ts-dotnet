using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace TS_NetCore_Scanner.Engine
{
    internal class TrustSourceProvider
    {
        private static string TrustSourceApiUrl = $"https://app.trustsource.io/api/v1"; //"https://test-green.trustsource.io/api/v1";

        public static string PostScan(string targetJson, string trustSourceUserName, string trustSourceApikey)
        {
            try
            {
                var client = new WebClient();

                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("user-agent", $"TS-NetCore-Scanner/1.0.0");
                client.Headers.Add("X-APIKEY", trustSourceApikey);
                client.Headers.Add("X-USER", trustSourceUserName);

                var response = client.UploadString(TrustSourceApiUrl + $"/scans", "POST", targetJson);

                return response;
            }
            catch (WebException wex)
            {
                string errorMessage;

                using (var stream = wex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    errorMessage = reader.ReadToEnd();
                }

                throw wex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
