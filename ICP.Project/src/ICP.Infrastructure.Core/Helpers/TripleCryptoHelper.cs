using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Helpers
{
    public class TripleCryptoHelper
    {
        /// <summary>
        /// TripleDES 加密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <param name="cipherMode"></param>
        /// <param name="paddingMode"></param>
        /// <returns></returns>
        public string Encrypt(string data, string key, string iv, CipherMode cipherMode, PaddingMode paddingMode)
        {
            var tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider()
            {
                Mode = cipherMode,
                Padding = paddingMode
            };

            ICryptoTransform crypto = tripleDESCryptoServiceProvider.CreateEncryptor(Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(iv));

            byte[] inputBuffer = HexStrToByteArray(data);
            byte[] resultTripleDes = crypto.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);

            return Convert.ToBase64String(resultTripleDes);
        }

        /// <summary>
        /// 把 16 進位字串轉換為 byte[]
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        private byte[] HexStrToByteArray(string hexString)
        {
            return Enumerable.Range(0, hexString.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hexString.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
