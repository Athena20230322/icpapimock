using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Helpers
{
    public class HashCryptoHelper
    {
        /// <summary>
        /// 雜湊 SHA256
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public string HashSha256(string content)
        {
            byte[] input = Encoding.UTF8.GetBytes(content);
            byte[] output = null;

            using (var provider = new SHA256CryptoServiceProvider())
            {
                output = provider.ComputeHash(input);
            }

            //return Convert.ToBase64String(output);
            return BitConverter.ToString(output).Replace("-", string.Empty);
        }

        /// <summary>
        /// 雜湊 SHA1
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public string HashSha1(string content)
        {
            byte[] input = Encoding.UTF8.GetBytes(content);
            byte[] output = null;

            using (var provider = new SHA1CryptoServiceProvider())
            {
                output = provider.ComputeHash(input);
            }

            return BitConverter.ToString(output).Replace("-", string.Empty);
        }

        /// <summary>
        /// 雜湊 MD5 (不建議使用)
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public string HashMD5(string content, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.Default;

            byte[] input = encoding.GetBytes(content);
            byte[] output = null;

            using (var provider = MD5.Create())
            {
                output = provider.ComputeHash(input);
            }

            return BitConverter.ToString(output).Replace("-", string.Empty);
        }
    }
}