﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;

namespace Providers
{
    public class Rand
    {
        public static string DateTimeTick()
        {
            return DateTime.Now.Ticks.ToString();
        }


        public static byte[] CreateRandomEntropy(int size)
        {
            byte[] entropy = new byte[size];
            new RNGCryptoServiceProvider().GetBytes(entropy);

            return entropy;
        }

        
        public static byte[] Byte(int size)
        {
            Random rnd = new Random();
            Byte[] b = new Byte[size];
            rnd.NextBytes(b);

            return b;
        }


        public static string Mix()
        {
            int _number = Number();
            string _str = Str();
            return _str + _number;
        }


        public static int Number()
        {
            Random rand = new Random();
            return rand.Next();
        }


        public static string Str()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", "");
            return path;
        }

        
        public static string GeneratePassword(int length, int numberOfNonAlphanumericCharacters)
        {
            if (length > 128 || numberOfNonAlphanumericCharacters > 128)
            {
                Random rand = new Random();
                int _numberOfNonAlphanumericCharacters = rand.Next(1, 127);
                return Membership.GeneratePassword(128, _numberOfNonAlphanumericCharacters);
            }
            else
            {
                return Membership.GeneratePassword(length, numberOfNonAlphanumericCharacters);
            }
        }


        public static string GUID()
        {
            Guid id = Guid.NewGuid();
            return id.ToString();
        }



        public static string CreateToken(string message, string secret)
        {
            secret = secret ?? "";
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }


        public static string Sha512()
        {
            var _key = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + DateTime.Now.Ticks.ToString() + GUID();
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(_key);
            byte[] messageBytes = encoding.GetBytes(_key);
            using (var hmacsha512 = new HMACSHA512(keyByte))
            {
                byte[] hashmessage = hmacsha512.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }

    }
}