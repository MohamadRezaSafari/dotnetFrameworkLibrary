using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Web.Helpers;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Net.Http;
using System.Net;

namespace Providers
{
    public class Upload : System.Web.Mvc. Controller
    {
        private readonly string[] ImageExtensions = new string[] { ".jpeg", ".jpg", ".png", ".gif" };
        private readonly string[] DocExtensions = new string[] { ".pdf", ".doc", ".docx", ".pptx", ".ppt", ".zip" };
        private readonly string[] ZipExtensions = new string[] { ".zip", ".rar" };
        private readonly string[] VideoExtensions = new string[] { ".mp4", ".webm", ".ogg", ".gif" };
        private string ImageName;
        private string ImageName_thumb;
        private string name;



        public string UploadImage(string dir, string folder, int MaxSize, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0 && ImageExtensions.Contains(Path.GetExtension(file.FileName).ToLower()) && file.ContentType.Contains("image"))
            {
                if (FileSize.Mb(file.ContentLength) > MaxSize)
                    throw new Exception("Big File!");

                try
                {
                    ImageName = folder + "_" + Rand.Mix() + Path.GetExtension(file.FileName);
                    
                    //string path = Path.Combine( 
                    //    Server.MapPath(dir),
                    //    //HostingEnvironment.MapPath(dir), 
                    //    Path.GetFileName(ImageName));
                    //file.SaveAs(path);

                    var FilePath = dir + Path.GetFileName(ImageName);
                    file.SaveAs(Server.MapPath(FilePath));

                    return ImageName;
                }
                catch (Exception error)
                {
                    throw new Exception(error.Message);
                }
            }
            else
            {
                return null;
            }

        }



