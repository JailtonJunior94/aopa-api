using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Aopa.Suporte.Business.Infra.Security
{
    public static class SecurityProvider
    {
        private static byte[] saltBytes = new byte[] { 1, 2, 3, 5, 8, 13, 21, 34 };

        public static string Encrypt<T>(this T input, string password)
        {
            if (input == null)
                return null;

            var jsonSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                SerializationBinder = new SerializationBinder(typeof(T))
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(input, jsonSettings);
            var encryptedJson = Encrypt(json, password);
            return encryptedJson;
        }

        public static string Encrypt(this string input, string password)
        {
            if (String.IsNullOrEmpty(input))
                return null;

            // Get the bytes of the string
            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
            string base64Password = Convert.ToBase64String(passwordBytes);


            byte[] bytesEncrypted = AES256Encrypt(bytesToBeEncrypted, base64Password);

            string result = Convert.ToBase64String(bytesEncrypted);

            return result;
        }

        public static string Decrypt(this string input, string password)
        {
            if (String.IsNullOrEmpty(input))
                return null;

            // Get the bytes of the string
            byte[] bytesToBeDecrypted = Convert.FromBase64String(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
            string base64Password = Convert.ToBase64String(passwordBytes);


            byte[] bytesDecrypted = AES256Decrypt(bytesToBeDecrypted, base64Password);

            string result = Encoding.UTF8.GetString(bytesDecrypted);

            return result;
        }

        public static T Decrypt<T>(this string input, string password)
        {
            if (String.IsNullOrEmpty(input))
                return default;

            var decripted = input.Decrypt(password);


            var jsonSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                SerializationBinder = new SerializationBinder(typeof(T))
            };

            T result = default(T);

            try
            {
                result = (T)Newtonsoft.Json.JsonConvert.DeserializeObject(decripted, typeof(T), jsonSettings);
            }
            catch (Exception ex)
            {
                throw new InvalidObjectException<T>(decripted, ex.Message);
            }

            return result;
        }

        private static byte[] AES256Encrypt(byte[] bytesToBeEncrypted, string password)
        {
            byte[] encryptedBytes = null;


            using (var ms = new MemoryStream())
            {
                using (var AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(password, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        private static byte[] AES256Decrypt(byte[] bytesToBeDecrypted, string password)
        {
            byte[] decryptedBytes = null;

            using (var ms = new MemoryStream())
            {
                using (var AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(password, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }
    }
}
