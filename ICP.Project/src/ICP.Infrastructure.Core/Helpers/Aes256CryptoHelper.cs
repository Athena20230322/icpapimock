using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ICP.Infrastructure.Core.Helpers
{
    public class Aes256CryptoHelper
    {
        public string Key { get; set; }

        public Aes256CryptoHelper()
        {
        }

        public Aes256CryptoHelper(string key)
        {
            Key = key;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string AES256Encrypt(string value)
		{
			byte[] key = Encoding.UTF8.GetBytes(Key);
			value = Convert.ToBase64String(Encoding.Unicode.GetBytes(value));
			return AESEncrypt(value, key);
		}
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
		public string AES256Decrypt(string value)
		{
			byte[] key = Encoding.UTF8.GetBytes(Key);
			byte[] response = Convert.FromBase64String(AESDecrypt(value, key));
			return Encoding.Unicode.GetString(response, 0, response.Length);
		}

		/// <summary>
		/// AES encrypt
		/// </summary>
		/// <param name="value">要加密的值</param>
		/// <returns></returns>
		private static string AESEncrypt(string value, byte[] key)
		{
			//加密後的Byte
			byte[] encryptedBytes = null;
			//將要加密的值轉成Byte
			byte[] sourceBytes = UTF8Encoding.ASCII.GetBytes(value);

			SHA1 sha1 = new SHA1CryptoServiceProvider();//建立一個SHA1
														//byte[] crypto = sha1.ComputeHash(key);//進行SHA1加密
			byte[] akey = copyOfRange(key, 32);
			byte[] aiv = copyOfRange(key, 16);

			using (MemoryStream ms = new MemoryStream())
			{
				using (RijndaelManaged AES = new RijndaelManaged())
				{
					AES.KeySize = 256;
					AES.BlockSize = 128;
					AES.Key = akey;
					AES.IV = aiv;

					AES.Mode = CipherMode.CBC;
					AES.Padding = PaddingMode.PKCS7;

					using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
					{
						cs.Write(sourceBytes, 0, sourceBytes.Length);
					}

					encryptedBytes = ms.ToArray();
				}
			}

			return Convert.ToBase64String(encryptedBytes, 0, encryptedBytes.Length);
		}

		/// <summary>
		/// AES decrypt
		/// </summary>
		/// <param name="value">加密過的值</param>
		/// <returns>解密後的值</returns>
		private static string AESDecrypt(string value, byte[] key)
		{
			byte[] decryptedBytes = null;
			byte[] toEncryptArray = Convert.FromBase64String(value);

			// Set your salt here, change it to meet your flavor:
			// The salt bytes must be at least 8 bytes.
			SHA1 sha1 = new SHA1CryptoServiceProvider();//建立一個SHA1
														//byte[] crypto = sha1.ComputeHash(key);//進行SHA1加密
			byte[] akey = copyOfRange(key, 32);
			byte[] aiv = copyOfRange(key, 16);

			using (MemoryStream ms = new MemoryStream())
			{
				using (RijndaelManaged AES = new RijndaelManaged())
				{
					AES.KeySize = 256;
					AES.BlockSize = 128;
					AES.Key = akey;
					AES.IV = aiv;

					AES.Mode = CipherMode.CBC;
					AES.Padding = PaddingMode.PKCS7;

					using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
					{
						cs.Write(toEncryptArray, 0, toEncryptArray.Length);
					}

					decryptedBytes = ms.ToArray();
				}
			}

			return UTF8Encoding.UTF8.GetString(decryptedBytes);
		}

		private static byte[] copyOfRange(byte[] src, int len)
		{
			byte[] dest = new byte[len];
			int srclen = src.Length;
			int getlen = len >= srclen ? srclen : len;
			Array.Copy(src, 0, dest, 0, getlen);

			if (len - srclen > 0)
			{
				for (int x = srclen - 1; x < len; x++)
				{
					dest[x] = 0;
				}
			}

			return dest;
		}
    }
}