using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ICP.Modules.Api.AccountLink.Services
{
    public class ChinaTrustSignatureService
    {
        public class CryptKeySize
        {
            /// <summary>
            /// 1024 Key Size
            /// </summary>
            public static CryptKeySize Crypt1024Key = new CryptKeySize(1024);

            /// <summary>
            /// 2048 Key Size
            /// </summary>
            public static CryptKeySize Crypt2048Key = new CryptKeySize(2048);

            /// <summary>
            /// Key Size
            /// </summary>
            private int keySize;

            private CryptKeySize(int keySize)
            {
                this.keySize = keySize;
            }

            /// <summary>
            /// 取得解簽block
            /// </summary>
            /// <returns></returns>
            public int getMaxDecryptBlock()
            {
                return this.keySize / 8;
            }
        }

        /// <summary>
        /// 基本簽驗章物件
        /// </summary>
        public abstract class BaseCrypt
        {
            /// <summary>
            /// 文字編碼，預設使用UTF-8
            /// </summary>
            public Encoding encoding = Encoding.UTF8;

            /// <summary>
            /// key size列舉物件
            /// </summary>
            protected CryptKeySize cryptKeySize;

            /// <summary>
            /// 將hex轉換成byte array
            /// </summary>
            /// <param name="hex"></param>
            /// <returns></returns>
            public byte[] HexToByteArray(String hex)
            {
                int NumberChars = hex.Length;
                byte[] bytes = new byte[NumberChars / 2];
                for (int i = 0; i < NumberChars; i += 2)
                {
                    bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
                }
                return bytes;
            }

            /// <summary>
            /// 使用公鑰驗證簽章值與內容是否相符
            /// </summary>
            /// <param name="signatureHex">簽章值</param>
            /// <param name="rawData">原始資料</param>
            /// <returns></returns>
            protected bool VerifyByPublicKey(String signatureHex, string rawData, AsymmetricKeyParameter publicKeyParameters)
            {
                //將原始資料進行SHA256並HEX
                string rawDataSha256Hex = BitConverter.ToString(SHA256.Create().ComputeHash(encoding.GetBytes(rawData))).Replace("-", string.Empty).ToUpper();
                RsaEngine rsaEngine = new RsaEngine();
                rsaEngine.Init(false, publicKeyParameters); //false=解簽

                byte[] rawSignature = rsaEngine.ProcessBlock(this.HexToByteArray(signatureHex), 0, this.cryptKeySize.getMaxDecryptBlock()); //執行解簽
                //轉換成hex
                string rawSignatureHex = BitConverter.ToString(rawSignature).Replace("-", string.Empty).ToUpper();
                string inputHash = rawSignatureHex.Substring(rawSignatureHex.Length - rawDataSha256Hex.Length); //公鑰解簽資料

                bool result = rawDataSha256Hex.Equals(inputHash); //驗章結果

                return result;
            }
        }

        /// <summary>
        /// 簽驗章元件
        /// </summary>
        [Guid("A9718CB0-A640-4F82-BBC4-98FFAB7C84D3")]
        [ClassInterface(ClassInterfaceType.None)]
        public class RSACoder : BaseCrypt
        {
            /// <summary>
            /// 公私鑰物件
            /// </summary>
            private AsymmetricCipherKeyPair asymmetricCipherKeyPair;

            /// <summary>
            /// 初始化憑證資訊，key size預設為2048
            /// </summary>
            /// <param name="privateKeyContent">私鑰內容(csr格式)</param>
            public RSACoder(string privateKeyContent)
            {
                var textReader = new StringReader(privateKeyContent); //私鑰內容
                this.asymmetricCipherKeyPair = (AsymmetricCipherKeyPair)new PemReader(textReader).ReadObject();
                this.cryptKeySize = CryptKeySize.Crypt2048Key; //key size列舉物件
            }

            /// <summary>
            /// 將原始資料以私鑰key加簽
            /// </summary>
            /// <param name="rawData">原始資料</param>
            /// <returns></returns>
            public string SignByPrivateKey(string rawData)
            {
                //將原始資料進行SHA256並HEX
                string rawDataSha256 = BitConverter.ToString(SHA256.Create().ComputeHash(encoding.GetBytes(rawData))).Replace("-", string.Empty).ToUpper();
                byte[] rawDataSha256HexBytes = this.HexToByteArray(rawDataSha256);
                
                var encryptEngine = new Pkcs1Encoding(new RsaEngine());
                encryptEngine.Init(true, asymmetricCipherKeyPair.Private); //加簽引擎

                //加簽
                byte[] rawSignature = encryptEngine.ProcessBlock(rawDataSha256HexBytes, 0, rawDataSha256HexBytes.Length);
                //轉換成hex
                return BitConverter.ToString(rawSignature).Replace("-", string.Empty).ToUpper();

            }

            /// <summary>
            /// 使用公鑰驗證簽章值與內容是否相符
            /// </summary>
            /// <param name="signatureHex">簽章值</param>
            /// <param name="rawData">原始資料</param>
            /// <returns></returns>
            public bool VerifyByPublicKey(String signatureHex, string rawData)
            {
                return VerifyByPublicKey(signatureHex, rawData, this.asymmetricCipherKeyPair.Public);
            }
        }

        /// <summary>
        /// 中信公鑰驗章元件
        /// </summary>
        [Guid("60CD386F-580B-4924-954C-DAF1A4C6CC64")]
        [ClassInterface(ClassInterfaceType.None)]
        public class SecpCoder : BaseCrypt
        {
            /// <summary>
            /// 中信公鑰物件
            /// </summary>
            private Org.BouncyCastle.Crypto.AsymmetricKeyParameter publicKeyParameters;

            /// <summary>
            /// 初始化中信公鑰，key size預設為2048
            /// </summary>
            /// <param name="publicKeyContent">中信公鑰內容(csr格式)</param>
            public SecpCoder(string publicKeyContent)
            {
                //取得中信公鑰物件
                var textReader = new StringReader(publicKeyContent);
                var reader = new Org.BouncyCastle.OpenSsl.PemReader(textReader);
                var req = reader.ReadObject() as Org.BouncyCastle.Pkcs.Pkcs10CertificationRequest;
                var info = req.GetCertificationRequestInfo();
                this.publicKeyParameters = (AsymmetricKeyParameter)PublicKeyFactory.CreateKey(info.SubjectPublicKeyInfo);

                this.cryptKeySize = CryptKeySize.Crypt2048Key; //key size列舉物件
            }

            /// <summary>
            /// 使用公鑰驗證簽章值與內容是否相符
            /// </summary>
            /// <param name="signatureHex">簽章值</param>
            /// <param name="rawData">原始資料</param>
            /// <returns></returns>
            public bool VerifyByPublicKey(string signatureHex, string rawData)
            {
                return VerifyByPublicKey(signatureHex, rawData, publicKeyParameters);
            }
        }


        /// <summary>
        /// 簽驗章
        /// </summary>
        /// <param name="rawData">原始資料</param>
        public string SignatureData(string rawData)
        {
            // 取得公鑰及私鑰
            string publicKeyPath = $@"D:\ecpay\ICP.Project\src\ICP.Infrastructure.Host.KeyApi\App_Data\icashCert\PublicKey-822.txt";
            string privateKeyPath = $@"D:\ecpay\ICP.Project\src\ICP.Infrastructure.Host.KeyApi\App_Data\icashCert\PrivateKey-822.txt";
            string signData = String.Empty;
            string privateKeyContent = null;        // 私鑰內容
            string publicKeyContent = null;         // 公鑰內容

            if (!System.IO.File.Exists(privateKeyPath) || !System.IO.File.Exists(publicKeyPath))
            {
                return signData;
            }

            privateKeyContent = System.IO.File.ReadAllText(privateKeyPath);
            //publicKeyContent = System.IO.File.ReadAllText(publicKeyPath);

            // 私鑰加簽
            RSACoder rsaCoder = new RSACoder(privateKeyContent);
            string hexSignature = rsaCoder.SignByPrivateKey(rawData);

            // 公鑰驗簽 比對簽章是否正確
            bool result = rsaCoder.VerifyByPublicKey(hexSignature, rawData);

            if (result)
            {
                signData = hexSignature;
            }

            return signData;
        }

    }
}
