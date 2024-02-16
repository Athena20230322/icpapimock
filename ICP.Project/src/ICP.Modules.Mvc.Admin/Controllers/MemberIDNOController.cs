using ICP.Library.Repositories.MemberRepositories;
using ICP.Library.Services.MemberServices;
using ICP.Modules.Mvc.Admin.Attributes;
using ICP.Modules.Mvc.Admin.Commands;
using ICP.Modules.Mvc.Admin.Enums;
using ICP.Modules.Mvc.Admin.Models.MemberModels;
using ICP.Modules.Mvc.Admin.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Controllers
{
    public class MemberIDNOController : BaseAdminController
    {
        MemberIDNOCommand _memberIDNOCommand;
        LibMemberAuthService _libMemberAuthService;
        MemberConfigRepository _memberConfigRepository;

        public MemberIDNOController(
            MemberIDNOCommand memberIDNOCommand,
            LibMemberAuthService libMemberAuthService,
            MemberConfigRepository memberConfigRepository
            )
        {
            _memberIDNOCommand = memberIDNOCommand;
            _libMemberAuthService = libMemberAuthService;
            _memberConfigRepository = memberConfigRepository;
        }

        #region 查詢
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Index(QueryMemberIDNOVM model)
        {
            var now = DateTime.Now;

            var viewModel = new QueryMemberIDNOVM();            

            if (model != null && !string.IsNullOrEmpty(model.IDNO))
            {
                viewModel.IDNO = model.IDNO;                
                viewModel.StartDate = DateTime.Parse(DateTime.Now.ToString("yyyy-01-01")).AddYears(-1).ToString("yyyy-MM-dd");
                viewModel.EndDate = now.ToString("yyyy-MM-dd");
            }
            else
            {
                viewModel.StartDate = now.ToString("yyyy-MM-01");
                viewModel.EndDate = now.ToString("yyyy-MM-dd");
            }

            return View(viewModel);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Query(QueryMemberIDNOVM query)
        {
            query.PageSize = 20;
            var result = _memberIDNOCommand.ListAuthMemberIDNO(query);
            if (!result.IsSuccess)
            {
                return RedirectAndAlert(Url.Action("Index"), result.RtnMsg);
            }

            ViewBag.QueryModel = query;
            var model = result.RtnData;

            return PagedListView(model, query);
        }
        #endregion

        #region 修改會員姓名
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult EditCName(long MID, string CName)
        {
            ViewBag.MID = MID;
            ViewBag.CName = CName;

            return View("EditCName");
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult EditCNameResult(long MID, string CName)
        { 
            var result = _memberIDNOCommand.UpdateCName(MID, CName, CurrentUser.Account, RealIP, ProxyIP);

            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("EditCName", new { MID, CName }), "成功");
            }
        }
        #endregion

        #region 更新會員身分驗證資料
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult EditAuthIDNO(long MID)
        {
            ViewBag.MID = MID;
            ViewBag.IssueLocations = _libMemberAuthService.ListIssueLocation();

            var model = _memberIDNOCommand.GetAuthIDNO(MID);

            return View(model);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult EditAuthIDNO(long MID, EditAuthIDNOVM model)
        {
            string urlDir = _memberConfigRepository.IDNOPath + $"/{DateTime.Today.ToString("yyyyMM")}";

            string saveImgDir = Server.MapPath(urlDir);

            var result = _memberIDNOCommand.UpdateAuthIDNO(MID, model, urlDir, saveImgDir, CurrentUser.Account, RealIP, ProxyIP);
            
            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("EditAuthIDNO", new { MID }), "成功");
            }
        }
        #endregion

        #region 文件審核
        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Check)]
        public ActionResult UpdatePaperAuthStatus(long MID, byte PaperAuthStatus)
        {
            var result = _memberIDNOCommand.UpdatePaperAuthStatus(MID, PaperAuthStatus, CurrentUser.Account, RealIP, ProxyIP);

            return Json(result);
        }
        #endregion

        #region 身分驗證
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Check)]
        public ActionResult AuthIDNO(long MID)
        {
            ViewBag.MID = MID;
            ViewBag.IssueLocations = _libMemberAuthService.ListIssueLocation();

            var model = _memberIDNOCommand.GetAuthIDNO(MID);

            return View(model);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Check)]
        public ActionResult AuthIDNO(long MID, EditAuthIDNOVM model)
        {
            var authIdnoResult = _memberIDNOCommand.AuthIDNO(MID, model, CurrentUser.Account, RealIP, ProxyIP);

            byte authStatus = authIdnoResult.IsSuccess ? (byte)1 : (byte)2;

            var result = _memberIDNOCommand.UpdateAuthIDNOStatus(MID, authStatus, CurrentUser.Account, RealIP, ProxyIP);
            if (result.RtnCode != 1)
            {
                return Json(result);
            }

            if (Request.IsAjaxRequest())
            {
                return Json(authIdnoResult);
            }
            else
            {
                return RedirectAndAlert(Url.Action("AuthIDNO", new { MID }), "驗證成功");
            }
        }
        #endregion
    }
}
