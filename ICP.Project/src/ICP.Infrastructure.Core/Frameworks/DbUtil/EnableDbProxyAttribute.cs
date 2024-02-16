using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Frameworks.DbUtil
{
    public class EnableDbProxyAttribute : Attribute
    {
        public int TransferTimeoutSec { get; set; } = 60;
    }
}
