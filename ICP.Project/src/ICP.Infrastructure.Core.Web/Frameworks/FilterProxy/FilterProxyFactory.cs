using Autofac.Features.Metadata;
using ICP.Infrastructure.Abstractions.FilterProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Web.Frameworks.FilterProxy
{
    public class FilterProxyFactory : IFilterProxyFactory
    {
        private readonly IEnumerable<Meta<IFilterProxy>> _filterProxies = null;

        public FilterProxyFactory(IEnumerable<Meta<IFilterProxy>> filterProxies)
        {
            _filterProxies = filterProxies;
        }

        public IFilterProxy Create(ProxyType proxyType)
        {
            return _filterProxies.FirstOrDefault(x => proxyType.Equals(x.Metadata[nameof(IFilterProxy)]))?.Value;
        }
    }
}
