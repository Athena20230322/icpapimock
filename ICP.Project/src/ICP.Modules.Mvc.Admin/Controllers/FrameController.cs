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

    public class 
        FrameController : BaseAdminController
    {
        FrameCommand _frameCommand;
        public FrameController(FrameCommand frameCommand)
        {
            _frameCommand = frameCommand;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Menu()
        {
            int UserID = base.CurrentUserID;
            var list = _frameCommand.ListMenuItem(UserID);
            return View(list);
        }

        public ActionResult Content()
        {
            return View();
        }
    }
}
