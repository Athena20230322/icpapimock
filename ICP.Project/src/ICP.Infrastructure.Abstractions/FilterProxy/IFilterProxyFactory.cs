using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Abstractions.FilterProxy
{
    public interface IFilterProxyFactory
    {
        IFilterProxy Create(ProxyType proxyType);
    }
}
