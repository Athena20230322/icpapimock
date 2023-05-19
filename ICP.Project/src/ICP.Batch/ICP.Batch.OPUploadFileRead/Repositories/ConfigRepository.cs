using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.OPUploadFileRead.Repositories
{
    using Library.Repositories.SystemRepositories;

    public class ConfigRepository: BaseConfigRepository
    {
        public string op_client_ReadPath
        {
            get
            {
                return GetAppSetting("op:client:ReadPath");
            }
        }

        public string op_client_ReadBackDir
        {
            get
            {
                return GetAppSetting("op:client:ReadBackDir");
            }
        }
    }
}
