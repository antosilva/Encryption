using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public static class RsaSamples
    {
        public static void Run()
        {
            Console.WriteLine("RSA sample ====================================================================");

            using RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);

            RSAParameters publicKey = rsa.ExportParameters(false);
            RSAParameters privateKey = rsa.ExportParameters(true);

            string helloWorld = "Hello, world!";

            byte[] encrypted = EncryptString(helloWorld, publicKey);
            string decrypted = DecryptString(encrypted, privateKey);

            Console.WriteLine($"Original text: {helloWorld}");
            Console.WriteLine($"Encrypted text in Base64: {Convert.ToBase64String(encrypted)}");
            Console.WriteLine($"Decrypted text: {decrypted}");

            //======================================
            // Export both keys to PEM format.
            //======================================
            byte[] publicKeyBytes = rsa.ExportSubjectPublicKeyInfo();
            string publicKeyPem = "-----BEGIN PUBLIC KEY-----\n" +
                                  Convert.ToBase64String(publicKeyBytes, Base64FormattingOptions.InsertLineBreaks) +
                                  "\n-----END PUBLIC KEY-----";
            Console.WriteLine("Public key in PEM format:\n" + publicKeyPem);

            byte[] privateKeyBytes = rsa.ExportPkcs8PrivateKey();
            string privateKeyPem = "-----BEGIN PRIVATE KEY-----\n" +
                                   Convert.ToBase64String(privateKeyBytes, Base64FormattingOptions.InsertLineBreaks) +
                                   "\n-----END PRIVATE KEY-----";
            Console.WriteLine("Private key in PEM:\n" + privateKeyPem);

            // Now import PEM formats and decrypt again to see if we have the original plain text.
            byte[] importPublicKeyBytes = Convert.FromBase64String(publicKeyPem.Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "").Replace("\n", ""));
            byte[] importPrivateKeyBytes = Convert.FromBase64String(privateKeyPem.Replace("-----BEGIN PRIVATE KEY-----", "").Replace("-----END PRIVATE KEY-----", "").Replace("\n", ""));

            using RSACryptoServiceProvider rsa2 = new RSACryptoServiceProvider();

            // Import PEM.
            rsa2.ImportSubjectPublicKeyInfo(importPublicKeyBytes, out _);
            rsa2.ImportPkcs8PrivateKey(importPrivateKeyBytes, out _);

            // Decrypt again with imported keys.
            string decrypted2 = DecryptString(encrypted, rsa2.ExportParameters(true));
            Console.WriteLine($"Decrypted text with imported key: {decrypted2}");

            Console.WriteLine("End of RSA sample ====================================================================");
            Console.ReadKey();
        }

        public static byte[] EncryptString(string plainText, RSAParameters publicKey)
        {
            using(RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(publicKey);
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] encryptedBytes = rsa.Encrypt(plainBytes, true);

                return encryptedBytes;
            }
        }

        public static string DecryptString(byte[] encryptedBytes, RSAParameters privateKey)
        {
            using(RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(privateKey);
                byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, true);
                string decryptedText = Encoding.UTF8.GetString(decryptedBytes);

                return decryptedText;
            }
        }
    }
}
