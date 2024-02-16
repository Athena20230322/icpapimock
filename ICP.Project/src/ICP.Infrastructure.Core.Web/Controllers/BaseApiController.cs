using AutoMapper;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using X.PagedList;

namespace ICP.Infrastructure.Core.Web.Controllers
{
    public class BaseApiController : BaseController
    {
        protected virtual ActionResult AppResult(DataResult obj)
        {
            var model = Mapper.Map<DataResult, AppResult>(obj);
            return Json(model);
        }

        protected ActionResult CustomResult<T>(DataResult obj) where T : DataResult
        {
            T model = Mapper.Map<DataResult, T>(obj);
            return Json(model);
        }
    }
}
