using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Models
{
    public class DbProxyRequest
    {
        public string AssemblyName { get; set; }

        public string FullName { get; set; }

        public string MethodName { get; set; }

        public IDictionary<int, object> Args { get; set; }
    }
}
