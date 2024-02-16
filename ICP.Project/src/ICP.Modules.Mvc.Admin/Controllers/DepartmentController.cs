using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Controllers
{
    using Models;
    using Commands;
    using Infrastructure.Core.Web.Extensions;
    using Infrastructure.Core.Models;

    public class DepartmentController : BaseAdminController
    {
        DepartmentCommand _departmentCommand;

        public DepartmentController(DepartmentCommand departmentCommand)
        {
            _departmentCommand = departmentCommand;
        }

        public ActionResult Index()
        {
            var list = _departmentCommand.ListDepartment();
            return View(list);
        }

        [HttpPost]
        public ActionResult Add(Department model)
        {
            BaseResult result;
            if (!ModelState.IsValid)
            {
                result = new BaseResult { RtnCode = 0 };
                return Json(result);
            }

            result = _departmentCommand.AddDepartment(model);
            return Json(result);
        }

        public ActionResult Edit(int id)
        {
            int DeptID = id;
            var model = _departmentCommand.GetDepartment(DeptID);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, Department model)
        {
            int DeptID = id;
            var result = _departmentCommand.UpdateDepartment(DeptID, model);
            if (result.IsSuccess)
            {
                ModelState.AddModelError("", result.RtnMsg);
                return View(model);
            }
            return View(model);
        }
    }
}