        public string UploadImage(string dir, int MaxSize, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0 && ImageExtensions.Contains(Path.GetExtension(file.FileName).ToLower()) && file.ContentType.Contains("image"))
            {
                if (FileSize.Mb(file.ContentLength) > MaxSize)
                    throw new Exception("Big File!");

                try
                {
                    ImageName = Rand.GUID() + Path.GetExtension(file.FileName);
                    string path = Path.Combine(HostingEnvironment.MapPath(dir), Path.GetFileName(ImageName));
                    file.SaveAs(path);
                    return ImageName;
                }
                catch (Exception error)
                {
                    throw new Exception(error.Message);
                }
            }
            else
            {
                return null;
            }

        }




        public string UploadZipFile(string dir, int MaxSize, HttpPostedFileBase file)
        {
            string fileName = String.Empty;

            try
            {
                if (FileSize.Mb(file.ContentLength) > MaxSize)
                    throw new Exception("Big File!");

                if (file != null && file.ContentLength > 0 && ZipExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
                {
                    fileName = Rand.DateTimeTick() + Path.GetExtension(file.FileName);
                    string path = Path.Combine(HostingEnvironment.MapPath(dir), Path.GetFileName(fileName));
                    file.SaveAs(path);
                }

                return fileName;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }



        public string UploadFile(string dir, int MaxSize, HttpPostedFileBase file)
        {
            string fileName = String.Empty;

            try
            {
                if (FileSize.Mb(file.ContentLength) > MaxSize)
                    throw new Exception("Big File!");

                if (file != null && file.ContentLength > 0 && DocExtensions.Contains(Path.GetExtension(file.FileName).ToLower()) && file.ContentType.Contains("application"))
                {
                    fileName = Rand.DateTimeTick() + Path.GetExtension(file.FileName);
                    string path = Path.Combine(HostingEnvironment.MapPath(dir), Path.GetFileName(fileName));
                    file.SaveAs(path);
                }

                return fileName;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }




        public string UploadVideo(string dir, int MaxSize, HttpPostedFileBase file)
        {
            string videoName = String.Empty;

            try
            {
                if (FileSize.Mb(file.ContentLength) > MaxSize)
                    throw new Exception("Big File!");

                if (file != null && file.ContentLength > 0 && VideoExtensions.Contains(Path.GetExtension(file.FileName).ToLower()) && file.ContentType.Contains("video"))
                {
                    videoName = Rand.Mix() + Path.GetExtension(file.FileName);
                    string path = Path.Combine(HostingEnvironment.MapPath(dir), Path.GetFileName(videoName));
                    file.SaveAs(path);                    
                }

                return videoName;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }




        // Web Api Document multipart/form-data
        /*
         *  var provider = await Request.Content.ReadAsMultipartAsync(new InMemoryMultipartFormDataStream());
            NameValueCollection formData = provider.FormData;
            IList<HttpContent> files = provider.Files;
            string name = await upload.FormDataDocument(provider.Files, "~/Upload/", ".pdf", 2);
          */
        public async Task<string> FormDataDocument(IList<HttpContent> Files, string Dir, string Extension, int MaxSizeMB)
        {
            if (Files.Count == 0)
            {
                throw new Exception("Null");
            }
            if (DocExtensions.Contains(Extension.ToLower()))
            {
                name = Rand.Mix() + Extension.ToLower();
            }
            else
            {
                throw new Exception("Invalid Extension");
            }
                
            Stream input = await Files[0].ReadAsStreamAsync();

            if (FileSize.KB(input.Length) <= 0)
            {
                throw new Exception("File is empty");
            }
            if (FileSize.Mb(input.Length) > MaxSizeMB)
            {
                throw new Exception("Maximum Size = " + MaxSizeMB + " mb");
            }

            string path = HostingEnvironment.MapPath(Dir + name);

            using (Stream file = System.IO.File.OpenWrite(path))
            {
                await input.CopyToAsync(file);
                file.Close();
            }

            return name;
        }

        // Web Api Image multipart/form-data   ---   1280 * 700
        /*
         *  var provider = await Request.Content.ReadAsMultipartAsync(new InMemoryMultipartFormDataStream());
            NameValueCollection formData = provider.FormData;
            IList<HttpContent> files = provider.Files;
            string name = await upload.FormDataImage(files, "~/Upload/", formData["extension"], 10, 10, 1);
        */
        public async Task<string> FormDataImage(IList<HttpContent> files, string dir, string extension, int Width, int Height, int MaxSizeMB)
        {
            if (ImageExtensions.Contains(extension.ToLower()))
            {
                name = Rand.GUID() + extension.ToLower();
            }
            else
            {
                throw new Exception("Invalid Extension");
            }
                            
            byte[] data = await files[0].ReadAsByteArrayAsync();

            if (FileSize.KB(data.Length) <= 0)
            {
                throw new Exception("File is empty");
            }
            if (FileSize.Mb(data.Length) > MaxSizeMB)
            {
                throw new Exception("Maximum Size = " + MaxSizeMB + " mb");
            }
                
            string path = HostingEnvironment.MapPath(dir + name);

            using (var stream = new MemoryStream(data))
            {
                var img = Image.FromStream(stream);
                var thumbnail = img.GetThumbnailImage(Width, Height, () => false, IntPtr.Zero);
                thumbnail.Save(path, ImageFormat.Jpeg);
            }

            return name;
        }

        // Web Api Image multipart/form-data     
        /*
         *  var provider = await Request.Content.ReadAsMultipartAsync(new InMemoryMultipartFormDataStream());
            NameValueCollection formData = provider.FormData;
            IList<HttpContent> files = provider.Files;
            string name = await upload.FormDataImage(files, "~/Upload/", formData["extension"], 1);
        */
        public async Task<string> FormDataImage(IList<HttpContent> files, string dir, string extension, int MaxSizeMB)
        {
            if (ImageExtensions.Contains(extension.ToLower()))
            {
                name = Rand.GUID() + extension.ToLower();
            }
            else
            {
                throw new Exception("Invalid Extension");
            }
               
            Stream input = await files[0].ReadAsStreamAsync();

            if (FileSize.KB(input.Length) <= 0)
            {
                throw new Exception("File is empty");
            }
            if (FileSize.Mb(input.Length) > MaxSizeMB)
            {
                throw new Exception("Maximum Size = " + MaxSizeMB + " mb");
            }
                
            string path = HostingEnvironment.MapPath(dir + name);

            using (Stream file = System.IO.File.OpenWrite(path))
            {
                await input.CopyToAsync(file);
                file.Close();
            }

            return name;
        }


       
        // Test
        public async Task<Array> ImageToByteArray()
        {
            byte[] arr = System.IO.File.ReadAllBytes(HostingEnvironment.MapPath("~/Upload/101.jpeg"));
            return await Task.FromResult(arr);
        }


        public string APIUpload(HttpPostedFileBase file)
        {
            var fileBytes = new byte[file.ContentLength];
            file.InputStream.Read(fileBytes, 0, fileBytes.Length);

            return Convert.ToBase64String(fileBytes);
        }


        public string ImageUpload(string dir, int Width, int Height, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0 && ImageExtensions.Contains(Path.GetExtension(file.FileName).ToLower()) && file.ContentType.Contains("image"))
            {
                try
                {
                    ImageName = Rand.GUID() + Path.GetExtension(file.FileName);
                    ImageName_thumb = Rand.Number() + "_thumb" + Path.GetExtension(file.FileName);

                    string path = Path.Combine(HostingEnvironment.MapPath(dir), Path.GetFileName(ImageName));
                    string path_tumb = Path.Combine(HostingEnvironment.MapPath(dir), Path.GetFileName(ImageName_thumb));

                    file.SaveAs(path);
                    Crop(Width, Height, file.InputStream, path_tumb);

                    return ImageName;
                }
                catch (Exception error)
                {
                    throw new Exception(error.Message);
                }
            }
            else
            {
                return null;
            }
        }


        public string ImageUpload(string dir, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0 && ImageExtensions.Contains(Path.GetExtension(file.FileName).ToLower()) && file.ContentType.Contains("image"))
            {
                try
                {
                    ImageName = Rand.GUID() + Path.GetExtension(file.FileName);
                    string path = Path.Combine(HostingEnvironment.MapPath(dir), Path.GetFileName(ImageName));
                    file.SaveAs(path);
                    return ImageName;
                }
                catch (Exception error)
                {
                    throw new Exception(error.Message);
                }
            }
            else
            {
                return null;
            }

        }


        public void DeleteFile(string dir, string name)
        {
            try
            {
                var fullPath = HostingEnvironment.MapPath(dir + name);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        

        public void DeleteFile(string fullPath)
        {
            try
            {
                var _fullPath = HostingEnvironment.MapPath(fullPath);

                if (System.IO.File.Exists(_fullPath))
                {
                    System.IO.File.Delete(_fullPath);
                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }



        public static void Crop(int Width, int Height, Stream streamImg, string saveFilePath)
        {
            try
            {
                WebImage img = new WebImage(streamImg);
                img.Resize(Width, Height);
                img.Save(saveFilePath);
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}
 