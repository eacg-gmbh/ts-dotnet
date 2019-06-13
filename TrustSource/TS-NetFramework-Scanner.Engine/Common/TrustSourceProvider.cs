using System;
using System.IO;
using System.Net;

namespace TS_NetFramework_Scanner.Engine
{
    public class TrustSourceProvider
    {
        private static string TrustSourceApiUrl = $"https://app.trustsource.io/api/v1"; //"https://test-green.trustsource.io/api/v1";

        public static string PostScan(string targetJson, string trustSourceUserName, string trustSourceApikey, string trustSourceApiUrl = "")
        {
            try
            {
                // This address is provided to allow Test instances on TrustSource if required.
                // By default empty string will be coming, so it will maintain its default value.
                if (!string.IsNullOrEmpty(trustSourceApiUrl))
                {
                    TrustSourceApiUrl = trustSourceApiUrl;
                }

                if (string.IsNullOrEmpty(TrustSourceApiUrl))
                {
                    throw new Exception("There is some problem with TrustSourceApiUrl");
                }

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
                throw wex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
