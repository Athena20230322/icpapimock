using ICP.Library.Repositories.MemberRepositories;
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
    public class MemberBankController : BaseAdminController
    {
        MemberBankCommand _memberBankCommand;
        MemberConfigRepository _memberConfigRepository;

        public MemberBankController(
            MemberBankCommand memberBankCommand,
            MemberConfigRepository memberConfigRepository
            )
        {
            _memberBankCommand = memberBankCommand;
            _memberConfigRepository = memberConfigRepository;
        }

        #region 查詢
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Index(QueryMemberBankAccountVM model)
        {

            var viewModel = new QueryMemberBankAccountVM();

            if (model != null && !string.IsNullOrEmpty(model.ICPMID))
            {
                viewModel.ICPMID = model.ICPMID;
                viewModel.StartDate = DateTime.Parse(DateTime.Now.ToString("yyyy-01-01")).AddYears(-1).ToString("yyyy-MM-dd");
                viewModel.EndDate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                viewModel.StartDate = DateTime.Now.ToString("yyyy-MM-01");
                viewModel.EndDate = DateTime.Now.ToString("yyyy-MM-dd");
            }          

            return View(viewModel);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Query(QueryMemberBankAccountVM query)
        {
            var result = _memberBankCommand.ListAuthMemberBankAccount(query);

            ViewBag.QueryModel = query;

            return PagedListView(result, query);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult ListBankCode(string BankCode)
        {
            var bankCodes = _memberBankCommand.ListBankCode(BankCode);

            return Json(bankCodes);
        }
        #endregion

        #region 修改銀行帳號
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult EditBankAccount(long AccountID)
        {
            var model = _memberBankCommand.GetBankAccount(AccountID);

            ViewBag.AccountID = AccountID;
            ViewBag.BankDetails = _memberBankCommand.ListBankDetail();
            ViewBag.BankCodes = _memberBankCommand.ListBankCode(model.MemberBankAccount.BankCode);

            return View(model);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult EditBankAccount(long AccountID, EditBankAccountVM model)
        {
            string urlDir = _memberConfigRepository.Path_BankAccount + $"/{DateTime.Today.ToString("yyyyMM")}";

            string saveImgDir = Server.MapPath(urlDir);

            var result = _memberBankCommand.UpdateBankAccount(AccountID, model, urlDir, saveImgDir, CurrentUser.Account);

            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("EditBankAccount", new { AccountID }), "成功");
            }
        }
        #endregion

        #region 文件審核
        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Check)]
        public ActionResult UpdatePaperAuthStatus(long AccountID, byte PaperAuthStatus)
        {
            var result = _memberBankCommand.UpdatePaperAuthStatus(AccountID, PaperAuthStatus, CurrentUser.Account);

            return Json(result);
        }
        #endregion

        #region 銀行帳號驗證
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Check)]
        public ActionResult AuthBankAccount(long AccountID, long MID)
        {
            ViewBag.AccountID = AccountID;
            ViewBag.MID = MID;

            var model = _memberBankCommand.GetBankAccount(AccountID);

            return View(model);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Check)]
        public ActionResult AuthBankAccount(long MID, long AccountID, EditBankAccountVM model)
        {
            var result = _memberBankCommand.AuthBankAccount(MID, model.MemberBankAccount.BankCode, model.MemberBankAccount.BankAccount);

            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("AuthBankAccount", new { AccountID }), "成功");
            }
        }
        #endregion
    }
}
