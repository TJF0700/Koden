#region License
// Copyright (c) 2014 Tim Fischer
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the 'Software'), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Koden.Utils.Encryption
{
    /// <summary>
    /// 
    /// </summary>
    public class Encryption
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Encryption"/> class.
        /// </summary>
        public Encryption() { }
        /// <summary>
        /// Generates the SHA1 hash.
        /// </summary>
        /// <param name="value">The string to generate.</param>
        /// <returns></returns>
        public static string GenerateSHA1Hash(string value)
        {
            return Convert.ToBase64String(new SHA1Managed().ComputeHash(Encoding.Default.GetBytes(value)));
        }

        /// <summary>
        /// Gets the decrypted SHA1 hash data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string GetDecryptedSHA1HashData(string data)
        {
            byte[] decData = Convert.FromBase64String(data);
            string dscString = Encoding.ASCII.GetString(decData);
            return dscString;
        }

        /// <summary>
        /// Generates the SHA256 hash.
        /// </summary>
        /// <param name="value">The string to generate.</param>
        /// <returns></returns>
        public static string GenerateSHA256Hash(string value)
        {
            return Convert.ToBase64String(new SHA256CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(value)));
        }

        /// <summary>
        /// Gets the decrypted SHA256 hash data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string GetDecryptedSHA256HashData(string data)
        {
            byte[] decData = Convert.FromBase64String(data);
            string dscString = Encoding.ASCII.GetString(decData);
            return dscString;
        }


        /// <summary>
        /// Encodes a string to Base64.
        /// </summary>
        /// <param name="plainText">The plain text string.</param>
        /// <returns> string encoded to Base 64</returns>
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Base64 decoding.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string Base64Decode(string data)
        {
            try
            {
                var encoder = new UTF8Encoding();
                var utf8Decode = encoder.GetDecoder();

                byte[] todecodeByte = Convert.FromBase64String(data);
                int charCount = utf8Decode.GetCharCount(todecodeByte, 0, todecodeByte.Length);
                var decodedChar = new char[charCount];
                utf8Decode.GetChars(todecodeByte, 0, todecodeByte.Length, decodedChar, 0);
                var result = new String(decodedChar);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Decode" + e.Message);
            }
        }

        /// <summary>
        /// Gets IP address as a hashed string
        /// </summary>
        /// <param name="ip">The ip address.</param>
        /// <returns></returns>
        public static string GetHashedIP(string ip)
        {
            string[] strArray = ip.Split(new char[1]
              {
                '.'
              });
            return GenerateSHA256Hash(Convert.ToString(Convert.ToInt32(strArray[0]), 2).PadLeft(8, '0') + Convert.ToString(Convert.ToInt32(strArray[1]), 2).PadLeft(8, '0') + Convert.ToString(Convert.ToInt32(strArray[2]), 2).PadLeft(8, '0').Substring(0, 4)).Trim();
        }

        #region TripleDES Encryption (http://www.codeproject.com/Articles/14150/Encrypt-and-Decrypt-Data-with-C)
        /// <summary>
        /// Encrypts a string using TripleDES encryption.
        /// </summary>
        /// <param name="toEncrypt">To encrypt.</param>
        /// <param name="key">The key.</param>
        /// <param name="useHashing">The use hashing.</param>
        /// <param name="iv">The iv.</param>
        /// <param name="cMode">The cipher mode.</param>
        /// <param name="pMode">The padding mode.</param>
        /// <returns></returns>
        public static string TripleDESEncrypt(string toEncrypt, string key, bool useHashing, byte[] iv, CipherMode cMode, PaddingMode pMode)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //Always release the resources and flush data
                // of the Cryptographic service provide. Best Practice
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. 
            tdes.Mode = cMode;
            //padding mode(if any extra byte added)

            tdes.Padding = pMode;

            if (iv != null) tdes.IV = iv;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// Decrypts a TripleDES encrypted string.
        /// </summary>
        /// <param name="cipherString">The cipher string.</param>
        /// <param name="key">The Security key.</param>
        /// <param name="useHashing">use hashing.</param>
        /// <param name="iv">The iv.</param>
        /// <param name="cMode">The cipher mode.</param>
        /// <param name="pMode">The padding mode.</param>
        /// <returns></returns>
        public static string TripleDESDecrypt(string cipherString, string key, bool useHashing, byte[] iv, CipherMode cMode, PaddingMode pMode)
        {
            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            if (useHashing)
            {
                //if hashing was used get the hash code with regards to your key
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //release any resource held by the MD5CryptoServiceProvider

                hashmd5.Clear();
            }
            else
            {
                //if hashing was not implemented get the byte code of the key
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. 
            tdes.Mode = cMode;
            //padding mode(if any extra byte added)
            tdes.Padding = pMode;
            if (iv != null)
                tdes.IV = iv;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        #endregion

        #region DES Encryption (http://www.codeproject.com/Articles/14150/Encrypt-and-Decrypt-Data-with-C)
        /// <summary>
        /// Encrypt a string using DES encryption with a password.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static string DESEncrypt(string message, string password)
        {
            if (string.IsNullOrEmpty(message) ||
                string.IsNullOrEmpty(password)
                 ) return message;

            // Encode message and password
            byte[] messageBytes = ASCIIEncoding.ASCII.GetBytes(message);
            byte[] passwordBytes = ASCIIEncoding.ASCII.GetBytes(password);

            // Set encryption settings -- Use password for both key and init. vector
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            ICryptoTransform transform = provider.CreateEncryptor(passwordBytes, passwordBytes);
            CryptoStreamMode mode = CryptoStreamMode.Write;

            // Set up streams and encrypt
            MemoryStream memStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memStream, transform, mode);
            cryptoStream.Write(messageBytes, 0, messageBytes.Length);
            cryptoStream.FlushFinalBlock();

            // Read the encrypted message from the memory stream
            byte[] encryptedMessageBytes = new byte[memStream.Length];
            memStream.Position = 0;
            memStream.Read(encryptedMessageBytes, 0, encryptedMessageBytes.Length);

            // Encode the encrypted message as base64 string
            string encryptedMessage = Convert.ToBase64String(encryptedMessageBytes);

            return encryptedMessage;
        }

        /// <summary>
        /// Decrypt a DES encrypted message with a password.
        /// </summary>
        /// <param name="encryptedMessage">The encrypted message.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static string DESDecrypt(string encryptedMessage, string password)
        {
            if (string.IsNullOrEmpty(encryptedMessage) ||
                string.IsNullOrEmpty(password)
                ) return encryptedMessage;

            // Convert encrypted message and password to bytes
            byte[] encryptedMessageBytes = Convert.FromBase64String(encryptedMessage);
            byte[] passwordBytes = ASCIIEncoding.ASCII.GetBytes(password);

            // Set encryption settings -- Use password for both key and init. vector
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            ICryptoTransform transform = provider.CreateDecryptor(passwordBytes, passwordBytes);
            CryptoStreamMode mode = CryptoStreamMode.Write;

            // Set up streams and decrypt
            MemoryStream memStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memStream, transform, mode);
            cryptoStream.Write(encryptedMessageBytes, 0, encryptedMessageBytes.Length);
            cryptoStream.FlushFinalBlock();

            // Read decrypted message from memory stream
            byte[] decryptedMessageBytes = new byte[memStream.Length];
            memStream.Position = 0;
            memStream.Read(decryptedMessageBytes, 0, decryptedMessageBytes.Length);

            // Encode deencrypted binary data to base64 string
            //string message = Convert.ToBase64String(decryptedMessageBytes);
            string message = ASCIIEncoding.ASCII.GetString(decryptedMessageBytes);

            return message;
        }
        #endregion

    }
}
