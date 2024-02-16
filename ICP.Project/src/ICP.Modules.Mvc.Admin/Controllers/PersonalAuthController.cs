using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Controllers
{
    using Commands;
    using ICP.Infrastructure.Abstractions.Authorization;
    using ICP.Infrastructure.Core.Models;
    using ICP.Library.Models.MemberModels;
    using ICP.Library.Repositories.MemberRepositories;
    using ICP.Library.Services.MemberServices;
    using ICP.Modules.Mvc.Admin.Attributes;
    using ICP.Modules.Mvc.Admin.Enums;
    using ICP.Modules.Mvc.Admin.Models.ViewModels;
    using Models.MemberModels;
    using System.Web;

    public class PersonalAuthController : BaseAdminController
    {
        PersonalAuthCommand _personalAuthCommand;
        LibMemberAuthService _libMemberAuthService;
        
        public PersonalAuthController(
            PersonalAuthCommand personalAuthCommand,
            LibMemberAuthService libMemberAuthService
            )
        {
            _personalAuthCommand = personalAuthCommand;
            _libMemberAuthService = libMemberAuthService;
        }

        #region P11驗證
        /// <summary>
        /// P11驗證
        /// </summary>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "P11Auth", Action = MappingMethodAction.Query)]
        public ActionResult P11Auth()
        {
            ViewBag.IssueLocations = _libMemberAuthService.ListIssueLocation();
            return View(new P11AuthVM());
        }
        [HttpPost]
        [UserLoginAuth(MappingMethod = "P11Auth", Action = MappingMethodAction.Check)]
        public ActionResult P11AuthQuery(P11AuthVM model)
        {
            var result = _personalAuthCommand.P11Auth(model, CurrentUser.CName, RealIP, ProxyIP);
            return PartialView("P11AuthQuery", result);
        }
        #endregion

        #region P11驗證紀錄查詢
        /// <summary>
        /// P11驗證紀錄查詢查詢
        /// </summary>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "P11AuthHistory", Action = MappingMethodAction.Query)]
        public ActionResult P11AuthHistory()
        {
            return View(new P11AuthHistoryVM());
        }
        [HttpPost]
        [UserLoginAuth(MappingMethod = "P11AuthHistory", Action = MappingMethodAction.Query)]
        public ActionResult P11AuthHistoryQuery(P11AuthHistoryVM model)
        {
            ViewBag.IssueLocations = _libMemberAuthService.ListIssueLocation();
            var list = _personalAuthCommand.QueryP11AuthLog(model);
            return PagedListView(list, model);
        }
        #endregion

        #region P33驗證
        /// <summary>
        /// P33驗證查詢
        /// </summary>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "P33Auth", Action = MappingMethodAction.Query)]
        public ActionResult P33Auth()
        {
            return View(new P33AuthVM());
        }
        [UserLoginAuth(MappingMethod = "P33Auth", Action = MappingMethodAction.Check)]
        public ActionResult P33AuthQuery(P33AuthVM model)
        {
            var result = _personalAuthCommand.P33Auth(model, CurrentUser.CName, RealIP, ProxyIP);
            return PartialView("P33AuthQuery", result);
        }
        #endregion

        #region P33驗證紀錄查詢查詢
        /// <summary>
        /// P33驗證紀錄查詢查詢
        /// </summary>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "P33AuthHistory", Action = MappingMethodAction.Query)]
        public ActionResult P33AuthHistory()
        {
            return View(new P33AuthHistoryVM());
        }
        [HttpPost]
        [UserLoginAuth(MappingMethod = "P33AuthHistory", Action = MappingMethodAction.Query)]
        public ActionResult P33AuthHistoryQuery(P33AuthHistoryVM model)
        {
            var list = _personalAuthCommand.QueryP33AuthLog(model);
            return PagedListView(list, model);
        }
        #endregion
    }
}
