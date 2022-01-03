using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Health.Models.DB;
using System.Net.Mime;

namespace Providers
{
    public class ExportData
    {
        /*
         *  var export = new ExportData();
            string x = await export.Word(_app.States.ToList());
         */
        public async Task<string> Word(IList data)
        {
            var fileName = Rand.Mix() + ".doc";
            GridView gridview = new GridView();
            gridview.DataSource = data;
            gridview.DataBind();
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.Charset = Encoding.UTF8.EncodingName;
            HttpContext.Current.Response.ContentType = "application/msword";
            HttpContext.Current.Response.AddHeader("Content-Disposition: ", String.Format(@"attachment; filename={0}", fileName));

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    gridview.RenderControl(htw);
                    await HttpContext.Current.Response.Output.WriteAsync(sw.ToString());                    
                    htw.Close();
                }
                sw.Close();
            }

            await HttpContext.Current.Response.FlushAsync();
            HttpContext.Current.Response.Close();

            return fileName;
        }


        /*
         *  var export = new ExportData();
            string x = await export.Excel(_app.States.ToList());
         */
        public async Task<string> Excel(IList data)
        {
            var fileName = Rand.Mix() + ".xls";
            GridView gridview = new GridView();
            gridview.DataSource = data;
            gridview.DataBind();
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.Charset = Encoding.UTF8.EncodingName;
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AddHeader("Content-Disposition: ", String.Format(@"attachment; filename={0}", fileName));              

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    gridview.RenderControl(htw);
                    await HttpContext.Current.Response.Output.WriteAsync(sw.ToString());
                    htw.Close();
                }
                sw.Close();
            }

            await HttpContext.Current.Response.FlushAsync();
            HttpContext.Current.Response.Close();

            return fileName;
        }
    }
}