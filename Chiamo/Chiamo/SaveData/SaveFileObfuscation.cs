using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MiffTheFox.Chiamo.SaveData
{
    // This encryption is awful, but truely encrypting save files
    // would not be feasable. That's why it's obfuscation and not
    // encryption!

    internal class SaveFileObfuscation
    {
        private static readonly byte[] _StaticSalt = new byte[] { 254, 81, 170, 249, 5, 168, 143, 68 };

        private readonly SymmetricAlgorithm _Algo;
        //private readonly byte[] _Key;

        public string KeyAsString { get; internal set; }

        public SaveFileObfuscation(string key)
        {
            KeyAsString = key;
            var derive = new Rfc2898DeriveBytes(key, _StaticSalt);
            //_Key = derive.GetBytes(16);

            _Algo = Aes.Create();
            _Algo.Key = derive.GetBytes(_Algo.KeySize / 8);
            _Algo.IV = derive.GetBytes(_Algo.BlockSize / 8);
        }

        public string Obfuscate(string json)
        {
            byte[] jsonbin = System.Text.Encoding.UTF8.GetBytes(json);
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, _Algo.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(jsonbin, 0, jsonbin.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public bool TryDeobfuscate(string cyphertext, out string plaintext)
        {
            try
            {
                using (var msIn = new MemoryStream(Convert.FromBase64String(cyphertext)))
                {
                    using (var msOut = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(msOut, _Algo.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            msIn.CopyTo(cs);
                        }
                        plaintext = Encoding.UTF8.GetString(msOut.ToArray());
                    }
                }
                return true;
            }
            catch
            {
                plaintext = null;
                return false;
            }
        }
    }
}