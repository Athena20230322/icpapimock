using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.OPUploadFileWrite.Repositories
{
    using Library.Repositories.SystemRepositories;

    public class ConfigRepository: BaseConfigRepository
    {
        public string op_client_WriteTempDir
        {
            get
            {
                return GetAppSetting("op:client:WriteTempDir");
            }
        }

        public string op_client_WriteDir
        {
            get
            {
                return GetAppSetting("op:client:WriteDir");
            }
        }

        public string op_client_WriteBackDir
        {
            get
            {
                return GetAppSetting("op:client:WriteBackDir");
            }
        }
    }
}
