using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ICP.Infrastructure.Core.Helpers
{
    public class XMLHelper
    {
        public T XMLStr2OB<T>(string XmlStr, string rootName = null, Encoding encoding = null) where T : class, new()
        {
            Type type = typeof(T);

            var result = XMLStr2OB(type, XmlStr, rootName, encoding);

            return result as T;
        }

        public object XMLStr2OB(Type type, string XmlStr, string rootName = null, Encoding encoding = null)
        {
            if (!XmlStr.StartsWith("<?xml"))
                XmlStr = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + XmlStr;

            if (encoding == null) encoding = Encoding.UTF8;

            using (var stream = new System.IO.MemoryStream(encoding.GetBytes(XmlStr)))
            {
                System.Xml.Serialization.XmlSerializer serializer;
                if (string.IsNullOrWhiteSpace(rootName))
                {
                    serializer = new System.Xml.Serialization.XmlSerializer(type);
                }
                else
                {
                    serializer = new System.Xml.Serialization.XmlSerializer(type, new System.Xml.Serialization.XmlRootAttribute(rootName));
                }

                return serializer.Deserialize(stream);
            }
        }

        public string OB2XML(object ob, string rootName = null)
        {
            using (var ms = new System.IO.MemoryStream())
            {
                Encoding encoding;
                using (var writer = XmlWriter.Create(ms))
                {
                    encoding = writer.Settings.Encoding;
                    System.Xml.Serialization.XmlSerializer serializer;
                    if (string.IsNullOrWhiteSpace(rootName))
                    {
                        serializer = new System.Xml.Serialization.XmlSerializer(ob.GetType());
                    }
                    else
                    {
                        serializer = new System.Xml.Serialization.XmlSerializer(ob.GetType(), new System.Xml.Serialization.XmlRootAttribute(rootName));
                    }

                    serializer.Serialize(writer, ob);

                    ms.Position = 0;
                }

                using (var sr = new System.IO.StreamReader(ms, encoding, true))
                {
                    var doc = new XmlDocument();
                    doc.LoadXml(sr.ReadToEnd());
                    doc.DocumentElement.Attributes.RemoveAll();
                    return doc.OuterXml;
                }
            }
        }

        /// <summary>
        /// 驗證 XML 文件的數位簽章
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public BaseResult VerifySign(XmlDocument doc, AsymmetricAlgorithm key, string signTagName)
        {
            var result = new BaseResult();
            result.SetError();

            var nodeList = doc.GetElementsByTagName(signTagName);
            if (nodeList == null || nodeList.Count == 0)
            {
                result.RtnMsg = $"{signTagName} tag is not exists";
                return result;
            }

            var signedXml = new SignedXml(doc);
            signedXml.LoadXml((XmlElement)nodeList[0]);

            try
            {
                if (!signedXml.CheckSignature(key))
                {
                    result.RtnMsg = "Verify sign failure";
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.RtnMsg = "Verify sign failure";
                return result;
            }
            

            result.SetSuccess();
            return result;
        }

        public class RsaPkCs1Sha256SignatureDescription : SignatureDescription
        {
            public RsaPkCs1Sha256SignatureDescription()
            {
                KeyAlgorithm = "System.Security.Cryptography.RSACryptoServiceProvider";
                DigestAlgorithm = "System.Security.Cryptography.SHA256Managed";
                FormatterAlgorithm = "System.Security.Cryptography.RSAPKCS1SignatureFormatter";
                DeformatterAlgorithm = "System.Security.Cryptography.RSAPKCS1SignatureDeformatter";
            }

            public override AsymmetricSignatureDeformatter CreateDeformatter(AsymmetricAlgorithm key)
            {
                var asymmetricSignatureDeformatter = (AsymmetricSignatureDeformatter)CryptoConfig.CreateFromName(DeformatterAlgorithm);
                asymmetricSignatureDeformatter.SetKey(key);
                asymmetricSignatureDeformatter.SetHashAlgorithm("SHA256");
                return asymmetricSignatureDeformatter;
            }
        }

        public XmlDocument SignDocument(XmlDocument doc, X509Certificate2 cert)
        {
            CryptoConfig.AddAlgorithm(typeof(RsaPkCs1Sha256SignatureDescription), @"http://www.w3.org/2001/04/xmldsig-more#rsa-sha256");

            var exportedKeyMaterial = cert.PrivateKey.ToXmlString(true);
            var key = new RSACryptoServiceProvider(new CspParameters(24));
            key.PersistKeyInCsp = false;
            key.FromXmlString(exportedKeyMaterial);

            SignedXml signedXml = new SignedXml(doc);
            signedXml.SigningKey = key;

            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigCanonicalizationUrl;


            KeyInfo keyInfo = new KeyInfo();
            KeyInfoX509Data keyInfoData = new KeyInfoX509Data(cert);

            X509IssuerSerial xserial;

            xserial.IssuerName = cert.Issuer;
            xserial.SerialNumber = cert.SerialNumber;

            keyInfoData.AddIssuerSerial(xserial.IssuerName, xserial.SerialNumber);
            keyInfoData.AddSubjectName(cert.Subject);

            keyInfo.AddClause(keyInfoData);
            signedXml.KeyInfo = keyInfo;

            signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";


            string referenceDigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";


            Reference reference = new Reference();

            reference.Uri = "#sign";
            reference.DigestMethod = referenceDigestMethod;

            signedXml.AddReference(reference);
            signedXml.ComputeSignature();

            XmlElement xmlDigitalSignature = signedXml.GetXml();

            doc.DocumentElement.AppendChild(
                doc.ImportNode(xmlDigitalSignature, true));

            return doc;
        }
    }
}
