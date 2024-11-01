using System.Text;

namespace Samples
{
    public static class XorSamples
    {
        public static void Run()
        {
            // Encrypting and decrypting data using the XOR cipher is not considered secure for encrypting data,
            // you should consider using other strong ciphers. This is just a demo.

            Console.WriteLine("XOR sample ====================================================================");

            string helloWorld = "Hello, world!";

            string mySecretKey = $"{Guid.NewGuid().ToString()}-{Guid.NewGuid().ToString()}";
            byte[] key = Encoding.UTF8.GetBytes(mySecretKey);

            byte[] encrypted = Encrypt(Encoding.UTF8.GetBytes(helloWorld), key);

            byte[] decryptedData = Decrypt(encrypted, key);
            string decrypted = Encoding.UTF8.GetString(decryptedData);

            Console.WriteLine($"Original Text: {helloWorld}");
            Console.WriteLine($"Encrypted Text in Base64: {Convert.ToBase64String(encrypted)}");
            Console.WriteLine($"Decrypted Text: {decrypted}");
            Console.WriteLine($"XOR key in Base64: {Convert.ToBase64String(key)}");

            Console.WriteLine("End of XOR sample ====================================================================");
            Console.ReadKey();
        }

        public static byte[] Encrypt(byte[] data, byte[] key)
        {
            byte[] encryptedData = new byte[data.Length];

            for(int i = 0; i < data.Length; i++)
            {
                encryptedData[i] = (byte) (data[i] ^ key[i % key.Length]);
            }

            return encryptedData;
        }

        public static byte[] Decrypt(byte[] encryptedData, byte[] key)
        {
            byte[] data = new byte[encryptedData.Length];

            for(int i = 0; i < encryptedData.Length; i++)
            {
                data[i] = (byte) (encryptedData[i] ^ key[i % key.Length]);
            }

            return data;
        }
    }
}
