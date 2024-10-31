using System.Security.Cryptography;
using System.Text;

namespace Samples;

public static class AesSamples
{
    public static void Run()
    {
        Console.WriteLine("AES sample ====================================================================");

        string helloWorld = "Hello, World!";

        using Aes aes = Aes.Create();

        aes.KeySize = 256;
        aes.GenerateKey();
        aes.GenerateIV();

        byte[] encrypted = Encrypt(helloWorld, aes.Key, aes.IV);
        string decrypted = Decrypt(encrypted, aes.Key, aes.IV);

        Console.WriteLine($"Original Text: {helloWorld}");
        Console.WriteLine($"Encrypted Text in Base64: {Convert.ToBase64String(encrypted)}");
        Console.WriteLine($"Decrypted Text: {decrypted}");
        Console.WriteLine($"AES key in Base64: {Convert.ToBase64String(aes.Key)}");
        Console.WriteLine($"AES IV in Base64: {Convert.ToBase64String(aes.IV)}");

        Console.WriteLine("End of AES sample ====================================================================");
        Console.ReadKey();
    }

    public static byte[] Encrypt(string plainText, byte[] key, byte[] iv)
    {
        using Aes aes = Aes.Create();

        aes.Key = key;
        aes.IV = iv;

        using var memoryStream = new MemoryStream();
        using var encryptor = aes.CreateEncryptor();
        using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        using var streamWriter = new StreamWriter(cryptoStream);

        streamWriter.Write(plainText);
        streamWriter.Close();

        return memoryStream.ToArray();
    }

    public static string Decrypt(byte[] cipherText, byte[] key, byte[] iv)
    {
        using Aes aes = Aes.Create();

        aes.Key = key;
        aes.IV = iv;

        using var memoryStream = new MemoryStream(cipherText);
        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream);

        return streamReader.ReadToEnd();
    }
}
