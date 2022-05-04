using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ketvirtasPraktinis
{
    class PasswordManager
    {
        private static string filePath = "passwords.txt";

        public static List<Password> ReadFile()
        {
            string fileText;
            using (StreamReader reader = new StreamReader(filePath))
            {
                fileText = reader.ReadToEnd();
            }
            if (String.IsNullOrWhiteSpace(fileText))
                return new List<Password>();
            string[] encryptedPasswords = fileText.Split(';');
            List<Password> passwords = new List<Password>();
            CipherAES aes = new CipherAES();
            int id = 1;
            for (int i = 0; i < encryptedPasswords.Length; i += 4)
            {
                Password psw = new Password();
                psw.id = id;
                id++;
                psw.title = aes.Decrypt(encryptedPasswords[i]);
                psw.password = encryptedPasswords[i + 1];
                psw.url = aes.Decrypt(encryptedPasswords[i + 2]);
                psw.notes = aes.Decrypt(encryptedPasswords[i + 3]);
                passwords.Add(psw);
            }
            return passwords;
        }

        public static void WriteToFile(List<Password> passwords)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                CipherAES aes = new CipherAES();
                for (int i = 0; i < passwords.Count; i++)
                {
                    Password psw = passwords[i];
                    writer.Write(aes.Encrypt(psw.title) + ";");
                    writer.Write(psw.password + ";");
                    writer.Write(aes.Encrypt(psw.url) + ";");
                    writer.Write(aes.Encrypt(psw.notes));
                    if (i != passwords.Count - 1)
                        writer.Write(";");
                }
            }
        }

        public static string GeneratePassword()
        {
            int length = 12;
            using (RNGCryptoServiceProvider cryptRNG = new RNGCryptoServiceProvider())
            {
                byte[] tokenBuffer = new byte[length];
                cryptRNG.GetBytes(tokenBuffer);
                return Convert.ToBase64String(tokenBuffer);
            }
        }
    }
}
