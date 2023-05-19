using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICP.Infrastructure.Core.Helpers;
using ICP.Modules.Api.TinyURL.Repositories;

namespace ICP.Modules.API.TinyURL.Repositories
{
    public class CyptRepository
    {
        AesCryptoHelper _aesCryptoHelper;
        ConfigRepository _configRepository;

        public CyptRepository()
        {
            _aesCryptoHelper = new AesCryptoHelper();
            _configRepository = new ConfigRepository();
        }

        /// AES解密
        /// </summary>
        /// <param name="ciphertext">密文</param>
        /// <param name="getKey">取Key</param>
        /// <param name="getIv">取Iv</param>
        /// <returns></returns>
        public string TinyURL_AesDecrypt(string ciphertext)
        {
            _aesCryptoHelper.Key = _configRepository.TinyURLHashKey;
            _aesCryptoHelper.Iv = _configRepository.TinyURLHashIV;
            return _aesCryptoHelper.Decrypt(ciphertext);
        }

        /// AES加密
        /// </summary>
        /// <param name="ciphertext">密文</param>
        /// <param name="getKey">取Key</param>
        /// <param name="getIv">取Iv</param>
        /// <returns></returns>
        public string TinyURL_AesEncrypt(string ciphertext)
        {
            _aesCryptoHelper.Key = _configRepository.TinyURLHashKey;
            _aesCryptoHelper.Iv = _configRepository.TinyURLHashIV;
            return _aesCryptoHelper.Encrypt(ciphertext);
        }
    }
}
