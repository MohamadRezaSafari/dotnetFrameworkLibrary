using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace Providers
{
    public class IPGeolocationAPI
    {
        /**
         * var x = await IPGeolocationAPI.SendRequest("46.225.163.185", "city");
         */
        public static async Task<string> SendRequest(string IP, string Subject)
        {
            var client = new HttpClient();
            var uri = new Uri(String.Format("http://ip-api.com/json/{0}", IP));
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            var response = await client.GetAsync(uri);
            var result = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(result);
            string custom = String.Empty;

            switch (Subject)
            {
                case "country":
                    custom = json["country"].ToString();
                    break;
                case "countryCode":
                    custom = json["countryCode"].ToString();
                    break;
                case "regionName":
                    custom = json["regionName"].ToString();
                    break;
                case "city":
                    custom = json["city"].ToString();
                    break;
                case "isp":
                    custom = json["isp"].ToString();
                    break;
                case "org":
                    custom = json["org"].ToString();
                    break;
                default:
                    custom = null;
                    break;
            }

            return custom;
        }


        /**
         * var x = await IPGeolocationAPI.SendRequest("46.225.163.185");
         * */
        public static async Task<JObject> SendRequest(string IP)
        {
            var client = new HttpClient();
            var uri = new Uri(String.Format("http://ip-api.com/json/{0}", IP));
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                      
            var response = await client.GetAsync(uri);
            var result = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(result);

            return json;
        }
    }
}