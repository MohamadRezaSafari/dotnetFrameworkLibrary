using System;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;
using System.IO;
using System.Threading;
using System.Web.Hosting;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Providers
{
    public class Database
    {
        private static readonly string downloadPath = HostingEnvironment.MapPath("~/Download");
        private static readonly string _DataSource = @".";
        private static readonly string _UserID = "1";
        private static readonly string _Password = "123456789#";

        /*
            Microsoft.SqlServer.ConnectionInfo
            Microsoft.SqlServer.Management.Sdk.Sfc
            Microsoft.SqlServer.Smo
            ------------------------
            var path = HostingEnvironment.MapPath("~/Download/");
            string fileName = "Gaurav.sql";
            byte[] fileBytes = System.IO.File.ReadAllBytes(path + fileName);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            --------------------------
            var x = Rand.Mix();
            Database.GenerateScript("Notifier",x);
         */
        public static void GenerateScript(string serverDbName, string fileName)
        {
            var server = new Server(new ServerConnection
            {
                ConnectionString = new SqlConnectionStringBuilder
                {
                    DataSource = _DataSource,
                    UserID = _UserID,
                    Password = _Password
                }.ToString()
            });

            server.ConnectionContext.Connect();
            var database = server.Databases[serverDbName];
            var path = downloadPath + "/" + fileName + ".sql";

            if (!Directory.Exists(downloadPath))
            {
                Directory.CreateDirectory(downloadPath);
            }
            
            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                foreach (Table table in database.Tables)
                {
                    if (table.Name != "sysdiagrams")
                    {
                        var scripter = new Scripter(server) { Options = { ScriptData = true } };
                        var script = scripter.EnumScript(new SqlSmoObject[] { table });
                        foreach(string line in script)
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
            }
            server.ConnectionContext.Disconnect();
        }


        /*
         * Database.GenerateDatabase("xxx", "tw5thsptwcr290620133.sql");
         */
        public static void GenerateDatabase(string dbCreateName, string dbScriptName)
        {
            var _server = new Server(new ServerConnection
            {
                ConnectionString = new SqlConnectionStringBuilder
                {
                    DataSource = _DataSource,
                    UserID = _UserID,
                    Password = _Password
                }.ToString()
            });

            _server.ConnectionContext.Connect();
            _server.ConnectionContext.ExecuteNonQuery("CREATE DATABASE " + dbCreateName);
            _server.ConnectionContext.Disconnect();
            Thread.Sleep(10);

            var server = new Server(new ServerConnection
            {
                ConnectionString = new SqlConnectionStringBuilder
                {
                    DataSource = _DataSource,
                    UserID = _UserID,
                    Password = _Password,
                    InitialCatalog = dbCreateName
                }.ToString()
            });

            server.ConnectionContext.Connect();
            string script = File.ReadAllText(downloadPath + "/" + dbScriptName);
            server.ConnectionContext.ExecuteNonQuery(script);
            server.ConnectionContext.Disconnect();
        }
    }
}
