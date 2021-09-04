using System;
using System.Security.Cryptography;
using System.Text;

namespace Providers
{
    public class DataProtectionSample
    {
        // Create byte array for additional entropy when using Protect method.
        static byte[] s_aditionalEntropy = Encoding.UTF8.GetBytes("J6zXuQzPikqRn9cz4C0tAPfMvfyh6EaUs5QFK6DP");


        public static string Protect(string enc)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(enc);
                // Encrypt the data using DataProtectionScope.CurrentUser. The result can be decrypted
                // only by the same current user.
                byte[] encrypt = ProtectedData.Protect(data, s_aditionalEntropy, DataProtectionScope.CurrentUser);
                
                return Convert.ToBase64String(encrypt);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("Data was not encrypted. An error occurred.");
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        public static string Unprotect(string dec)
        {
            try
            {
                byte[] data = Convert.FromBase64String(dec.Replace(' ', '+'));

                //Decrypt the data using DataProtectionScope.CurrentUser.
                byte[] decrypt = ProtectedData.Unprotect(data, s_aditionalEntropy, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(decrypt); 
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("Data was not decrypted. An error occurred.");
                Console.WriteLine(e.ToString());
                return null;
            }
        }

    }
}