using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Filters;

namespace Providers
{
    // [WebApiFile(File = "UploadedImage", Folder = "~/Upload/api/", MaxSize = 5, Extension = ".jpeg")]
    public class WebApiFileAttribute : ActionFilterAttribute
    {
        public string File { get; set; }
        public string Folder { get; set; }
        public int MaxSize { get; set; }
        public string Extension { set; get; }
        private readonly string[] ValidExtension = { ".jpeg", ".jpg", ".png", ".gif", ".pdf", ".doc", ".docx", ".pptx", ".ppt" };

        public override void OnActionExecuted(HttpActionExecutedContext actContext)
        {
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var httpPostedFile = HttpContext.Current.Request.Files[File];

                if (httpPostedFile != null)
                {
                    var _size = httpPostedFile.ContentLength;

                    if (FileSize.Mb(_size) > MaxSize)
                        throw new HttpResponseException(HttpStatusCode.RequestEntityTooLarge);
                    if (!ValidExtension.Contains(Extension))
                        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

                    try
                    {
                        var fileName = Rand.DateTimeTick() + Extension;
                        var fileSavePath = Path.Combine(HostingEnvironment.MapPath(Folder), fileName);
                        httpPostedFile.SaveAs(fileSavePath);
                    }
                    catch (Exception)
                    {
                        throw new HttpResponseException(HttpStatusCode.ExpectationFailed);
                    }
                }
            }
        }
    }
}