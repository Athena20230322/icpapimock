using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Authorization.Models
{
    public class ICPRequestHeaders
    {
        public long EncKeyID { get; set; }

        public string Signature { get; set; }
    }
}
