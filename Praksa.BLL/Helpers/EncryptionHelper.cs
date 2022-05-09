using Praksa.BLL.Contracts.Helpers;
using Praksa.BLL.DTOs;
using System.Security.Cryptography;
using System.Text;

namespace Praksa.BLL.Helpers
{
    public class EncryptionHelper : IEncryptionHelper
    {
        public string EncryptStringDES(string strData, string strKey)
        {
            byte[] key = { }; //Encryption Key   
            byte[] IV = { 10, 20, 30, 40, 50, 60, 70, 80 };
            byte[] inputByteArray;
            int a = strKey.Length;

            key = Convert.FromBase64String(strKey);

            DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();

            inputByteArray = Encoding.ASCII.GetBytes(strData);

            MemoryStream memoryStream = new MemoryStream();

            CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(key, IV), CryptoStreamMode.Write);

            cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
            cryptoStream.FlushFinalBlock();

            return Convert.ToBase64String(memoryStream.ToArray());//encrypted string  

        }
        public string DecryptStringDES(string strData, string strKey)
        {
            byte[] key = { };// Key   
            byte[] IV = { 10, 20, 30, 40, 50, 60, 70, 80 };
            byte[] inputByteArray = new byte[strData.Length];

            key = Convert.FromBase64String(strKey);

            DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();

            inputByteArray = Convert.FromBase64String(strData);

            MemoryStream memoryStream = new MemoryStream();

            CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateDecryptor(key, IV), CryptoStreamMode.Write);

            cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
            cryptoStream.FlushFinalBlock();

            Encoding encoding = Encoding.UTF8;
            return encoding.GetString(memoryStream.ToArray());

        }

        public byte[] GenerateKeyDES()
        {
            return RandomNumberGenerator.GetBytes(8);
        }
    }
}
