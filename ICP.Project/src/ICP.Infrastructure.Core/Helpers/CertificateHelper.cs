using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Helpers
{
    public class CertificateHelper
    {
        #region 取得 StoreName
        /// <summary>
        /// 檔名轉類別
        /// </summary>
        public StoreName GetStoreName(string _storeName)
        {
            StoreName _StoreName = new StoreName();

            if (_storeName == "AddressBook")
            {
                _StoreName = StoreName.AddressBook;
            }
            else if (_storeName == "AuthRoot")
            {
                _StoreName = StoreName.AuthRoot;
            }
            else if (_storeName == "CertificateAuthority")
            {
                _StoreName = StoreName.CertificateAuthority;
            }
            else if (_storeName == "Root")
            {
                _StoreName = StoreName.Root;
            }
            else if (_storeName == "TrustedPeople")
            {
                _StoreName = StoreName.TrustedPeople;
            }
            else if (_storeName == "TrustedPublisher")
            {
                _StoreName = StoreName.TrustedPublisher;
            }
            else if (_storeName == "My")
            {
                _StoreName = StoreName.My;
            }
            return _StoreName;
        }
        #endregion

        #region 取得 Location
        /// <summary>
        /// 取得 Location
        /// </summary>
        public StoreLocation GetLocation(string _storeLocation)
        {
            StoreLocation _StoreLocation = new StoreLocation();

            if (_storeLocation == "CurrentUser")
            {
                _StoreLocation = StoreLocation.CurrentUser;
            }
            else if (_storeLocation == "LocalMachine")
            {
                _StoreLocation = StoreLocation.LocalMachine;
            }

            return _StoreLocation;
        }
        #endregion

        public X509Certificate2 GetLocalCert(string StoreName, string StoreLocation, string CertificateSubject, string SerialNumber)
        {
            StringBuilder CertificateSubjectString = new StringBuilder();
            X509Store store = new X509Store(GetStoreName(StoreName), GetLocation(StoreLocation));
            store.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection certCollection = store.Certificates;

            foreach (X509Certificate2 c in certCollection)
            {
                CertificateSubjectString.AppendLine(c.Subject);

                if (c.Subject == CertificateSubject && c.SerialNumber == SerialNumber)
                {
                    return c;
                }
            }

            return null;
        }
    }
}
