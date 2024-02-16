using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.TinyURL.Repositories
{
    
    public class ConfigRepository : BaseConfigRepository
    {
        public string TinyURLHashKey
        {
            get
            {
                return GetKeyApiValue("TinyURLHashKey");
            }
        }

        public string TinyURLHashIV
        {
            get
            {
                return GetKeyApiValue("TinyURLHashIV");
            }
        }

    }
}
