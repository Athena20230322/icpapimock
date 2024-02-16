using ICP.Infrastructure.Core.Web.Models.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ICP.Infrastructure.Core.Web.Extensions
{
    public static class HttpContextExtensions
    {
        /// <summary>
        /// 取得 Request 唯一識別碼
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static string GetRequestId(this HttpContext httpContext)
        {
            if (httpContext == null ||
                httpContext.Items == null ||
                !httpContext.Items.Contains(MvcConst.RequestId))
            {
                return null;
            }

            return httpContext.Items[MvcConst.RequestId].ToString();
        }

        /// <summary>
        /// 產生 Request 唯一識別碼
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="guid"></param>
        public static void GenerateRequestId(this HttpContext httpContext, string guid = null)
        {
            if (httpContext != null && httpContext.Items != null)
            {
                guid = guid ?? Guid.NewGuid().ToString();
                httpContext.Items.Add(MvcConst.RequestId, guid);
            }
        }
    }
}
