using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    class Encrypt
    {
        AesCryptoServiceProvider crypt_provider;
        private byte[] key, iv;

        public Encrypt()
        {
            crypt_provider = new AesCryptoServiceProvider();
            crypt_provider.BlockSize = 128;
            crypt_provider.KeySize = 256;
            crypt_provider.GenerateKey();
            crypt_provider.Mode = CipherMode.CBC;
            crypt_provider.Padding = PaddingMode.PKCS7;


        }
        public void SetKey(byte[] key)
        {
            crypt_provider.Key = key;
        }
        public String Encryption(String clear_text)
        {
            ICryptoTransform transform = crypt_provider.CreateEncryptor();
            byte[] encrypted_bytes = transform.TransformFinalBlock(ASCIIEncoding.ASCII.GetBytes(clear_text), 0, clear_text.Length);
            string str = Convert.ToBase64String(encrypted_bytes);

            return str;
        }
        public String Encryption(String clear_text, Byte[] key, Byte[] IV)
        {
            ICryptoTransform transform = crypt_provider.CreateEncryptor(key, IV);
            byte[] encrypted_bytes = transform.TransformFinalBlock(ASCIIEncoding.ASCII.GetBytes(clear_text), 0, clear_text.Length);
            string str = Convert.ToBase64String(encrypted_bytes);

            return str;
        }
       
        public byte[] GetKey()
        {
            
            return crypt_provider.Key;
        }
        public byte[] GetIV()
        {
            
            return crypt_provider.IV;
        }
        public String Decryption(String cipher_text)
        {
            ICryptoTransform transform = crypt_provider.CreateDecryptor();
            byte[] enc_bytes = Convert.FromBase64String(cipher_text);
            byte[] decrypted_bytes = transform.TransformFinalBlock(enc_bytes, 0, enc_bytes.Length);
            string str = ASCIIEncoding.ASCII.GetString(decrypted_bytes);

            return str;
        }
    }
}
