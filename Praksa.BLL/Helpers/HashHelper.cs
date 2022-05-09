using Praksa.BLL.Contracts.Helpers;
using System.Security.Cryptography;
using System.Text;

namespace Praksa.BLL.Helpers
{
    public class HashHelper : IHashHelper
    {
        public string Hash(string password)
        {
            byte[] hashedPassword;

            byte[] bytePassword = Encoding.ASCII.GetBytes(password);

            using (SHA256 mySHA256 = SHA256.Create())
            {
                hashedPassword = mySHA256.ComputeHash(bytePassword);
            }

            return ByteArrayToString(hashedPassword);
        }
        public bool CompareHashCodes(string hash1, string hash2)
        {
            byte[] code1 = Encoding.ASCII.GetBytes(hash1);
            byte[] code2 = Encoding.ASCII.GetBytes(hash2);

            if (code1.Length == code2.Length)
            {
                int i = 0;
                while ((i < code1.Length) && (code1[i] == code2[i]))
                {
                    i += 1;
                }
                if (i == code1.Length)
                {
                    return true;
                }

            }

            return false;
        }
        public string ByteArrayToString(byte[] arrInput)
        {
            StringBuilder sOutput = new StringBuilder(arrInput.Length);

            for (int i = 0; i < arrInput.Length - 1; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }
    }
}
