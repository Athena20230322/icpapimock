using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Models
{
    public abstract class BaseIPModel
    {
        public long RealIP { get; set; }

        public long ProxyIP { get; set; }
    }
}
