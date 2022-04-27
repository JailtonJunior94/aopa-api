using System.Text;
using System.Security.Cryptography;

namespace Aopa.Suporte.gRPC.Crypt
{
    public static class CryptExtension
    {
        private static readonly byte[] saltBytes = new byte[] { 1, 2, 3, 5, 8, 13, 21, 34 };

        public static string Encrypt(this string input, string password)
        {
            if (string.IsNullOrEmpty(input)) return null;

            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
            string base64Password = Convert.ToBase64String(passwordBytes);
            byte[] bytesEncrypted = AES256Encrypt(bytesToBeEncrypted, base64Password);

            string result = Convert.ToBase64String(bytesEncrypted);
            return result;
        }

        private static byte[] AES256Encrypt(byte[] bytesToBeEncrypted, string password)
        {
            byte[] encryptedBytes = null;

            using (var ms = new MemoryStream())
            {
                using var AES = Aes.Create("AesManaged");

                AES.KeySize = 256;
                AES.BlockSize = 128;

                Rfc2898DeriveBytes key = new(password, saltBytes, 1000);

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

            return encryptedBytes;
        }

        public static string Decrypt(this string input, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(input)) return null;

                byte[] bytesToBeDecrypted = Convert.FromBase64String(input);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
                string base64Password = Convert.ToBase64String(passwordBytes);

                byte[] bytesDecrypted = AES256Decrypt(bytesToBeDecrypted, base64Password);

                string result = Encoding.UTF8.GetString(bytesDecrypted);
                return result;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private static byte[] AES256Decrypt(byte[] bytesToBeDecrypted, string password)
        {
            byte[] decryptedBytes = null;

            using (var ms = new MemoryStream())
            {
                using var AES = Aes.Create("AesManaged");

                AES.KeySize = 256;
                AES.BlockSize = 128;

                Rfc2898DeriveBytes key = new(password, saltBytes, 1000);

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

            return decryptedBytes;
        }
    }
}
