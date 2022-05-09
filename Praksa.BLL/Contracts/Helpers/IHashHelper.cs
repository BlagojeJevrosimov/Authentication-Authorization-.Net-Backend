namespace Praksa.BLL.Contracts.Helpers
{
    public interface IHashHelper
    {
        string Hash(string password);
        bool CompareHashCodes(string hash1, string hash2);
        string ByteArrayToString(byte[] arrInput);
    }
}
