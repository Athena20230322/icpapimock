using ICP.Infrastructure.Abstractions.Authorization;
using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using X.PagedList;

namespace ICP.Infrastructure.Core.Web.Controllers
{
    public class BaseMvcController : BaseController
    {
        protected ActionResult RedirectAndAlert(string url, string msg)
        {
            var sb = new StringBuilder();

            sb.Append("<script>");
            sb.AppendFormat("window.alert('{0}');", msg);
            sb.AppendFormat("window.location.href = '{0}';", url);
            sb.Append("</script>");

            return Content(sb.ToString());
        }

        protected ActionResult PagedListView<T>(List<T> list, PageModel pageModel) where T : BaseListModel
        {
            int totalCount = 0;

             if (list != null && list.Any())
            {
                totalCount = list.First().TotalCount;
            }

            var results = new StaticPagedList<T>(list, pageModel.PageNo, pageModel.PageSize, totalCount);

            ViewBag.QueryModel = pageModel;
            return View(results);
        }
    }
}
