using System.Security.Cryptography;
using System.Text;

namespace CgvMate.Services;

public abstract class CgvServiceBase
{
    private Aes _aes;
    private static SHA256 _sha256;
    private static MD5 _md5;

    static CgvServiceBase()
    {
        _sha256 = SHA256.Create();
        _md5 = MD5.Create();
    }

    public CgvServiceBase(string iv, string key)
    {
        _aes = Aes.Create();
        _aes.IV = Convert.FromBase64String(iv);
        _aes.Key = Convert.FromBase64String(key);
    }

    protected string Encrypt(string data)
    {
        var bit = Encoding.UTF8.GetBytes(data);
        var encryptResult = _aes.EncryptCbc(bit, _aes.IV);
        return Convert.ToBase64String(encryptResult);
    }

    protected string Decrypt(string data)
    {
        var bit = Convert.FromBase64String(data);
        var decryptResult = _aes.DecryptCbc(bit, _aes.IV);
        return Encoding.UTF8.GetString(decryptResult);
    }

    protected string ComputeSha256Hash(string data)
    {
        return Convert.ToHexString(_sha256.ComputeHash(Encoding.UTF8.GetBytes(data))).ToLower();
    }

    protected string ComputeMD5Hash(string data)
    {
        return Convert.ToHexString(_md5.ComputeHash(Encoding.UTF8.GetBytes(data))).ToLower();
    }
}
