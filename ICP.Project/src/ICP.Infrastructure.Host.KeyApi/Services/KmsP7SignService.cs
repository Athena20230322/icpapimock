using System;
using ICashKms.Crypto;
using ICashKms.Common;
using ICashKms.Utility;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ICP.Infrastructure.Host.KeyApi.Services
{
    /// <summary>
    /// 簽章
    /// </summary>
    class KmsP7SignService
    {
        private IASymWeb2Worker pGAsymWeb2Worker;  //transient
        private ASymWorkerP7 aSymWorkerPublic; // for p7 verify
        private IHexConverter hexConverter;

        private readonly string _bankCode;
        private readonly string _keyLabel;
        private readonly string _certId;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode">銀行代碼</param>
        /// <param name="keyLabel">key標籤</param>
        /// <param name="certId">憑證編號</param>
        public KmsP7SignService(string bankCode, string keyLabel, string certId)
        {
            _bankCode = bankCode;
            _keyLabel = keyLabel;
            _certId = certId;

            InitContext();
        }

        /// <summary>
        /// 初始
        /// </summary>
        private void InitContext()
        {
            hexConverter = new HexConverter();
            //
            var kms2ConfigReader = new Kms2ConfigReader(
                assemblyName: "ICashKms.Crypto",
                cfgFileName: "ICashKms.Crypto.Config.HttpICARemoteTest.xml"
            );
            var esKmsWebApi = new EsKmsWebApiPublicKey
            (
                kms2ConfigReader: kms2ConfigReader,
                hexConverter: hexConverter,
                connectionPool: new ConnectionSingle()
            );
            //
            this.pGAsymWeb2Worker = new ASymWeb2Worker(esCryptor: esKmsWebApi, keyLabel: _keyLabel);

            this.aSymWorkerPublic = new ASymWorkerP7(); // self contained 
        }

        /// <summary>
        /// 簽章
        /// </summary>
        /// <param name="rawData">欲簽章的字串</param>
        /// <returns></returns>
        public string Sign(string rawData)
        {
            string result = string.Empty;

            if (_bankCode == "013")
            {
                // Sign Data UTF-16LE
                byte[] signData = Encoding.GetEncoding("UTF-16LE").GetBytes(rawData);
                //
                byte[] signature = this.pGAsymWeb2Worker.SignP7(signData, _certId);
                //
                string signBase64 = Convert.ToBase64String(signature);

                // do verify
                bool verify = this.aSymWorkerPublic.Verify(signature, null);

                result = $"-----BEGIN PKCS #7-----\r\n{signBase64}\r\n-----END PKCS #7-----";
            }
            else
            {
                string hexData = this.hexConverter.Str2Hex(rawData);

                // Sign Data Hex
                byte[] signData = this.hexConverter.Hex2Bytes(hexData);
                //
                byte[] signature = this.pGAsymWeb2Worker.SignP7(signData, _certId);
                //
                string signBase64 = Convert.ToBase64String(signature);

                // do verify
                bool verify = this.aSymWorkerPublic.Verify(signature, null);

                result = signBase64;
            }

            return result;
        }

        /// <summary>
        /// 驗證簽章
        /// </summary>
        /// <param name="signDataHex">16進位的簽章字串</param>
        /// <returns></returns>
        public bool VerifySign(string signDataHex)
        {
            byte[] signData = this.hexConverter.Hex2Bytes(signDataHex);

            byte[] signature = this.pGAsymWeb2Worker.SignP7(signData, _certId);

            bool verify = this.aSymWorkerPublic.Verify(signature, null);

            return verify;
        }

    }
}
