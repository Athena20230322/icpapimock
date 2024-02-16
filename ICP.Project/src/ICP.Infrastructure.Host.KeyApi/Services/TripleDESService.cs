using ICashKms.Common;
using ICashKms.Crypto;
using ICashKms.Utility;

namespace ICP.Infrastructure.Host.KeyApi.Services
{
    /// <summary>
    /// TripleDES 加解密
    /// </summary>
    class TripleDESService
    {
        private IHexConverter hexConverter;
        private ISymWeb2Worker symWeb2Worker;
        private ISymCryptor desCryptor;
        private IPaddingHelper pkcs7PaddingHelper;

        private readonly string _keyLabel;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyLabel">key標籤</param>
        public TripleDESService(string keyLabel)
        {
            _keyLabel = keyLabel;

            InitContext();
        }

        /// <summary>
        /// 初始
        /// </summary>
        private void InitContext()
        {
            this.hexConverter = new HexConverter();
            //
            var kms2ConfigReader = new Kms2ConfigReader(
                assemblyName: "ICashKms.Crypto",
                cfgFileName: "ICashKms.Crypto.Config.HttpICARemoteTest.xml",
                symMechanism: "DES3"
            );
            var esKmsWebApi = new EsKmsWebApi
            (
                kms2ConfigReader: kms2ConfigReader,
                hexConverter: this.hexConverter,
                connectionPool: new ConnectionSingle()
            );
            var byteWorker = new ByteWorker();
            //
            this.symWeb2Worker = new SymWeb2Worker(esCryptor: esKmsWebApi, keyLabel: _keyLabel);
            //
            this.desCryptor = new DesCryptor(
                alg: "TripleDES",
                cipherMode: "ECB"
            );
            //
            this.pkcs7PaddingHelper = new Pkcs7PaddingHelper(byteWorker: byteWorker, blockSize: 8);
        }

        /// <summary>
        /// 3DES加密
        /// </summary>
        /// <param name="rawData">欲加密的字串</param>
        /// <returns></returns>
        public string Encrypt(string rawData)
        {
            string rawDataHex = hexConverter.Str2Hex(rawData);

            byte[] rawDataBytes = this.hexConverter.Hex2Bytes(rawDataHex);

            // use KMS encrypt            
            byte[] result = this.symWeb2Worker.Encrypt(this.pkcs7PaddingHelper.AddPadding(rawDataBytes));

            return this.hexConverter.Bytes2Hex(result);
        }

        /// <summary>
        /// 3DES加密
        /// </summary>
        /// <param name="rawData">欲加密的字串</param>
        /// <param name="keyHex">16進位的key</param>
        /// <returns></returns>
        public string Encrypt(string rawData, string keyHex)
        {
            string rawDataHex = hexConverter.Str2Hex(rawData);

            byte[] rawDataBytes = this.hexConverter.Hex2Bytes(rawDataHex);

            // use BouncyCastle encrypt
            byte[] key = this.hexConverter.Hex2Bytes(keyHex);
            this.desCryptor.SetKey(key);
            byte[] result = this.desCryptor.Encrypt(this.pkcs7PaddingHelper.AddPadding(rawDataBytes));

            return this.hexConverter.Bytes2Hex(result);
        }

        /// <summary>
        /// 3DES解密
        /// </summary>
        /// <param name="encryptedHex">欲解密的16進位字串</param>
        /// <returns></returns>
        public string Decrypt(string encryptedHex)
        {
            byte[] encrypted = this.hexConverter.Hex2Bytes(encryptedHex);

            // use KMS do decrypt           
            byte[] result = this.pkcs7PaddingHelper.RemovePadding(this.symWeb2Worker.Decrypt(encrypted));

            string rawHex = this.hexConverter.Bytes2Hex(result);

            return this.hexConverter.Hex2Str(rawHex);
        }

        /// <summary>
        /// 3DES解密
        /// </summary>
        /// <param name="encryptedHex">欲解密的16進位字串</param>
        /// <param name="keyHex">16進位的key</param>
        /// <returns></returns>
        public string Decrypt(string encryptedHex, string keyHex)
        {
            byte[] encrypted = this.hexConverter.Hex2Bytes(encryptedHex);

            // use BouncyCastle to decrypt
            byte[] key = this.hexConverter.Hex2Bytes(keyHex);
            this.desCryptor.SetKey(key);
            byte[] result = this.pkcs7PaddingHelper.RemovePadding(this.desCryptor.Decrypt(encrypted));

            string rawHex = this.hexConverter.Bytes2Hex(result);

            return this.hexConverter.Hex2Str(rawHex);
        }

    }
}