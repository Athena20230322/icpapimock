using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Controllers
{
    using Commands;
    using ICP.Infrastructure.Core.Models;
    using ICP.Library.Models.MemberModels;
    using ICP.Library.Repositories.MemberRepositories;
    using ICP.Library.Services.MemberServices;
    using ICP.Modules.Mvc.Admin.Attributes;
    using ICP.Modules.Mvc.Admin.Enums;
    using Models.MemberModels;
    using System.Web;

    public class MemberTeenagersController : BaseAdminController
    {
        MemberTeenagersCommand _memberTeenagersCommand;
        LibMemberAuthService _libMemberAuthService;
        MemberConfigRepository _memberConfigRepository;

        public MemberTeenagersController(
            MemberTeenagersCommand memberTeenagersCommand,
            LibMemberAuthService libMemberAuthService,
            MemberConfigRepository memberConfigRepository
            )
        {
            _memberTeenagersCommand = memberTeenagersCommand;
            _libMemberAuthService = libMemberAuthService;
            _memberConfigRepository = memberConfigRepository;
        }

        #region 首頁
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Index()
        {
            return View(new MemberTeenagersQuery());
        }
        #endregion

        #region 查詢
        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Query(MemberTeenagersQuery query, byte queryAuthStatus = 0)
        {
            var list = _memberTeenagersCommand.ListTeenagers(query, queryAuthStatus);

            return PagedListView(list, query);
        }
        #endregion

        #region 瀏覽
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult ViewTeenager(long id)
        {
            long MID = id;

            var result = _memberTeenagersCommand.ViewTeenager(MID);

            if (!result.IsSuccess)
            {
                return HttpNotFound();
            }


            ViewBag.IssueLocations = _libMemberAuthService.ListIssueLocation();
            ViewBag.ReadOnly = true;
            return View(result.RtnData);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult ViewTeenager(long id, string Note)
        {
            long MID = id;

            var result = _memberTeenagersCommand.UpdateTeenagerNote(MID, CurrentUser.Account, Note, RealIP: RealIP, ProxyIP: ProxyIP);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.RtnMsg);
                ViewBag.ReadOnly = false;
                return ViewTeenager(id);
            }

            return Json(result);
        }
        #endregion

        #region 修改
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult UpdateTeenager(long id)
        {
            long MID = id;

            var result = _memberTeenagersCommand.ViewTeenager(MID);
            if (!result.IsSuccess)
            {
                return HttpNotFound();
            }

            ViewBag.MID = MID;
            ViewBag.ReadOnly = false;
            ViewBag.IssueLocations = _libMemberAuthService.ListIssueLocation();
            return View(result.RtnData);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult UpdateTeenager(long id, UpdateTeenagerModel model)
        {
            long MID = id;

            var result = _memberTeenagersCommand.UpdateTeenager(MID, CurrentUser.Account, model, RealIP: RealIP, ProxyIP: ProxyIP);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.RtnMsg);
                ViewBag.ReadOnly = false;
                return UpdateTeenager(id);
            }

            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("UpdateTeenager", new { id }), "成功");
            }
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public JsonResult UpdateTeenagerFile(long id, long LegalMID, byte UploadType, HttpPostedFileBase file)
        {
            long TeenagerMID = id;

            if (new int[] { 1, 2 }.Contains(UploadType))
            {
                string urlDir = _memberConfigRepository.IDNOPath.TrimEnd('/') + $"/{DateTime.Today.ToString("yyyyMM")}";

                string saveImgDir = Server.MapPath(urlDir);

                var result1 = _memberTeenagersCommand.UpdateTeenagerAuthIDNOFile(TeenagerMID, UploadType, file, urlDir, saveImgDir, CurrentUser.Account, RealIP, ProxyIP);

                return Json(result1);
            }
            else if (new int[] { 3, 4, 5, 6, 7, 8 }.Contains(UploadType))
            {
                string urlDir = _memberConfigRepository.Path_TeenagersLegalDetail.TrimEnd('/') + $"/{DateTime.Today.ToString("yyyyMM")}";

                string saveImgDir = Server.MapPath(urlDir);

                var result2 = _memberTeenagersCommand.UpdateTeenagerLegalDetailFile(LegalMID, TeenagerMID, UploadType, file, urlDir, saveImgDir, CurrentUser.Account, RealIP, ProxyIP);

                return Json(result2);
            }

            return Json(new BaseResult{ RtnCode = 1 });
        }
        #endregion

        #region 審核代理人
        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Check)]
        public JsonResult UpdateTeenagersLPAuthStatus(long id, byte Status)
        {
            long MID = id;

            var result = _memberTeenagersCommand.UpdateTeenagersLPAuthStatus(
                MID: MID,
                Modifier: CurrentUser.Account,
                LPAuthStatus: Status,
                RealIP: RealIP,
                ProxyIP: ProxyIP);

            return Json(result);
        }
        #endregion

        #region 審核未成年身份證
        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Check)]
        public JsonResult UpdateTeenagersIDNOStatus(long id)
        {
            long TeenagersMID = id;

            var result = _memberTeenagersCommand.UpdateTeenagersIDNOStatus(
                MID: TeenagersMID,
                Modifier: CurrentUser.Account,
                RealIP: RealIP,
                ProxyIP: ProxyIP);

            return Json(result);
        }
        #endregion
    }
}
