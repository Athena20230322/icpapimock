using ICP.Modules.Mvc.Admin.Attributes;
using ICP.Modules.Mvc.Admin.Commands;
using ICP.Modules.Mvc.Admin.Enums;
using ICP.Modules.Mvc.Admin.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Controllers
{
    public class IPRecordController : BaseAdminController
    {
        IPRecordCommand _ipRecordCommand;

        public IPRecordController(IPRecordCommand ipRecordCommand)
        {
            _ipRecordCommand = ipRecordCommand;
        }

        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Index()
        {
            return View(new QueryIPRecordVM());
        }
        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="query">查詢物件</param>
        /// <returns></returns>
        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Query(QueryIPRecordVM query)
        {
            query.PageSize = int.MaxValue - 1;
            return PagedListView(_ipRecordCommand.ListLoginRecord(query), query);
        }
        /// <summary>
        /// 匯出excel
        /// </summary>
        /// <param name="query">查詢物件</param>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Export)]
        public ActionResult ExportListIPRecord(QueryIPRecordVM query)
        {
            query.PageNo = 1;
            query.PageSize = int.MaxValue - 1;
            MemoryStream file = _ipRecordCommand.ExportIPRecord(query);
            file.Flush();
            file.Position = 0;

            return File(file, "application/ms-excel", $"IPQuiry_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xls");
        }
    }
}
