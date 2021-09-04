using System;
using System.Security.Cryptography;
using System.Text;

namespace Providers
{
    public class RSACryptoSystem
    {
        private static RSAParameters PublicKey;
        private static RSAParameters PrivateKey;
        protected static string encrypt;
        protected static string decrypt;


        public static string[] Keys(int KeySize)
        {
            try
            {
                using (var rsa = new RSACryptoServiceProvider(KeySize))
                {
                    rsa.PersistKeyInCsp = false;
                    PublicKey = rsa.ExportParameters(false);
                    PrivateKey = rsa.ExportParameters(true);
                };

                return new string[] {
                    string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent></RSAKeyValue>",
                        Convert.ToBase64String(PublicKey.Modulus),
                        Convert.ToBase64String(PublicKey.Exponent))
                    ,
                    string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
                        Convert.ToBase64String(PrivateKey.Modulus),
                        Convert.ToBase64String(PrivateKey.Exponent),
                        Convert.ToBase64String(PrivateKey.P),
                        Convert.ToBase64String(PrivateKey.Q),
                        Convert.ToBase64String(PrivateKey.DP),
                        Convert.ToBase64String(PrivateKey.DQ),
                        Convert.ToBase64String(PrivateKey.InverseQ),
                        Convert.ToBase64String(PrivateKey.D))
                };
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        // Use Encrypt 
        /*
         *  string[] test = RSACryptoSystem.Keys(2048);
            RSACryptoSystem.UseEncrypt("ali", test[0], 2048);
        */
        public static string UseEncrypt(string Txt, string PublicK, int KeySize)
        {
            byte[] encryptByte;

            encryptByte = Encoding.UTF8.GetBytes(Txt);
            encrypt = Convert.ToBase64String(Encrypt(encryptByte, PublicK, KeySize));

            return encrypt;
        }


        // Use Decrypt 
        /*
         *  string[] test = RSACryptoSystem.Keys(2048);
            RSACryptoSystem.UseDecrypt(TempData["enc"].ToString(), test[1], 2048);
        */
        public static string UseDecrypt(string Txt, string PrivateK, int KeySize)
        {
            byte[] decryptByte;
            byte[] dec;

            decryptByte = Convert.FromBase64String(Txt);
            dec = Decrypt(decryptByte, PrivateK, KeySize);
            decrypt = Encoding.UTF8.GetString(dec);

            return decrypt;
        }


        public static byte[] Encrypt(byte[] input, string _publicKey, int KeySize)
        {
            try
            {
                byte[] encrypted;
                using (var rsa = new RSACryptoServiceProvider(KeySize))
                {
                    rsa.PersistKeyInCsp = false;
                    rsa.FromXmlString(_publicKey);
                    encrypted = rsa.Encrypt(input, true);
                };
                return encrypted;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }


        public static byte[] Decrypt(byte[] input, string _privateKey, int KeySize)
        {
            try
            {
                byte[] decrypted;
                using (var rsa = new RSACryptoServiceProvider(KeySize))
                {
                    rsa.PersistKeyInCsp = false;
                    rsa.FromXmlString(_privateKey);
                    decrypted = rsa.Decrypt(input, true);
                };
                return decrypted;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }


        /*static RSAProvider()
        {
            GenerateKeys();
        }


        private static void GenerateKeys()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                publicKey = rsa.ExportParameters(false);
                privateKey = rsa.ExportParameters(true);

                //File.WriteAllText(HostingEnvironment.MapPath("~/Keys/" + "PrivateKey.xml"), rsa.ToXmlString(true)); 
                //File.WriteAllText(HostingEnvironment.MapPath("~/Keys/" + "PublicKey.xml"), rsa.ToXmlString(false));
            };
        }*/
        /*public static byte[] Encrypt(byte[] input)
        {
            byte[] encrypted;
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.ImportParameters(publicKey);
                encrypted = rsa.Encrypt(input, true);
            };
            return encrypted;
        }



        public static byte[] Decrypt(byte[] input)
        {
            byte[] decrypted;
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.ImportParameters(privateKey);
                decrypted = rsa.Decrypt(input, true);
            };
            return decrypted;
        }*/
    }
}