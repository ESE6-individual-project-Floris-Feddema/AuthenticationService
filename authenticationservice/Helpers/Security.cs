using System;
using System.Security.Cryptography;
using System.Text;

namespace authenticationservice.Helpers
{
    public class Security
    {
        public static string Encrypt(string key, string toEncrypt, bool useHashing = true)
        {
            byte[] resultArray = null;
            var toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
            try
            {
                byte[] keyArray;

                if (useHashing)
                {
                    using var hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                }
                else
                    keyArray = Encoding.UTF8.GetBytes(key);


                using var tdes = new TripleDESCryptoServiceProvider
                {
                    Key = keyArray, 
                    Mode = CipherMode.ECB, 
                    Padding = PaddingMode.PKCS7
                };
                var cTransform = tdes.CreateEncryptor();
                resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            }
            catch (Exception ex)
            {
                // ignored
            }

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string key, string cipherString, bool useHashing = true)
        {
            byte[] resultArray = null;
            var toEncryptArray = Convert.FromBase64String(cipherString);
            try
            {
                byte[] keyArray;

                if (useHashing)
                {
                    using var hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                }
                else
                    keyArray = Encoding.UTF8.GetBytes(key);


                using var tdes = new TripleDESCryptoServiceProvider
                {
                    Key = keyArray, 
                    Mode = CipherMode.ECB, 
                    Padding = PaddingMode.PKCS7
                };

                var cTransform = tdes.CreateDecryptor();
                resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            }
            catch (Exception ex)
            {
                // ignored
            }

            return Encoding.UTF8.GetString(resultArray);
        }
    }
}