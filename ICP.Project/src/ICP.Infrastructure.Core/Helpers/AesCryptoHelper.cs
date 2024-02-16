using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Helpers
{
    public class AesCryptoHelper
    {
        public string Key { get; set; }
        public string Iv { get; set; }

        public AesCryptoHelper() { }

        public AesCryptoHelper(string key, string iv)
        {
            Key = key;
            Iv = iv;
        }

        /// <summary>
        /// 產生 AES 金鑰
        /// </summary>
        /// <returns></returns>
        public (string Key, string Iv) GenerateKey(int keyLength = 32, int ivLength = 16)
        {
            string key = generateRandomString(keyLength);
            string iv = generateRandomString(ivLength);
            return (key, iv);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="base64Content"></param>
        /// <returns></returns>
        public string Decrypt(string base64Content)
        {
            return aesEncryptProcess(false, base64Content, Key, Iv);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public string Encrypt(string content)
        {
            return aesEncryptProcess(true, content, Key, Iv);
        }

        private string aesEncryptProcess(bool isEncrypt, string str, string key, string iv)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new Exception("欲加解密字串不能為空");
            }

            byte[] keyData = Encoding.UTF8.GetBytes(key);
            byte[] ivData = Encoding.UTF8.GetBytes(iv);

            byte[] dataByteArray = switchEncryptInput(isEncrypt, str);

            byte[] byteArray = aesProcess(isEncrypt, dataByteArray, keyData, ivData);
            string result = switchEncryptResult(isEncrypt, byteArray);

            return result;
        }

        private byte[] aesProcess(bool isEncrypt, byte[] str, byte[] key, byte[] iv)
        {
            byte[] result = null;

            using (var aes = new AesCryptoServiceProvider())
            {
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform cryptoTransform = null;
                if (isEncrypt)
                {
                    cryptoTransform = aes.CreateEncryptor();
                }
                else
                {
                    cryptoTransform = aes.CreateDecryptor();
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
                    {
                        cs.Write(str, 0, str.Length);
                        cs.FlushFinalBlock();
                        result = ms.ToArray();
                    }
                }

                return result;
            }
        }

        private string switchEncryptResult(bool isEncrypt, byte[] byteArray)
        {
            string result = null;

            if (byteArray != null && byteArray.Length > 0)
            {
                if (isEncrypt)
                {
                    result = Convert.ToBase64String(byteArray);
                }
                else
                {
                    result = Encoding.UTF8.GetString(byteArray);
                }
            }

            return result;
        }

        private byte[] switchEncryptInput(bool isEncrypt, string str)
        {
            byte[] result = null;

            if (!string.IsNullOrWhiteSpace(str))
            {
                if (isEncrypt)
                {
                    result = Encoding.UTF8.GetBytes(str);
                }
                else
                {
                    result = Convert.FromBase64String(str);
                }
            }

            return result;
        }

        private string generateRandomString(int size)
        {
            const string seeds = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var list = Enumerable.Range(0, size)
                                 .Select(x =>
                                 {
                                     var random = new Random(Guid.NewGuid().GetHashCode());
                                     return seeds[random.Next(0, seeds.Length)];
                                 });
            return string.Join(string.Empty, list);
        }
    }
}
