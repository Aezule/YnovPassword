using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class PasswordHelper
{

    private static readonly byte[] Key = Convert.FromBase64String("um0wAouKqCVQeNHtCPO4wRCfFb0ALLuxUOrPVYpK6nA=");
    private static readonly byte[] IV = Convert.FromBase64String("qlTojZ9YDsz/u5Ug9cRtnQ==");
    public static string ComputeSha256Hash(string rawData)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            StringBuilder builder = new StringBuilder();
            foreach (var b in bytes)
                builder.Append(b.ToString("x2"));
            return builder.ToString();
        }
    }


    public static string Encrypt(string plainText)
    {
        if (string.IsNullOrEmpty(plainText))
            return plainText;

        using Aes aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;

        using MemoryStream ms = new();
        using CryptoStream cs = new(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
        using (StreamWriter sw = new(cs))
        {
            sw.Write(plainText);
        }
        return Convert.ToBase64String(ms.ToArray());
    }

    public static string Decrypt(string cipherText)
    {
        if (string.IsNullOrEmpty(cipherText))
            return cipherText;

        using Aes aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;

        var buffer = Convert.FromBase64String(cipherText);

        using MemoryStream ms = new(buffer);
        using CryptoStream cs = new(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
        using StreamReader sr = new(cs);
        {
            return sr.ReadToEnd();
        }
    }
}
