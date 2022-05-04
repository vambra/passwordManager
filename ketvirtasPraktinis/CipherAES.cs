using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ketvirtasPraktinis
{
    class CipherAES
    {
        private Aes aes;
        public CipherAES()
        {
            aes = Aes.Create();
            aes.BlockSize = 128;
            byte[] zeroBytes = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            //aes.GenerateIV();
            aes.Padding = PaddingMode.PKCS7;
            aes.FeedbackSize = 8;
            aes.KeySize = zeroBytes.Length * 8;
            aes.Key = zeroBytes;
            aes.Mode = CipherMode.ECB;
        }
        public void setKey(string key)
        {
            aes.KeySize = key.Length * 8;
            aes.Key = Encoding.UTF8.GetBytes(key);
        }

        public void setCipherMode(CipherMode mode)
        {
            aes.Mode = mode;
        }

        public string Encrypt(string plainText)
        {
            try
            {
                byte[] cipherBytes;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        cipherBytes = msEncrypt.ToArray();
                    }
                }
                return Convert.ToBase64String(cipherBytes);
            }
            catch (Exception e)
            {
                return "Error: " + e.Message;
            }
        }

        public string Decrypt(string cipherText)
        {
            try
            {
                string plainText = null;
                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plainText = srDecrypt.ReadToEnd();
                        }
                    }
                }
                return plainText;
            }
            catch (Exception ex) when (ex is FormatException ||
                                       ex is CryptographicException)
            {
                return "Initial text length is invalid";
            }
        }
    }
}
