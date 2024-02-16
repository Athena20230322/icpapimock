using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Abstractions.FilterProxy
{
    public enum ProxyType
    {
        /// <summary>
        /// App Api
        /// </summary>
        AuthorizationApi,

        /// <summary>
        /// Web Mvc
        /// </summary>
        AuthorizationMvc,

        /// <summary>
        /// 自訂 Api
        /// </summary>
        OPCustomApi,

        /// <summary>
        /// Web UI Api
        /// </summary>
        OPWebUIApi,

        /// <summary>
        /// Api for Admin
        /// </summary>
        AdminAuthorizationApi
    }
}
