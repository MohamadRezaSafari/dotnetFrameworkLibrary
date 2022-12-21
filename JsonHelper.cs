using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Web.Hosting;

namespace Providers
{
    public class JsonHelper
    {
        /**
         *  JsonHelper.JsonFileAddParameter("~/app.json", "hot", "YEEE");
         * */
        public static void JsonFileAddParameter(string path, string parameter, string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentNullException("Value");
            if (String.IsNullOrEmpty(parameter))
                throw new ArgumentNullException("Parameter");

            dynamic treatments;
            string _path = HostingEnvironment.MapPath(path);

            using (StreamReader sr = new StreamReader(_path))
            {
                treatments = JsonConvert.DeserializeObject(sr.ReadToEnd());
                sr.Close();
            }
            
            //JObject property = new JObject( new JProperty(parameter, value) );
            treatments[parameter] = value;
            File.WriteAllText(_path, String.Empty);
            string json = JsonConvert.SerializeObject(treatments);
            File.WriteAllText(_path, json);
        }



        /**
         *  JsonHelper.JsonFileEdit("~/app.json", "AppKey", "123456789");
         * */
        public static void JsonFileEdit(string path, string parameter, string editValue)
        {
            if (String.IsNullOrEmpty(editValue))
                throw new ArgumentNullException("Edit Value");
            if (String.IsNullOrEmpty(parameter))
                throw new ArgumentNullException("Parameter");

            dynamic treatments;
            string _path = HostingEnvironment.MapPath(path);

            using (StreamReader sr = new StreamReader(_path))
            {
                treatments = JsonConvert.DeserializeObject(sr.ReadToEnd());
                sr.Close();
            }

            if (treatments[parameter] == null)
                throw new ArgumentNullException(parameter);

            treatments[parameter] = editValue;
            File.WriteAllText(_path, String.Empty);
            string json = JsonConvert.SerializeObject(treatments);
            File.WriteAllText(_path, json);
        }


        /**
         *  var lol = JsonHelper.JsonReader("~/app.json", "AppKey");
         * */
        public static dynamic JsonReader(string path, string parameter)
        {
            dynamic treatments;

            using (StreamReader sr = new StreamReader(HostingEnvironment.MapPath(path)))
            {
                treatments = JsonConvert.DeserializeObject(sr.ReadToEnd());
                sr.Close();
            }

            return treatments[parameter];
        }



        /**
         * var lol = JsonHelper.Json("~/package.json");
         * */
        public static dynamic JsonReader(string path)
        {
            /**
             *  using(StreamReader sr = new StreamReader(Server.MapPath("~/Content/treatments.json")))
                {
                      treatments = JsonConvert.DeserializeObject<List<Treatment>>(sr.ReadToEnd());
                }
             * */
            dynamic treatments;

            using (StreamReader sr = new StreamReader(HostingEnvironment.MapPath(path)))
            {
                treatments = JsonConvert.DeserializeObject(sr.ReadToEnd());
                sr.Close();
            }
            
            return treatments;
        }
    }
}