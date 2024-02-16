using ICP.Modules.Mvc.Admin.Attributes;
using ICP.Modules.Mvc.Admin.Commands;
using ICP.Modules.Mvc.Admin.Enums;
using ICP.Modules.Mvc.Admin.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Controllers
{
    public class UnregisteredDataController : BaseAdminController
    {
        UnregisteredDataCommand _unregisteredDataCommand;

        public UnregisteredDataController(UnregisteredDataCommand unregisteredDataCommand)
        {
            _unregisteredDataCommand = unregisteredDataCommand;
        }

        public ActionResult Index()
        {
            return View(new QueryUnregisteredDataVM());
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Query(QueryUnregisteredDataVM query)
        {
            query.PageSize = 10;
            var result = _unregisteredDataCommand.ListUnregisteredData(query);

            ViewBag.QueryModel = query;

            return PagedListView(result, query);
        }

        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Detail(long MID)
        {
            var result = _unregisteredDataCommand.GetUnregisteredData(MID);

            return View(result);
        }

        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Export)]
        public ActionResult Export(QueryUnregisteredDataVM query)
        {
            query.PageSize = int.MaxValue;
            var xlsStream = _unregisteredDataCommand.Export(query);
            if (xlsStream == null)
                return Content(string.Empty);

            string fileName = "MemberDeleteRecord_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            xlsStream.Flush();
            xlsStream.Position = 0;
            return File(xlsStream, "application/ms-excel", fileName);
        }
    }
}
