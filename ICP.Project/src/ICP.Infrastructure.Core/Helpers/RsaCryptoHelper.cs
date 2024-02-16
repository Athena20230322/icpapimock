using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ICP.Infrastructure.Core.Extensions;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace ICP.Infrastructure.Core.Helpers
{
    public class RsaCryptoHelper
    {
        private string _publicKey = null;
        private string _privateKey = null;

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public string Encrypt(string content)
        {
            byte[] input = Encoding.UTF8.GetBytes(content);
            byte[] output = processCrypto(true, _publicKey, input);
            return Convert.ToBase64String(output);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="base64Content"></param>
        /// <returns></returns>
        public string Decrypt(string base64Content)
        {
            byte[] input = Convert.FromBase64String(base64Content);
            byte[] output = processCrypto(false, _privateKey, input);
            return Encoding.UTF8.GetString(output);
        }

        /// <summary>
        /// 簽章 RSA Signing - SHA256
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public string SignDataWithSha256(string content)
        {
            byte[] input = Encoding.UTF8.GetBytes(content);
            byte[] output = null;

            using (var provider = new RSACryptoServiceProvider())
            {
                provider.FromXmlString(_privateKey);

                using (var sha256 = new SHA256CryptoServiceProvider())
                {
                    output = provider.SignData(input, sha256);
                }
            }

            return Convert.ToBase64String(output);
        }

        /// <summary>
        /// 驗證 RSA Signing - SHA256
        /// </summary>
        /// <param name="base64Content"></param>
        /// <param name="base64Signature"></param>
        /// <returns></returns>
        public bool VerifySignDataWithSha256(string content, string base64Signature)
        {
            byte[] input = Encoding.UTF8.GetBytes(content);
            byte[] inputSignature = Convert.FromBase64String(base64Signature);

            using (var provider = new RSACryptoServiceProvider())
            {
                provider.FromXmlString(_publicKey);

                using (var sha256 = new SHA256CryptoServiceProvider())
                {
                    return provider.VerifyData(input, sha256, inputSignature);
                }
            }
        }

        /// <summary>
        /// 產生 RSA 金鑰(Pem)
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public (string PublicKey, string PrivateKey) GeneratePemKey(int size = 2048)
        {
            var key = GenerateXmlKey(size);
            using (var provider = new RSACryptoServiceProvider())
            {
                provider.FromXmlString(key.PrivateKey);

                string publicKey = exportPublicKey(provider);
                string privateKey = exportPrivateKey(provider);

                return (publicKey, privateKey);
            }
        }

        /// <summary>
        /// 產生 RSA 金鑰(Xml)
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public (string PublicKey, string PrivateKey) GenerateXmlKey(int size = 2048)
        {
            using (var provider = new RSACryptoServiceProvider(size))
            {
                string publicKey = provider.ToXmlString(false);
                string privateKey = provider.ToXmlString(true);

                return (publicKey, privateKey);
            }
        }

        /// <summary>
        /// 匯入 Private XML 金鑰
        /// </summary>
        /// <param name="xml"></param>
        public void ImportXmlPrivateKey(string xml)
        {
            _privateKey = xml;
        }

        /// <summary>
        /// 匯入 Public XML 金鑰
        /// </summary>
        /// <param name="xml"></param>
        public void ImportXmlPublicKey(string xml)
        {
            _publicKey = xml;
        }

        /// <summary>
        /// Import OpenSSH PEM private key string into MS RSACryptoServiceProvider
        /// </summary>
        /// <param name="pem"></param>
        /// <returns></returns>
        public void ImportPemPrivateKey(string pem)
        {
            pem = $"-----BEGIN RSA PRIVATE KEY-----{Environment.NewLine + pem + Environment.NewLine}-----END RSA PRIVATE KEY-----";

            PemReader pr = new PemReader(new StringReader(pem));
            AsymmetricCipherKeyPair KeyPair = (AsymmetricCipherKeyPair)pr.ReadObject();
            RSAParameters rsaParams = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)KeyPair.Private);

            using (var provider = new RSACryptoServiceProvider())
            {
                provider.ImportParameters(rsaParams);
                _privateKey = provider.ToXmlString(true);
                _publicKey = provider.ToXmlString(false);
            }
        }

        /// <summary>
        /// Import OpenSSH PEM public key string into MS RSACryptoServiceProvider
        /// </summary>
        /// <param name="pem"></param>
        /// <returns></returns>
        public void ImportPemPublicKey(string pem)
        {
            // iOS PKCS#1
            if (pem.Length == 360)
            {
                pem = $"-----BEGIN RSA PUBLIC KEY-----{Environment.NewLine + pem + Environment.NewLine}-----END RSA PUBLIC KEY-----";
            }
            // PKCS#8
            else
            {
                pem = $"-----BEGIN PUBLIC KEY-----{Environment.NewLine + pem + Environment.NewLine}-----END PUBLIC KEY-----";
            }

            PemReader pr = new PemReader(new StringReader(pem));
            AsymmetricKeyParameter publicKey = (AsymmetricKeyParameter)pr.ReadObject();
            RSAParameters rsaParams = DotNetUtilities.ToRSAParameters((RsaKeyParameters)publicKey);

            using (var provider = new RSACryptoServiceProvider())
            {
                provider.ImportParameters(rsaParams);
                _publicKey = provider.ToXmlString(false);
            }
        }

        /// <summary>
        /// Export private (including public) key from MS RSACryptoServiceProvider into OpenSSH PEM string
        /// slightly modified from https://stackoverflow.com/a/23739932/2860309
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        private string exportPrivateKey(RSACryptoServiceProvider provider)
        {
            using (var sw = new StringWriter())
            {
                if (provider.PublicOnly)
                {
                    throw new ArgumentException("CSP does not contain a private key", "csp");
                }

                var parameters = provider.ExportParameters(true);
                using (var ms = new MemoryStream())
                {
                    using (var bw = new BinaryWriter(ms))
                    {
                        bw.Write((byte)0x30); // SEQUENCE
                        using (var innerMS = new MemoryStream())
                        {
                            using (var innerBW = new BinaryWriter(innerMS))
                            {
                                encodeIntegerBigEndian(innerBW, new byte[] { 0x00 }); // Version
                                encodeIntegerBigEndian(innerBW, parameters.Modulus);
                                encodeIntegerBigEndian(innerBW, parameters.Exponent);
                                encodeIntegerBigEndian(innerBW, parameters.D);
                                encodeIntegerBigEndian(innerBW, parameters.P);
                                encodeIntegerBigEndian(innerBW, parameters.Q);
                                encodeIntegerBigEndian(innerBW, parameters.DP);
                                encodeIntegerBigEndian(innerBW, parameters.DQ);
                                encodeIntegerBigEndian(innerBW, parameters.InverseQ);

                                int length = (int)innerMS.Length;
                                encodeLength(bw, length);
                                bw.Write(innerMS.GetBuffer(), 0, length);
                            }
                        }

                        char[] base64 = Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length).ToCharArray();
                        // Output as Base64 with lines chopped at 64 characters
                        for (var i = 0; i < base64.Length; i += 64)
                        {
                            sw.Write(base64, i, Math.Min(64, base64.Length - i));
                        }
                    }
                }

                return sw.ToString();
            }
        }

        /// <summary>
        /// Export public key from MS RSACryptoServiceProvider into OpenSSH PEM string
        /// slightly modified from https://stackoverflow.com/a/28407693
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        private string exportPublicKey(RSACryptoServiceProvider provider)
        {
            using (var sw = new StringWriter())
            {
                var parameters = provider.ExportParameters(false);
                using (var ms = new MemoryStream())
                {
                    using (var bw = new BinaryWriter(ms))
                    {
                        bw.Write((byte)0x30); // SEQUENCE
                        using (var innerMS = new MemoryStream())
                        {
                            using (var innerBW = new BinaryWriter(innerMS))
                            {
                                innerBW.Write((byte)0x30); // SEQUENCE
                                encodeLength(innerBW, 13);
                                innerBW.Write((byte)0x06); // OBJECT IDENTIFIER
                                var rsaEncryptionOid = new byte[] { 0x2a, 0x86, 0x48, 0x86, 0xf7, 0x0d, 0x01, 0x01, 0x01 };
                                encodeLength(innerBW, rsaEncryptionOid.Length);
                                innerBW.Write(rsaEncryptionOid);
                                innerBW.Write((byte)0x05); // NULL
                                encodeLength(innerBW, 0);
                                innerBW.Write((byte)0x03); // BIT STRING
                                using (var bitStringStream = new MemoryStream())
                                {
                                    using (var bitStringWriter = new BinaryWriter(bitStringStream))
                                    {
                                        bitStringWriter.Write((byte)0x00); // # of unused bits
                                        bitStringWriter.Write((byte)0x30); // SEQUENCE
                                        using (var paramsStream = new MemoryStream())
                                        {
                                            using (var paramsWriter = new BinaryWriter(paramsStream))
                                            {
                                                encodeIntegerBigEndian(paramsWriter, parameters.Modulus); // Modulus
                                                encodeIntegerBigEndian(paramsWriter, parameters.Exponent); // Exponent

                                                int paramsLength = (int)paramsStream.Length;
                                                encodeLength(bitStringWriter, paramsLength);
                                                bitStringWriter.Write(paramsStream.GetBuffer(), 0, paramsLength);
                                            }
                                        }

                                        var bitStringLength = (int)bitStringStream.Length;
                                        encodeLength(innerBW, bitStringLength);
                                        innerBW.Write(bitStringStream.GetBuffer(), 0, bitStringLength);
                                    }
                                }

                                int length = (int)innerMS.Length;
                                encodeLength(bw, length);
                                bw.Write(innerMS.GetBuffer(), 0, length);
                            }
                        }

                        char[] base64 = Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length).ToCharArray();
                        for (var i = 0; i < base64.Length; i += 64)
                        {
                            sw.Write(base64, i, Math.Min(64, base64.Length - i));
                        }
                    }
                }

                return sw.ToString();
            }
        }

        /// <summary>
        /// https://stackoverflow.com/a/23739932/2860309
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="length"></param>
        private void encodeLength(BinaryWriter stream, int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length", "Length must be non-negative");
            }

            if (length < 0x80)
            {
                // Short form
                stream.Write((byte)length);
            }
            else
            {
                // Long form
                var temp = length;
                var bytesRequired = 0;
                while (temp > 0)
                {
                    temp >>= 8;
                    bytesRequired++;
                }
                stream.Write((byte)(bytesRequired | 0x80));
                for (var i = bytesRequired - 1; i >= 0; i--)
                {
                    stream.Write((byte)(length >> (8 * i) & 0xff));
                }
            }
        }

        /// <summary>
        /// https://stackoverflow.com/a/23739932/2860309
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        /// <param name="forceUnsigned"></param>
        private void encodeIntegerBigEndian(BinaryWriter stream, byte[] value, bool forceUnsigned = true)
        {
            stream.Write((byte)0x02); // INTEGER
            var prefixZeros = 0;
            for (var i = 0; i < value.Length; i++)
            {
                if (value[i] != 0) break;
                prefixZeros++;
            }
            if (value.Length - prefixZeros == 0)
            {
                encodeLength(stream, 1);
                stream.Write((byte)0);
            }
            else
            {
                if (forceUnsigned && value[prefixZeros] > 0x7f)
                {
                    // Add a prefix zero to force unsigned if the MSB is 1
                    encodeLength(stream, value.Length - prefixZeros + 1);
                    stream.Write((byte)0);
                }
                else
                {
                    encodeLength(stream, value.Length - prefixZeros);
                }
                for (var i = prefixZeros; i < value.Length; i++)
                {
                    stream.Write(value[i]);
                }
            }
        }

        private byte[] processCrypto(bool isEncrypt, string key, byte[] input)
        {
            using (var provider = new RSACryptoServiceProvider())
            {
                provider.FromXmlString(key);

                // 1024的密鑰長度加密上限為117，2048的為245
                int bufferSize = (provider.KeySize / 8);
                if (isEncrypt)
                {
                    bufferSize -= 11;
                }

                byte[] buffer = new byte[bufferSize];

                // 分段處理
                using (var inputMS = new MemoryStream(input))
                {
                    using (var ouputMS = new MemoryStream())
                    {
                        while (true)
                        {
                            int readLine = inputMS.Read(buffer, 0, bufferSize);
                            if (readLine <= 0)
                            {
                                break;
                            }

                            byte[] temp = new byte[readLine];
                            Array.Copy(buffer, 0, temp, 0, readLine);

                            byte[] crypt = null;
                            if (isEncrypt)
                            {
                                crypt = provider.Encrypt(temp, false);
                            }
                            else
                            {
                                crypt = provider.Decrypt(temp, false);
                            }

                            ouputMS.Write(crypt, 0, crypt.Length);
                        }

                        return ouputMS.ToArray();
                    }
                }
            }
        }

        #region 還原公鑰
        /// <summary>
        /// This helper function parses an RSA private key using the ASN.1 format
        /// </summary>
        /// <param name="privateKeyBytes">Byte array containing PEM string of private key.</param>
        /// <returns>An instance of <see cref="RSACryptoServiceProvider"/> rapresenting the requested private key.
        /// Null if method fails on retriving the key.</returns>
        public RSACryptoServiceProvider DecodeRsaPublicKey(string publicKey)
        {
            return DecodeRsaPublicKey(GetBytesFromPEM(publicKey));
        }
        public RSACryptoServiceProvider DecodeRsaPublicKey(byte[] publicKeyBytes)   // PKCS1
        {
            MemoryStream ms = new MemoryStream(publicKeyBytes);
            BinaryReader rd = new BinaryReader(ms);
            try
            {
                byte byteValue;
                ushort shortValue;

                shortValue = rd.ReadUInt16();

                switch (shortValue)
                {
                    case 0x8130:
                        // If true, data is little endian since the proper logical seq is 0x30 0x81
                        rd.ReadByte(); //advance 1 byte
                        break;
                    case 0x8230:
                        rd.ReadInt16();  //advance 2 bytes
                        break;
                    default:
                        //Debug.Assert(false);     // Improper ASN.1 format
                        return null;
                }

                shortValue = rd.ReadUInt16();
                if (shortValue != 0x0102) // (version number)
                {
                    //Debug.Assert(false);     // Improper ASN.1 format, unexpected version number
                    return null;
                }

                byteValue = rd.ReadByte();
                if (byteValue != 0x00)
                {
                    //Debug.Assert(false);     // Improper ASN.1 format
                    return null;
                }

                // The data following the version will be the ASN.1 data itself, which in our case
                // are a sequence of integers.

                // In order to solve a problem with instancing RSACryptoServiceProvider
                // via default constructor on .net 4.0 this is a hack
                CspParameters parms = new CspParameters();
                parms.Flags = CspProviderFlags.NoFlags;
                parms.KeyContainerName = Guid.NewGuid().ToString().ToUpperInvariant();
                parms.ProviderType = ((Environment.OSVersion.Version.Major > 5) || ((Environment.OSVersion.Version.Major == 5) && (Environment.OSVersion.Version.Minor >= 1))) ? 0x18 : 1;

                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(parms);
                RSAParameters rsAparams = new RSAParameters();

                rsAparams.Modulus = rd.ReadBytes(DecodeIntegerSize(rd));

                // Argh, this is a pain.  From emperical testing it appears to be that RSAParameters doesn't like byte buffers that
                // have their leading zeros removed.  The RFC doesn't address this area that I can see, so it's hard to say that this
                // is a bug, but it sure would be helpful if it allowed that. So, there's some extra code here that knows what the
                // sizes of the various components are supposed to be.  Using these sizes we can ensure the buffer sizes are exactly
                // what the RSAParameters expect.  Thanks, Microsoft.
                RSAParameterTraits traits = new RSAParameterTraits(rsAparams.Modulus.Length * 8);

                rsAparams.Modulus = AlignBytes(rsAparams.Modulus, traits.size_Mod);
                rsAparams.Exponent = AlignBytes(rd.ReadBytes(DecodeIntegerSize(rd)), traits.size_Exp);
                //rsAparams.D = Helpers.AlignBytes(rd.ReadBytes(Helpers.DecodeIntegerSize(rd)), traits.size_D);
                //rsAparams.P = Helpers.AlignBytes(rd.ReadBytes(Helpers.DecodeIntegerSize(rd)), traits.size_P);
                //rsAparams.Q = Helpers.AlignBytes(rd.ReadBytes(Helpers.DecodeIntegerSize(rd)), traits.size_Q);
                //rsAparams.DP = Helpers.AlignBytes(rd.ReadBytes(Helpers.DecodeIntegerSize(rd)), traits.size_DP);
                //rsAparams.DQ = Helpers.AlignBytes(rd.ReadBytes(Helpers.DecodeIntegerSize(rd)), traits.size_DQ);
                //rsAparams.InverseQ = Helpers.AlignBytes(rd.ReadBytes(Helpers.DecodeIntegerSize(rd)), traits.size_InvQ);

                rsa.ImportParameters(rsAparams);
                return rsa;
            }
            catch (Exception e)
            {
                //Debug.Assert(false);
                return null;
            }
            finally
            {
                rd.Close();
            }
        }
        public RSACryptoServiceProvider DecodeX509PublicKey(string publicKey)
        {
            return DecodePublicKey(GetBytesFromPEM(publicKey));
        }
        public RSACryptoServiceProvider DecodePublicKey(byte[] publicKeyBytes)       // x509
        {
            MemoryStream ms = new MemoryStream(publicKeyBytes);
            BinaryReader rd = new BinaryReader(ms);
            byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] seq = new byte[15];

            try
            {
                byte byteValue;
                ushort shortValue;

                shortValue = rd.ReadUInt16();

                switch (shortValue)
                {
                    case 0x8130:
                        // If true, data is little endian since the proper logical seq is 0x30 0x81
                        rd.ReadByte(); //advance 1 byte
                        break;
                    case 0x8230:
                        rd.ReadInt16();  //advance 2 bytes
                        break;
                    default:
                        //Debug.Assert(false);     // Improper ASN.1 format
                        return null;
                }

                seq = rd.ReadBytes(15);     //read the Sequence OID
                if (!CompareBytearrays(seq, SeqOID))    //make sure Sequence for OID is correct
                    return null;

                shortValue = rd.ReadUInt16();
                if (shortValue == 0x8103)   //data read as little endian order (actual data order for Bit String is 03 81)
                    rd.ReadByte();  //advance 1 byte
                else if (shortValue == 0x8203)
                    rd.ReadInt16(); //advance 2 bytes
                else
                    return null;

                byteValue = rd.ReadByte();
                if (byteValue != 0x00)
                {
                    //Debug.Assert(false);     // Improper ASN.1 format
                    return null;
                }

                shortValue = rd.ReadUInt16();
                if (shortValue == 0x8130)   //data read as little endian order (actual data order for Sequence is 30 81)
                    rd.ReadByte();  //advance 1 byte
                else if (shortValue == 0x8230)
                    rd.ReadInt16(); //advance 2 bytes
                else
                    return null;

                // The data following the version will be the ASN.1 data itself, which in our case
                // are a sequence of integers.

                // In order to solve a problem with instancing RSACryptoServiceProvider
                // via default constructor on .net 4.0 this is a hack
                CspParameters parms = new CspParameters();
                parms.Flags = CspProviderFlags.NoFlags;
                parms.KeyContainerName = Guid.NewGuid().ToString().ToUpperInvariant();
                parms.ProviderType = ((Environment.OSVersion.Version.Major > 5) || ((Environment.OSVersion.Version.Major == 5) && (Environment.OSVersion.Version.Minor >= 1))) ? 0x18 : 1;

                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(parms);
                RSAParameters rsAparams = new RSAParameters();

                rsAparams.Modulus = rd.ReadBytes(DecodeIntegerSize(rd));

                // Argh, this is a pain.  From emperical testing it appears to be that RSAParameters doesn't like byte buffers that
                // have their leading zeros removed.  The RFC doesn't address this area that I can see, so it's hard to say that this
                // is a bug, but it sure would be helpful if it allowed that. So, there's some extra code here that knows what the
                // sizes of the various components are supposed to be.  Using these sizes we can ensure the buffer sizes are exactly
                // what the RSAParameters expect.  Thanks, Microsoft.
                RSAParameterTraits traits = new RSAParameterTraits(rsAparams.Modulus.Length * 8);

                rsAparams.Modulus = AlignBytes(rsAparams.Modulus, traits.size_Mod);
                rsAparams.Exponent = AlignBytes(rd.ReadBytes(DecodeIntegerSize(rd)), traits.size_Exp);
                //rsAparams.D = Helpers.AlignBytes(rd.ReadBytes(Helpers.DecodeIntegerSize(rd)), traits.size_D);
                //rsAparams.P = Helpers.AlignBytes(rd.ReadBytes(Helpers.DecodeIntegerSize(rd)), traits.size_P);
                //rsAparams.Q = Helpers.AlignBytes(rd.ReadBytes(Helpers.DecodeIntegerSize(rd)), traits.size_Q);
                //rsAparams.DP = Helpers.AlignBytes(rd.ReadBytes(Helpers.DecodeIntegerSize(rd)), traits.size_DP);
                //rsAparams.DQ = Helpers.AlignBytes(rd.ReadBytes(Helpers.DecodeIntegerSize(rd)), traits.size_DQ);
                //rsAparams.InverseQ = Helpers.AlignBytes(rd.ReadBytes(Helpers.DecodeIntegerSize(rd)), traits.size_InvQ);

                rsa.ImportParameters(rsAparams);
                return rsa;
            }
            catch (Exception e)
            {
                //Debug.Assert(false);
                return null;
            }
            finally
            {
                rd.Close();
            }
        }

        internal class RSAParameterTraits
        {
            public RSAParameterTraits(int modulusLengthInBits)
            {
                // The modulus length is supposed to be one of the common lengths, which is the commonly referred to strength of the key,
                // like 1024 bit, 2048 bit, etc.  It might be a few bits off though, since if the modulus has leading zeros it could show
                // up as 1016 bits or something like that.
                int assumedLength = -1;
                double logbase = Math.Log(modulusLengthInBits, 2);
                if (logbase == (int)logbase)
                {
                    // It's already an even power of 2
                    assumedLength = modulusLengthInBits;
                }
                else
                {
                    // It's not an even power of 2, so round it up to the nearest power of 2.
                    assumedLength = (int)(logbase + 1.0);
                    assumedLength = (int)(Math.Pow(2, assumedLength));
                    System.Diagnostics.Debug.Assert(false);  // Can this really happen in the field?  I've never seen it, so if it happens
                                                             // you should verify that this really does the 'right' thing!
                }

                switch (assumedLength)
                {
                    case 512:
                        this.size_Mod = 0x40;
                        this.size_Exp = -1;
                        this.size_D = 0x40;
                        this.size_P = 0x20;
                        this.size_Q = 0x20;
                        this.size_DP = 0x20;
                        this.size_DQ = 0x20;
                        this.size_InvQ = 0x20;
                        break;
                    case 1024:
                        this.size_Mod = 0x80;
                        this.size_Exp = -1;
                        this.size_D = 0x80;
                        this.size_P = 0x40;
                        this.size_Q = 0x40;
                        this.size_DP = 0x40;
                        this.size_DQ = 0x40;
                        this.size_InvQ = 0x40;
                        break;
                    case 2048:
                        this.size_Mod = 0x100;
                        this.size_Exp = -1;
                        this.size_D = 0x100;
                        this.size_P = 0x80;
                        this.size_Q = 0x80;
                        this.size_DP = 0x80;
                        this.size_DQ = 0x80;
                        this.size_InvQ = 0x80;
                        break;
                    case 4096:
                        this.size_Mod = 0x200;
                        this.size_Exp = -1;
                        this.size_D = 0x200;
                        this.size_P = 0x100;
                        this.size_Q = 0x100;
                        this.size_DP = 0x100;
                        this.size_DQ = 0x100;
                        this.size_InvQ = 0x100;
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false); // Unknown key size?
                        break;
                }
            }

            public int size_Mod = -1;
            public int size_Exp = -1;
            public int size_D = -1;
            public int size_P = -1;
            public int size_Q = -1;
            public int size_DP = -1;
            public int size_DQ = -1;
            public int size_InvQ = -1;
        }

        /// <summary>
        /// This helper function parses an integer size from the reader using the ASN.1 format
        /// </summary>
        /// <param name="rd"></param>
        /// <returns></returns>
        int DecodeIntegerSize(BinaryReader rd)
        {
            byte byteValue;
            int count;

            byteValue = rd.ReadByte();
            if (byteValue != 0x02)        // indicates an ASN.1 integer value follows
                return 0;

            byteValue = rd.ReadByte();
            if (byteValue == 0x81)
            {
                count = rd.ReadByte();    // data size is the following byte
            }
            else if (byteValue == 0x82)
            {
                byte hi = rd.ReadByte();  // data size in next 2 bytes
                byte lo = rd.ReadByte();
                count = BitConverter.ToUInt16(new[] { lo, hi }, 0);
            }
            else
            {
                count = byteValue;        // we already have the data size
            }

            //remove high order zeros in data
            while (rd.ReadByte() == 0x00)
            {
                count -= 1;
            }
            rd.BaseStream.Seek(-1, System.IO.SeekOrigin.Current);

            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputBytes"></param>
        /// <param name="alignSize"></param>
        /// <returns></returns>
        byte[] AlignBytes(byte[] inputBytes, int alignSize)
        {
            int inputBytesSize = inputBytes.Length;

            if ((alignSize != -1) && (inputBytesSize < alignSize))
            {
                byte[] buf = new byte[alignSize];
                for (int i = 0; i < inputBytesSize; ++i)
                {
                    buf[i + (alignSize - inputBytesSize)] = inputBytes[i];
                }
                return buf;
            }
            else
            {
                return inputBytes;      // Already aligned, or doesn't need alignment
            }
        }

        enum PEMtypes
        {
            PEM_X509_OLD,
            PEM_X509,
            PEM_X509_PAIR,
            PEM_X509_TRUSTED,
            PEM_X509_REQ_OLD,
            PEM_X509_REQ,
            PEM_X509_CRL,
            PEM_EVP_PKEY,
            PEM_PUBLIC,
            PEM_RSA,
            PEM_RSA_PUBLIC,
            PEM_DSA,
            PEM_DSA_PUBLIC,
            PEM_PKCS7,
            PEM_PKCS7_SIGNED,
            PEM_PKCS8,
            PEM_PKCS8INF,
            PEM_DHPARAMS,
            PEM_SSL_SESSION,
            PEM_DSAPARAMS,
            PEM_ECDSA_PUBLIC,
            PEM_ECPARAMETERS,
            PEM_ECPRIVATEKEY,
            PEM_CMS,
            PEM_SSH2_PUBLIC,
            unknown
        }

        PEMtypes getPEMType(string pemString)
        {
            foreach (PEMtypes d in Enum.GetValues(typeof(PEMtypes)))
            {
                if (pemString.Contains(PEMheader(d)) && pemString.Contains(PEMfooter(d))) return d;
            }
            return PEMtypes.unknown;
        }

        Dictionary<PEMtypes, string> _PEMs;
        Dictionary<PEMtypes, string> PEMs
        {
            get
            {
                if (_PEMs == null)
                {
                    _PEMs = new Dictionary<PEMtypes, string>
                    {
                        { PEMtypes.PEM_X509_OLD , "X509 CERTIFICATE"},
                        { PEMtypes.PEM_X509 , "CERTIFICATE"},
                        { PEMtypes.PEM_X509_PAIR , "CERTIFICATE PAIR"},
                        { PEMtypes.PEM_X509_TRUSTED , "TRUSTED CERTIFICATE"},
                        { PEMtypes.PEM_X509_REQ_OLD , "NEW CERTIFICATE REQUEST"},
                        { PEMtypes.PEM_X509_REQ , "CERTIFICATE REQUEST"},
                        { PEMtypes.PEM_X509_CRL , "X509 CRL"},
                        { PEMtypes.PEM_EVP_PKEY , "ANY PRIVATE KEY"},
                        { PEMtypes.PEM_PUBLIC , "PUBLIC KEY"},
                        { PEMtypes.PEM_RSA , "RSA PRIVATE KEY"},
                        { PEMtypes.PEM_RSA_PUBLIC , "RSA PUBLIC KEY"},
                        { PEMtypes.PEM_DSA , "DSA PRIVATE KEY"},
                        { PEMtypes.PEM_DSA_PUBLIC , "DSA PUBLIC KEY"},
                        { PEMtypes.PEM_PKCS7 , "PKCS7"},
                        { PEMtypes.PEM_PKCS7_SIGNED , "PKCS #7 SIGNED DATA"},
                        { PEMtypes.PEM_PKCS8 , "ENCRYPTED PRIVATE KEY"},
                        { PEMtypes.PEM_PKCS8INF , "PRIVATE KEY"},
                        { PEMtypes.PEM_DHPARAMS , "DH PARAMETERS"},
                        { PEMtypes.PEM_SSL_SESSION , "SSL SESSION PARAMETERS"},
                        { PEMtypes.PEM_DSAPARAMS , "DSA PARAMETERS"},
                        { PEMtypes.PEM_ECDSA_PUBLIC , "ECDSA PUBLIC KEY"},
                        { PEMtypes.PEM_ECPARAMETERS , "EC PARAMETERS"},
                        { PEMtypes.PEM_ECPRIVATEKEY , "EC PRIVATE KEY"},
                        { PEMtypes.PEM_CMS , "CMS"},
                        { PEMtypes.PEM_SSH2_PUBLIC , "SSH2 PUBLIC KEY"},
                        { PEMtypes.unknown , "UNKNOWN"}
                    };
                }
                return _PEMs;
            }
        }

        string PEMheader(PEMtypes p)
        {
            if (p == PEMtypes.PEM_SSH2_PUBLIC)
            {
                return "---- BEGIN " + PEMs[p] + " ----";
            }
            else
            {
                return "-----BEGIN " + PEMs[p] + "-----";
            }
        }
        string PEMfooter(PEMtypes p)
        {
            if (p == PEMtypes.PEM_SSH2_PUBLIC)
            {
                return "---- END " + PEMs[p] + " ----";
            }
            else
            {
                return "-----END " + PEMs[p] + "-----";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pemString"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        byte[] GetBytesFromPEM(string pemString)
        {
            PEMtypes keyType = getPEMType(pemString);
            Dictionary<string, string> extras;
            if (keyType == PEMtypes.unknown) return null;
            return GetBytesFromPEM(pemString, keyType, out extras);
        }
        byte[] GetBytesFromPEM(string pemString, PEMtypes type)
        {
            Dictionary<string, string> extras;
            return GetBytesFromPEM(pemString, type, out extras);
        }
        byte[] GetBytesFromPEM(string pemString, out Dictionary<string, string> extras)
        {
            PEMtypes type = getPEMType(pemString);
            return GetBytesFromPEM(pemString, type, out extras);
        }
        byte[] GetBytesFromPEM(string pemString, PEMtypes type, out Dictionary<string, string> extras)
        {
            extras = new Dictionary<string, string>();
            string header; string footer;
            string data = "";
            header = PEMheader(type);
            footer = PEMfooter(type);

            foreach (string s in pemString.Replace("\r", "").Split('\n'))
            {
                if (s.Contains(":"))
                {
                    extras.Add(s.Substring(0, s.IndexOf(":") - 1), s.Substring(s.IndexOf(":") + 1));
                }
                else
                {
                    if (s != "") data += s + "\n";
                }
            }

            int start = data.IndexOf(header) + header.Length;
            int end = data.IndexOf(footer, start) - start;

            return Convert.FromBase64String(data.Substring(start, end));
        }

        bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }
        #endregion

        public RSACryptoServiceProvider DecodePublicKey(string pem)
        {
            // iOS PKCS#1
            if (pem.Length == 360)
            {
                pem = $"-----BEGIN RSA PUBLIC KEY-----{Environment.NewLine + pem + Environment.NewLine}-----END RSA PUBLIC KEY-----";
            }
            // PKCS#8
            else
            {
                pem = $"-----BEGIN PUBLIC KEY-----{Environment.NewLine + pem + Environment.NewLine}-----END PUBLIC KEY-----";
            }

            PemReader pr = new PemReader(new StringReader(pem));
            AsymmetricKeyParameter publicKey = (AsymmetricKeyParameter)pr.ReadObject();
            RSAParameters rsaParams = DotNetUtilities.ToRSAParameters((RsaKeyParameters)publicKey);

            var provider = new RSACryptoServiceProvider();
            provider.ImportParameters(rsaParams);
            return provider;
        }

        public RSACryptoServiceProvider DecodePrivateKey(string pem)
        {
            pem = $"-----BEGIN RSA PRIVATE KEY-----{Environment.NewLine + pem + Environment.NewLine}-----END RSA PRIVATE KEY-----";

            PemReader pr = new PemReader(new StringReader(pem));
            AsymmetricCipherKeyPair KeyPair = (AsymmetricCipherKeyPair)pr.ReadObject();
            RSAParameters rsaParams = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)KeyPair.Private);

            var provider = new RSACryptoServiceProvider();
            provider.ImportParameters(rsaParams);
            return provider;
        }
    }
}