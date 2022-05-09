namespace Praksa.BLL.Contracts.Helpers
{
    public interface IEncryptionHelper
    {
        string EncryptStringDES(string strData, string strKey);
        string DecryptStringDES(string strData, string strKey);
        byte[] GenerateKeyDES();
    }
}
