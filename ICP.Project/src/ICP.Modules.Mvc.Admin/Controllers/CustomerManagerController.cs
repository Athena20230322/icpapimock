using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Web.Extensions;
using ICP.Library.Models.MemberModels;
using ICP.Library.Services.MemberServices;
using ICP.Library.Services.SMSLibrary;
using ICP.Modules.Mvc.Admin.Attributes;
using ICP.Modules.Mvc.Admin.Commands;
using ICP.Modules.Mvc.Admin.Enums;
using ICP.Modules.Mvc.Admin.Models.CustomerManager;
using ICP.Modules.Mvc.Admin.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Controllers
{
    public class CustomerManagerController : BaseAdminController
    {
        CustomerManagerCommand _customerManagerCommand;
        LibMemberAuthService _libMemberAuthService;
        SMSService _sMSService;

        public CustomerManagerController(
            CustomerManagerCommand customerManagerCommand,
            LibMemberAuthService libMemberAuthService,
            SMSService sMSService
            )
        {
            _customerManagerCommand = customerManagerCommand;
            _libMemberAuthService = libMemberAuthService;
            _sMSService = sMSService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        [HttpPost]
        public ActionResult Query(QueryMemberVM query)
        {
            query.PageSize = 10;
            //驗證查詢條件
            var vaildQueryMemberResult = _customerManagerCommand.vaildQueryMember(query);

            ViewBag.QueryModel = query;

            //查詢資料
            var list = _customerManagerCommand.QueryMember(query);
            return PagedListView(list, query);
        }


        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Detail(long id)
        {
            //查詢資料
            var getMemberDetailData = _customerManagerCommand.GetMemberDetail(id);

            ViewBag.LevelIDName = _customerManagerCommand.GetLevelIDName(getMemberDetailData.basic.LevelID);

            //取出會員驗證狀態資料
            var GetMemberVerifyStatusData = _customerManagerCommand.GetMemberVerifyStatus(id);
            var IDNOP33VerifyDate = GetMemberVerifyStatusData.P33AuthOKDate;
            ViewBag.IDNOP33VerifyDT = (IDNOP33VerifyDate != null)? IDNOP33VerifyDate.Value.ToString("yyyy/MM/dd HH:mm:ss") : "-";            

            var CellPhoneAuthOKDate = GetMemberVerifyStatusData.CellPhoneAuthOKDate;
            ViewBag.CellPhoneAuthOKDate = (CellPhoneAuthOKDate != null) ? CellPhoneAuthOKDate.Value.ToString("yyyy/MM/dd HH:mm:ss") : "-";

            //取出 P11驗證資料
            ViewBag.P11Data = _customerManagerCommand.GetMemberAuthIDNO(id, 0);

            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();
            if (ViewBag.P11Data != null)
            {
                ViewBag.IssueDate = ViewBag.P11Data.IssueDate;
                ViewBag.IssueDate = string.Format("{0}/{1}/{2}",
                   taiwanCalendar.GetYear(ViewBag.IssueDate),
                   ViewBag.IssueDate.Month,
                   ViewBag.IssueDate.Day);
            }

            //取出 OTP 當日發送次數
            ViewBag.OTPCount = _customerManagerCommand.GetOTPCount(getMemberDetailData.detail.CellPhone);

            //取出 銀行帳戶 資料 提領類別: 0: 提領轉入帳戶 1: 連結扣款帳戶
            ViewBag.bankAccountList= _customerManagerCommand.ListMemberOnBankAccount(id);

            //取出 金融驗證工具資料
            ViewBag.bankAuthFinancial = _customerManagerCommand.GetAuthFinancial(id);

            // Type: 1 現金帳戶餘額 2 儲值帳戶餘額 3 可轉出金額(可提領) 4 電支帳戶餘額 5 凍結金額
            ViewBag.AvailableOutCash = _customerManagerCommand.GetUserCoinsOnBalanceByType(id, 0);
            ViewBag.TopUpCash = _customerManagerCommand.GetUserCoinsOnBalanceByType(id, 2);
            ViewBag.FreezeCash = _customerManagerCommand.GetUserCoinsOnBalanceByType(id, 5);

            return View(getMemberDetailData);
        }

        /// <summary>
        /// 姓名修改記錄
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult ListAuthCNameLog(long id)
        {
            var listAuthCNameLogData = _customerManagerCommand.ListAuthCNameListLog(id);
            return PartialView(listAuthCNameLogData);
        }

        /// <summary>
        /// 手機號碼修改記錄
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult ListAuthCellPhoneLog(long id)
        {
            var listAuthCellPhoneLogData = _customerManagerCommand.ListAuthCellPhoneListLog(id);
            return PartialView(listAuthCellPhoneLogData);
        }

        /// <summary>
        /// 電支使用者升級歷程
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult ListMemberUpgradeListLog(long id)
        {
            var listMemberUpgradeLogData = _customerManagerCommand.ListMemberUpgradeListLog(id);
            return PartialView(listMemberUpgradeLogData);

        }

        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]        
        public ActionResult Edit(long id)
        {
            //查詢資料
            var getMemberDetailData = _customerManagerCommand.GetMemberDetail(id);

            ViewBag.LevelIDName = _customerManagerCommand.GetLevelIDName(getMemberDetailData.basic.LevelID);

            //取出P33驗證成功資料            
            var GetMemberVerifyStatusData = _customerManagerCommand.GetMemberVerifyStatus(id);
            var IDNOP33VerifyDate = GetMemberVerifyStatusData.P33AuthOKDate;
            ViewBag.IDNOP33VerifyDT = (IDNOP33VerifyDate != null) ? IDNOP33VerifyDate.Value.ToString("yyyy/MM/dd HH:mm:ss") : "-";

            var CellPhoneAuthOKDate = GetMemberVerifyStatusData.CellPhoneAuthOKDate;
            ViewBag.CellPhoneAuthOKDate = (CellPhoneAuthOKDate != null) ? CellPhoneAuthOKDate.Value.ToString("yyyy/MM/dd HH:mm:ss") : "-";

            //取出 P11驗證資料
            ViewBag.P11Data = _customerManagerCommand.GetMemberAuthIDNO(id, 0);

            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();
            if (ViewBag.P11Data != null)
            {
                ViewBag.IssueDate = ViewBag.P11Data.IssueDate;
                ViewBag.IssueDate = string.Format("{0}/{1}/{2}",
                   taiwanCalendar.GetYear(ViewBag.IssueDate),
                   ViewBag.IssueDate.Month,
                   ViewBag.IssueDate.Day);
            }

            //取出 OTP 當日發送次數
            ViewBag.OTPCount = _customerManagerCommand.GetOTPCount(getMemberDetailData.detail.CellPhone);

            //取出 銀行帳戶 資料 提領類別: 0: 提領轉入帳戶 1: 連結扣款帳戶
            ViewBag.bankAccountList = _customerManagerCommand.ListMemberOnBankAccount(id);

            //取出 金融驗證工具資料
            ViewBag.bankAuthFinancial = _customerManagerCommand.GetAuthFinancial(id);

            // Type: 1 現金帳戶餘額 2 儲值帳戶餘額 3 可轉出金額(可提領) 4 電支帳戶餘額 5 凍結金額
            ViewBag.AvailableOutCash = _customerManagerCommand.GetUserCoinsOnBalanceByType(id, 0);
            ViewBag.TopUpCash = _customerManagerCommand.GetUserCoinsOnBalanceByType(id, 2);
            ViewBag.FreezeCash = _customerManagerCommand.GetUserCoinsOnBalanceByType(id, 5);

            ViewBag.id = id;
            return View(getMemberDetailData);
        }

        #region 手機簡訊解鎖
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult UnlockSMS(long id)
        {
            //查詢資料
            var getUnLockSMSData = _customerManagerCommand.GetUnLockSMSData(id);

            ViewBag.id = id;

            return PartialView(getUnLockSMSData);
        }

        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        [HttpPost]
        public ActionResult ModifyUnlockSMS(long id)
        {
            var result = _customerManagerCommand.UpdateUnLockSMS(id, CurrentUser.Account);

            return Json(result);

        }
        #endregion

        #region 修改手機號碼
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult EditCellPhone(long id)
        {
            //查詢資料
            var getCellPhoneData = _customerManagerCommand.GetCellPhoneData(id);

            return PartialView(getCellPhoneData);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult EditCellPhone(EditCellPhoneModel model)
        {
            long id = model.MID;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.ProxyIP = ProxyIP;
            model.RealIP = RealIP;
            model.ModifyUser = CurrentUser.Account;

            var result = _customerManagerCommand.UpdateCellPhone(model);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.RtnMsg);
                return Json(result);
            }

            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("Edit", new { id }), "成功");
            }


        }
        #endregion

        #region 會員的訊息中心記錄
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult ListMemberNotifyMessage(long id)
        {
            ViewBag.id = id;
            return View();           
        }

        [HttpPost]
        public ActionResult ListMemberNotifyMessageResult(QueryFreezeCoinsModel query)
        {
            int TotalCount = 0;
            PageModel pageModel = new PageModel
            {
                PageNo = query.PageNo,
                PageSize = 10
            };

            query.PageSize = 10;

            var ListNotifyMessage = _customerManagerCommand.MemberNotifyMessage(query.MID, ref TotalCount, ref pageModel, null);            

            return PagedListView(ListNotifyMessage, query);
        }

        #endregion

        #region 會員的訊息中心記錄明細
        public ActionResult MemberNotifyMessageDetail(long id, long NotifyMessageID)
        {
            ViewBag.id = id;    
            var ListNotifyMessageDetail = _customerManagerCommand.MemberNotifyMessageDetail(NotifyMessageID, id, null);
            return PartialView(ListNotifyMessageDetail);
        }
        #endregion

        #region 凍結款項列表
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult ListFreezeCoins(long id)
        {
            ViewBag.id = id;
            return View();            
        }

        public ActionResult ListFreezeCoinsResult(QueryFreezeCoinsModel query)
        {  
            var freezeObject = _customerManagerCommand.ListFreezeCoins(query);
            return PagedListView(freezeObject, query);
        }


        #endregion

        #region 凍結款項明細
        public ActionResult ListFreezeCoinsLog(long id, long FreezeID)
        {            
            var result = _customerManagerCommand.ListFreezeCoinsLog(id, FreezeID);

            return PartialView(result);

        }
        #endregion

        #region 新增凍結餘額資料
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult AddFreezeCoins(long id)
        {
            ViewBag.id = id;

            //目前電支帳戶餘額
            // Type: 1 現金帳戶餘額 2 儲值帳戶餘額 3 可轉出金額(可提領) 4 電支帳戶餘額 5 凍結金額
            //ViewBag.AvailableOutCash = _customerManagerCommand.GetUserCoinsOnBalanceByType(id, 0);
            var result = _customerManagerCommand.GetUserCoinsOnBalanceData(id, 0);
            if (!result.IsSuccess) return HttpNotFound();
            return View(result.RtnData);

        }

        [HttpPost]
        public ActionResult AddFreezeCoins(long id, AddFreezeCoinsModel model)
        {
            //檢查Model
            if (model.FreezeCash > model.TotalCash)
            {
                ModelState.AddModelError("FreezeCash", "欲凍結金額不得大於帳戶餘額");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Creator = CurrentUser.Account;
            model.Status = 1;

            var result = _customerManagerCommand.AddFreezeCoins(model);

            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("ListFreezeCoins", new { id }), "成功");
            }


        }
        #endregion

        #region 返還凍結餘額
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult ReturnFreezeCoins(long id, long FreezeID)
        {
            ViewBag.id = id;
            ViewBag.FreezeID = FreezeID;

            var FreezeCoinsLog = _customerManagerCommand.ListFreezeCoinsLog(id, FreezeID);
            ViewBag.FreezeCash = FreezeCoinsLog.First().FreezeCash;
            return View();

        }

        [HttpPost]
        public ActionResult ReturnFreezeCoins(long id, long FreezeID, decimal FreezeCash, ReturnFreezeCoinsModel model)
        {
            //檢查Model
            if (string.IsNullOrEmpty(model.Remark))
            {
                ModelState.AddModelError("Remark", "原因備註不得為空");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.id = id;
                ViewBag.FreezeID = FreezeID;
                ViewBag.FreezeCash = FreezeCash;
                return View(model);
            }

            model.Creator = CurrentUser.Account;
            model.Status = 2;

            var result = _customerManagerCommand.ReturnFreezeCoins(model);

            return Json(result);

        }
        #endregion

        #region 解除凍結金額
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Edit)]
        public ActionResult ReleaseFreezeCoins(long id, long FreezeID)
        {
            ViewBag.id = id;
            ViewBag.FreezeID = FreezeID;

            var FreezeCoinsLog = _customerManagerCommand.ListFreezeCoinsLog(id, FreezeID);
            ViewBag.FreezeCash = FreezeCoinsLog.First().FreezeCash;
            return View();

        }

        [HttpPost]
        public ActionResult ReleaseFreezeCoins(long id, long FreezeID, decimal FreezeCash, ReleaseFreezeCoinsModel model)
        {

            //檢查Model
            if (string.IsNullOrEmpty(model.Remark))
            {
                ModelState.AddModelError("Remark", "原因備註不得為空");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.id = id;
                ViewBag.FreezeID = FreezeID;
                ViewBag.FreezeCash = FreezeCash;
                return View(model);
            }

            model.Creator = CurrentUser.Account;
            model.Status = 3;

            var result = _customerManagerCommand.ReleaseFreezeCoins(model);

            return Json(result);

        }
        #endregion

        #region 外籍電支使用者批次上傳升級相關

        #region 外籍電支使用者列表

        public ActionResult ListOverSeaUser()
        {
            ////日期條件
            //ViewBag.StartDate = 
            //ViewBag.EndDate = new DateTime(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month, 1).AddDays(-1) == DateTime.Now
            //    ? DateTime.Now.ToString("yyyy-MM-dd")
            //    : DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            return View(new ListOverSeaUserQryVM());
    }

        [HttpPost]
        public ActionResult ListOverSeaUserResult(ListOverSeaUserQryVM query)
        {
            //一頁50筆
            //query.PageSize = 50;

            var result = _customerManagerCommand.ListOverSeaUser(query);
            foreach (var item in result)
            {
                TaiwanCalendar taiwanCalendar = new TaiwanCalendar();
                if (item.UniformExpireDate != null)
                {
                    var UniformExpire = item.UniformExpireDate;
                    var UniformIssue = item.UniformIssueDate;
                    
                    item.UniformExpireDateYYYMMDD = string.Format("{0}/{1}/{2}",
                       taiwanCalendar.GetYear(UniformExpire),
                       UniformExpire.Month,
                       UniformExpire.Day);

                    item.UniformIssueDateYYYMMDD = string.Format("{0}/{1}/{2}",
                       taiwanCalendar.GetYear(UniformIssue),
                       UniformIssue.Month,
                       UniformIssue.Day);
}

            }
            return PagedListView(result, query);


            
        }

        #endregion

        #region 上傳基本資料，匯入CSV
        public ActionResult AddOverSeaMemberData()
        {            
            ViewBag.SuccessCounts = 0;
            ViewBag.FailCounts = 0;
            ViewBag.TotalCounts = 0;

            return View();
        }
        [HttpPost]
        public ActionResult AddOverSeaMemberData(HttpPostedFileBase file)
        {
            // 檢查上傳的檔案內容
            string Creator = CurrentUser.Account;
            long realIP = RealIP;
            long proxyIP = ProxyIP;

            // 檢查檔案格式
            var vaildFileCSV = _customerManagerCommand.VaildCSVFile(file);
            if (vaildFileCSV.RtnCode != 1)
            {
                return View(vaildFileCSV);
            }

            int SuccessCounts = 0;
            int FailCounts = 0;
                                   
            // 檔案內容解析 & 上傳
            var UploadOverSeaFileResult = _customerManagerCommand.UploadOverSeaFile(vaildFileCSV.RtnMsg, Creator, RealIP, ProxyIP, ref SuccessCounts, ref FailCounts);
            if (UploadOverSeaFileResult != null )
            {
                ViewBag.SuccessCounts = SuccessCounts;
                ViewBag.FailCounts = FailCounts;
                ViewBag.TotalCounts = SuccessCounts + FailCounts;

                return View(UploadOverSeaFileResult);
            }
            
            return View();
        }
        #endregion

        #region 批次證件照片上傳
        public ActionResult UploadOverSeaFile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadOverSeaFile(HttpPostedFileBase file)
        {
            string Creator = CurrentUser.Account;
            // 檢查檔案是否符合規則，符合規則才上傳
            var validFile = _customerManagerCommand.ValidFile(file, Creator);
           
            if (!validFile.IsSuccess)
            {
                return Json(validFile);
            } 

            return Json(new BaseResult { RtnCode = 1, RtnMsg = file.FileName + "成功" });
            
        }
        #endregion


        #region 身份驗證
        [HttpPost]
        public ActionResult UpdateUniformIDStatus(string idList)
        {
            // 需判斷是單筆還是多筆
            string[] ArrayMID = idList.Split(',');
            if (ArrayMID.Length != 0)
            {
                foreach (var item in ArrayMID)
                {
                    long MID = Convert.ToInt64(item);
                    string UniformID = _customerManagerCommand.GetMemberAuthIDNO(MID, 1).UniformID;
                    string Creator = CurrentUser.Account;
                    long realIP = RealIP;
                    long proxyIP = ProxyIP;

                    // 準備 model
                    UpdateUniformIDStatusModel model = new UpdateUniformIDStatusModel {
                        MID = MID,
                        UniformID = UniformID,
                        CreateUser = Creator,
                        RealIP = realIP,
                        ProxyIP = proxyIP
                    };

                    var result = _customerManagerCommand.UpdateUniformIDStatus(model);
                    if (result.RtnCode != 1)
                    {
                        return Json(result);
                    }

                    // 發送註冊完成通知簡訊
                    /*
                     todo 簡訊
                     Please sign in with帳號：A1070密碼：D0002，and reset the 帳號&密碼&安全密碼
                     臨時帳號邏輯: 統一證號第一碼英文 + 前四碼數字 + 1(若重複就 +2 以此類推)
                     臨時密碼邏輯: 統一證號第二碼英文 + 後四碼數字 + 1
                     */
                    // 簡訊發送有問題，待確認後再移除註解
                    //var getMemberDetailData = _customerManagerCommand.GetMemberDetail(MID);
                    //if (getMemberDetailData != null)
                    //{
                    //    string CellPhone = getMemberDetailData.detail.CellPhone;
                    //    string account = getMemberDetailData.basic.Account;
                    //    string pwd = UniformID.Substring(1, 1) + UniformID.Substring(6, 4) + 1;
                    //    string MsgData = "Please sign in with帳號：" + account + "密碼：" + pwd + "，and reset the 帳號&密碼&安全密碼";
                    //    _sMSService.SendSMS(CellPhone, MsgData, 0);
                    //}           

                   // 發送金融支付工具驗證請求(新增一元驗證資料) 
                   var addBankTransferResult = _customerManagerCommand.AddBankTransfer(MID);                     

                }
            }
            
            
            return Json(new BaseResult { RtnCode = 1, RtnMsg = "成功" });
        }
        #endregion



        #endregion

    }
}
