using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konscious.Security.Cryptography;

namespace ketvirtasPraktinis
{
    class CipherArgon
    {
        static public string encrypt(string plainText)
        {
            byte[] plainBytes = Encoding.ASCII.GetBytes(plainText);
            Argon2d argon2 = new Argon2d(plainBytes);
            argon2.DegreeOfParallelism = 16;
            argon2.MemorySize = 8192;
            argon2.Iterations = 40;
            var hash = argon2.GetBytes(128);
            string cipherText = Encoding.ASCII.GetString(hash);
            return cipherText;
        }
    }
}
