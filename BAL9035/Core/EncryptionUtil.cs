using BAL9035.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BAL9035.Core
{
    /*
     * Encryptio Util for Secret Key Encryption and Decryption
     * 
     */
    public class EncryptionUtil
    {

        private static string KEY = GetAppSettings.GetAppSettingsValues().SecretKey; // pick some other 32 chars
        private static byte[] KEY_BYTES = Encoding.UTF8.GetBytes(KEY);

        private static string ESKEY = GetAppSettings.GetAppSettingsValues().ESSecretKey; // pick some other 32 chars
        private static byte[] ESKEY_BYTES = Encoding.UTF8.GetBytes(ESKEY);
        // Enrcypt SecretKey
        public static string Encrypt(string plainText, string keytype = "")
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");

            byte[] encrypted;
            // Create an AesManaged object
            // with the specified key and IV.
            using (Rijndael algorithm = Rijndael.Create())
            {
                if (String.IsNullOrEmpty(keytype))
                    algorithm.Key = KEY_BYTES;
                else
                    algorithm.Key = ESKEY_BYTES;

                // Create a decrytor to perform the stream transform.
                var encryptor = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV);

                // Create the streams used for encryption.
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            // Write IV first
                            msEncrypt.Write(algorithm.IV, 0, algorithm.IV.Length);
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return Convert.ToBase64String(encrypted);
        }
        //Decrypt SecretKey
        public static string Decrypt(string cipherText, string keytype = "")
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an AesManaged object
            // with the specified key and IV.
            using (Rijndael algorithm = Rijndael.Create())
            {
                if (String.IsNullOrEmpty(keytype))
                    algorithm.Key = KEY_BYTES;
                else
                    algorithm.Key = ESKEY_BYTES;

                // Get bytes from input string
                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                {
                    // Read IV first
                    byte[] IV = new byte[16];
                    msDecrypt.Read(IV, 0, IV.Length);

                    // Assign IV to an algorithm
                    algorithm.IV = IV;

                    // Create a decrytor to perform the stream transform.
                    var decryptor = algorithm.CreateDecryptor(algorithm.Key, algorithm.IV);

                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
    }
}