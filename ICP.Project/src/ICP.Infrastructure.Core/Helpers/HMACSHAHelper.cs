using System;
using System.Security.Cryptography;
using System.Text;

namespace ICP.Infrastructure.Core.Helpers
{
    public class HMACSHAHelper
    {
        /// <summary>
        /// HMACSHA並轉換Base64String
        /// </summary>
        /// <param name="hashKey"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string HMACSHA1Base64(string hashKey, string data)
        {
            byte[] key = Encoding.UTF8.GetBytes(hashKey);
            HMACSHA1 sha1 = new HMACSHA1(key);
            byte[] source = Encoding.UTF8.GetBytes(data);
            byte[] crypto = sha1.ComputeHash(source);
            return Convert.ToBase64String(crypto);
        }

        /// <summary>
        /// HMACSHA256並轉換Base64String
        /// </summary>
        /// <param name="hashKey"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string HMACSHA256Base64(string hashKey, string data)
        {
            byte[] key = Encoding.UTF8.GetBytes(hashKey);
            HMACSHA256 sha256 = new HMACSHA256(key);
            byte[] source = Encoding.UTF8.GetBytes(data);
            byte[] crypto = sha256.ComputeHash(source);
            return Convert.ToBase64String(crypto);
        }
    }
}