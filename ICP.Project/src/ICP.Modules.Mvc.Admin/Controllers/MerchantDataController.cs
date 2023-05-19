using ICP.Library.Services.MemberServices;
using ICP.Modules.Mvc.Admin.Attributes;
using ICP.Modules.Mvc.Admin.Commands;
using ICP.Modules.Mvc.Admin.Enums;
using ICP.Modules.Mvc.Admin.Models.MerchantModels;
using ICP.Modules.Mvc.Admin.Models.ViewModels;

using ICP.Modules.Mvc.Admin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Controllers
{
    public class MerchantDataController : BaseAdminController
    {
        MerchantDataCommand _merchantDataCommand;
        MerchantDataService _merchantDataService;
        UserService _userService;
        LoggingService _loggingService;
        LibMemberBankService _libMemberBankService;
        LibMemberInfoService _libMemberInfoService;
        LibCommonService _libCommonService;
        PrivilegeService _privilegeService;

        public MerchantDataController(
            MerchantDataCommand merchantDataCommand,
            MerchantDataService merchantDataService,
            UserService userService,
            LoggingService loggingService,
            LibMemberBankService libMemberBankService,
            LibMemberInfoService libMemberInfoService,
            LibCommonService libCommonService,
            PrivilegeService privilegeService
            )
        {
            _merchantDataCommand = merchantDataCommand;
            _merchantDataService = merchantDataService;
            _userService = userService;
            _loggingService = loggingService;
            _libMemberBankService = libMemberBankService;
            _libMemberInfoService = libMemberInfoService;
            _libCommonService = libCommonService;
            _privilegeService = privilegeService;
        }

        #region 共用
        private void SetCustomerDataViewBag(CustomerDataModel model, string ActionName)
        {
            var basic = model.basic;

            var AuditStatus = basic?.AuditStatus;

            var CustomerStatus = basic?.CustomerStatus;

            int Permission = _privilegeService.GetFunctionActionByUser(CurrentUserID, ControllerName: "MerchantData", MethodName: ActionName);

            //可使用之審核狀態
            ViewBag.ListNextAuditStatus = _merchantDataService.ListNextAuditStatus(AuditStatus ?? 0, Permission, CustomerStatus ?? 0);

            //業務
            ViewBag.ListSales = _userService.ListSales();

            //銀行種類
            ViewBag.ListBankType = _libMemberBankService.ListBankType();

            //職業
            ViewBag.ListOccupation = _libMemberInfoService.ListOccupation();

            //縣市
            ViewBag.ListZipArea = _libMemberInfoService.ListZipCodeArea();

            //國家
            ViewBag.ListNationality = _libMemberInfoService.ListNationality();

            //公司地址區域
            ViewBag.CompanyZipArea = _libCommonService.GetAreaID(model.detail?.CompanyZipCode);

            //發票地址區域
            ViewBag.InvoiceZipArea = _libCommonService.GetAreaID(model.detail?.InvoiceZipCode);
        }

        public JsonResult ListBankCode(byte id)
        {
            byte BankTypeID = id;

            var list = _libMemberBankService.ListBankCode(BankTypeID);

            var items = list.Select(t => new { Text = t.BankName, Value = t.BankCode }).ToList();

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListBankBranchCode(string id)
        {
            string BankCode = id;

            var list = _libMemberBankService.ListBankBranchCode(BankCode);

            var items = list.Select(t => new { Text = t.BankName, Value = t.BankBranchCode }).ToList();

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListZipCode(string id)
        {
            string AreaID = id;
            
            var list = _libMemberInfoService.ListZipCodeArea(AreaID);

            var items = list.Select(t => new { Text = t.AreaName, Value = t.ZipCode }).ToList();

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckAccountRepeat(string value, long MID = 0)
        {
            var result = _libMemberInfoService.CheckUserCodeUnique(value, MID);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 未過件

        #region 查詢
        [UserLoginAuth(MappingMethod = "UnFinished", Action = MappingMethodAction.Query)]
        public ActionResult UnFinished()
        {
            ViewBag.ListAuditStatus = _merchantDataService.ListAuditStatus(CustomerStatus: 0);
            ViewBag.ListSales = _userService.ListSales();

            var query = new CustomerDataQueryModel();
            query.CreateEnd = DateTime.Today;
            query.CreateBegin = DateTime.Today.AddMonths(-1);

            return View(query);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "UnFinished", Action = MappingMethodAction.Query)]
        public ActionResult QueryUnFinished(CustomerDataQueryModel query)
        {
            ViewBag.ListAuditStatus = _merchantDataService.ListAuditStatus(CustomerStatus: 0);

            var list = _merchantDataCommand.ListUnFinishedData(query);

            return PagedListView(list, query);
        }
        #endregion

        #region 新增
        [UserLoginAuth(MappingMethod = "UnFinished", Action = MappingMethodAction.Add)]
        public ActionResult NewData()
        {
            string ActionName = "UnFinished";

            SetCustomerDataViewBag(new CustomerDataModel(), ActionName);
            return View();
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "UnFinished", Action = MappingMethodAction.Add)]
        public ActionResult NewData(CustomerDataModel model)
        {
            string ActionName = "UnFinished";

            // 根據 model 部分欄位, 忽略某些可以略過驗證的欄位
            var ignoreFields = _merchantDataService.GetIgnoreFields(model);
            ignoreFields.ForEach(ignoreField =>
            {
                if (!ModelState.ContainsKey(ignoreField)) return;
                ModelState[ignoreField].Errors.Clear();
            });

            // model 格式驗證
            if (!ModelState.IsValid)
            {
                SetCustomerDataViewBag(model, ActionName);
                return View(model);
            }

            _merchantDataCommand.AddCustomerData(model, CurrentUser.Account, RealIP, ProxyIP);


            SetCustomerDataViewBag(model, ActionName);
            return View(model);
        }
        #endregion

        #region 編輯
        [UserLoginAuth(MappingMethod = "UnFinished", Action = MappingMethodAction.Edit)]
        public ActionResult EditUnFinished(long id)
        {
            long CustomerID = id;

            return View();
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "UnFinished", Action = MappingMethodAction.Edit)]
        public ActionResult EditUnFinished(long id, object model)
        {
            long CustomerID = id;

            return View();
        }
        #endregion

        #endregion
    }
}