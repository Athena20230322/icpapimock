using ICP.Infrastructure.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Repositories.MemberRepositories
{
    public class MemberConfigCyptRepository: MemberConfigRepository
    {
        AesCryptoHelper _aesCryptoHelper;
        HashCryptoHelper _hashCryptoHelper;
        private Aes256CryptoHelper _aes256CryptoHelper;

        public MemberConfigCyptRepository()
        {
            _aesCryptoHelper = new AesCryptoHelper();
            _hashCryptoHelper = new HashCryptoHelper();
            _aes256CryptoHelper = new Aes256CryptoHelper();
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="text">原文</param>
        /// <param name="getKey">取Key</param>
        /// <param name="getIv">取Iv</param>
        /// <returns></returns>
        private string aesEncrypt(string text, Func<MemberConfigRepository, string> getKey, Func<MemberConfigRepository, string> getIv)
        {
            _aesCryptoHelper.Key = getKey(this);
            _aesCryptoHelper.Iv = getIv(this);
            return _aesCryptoHelper.Encrypt(text);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="ciphertext">密文</param>
        /// <param name="getKey">取Key</param>
        /// <param name="getIv">取Iv</param>
        /// <returns></returns>
        private string aesDecrypt(string ciphertext, Func<MemberConfigRepository, string> getKey, Func<MemberConfigRepository, string> getIv)
        {
            _aesCryptoHelper.Key = getKey(this);
            _aesCryptoHelper.Iv = getIv(this);
            return _aesCryptoHelper.Decrypt(ciphertext);
        }

        /// <summary>
        /// AES256加密
        /// </summary>
        /// <param name="text">原文</param>
        /// <param name="getKey">取Key</param>
        /// <param name="getIv">取Iv</param>
        /// <returns></returns>
        private string aes256Encrypt(string text, Func<MemberConfigRepository, string> getKey)
        {
            _aes256CryptoHelper.Key = getKey(this);
            return _aes256CryptoHelper.AES256Encrypt(text);
        }

        /// <summary>
        /// AES256解密
        /// </summary>
        /// <param name="ciphertext">密文</param>
        /// <param name="getKey">取Key</param>
        /// <param name="getIv">取Iv</param>
        /// <returns></returns>
        private string aes256Decrypt(string ciphertext, Func<MemberConfigRepository, string> getKey)
        {
            _aesCryptoHelper.Key = getKey(this);
            return _aesCryptoHelper.Decrypt(ciphertext);
        }

        /// <summary>
        /// 雜湊(key+原文+iv)
        /// </summary>
        /// <param name="text">原文</param>
        /// <param name="getKey">取Key</param>
        /// <param name="getIv">取Iv</param>
        /// <returns></returns>
        private string hash(string text, Func<MemberConfigRepository, string> getKey, Func<MemberConfigRepository, string> getIv)
        {
            string Key = getKey(this);
            string Iv = getIv(this);
            return _hashCryptoHelper.HashSha256(Key + text + Iv);
        }

        /// <summary>
        /// MD5 雜湊
        /// </summary>
        /// <param name="text"></param>
        /// <param name="getKey"></param>
        /// <param name="getIv"></param>
        /// <returns></returns>
        private string md5(string text, Func<MemberConfigRepository, string> getKey, Func<MemberConfigRepository, string> getIv)
        {
            string Key = getKey(this);
            string Iv = getIv(this);
            return _hashCryptoHelper.HashMD5(Key + text + Iv);
        }

        /// <summary>
        /// 帳號加密
        /// </summary>
        /// <param name="UserCode">帳號</param>
        /// <returns></returns>
        public string Encrypt_UserCode(string UserCode)
        {
            return aesEncrypt(UserCode, t => t.UserCodeHashKey, t => t.UserCodeHashIV);
        }

        /// <summary>
        /// 帳號解密
        /// </summary>
        /// <param name="EncryptUserCode">加密帳號</param>
        /// <returns></returns>
        public string Decrypt_UserCode(string EncryptUserCode)
        {
            return aesDecrypt(EncryptUserCode, t => t.UserCodeHashKey, t => t.UserCodeHashIV);
        }

        /// <summary>
        /// 雜湊密碼
        /// </summary>
        /// <param name="UserPwd">密碼</param>
        /// <returns></returns>
        public string Hash_UserPwd(string UserPwd)
        {
            return hash(UserPwd, t => t.MemberPasswordKey, t => t.MemberPasswordIV);
        }

        /// <summary>
        /// MD5 雜湊出 OpenWallet API Mask 參數
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public string MD5_MemberOpenWalletMask(string Value)
        {
            return md5(Value, t => t.MemberOpenWalletKey, t => t.MemberOpenWalletIV);
        }

        /// <summary>
        /// MD5 雜湊出 客制 OpenWallet API Mask 參數
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public string MD5_CustomOpenWalletMask(string Value)
        {
            return md5(Value, t => t.CustomOpenWalletHashKey, t => t.CustomOpenWalletHashIV);
        }

        /// <summary>
        /// 加密 客制 OpenWallet API EncData 參數
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public string Encrypt_CustomOpenWalletEncData(string Value)
        {
            return aesEncrypt(Value, t => t.CustomOpenWalletAESKey, t => t.CustomOpenWalletAESIV);
        }

        /// <summary>
        /// 解密 客制 OpenWallet API EncData 參數
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public string Decrypt_CustomOpenWalletEncData(string Value)
        {
            return aesDecrypt(Value, t => t.CustomOpenWalletAESKey, t => t.CustomOpenWalletAESIV);
        }

        /// <summary>
        /// 加密 AppRssPush API 字串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Encrypt_AppRssPushData(string value)
        {
            return aes256Encrypt(value, t => t.AppRssAESKey);
        }

        /// <summary>
        /// 雜湊  AppRssPush  API Mask
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string MD5_AppRssPushMask(string value)
        {
            return md5(value, t => t.AppRssFrontKey, t => t.AppRssRearKey);
        }
        
        /// MD5 雜湊出 OP WebUI API Mask 參數
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public string MD5_OPWebUIApiMask(string Value)
        {
            return md5(Value, t => t.OPWebUIApiHashKey, t => t.OPWebUIApiHashIV);
        }

        #region 電子發票相關

        /// <summary>
        /// 加密 電子發票相關字串
        /// </summary>
        /// <returns></returns>
        public string Encrypt_InvoiceData(string value)
        {
            return aesEncrypt(value, t => t.InvoiceDataHashKey, t => t.InvoiceDataHashIV);
        }

        /// <summary>
        /// 將電子發票相關字串解密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string Decrypt_InvoiceData(string value)
        {
            return aesDecrypt(value, t => t.InvoiceDataHashKey, t => t.InvoiceDataHashIV);
        }

        /// <summary>
        /// 加密 電子發票載具驗證碼
        /// </summary>
        /// <param name="UserCode"></param>
        /// <returns></returns>
        public string Encrypt_InvoiceVerification(string UserCode)
        {
            return aesEncrypt(UserCode, t => t.InvoiceVerificationCodeHashKey, t => t.InvoiceVerificationCodeHashIv);
        }

        /// <summary>
        /// 解密 電子發票載具驗證碼
        /// </summary>
        /// <param name="UserCode"></param>
        /// <returns></returns>
        public string Decrypt_InvoiceVerification(string UserCode)
        {
            return aesDecrypt(UserCode, t => t.InvoiceVerificationCodeHashKey, t => t.InvoiceVerificationCodeHashIv);
        }
        #endregion
        /// <summary>
        /// 加密 客制 OpenWallet API EncData 參數
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public string Encrypt_OPWebUIEncData(string Value)
        {
            return aesEncrypt(Value, t => t.OPWebUIApiAESKey, t => t.OPWebUIApiAESIV);
        }

        /// <summary>
        /// 解密 客制 OpenWallet API EncData 參數
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public string Decrypt_OPWebUIEncData(string Value)
        {
            return aesDecrypt(Value, t => t.OPWebUIApiAESKey, t => t.OPWebUIApiAESIV);
        }
    }
}
