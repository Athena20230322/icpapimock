using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.EInvoiceAwardNotify.Repositories
{
    using Library.Repositories.SystemRepositories;

    public class ConfigRepository : BaseConfigRepository
    {
        public string UniformCode
        {
            get { return GetAppSetting("UniformCode"); }
        }

        public string UserName
        {
            get { return GetAppSetting("UserName"); }
        }

        public string Password
        {
            get { return GetAppSetting("Password"); }
        }

        public string Tools
        {
            get { return GetAppSetting("Tools"); }
        }

        public bool DownloadAwardNumber
        {
            get { return GetAppSetting("DownloadAwardNumber") == "1"; }
        }

        public bool DecryptFiles
        {
            get { return GetAppSetting("DecryptFiles") == "1"; }
        }

        public bool DecompressFiles
        {
            get { return GetAppSetting("DecompressFiles") == "1"; }
        }

        public bool SaveToDB
        {
            get { return GetAppSetting("SaveToDB") == "1"; }
        }

        public bool InvoiceAwardCheck
        {
            get { return GetAppSetting("InvoiceAwardCheck") == "1"; }
        }

        public bool SendNotify
        {
            get { return GetAppSetting("SendNotify") == "1"; }
        }

        public bool UpdateUnaward
        {
            get { return GetAppSetting("UpdateUnaward") == "1"; }
        }
    }
}
