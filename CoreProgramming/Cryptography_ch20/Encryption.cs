using System.Security.Cryptography;
using System.Text;

namespace Cryptography_ch20;

public class Encryption
{
    public static string Encrypt(string rawData, byte[] key, byte[] iv)
    {
        byte[] encrypted = Encrypt(Encoding.UTF8.GetBytes(rawData), key, iv);
        return Convert.ToBase64String(encrypted);
    }

    private static byte[] Encrypt(byte[] data, byte[] key, byte[] iv)
    {
        using (Aes aes = Aes.Create())
        using (ICryptoTransform encryptor = aes.CreateEncryptor(key, iv))
        {
            return Crypt(data, encryptor);
        }
    }

    public static string Decrypt(string encryptedData, byte[] key, byte[] iv)
    {
        byte[] decrypted = Decrypt(Convert.FromBase64String(encryptedData), key, iv);
        return Encoding.UTF8.GetString(decrypted);
    }

    private static byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
    {
        using (Aes aes = Aes.Create())
        using (ICryptoTransform decryptor = aes.CreateDecryptor(key, iv))
        {
            return Crypt(data, decryptor);
        }
    }

    private static byte[] Crypt(byte[] data, ICryptoTransform cryptoTransform)
    {
        MemoryStream stream = new MemoryStream();
        using (CryptoStream cryptoStream = new CryptoStream(stream, cryptoTransform, CryptoStreamMode.Write))
        {
            cryptoStream.Write(data, 0, data.Length);
        }

        return stream.ToArray();
    }
}