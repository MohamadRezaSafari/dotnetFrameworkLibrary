using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Providers
{
    public class ChunkFile
    {
        public FileStream fs;
        string MergeFolder;
        List<string> Packets = new List<string>();
        string SaveFileFolder = @"D:\Develop\";


        // await chunk.SplitFileAsync(@"D:\Develop\farakhan54.sql", 3);
        public async Task<bool> SplitFileAsync(string SourceFile, int nNoofFiles)
        {
            bool Split = false;

            try
            {
                FileStream fs = new FileStream(SourceFile, FileMode.Open, FileAccess.Read);
                int SizeofEachFile = (int)Math.Ceiling((double)fs.Length / nNoofFiles);

                for (int i = 0; i < nNoofFiles; i++)
                {
                    string baseFileName = Path.GetFileNameWithoutExtension(SourceFile);
                    string Extension = Path.GetExtension(SourceFile);

                    FileStream outputFile = new FileStream(Path.GetDirectoryName(SourceFile) + "\\" + baseFileName + "." +
                                                           i.ToString().PadLeft(5, Convert.ToChar("0")) + Extension + ".tmp", FileMode.Create, FileAccess.Write);

                    MergeFolder = Path.GetDirectoryName(SourceFile);

                    int bytesRead = 0;
                    byte[] buffer = new byte[SizeofEachFile];

                    if ((bytesRead = fs.Read(buffer, 0, SizeofEachFile)) > 0)
                    {
                        await outputFile.WriteAsync(buffer, 0, bytesRead);
                        //outp.Write(buffer, 0, BytesRead);

                        string packet = baseFileName + "." + i.ToString().PadLeft(3, Convert.ToChar("0")) + Extension.ToString();
                        Packets.Add(packet);
                    }
                    outputFile.Close();
                }
                fs.Close();
            }
            catch (Exception Ex)
            {
                throw new ArgumentException(Ex.Message);
            }
            return Split;
        }




        // await chunk.MergeFileAsync(@"D:\Develop\");
        public async Task<bool> MergeFileAsync(string inputfoldername)
        {
            bool Output = false;

            try
            {
                string[] tmpfiles = Directory.GetFiles(inputfoldername, "*.tmp");

                FileStream outPutFile = null;
                string PrevFileName = "";

                foreach (string tempFile in tmpfiles)
                {
                    string fileName = Path.GetFileNameWithoutExtension(tempFile);
                    string baseFileName = fileName.Substring(0, fileName.IndexOf(Convert.ToChar(".")));
                    string extension = Path.GetExtension(fileName);

                    if (!PrevFileName.Equals(baseFileName))
                    {
                        if (outPutFile != null)
                        {
                            await outPutFile.FlushAsync();
                            outPutFile.Close();
                        }
                        outPutFile = new FileStream(SaveFileFolder + "\\" + baseFileName + extension, FileMode.OpenOrCreate, FileAccess.Write);
                    }

                    int bytesRead = 0;
                    byte[] buffer = new byte[1024];
                    FileStream inputTempFile = new FileStream(tempFile, FileMode.OpenOrCreate, FileAccess.Read);

                    while ((bytesRead = inputTempFile.Read(buffer, 0, 1024)) > 0)
                        await outPutFile.WriteAsync(buffer, 0, bytesRead);

                    inputTempFile.Close();
                    File.Delete(tempFile);
                    PrevFileName = baseFileName;
                }
                outPutFile.Close();
            }
            catch (Exception Ex)
            {
                throw new ArgumentException(Ex.Message);
            }
            return Output;
        }


        // ChunkFile.CreateFolder(HostingEnvironment.MapPath("~/HHHHHHHH"));
        public static void CreateFolder(string path)
        {
            if (!(Directory.Exists(path)))
            {
                Directory.CreateDirectory(path);
            }
        }


        // ChunkFile.DeleteFolder(HostingEnvironment.MapPath("~/HHHHHHHH"));
        public static void DeleteFolder(string path)
        {
            if ((Directory.Exists(path)))
            {
                Directory.Delete(path);
            }
        }
    }
} 
  