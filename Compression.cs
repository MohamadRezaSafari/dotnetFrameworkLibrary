using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace Providers
{
    public class Compression
    {
        private static string ImagePath = HostingEnvironment.MapPath("~/Upload/IMG/");
        private static string ImageFolderPath = HostingEnvironment.MapPath("~/Upload/IMG/");
        private static int imageQuality;
        private static string _name;

        public static async Task<string> VariousQualityFromStream(HttpPostedFileBase file, string quality, string folder, int maxSize)
        {
            if (FileSize.Mb(file.ContentLength) >= maxSize)
                throw new Exception("Big File!");

            Image original = Image.FromStream(file.InputStream, true, true);
            ImageCodecInfo jpgEncoder = null;
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == ImageFormat.Jpeg.Guid)
                {
                    jpgEncoder = codec;
                    break;
                }
            }
            if (jpgEncoder != null)
            {
                Encoder encoder = Encoder.Quality;
                EncoderParameters encoderParameters = new EncoderParameters(1);
                string originalFileName = Rand.Mix();

                switch (quality)
                {
                    case "low":
                        imageQuality = 10;
                        break;
                    case "medium":
                        imageQuality = 50;
                        break;
                    case "high":
                        imageQuality = 100;
                        break;
                    default:
                        imageQuality = 70;
                        break;
                }

                await Task.Run(async () =>
                {
                    EncoderParameter encoderParameter = new EncoderParameter(encoder, imageQuality);
                    encoderParameters.Param[0] = encoderParameter;

                    _name = folder + "-" + originalFileName + ".jpeg";
                    string fileOut = Path.Combine(ImagePath, _name);
                    FileStream ms = new FileStream(fileOut, FileMode.Create, FileAccess.Write);
                    original.Save(ms, jpgEncoder, encoderParameters);
                    await ms.FlushAsync();
                    ms.Close();
                });    
            }
            return _name;
        }


        /*
         *   Compression.VariousQuality("e6418d2c-d816-4b85-ac54-f5b2a0a3bace.jpeg", "low");
         */
        public static string VariousQuality(string fileName, string quality)
        {
            if (!Directory.Exists(ImageFolderPath))
                Directory.CreateDirectory(ImageFolderPath);

            string file = Path.Combine(ImagePath + fileName);
            if (!File.Exists(file))
                throw new Exception("File not found");

            Image original = Image.FromFile(file);
            ImageCodecInfo jpgEncoder = null;
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == ImageFormat.Jpeg.Guid)
                {
                    jpgEncoder = codec;
                    break;
                }
            }
            if (jpgEncoder != null)
            {
                Encoder encoder = Encoder.Quality;
                EncoderParameters encoderParameters = new EncoderParameters(1);
                string originalFileName = Path.GetFileNameWithoutExtension(file);


                switch (quality)
                {
                    case "low":
                        imageQuality = 10;
                        break;
                    case "medium":
                        imageQuality = 50;
                        break;
                    case "high":
                        imageQuality = 100;
                        break;
                    default:
                        imageQuality = 70;
                        break;
                }


                EncoderParameter encoderParameter = new EncoderParameter(encoder, imageQuality);
                encoderParameters.Param[0] = encoderParameter;

                _name = originalFileName + "__quality(" + imageQuality + ").jpeg";
                string fileOut = Path.Combine(ImagePath, _name);
                FileStream ms = new FileStream(fileOut, FileMode.Create, FileAccess.Write);
                original.Save(ms, jpgEncoder, encoderParameters);
                ms.Flush();
                ms.Close();                
            }
            return _name;
        }
    }
}