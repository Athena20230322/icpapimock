using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using X.PagedList;

namespace ICP.Infrastructure.Core.Web.Controllers
{
    public class BaseController : Controller
    {
        public string sRealIP
        {
            get
            {
                return Request.RemoteRealIP();
            }
        }

        public string sProxyIP
        {
            get
            {
                return Request.RemoteProxyIP();
            }
        }

        public long RealIP
        {
            get
            {
                return Request.RealIP();
            }
        }

        public long ProxyIP
        {
            get
            {
                return Request.ProxyIP();
            }
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            // Call JsonResult to throw the same exception as JsonResult
            if (behavior == JsonRequestBehavior.DenyGet &&
                string.Equals(Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                return new JsonResult();
            }

            return new JsonNetResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding
            };
        }
    }
}
