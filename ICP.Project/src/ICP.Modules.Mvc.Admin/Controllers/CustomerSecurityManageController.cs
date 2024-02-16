using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Models.Consts;
using ICP.Modules.Mvc.Admin.Attributes;
using ICP.Modules.Mvc.Admin.Commands;
using ICP.Modules.Mvc.Admin.Enums;
using ICP.Modules.Mvc.Admin.Models.CustomerSecurityManage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Controllers
{
    public class CustomerSecurityManageController : BaseAdminController
    {
        CustomerSecurityManageCommand _customerSecurityManageCommand;

        public CustomerSecurityManageController(CustomerSecurityManageCommand customerSecurityManageCommand)
        {
            _customerSecurityManageCommand = customerSecurityManageCommand;
        }

        #region IP黑名單相關

        #region IP黑名單資料列表查詢
        [UserLoginAuth(MappingMethod = "IPBlackIndex", Action = MappingMethodAction.Query)]
        public ActionResult IPBlackIndex()
        {
            return View();
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "IPBlackIndex", Action = MappingMethodAction.Query)]
        public ActionResult IPBlackQuery(IPBlackQryModel query)
        {
            ViewBag.QueryModel = query;

            var list = _customerSecurityManageCommand.ListIPBlackList(query);
            return PagedListView(list, query);
        }
        #endregion

        #region 新增IP黑名單
        [UserLoginAuth(MappingMethod = "IPBlackIndex", Action = MappingMethodAction.Add)]
        public ActionResult IPBlackAdd()
        {
            return View();
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "IPBlackIndex", Action = MappingMethodAction.Add)]
        public ActionResult IPBlackAdd(IPBlackAddModel model)
        {
            model.CreateUser = CurrentUser.Account;
            model.Status = 1;
            model.ProxyIP = ProxyIP;
            model.RealIP = RealIP;

            var result = _customerSecurityManageCommand.AddIPBlackList(model);

            if (Request.IsAjaxRequest())
            {
                if (result.RtnCode == 1)
                {
                    return Json(new { result.RtnMsg, result.RtnCode, success = true, IP = model.IP }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                return RedirectAndAlert(Url.Action("IPBlackIndex", new { }), "成功");
            }
        }
        #endregion

        #region 鎖定/解鎖IP黑名單
        [UserLoginAuth(MappingMethod = "IPBlackIndex", Action = MappingMethodAction.Edit)]
        public ActionResult IPBlackUpdate(long Sn, int Status, string IP)
        {
            ViewBag.IP = IP;
            ViewBag.Sn = Sn;
            ViewBag.Status = Status;
            return View();
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "IPBlackIndex", Action = MappingMethodAction.Edit)]
        public ActionResult IPBlackUpdate(IPBlackUpdateModel model)
        {
            model.Modifier = CurrentUser.Account;
            model.ProxyIP = ProxyIP;
            model.RealIP = RealIP;

            var result = _customerSecurityManageCommand.UpdateIPBlackList(model);

            if (Request.IsAjaxRequest())
            {
                return Json(new { result.RtnMsg, result.RtnCode, success = true, IP = model.IP }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectAndAlert(Url.Action("IPBlackIndex", new { }), "成功");
            }
        }
        #endregion

        #region IP黑名單歷程
        [UserLoginAuth(MappingMethod = "IPBlackIndex", Action = MappingMethodAction.Query)]
        public ActionResult ListIPBlackListLog(string IP)
        {
            var result = _customerSecurityManageCommand.ListIPBlackListLog(IP);
            ViewBag.IP = IP;
            return View("IPBlackListLog", result);
        }
        #endregion

        #region IP黑名單匯出CSV
        public ActionResult IPBlackListExportCSV(string IP)
        {
            IPBlackQryModel query = new IPBlackQryModel
            {
                PageSize = 65535,
                IP = IP
            };

            MemoryStream fileStream = _customerSecurityManageCommand.GetIPBlackExport(query); 

            string fileName = Server.UrlPathEncode(string.Format("RegistIPBlacklist_{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss")));

            fileStream.Flush();
            fileStream.Position = 0;

            return File(fileStream, "application/ms-excel", fileName);            
           
        }       

        #endregion

        #region IP黑名單Log匯出CSV
        public ActionResult IPBlackListExportLogCSV(string IP)
        {

            var fileStream = _customerSecurityManageCommand.GetIPBlackLogExport(IP); 

            string fileName = Server.UrlPathEncode(string.Format("RegistIPBlacklistRecord_{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss")));

            fileStream.Flush();
            fileStream.Position = 0;

            return File(fileStream, "application/ms-excel", fileName);
            
        }        
        #endregion

        #endregion

        #region 身份證黑名單相關

        #region 新增身份證黑名單
        [UserLoginAuth(MappingMethod = "IDNOBlackIndex", Action = MappingMethodAction.Edit)]
        public ActionResult IDNOBlackAdd()
        {
            return View();
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "IDNOBlackIndex", Action = MappingMethodAction.Edit)]
        public ActionResult IDNOBlackAdd(IDNOBlackAddOrUpdateModel model)
        {
            model.ModifyUser = CurrentUser.Account;
            model.Status = 1;
            model.IsAdd = 1;
            model.RealIP = RealIP;
            model.ProxyIP = ProxyIP;

            var result = _customerSecurityManageCommand.AddOrUpdateIDNOBlackList(model);

            if (Request.IsAjaxRequest())
            {
                if (result.RtnCode == 1)
                {
                    return Json(new { result.RtnMsg, result.RtnCode, success = true, IDNO = model.IDNO }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(result);
                }
            }
            else
            {
                return RedirectAndAlert(Url.Action("IDNOBlackIndex", new { }), "成功");
            }
        }
        #endregion

        #region 新增/解鎖/封鎖身份證黑名單
        [UserLoginAuth(MappingMethod = "IDNOBlackIndex", Action = MappingMethodAction.Edit)]
        public ActionResult IDNOBlackUpdate(string IDNO, int Status)
        {
            ViewBag.IDNO = IDNO;
            ViewBag.Status = Status;
            return View();
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "IDNOBlackIndex", Action = MappingMethodAction.Edit)]
        public ActionResult IDNOBlackUpdate(IDNOBlackAddOrUpdateModel model)
        {
            model.ModifyUser = CurrentUser.Account;
            
            var result = _customerSecurityManageCommand.AddOrUpdateIDNOBlackList(model);

            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("IDNOBlackIndex", new { }), "成功");
            }
        }
        #endregion

        #region 身份證黑名單列表
        [UserLoginAuth(MappingMethod = "IDNOBlackIndex", Action = MappingMethodAction.Query)]
        public ActionResult IDNOBlackIndex()
        {
            return View();
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "IDNOBlackIndex", Action = MappingMethodAction.Query)]
        public ActionResult IDNOBlackQuery(IDNOBlackQryModel query)
        {
            ViewBag.QueryModel = query;

            var list = _customerSecurityManageCommand.ListIDNOBlackList(query);
            return PagedListView(list, query);
        }
        #endregion

        #region 身份證黑名單Log列表
        [UserLoginAuth(MappingMethod = "IDNOBlackIndex", Action = MappingMethodAction.Query)]
        public ActionResult ListIDNOBlackListLog(string IDNO)
        {
            var result = _customerSecurityManageCommand.ListIDNOBlackListLog(IDNO);
            ViewBag.IDNO = IDNO;
            return View("IDNOBlackListLog", result);
        }
        #endregion

        #region 身份證黑名單匯出CSV
        public ActionResult IDNOBlackListExportCSV(string IDNO)
        {            
            IDNOBlackQryModel query = new IDNOBlackQryModel
            {
                PageSize = 65535,
                IDNO = IDNO
            };

            MemoryStream fileStream = _customerSecurityManageCommand.GetIDNOBlackExport(query);

            string fileName = Server.UrlPathEncode(string.Format("IDBlacklist_{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss")));

            fileStream.Flush();
            fileStream.Position = 0;

            return File(fileStream, "application/ms-excel", fileName);

        }

        public ActionResult IDNOBlackListLogExportCSV(string IDNO)
        {                        

            MemoryStream fileStream = _customerSecurityManageCommand.GetIDNOBlackLogExport(IDNO);

            string fileName = Server.UrlPathEncode(string.Format("IDBlacklistRecord_{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss")));

            fileStream.Flush();
            fileStream.Position = 0;

            return File(fileStream, "application/ms-excel", fileName);
            
        }

        #endregion

        #endregion

        #region OTP黑名單相關

        #region 列表-[OTP黑名單]
        [UserLoginAuth(MappingMethod = "OTPBlackIndex", Action = MappingMethodAction.Query)]
        public ActionResult OTPBlackIndex()
        {
            var viewModel = new OTPBlackQryVM
            {
                StartDate = DateTime.Now.ToString("yyyy-MM-01"),
                EndDate = DateTime.Now.ToString("yyyy-MM-dd")
            };
            return View(viewModel);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "OTPBlackIndex", Action = MappingMethodAction.Query)]
        public ActionResult OTPBlackQuery(OTPBlackQryVM query)
        {
            
            ViewBag.QueryModel = query;

            var list = _customerSecurityManageCommand.ListBlackOTP(query);
            return PagedListView(list, query);
            
        }
        #endregion

        #region 新增-[OTP黑名單]
        [UserLoginAuth(MappingMethod = "OTPBlackIndex", Action = MappingMethodAction.Edit)]
        public ActionResult AddBlackOTP()
        {
            return View("OTPBlackAdd");
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "OTPBlackIndex", Action = MappingMethodAction.Edit)]
        public ActionResult AddBlackOTP(OTPBlackLockModel model)
        {

            model.LockType = 1;
            model.LockUser = CurrentUser.Account;
            model.ProxyIP = ProxyIP;
            model.RealIP = RealIP;

            var result = _customerSecurityManageCommand.AddBlackOTP(model);

            if (Request.IsAjaxRequest())
            {
                if (result.RtnCode == 1)
                {
                    return Json(new { result.RtnMsg, result.RtnCode, success = true, CellPhone = model.CellPhone }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(result);
                }
                
            }
            else
            {
                return RedirectAndAlert(Url.Action("BlackOTPIndex", new { }), "成功");
            }
        }
        #endregion

        #region 鎖定-[OTP黑名單]
        [UserLoginAuth(MappingMethod = "OTPBlackIndex", Action = MappingMethodAction.Edit)]
        public ActionResult LockBlackOTP(string CellPhone, int Status)
        {
            ViewBag.CellPhone = CellPhone;
            ViewBag.Status = Status;
            return View("OTPBlackUpdate");
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "OTPBlackIndex", Action = MappingMethodAction.Edit)]
        public ActionResult LockBlackOTP(OTPBlackUpdateModel model)
        {
            OTPBlackLockModel dataModel = new OTPBlackLockModel
            {
                CellPhone = model.CellPhone,
                LockType = 1,
                LockMemo = model.Memo,
                LockUser = CurrentUser.Account,
                ProxyIP = ProxyIP,
                RealIP = RealIP
            };

            var result = _customerSecurityManageCommand.AddBlackOTP(dataModel);

            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("BlackOTPIndex", new { }), "成功");
            }
        }
        #endregion

        #region 解鎖-[OTP黑名單]
        [UserLoginAuth(MappingMethod = "OTPBlackIndex", Action = MappingMethodAction.Edit)]
        public ActionResult UnLockBlackOTP(string CellPhone, int Status)
        {
            ViewBag.CellPhone = CellPhone;
            ViewBag.Status = Status;
            return View("OTPBlackUpdate");
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "OTPBlackIndex", Action = MappingMethodAction.Edit)]
        public ActionResult UnLockBlackOTP(OTPBlackUpdateModel model)
        {
            OTPBlackUnLockModel dataModel = new OTPBlackUnLockModel {
                CellPhone = model.CellPhone,
                LockType = 1,
                UnLockUser = CurrentUser.Account,
                UnLockMemo = model.Memo,
                ProxyIP = ProxyIP,
                RealIP = RealIP
            };

            var result = _customerSecurityManageCommand.UnLockBlackOTP(dataModel);

            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("BlackOTPIndex", new { }), "成功");
            }
        }
        #endregion

        #region 列表-[OTP黑名單歷程紀錄]
        [UserLoginAuth(MappingMethod = "OTPBlackIndex", Action = MappingMethodAction.Query)]
        public ActionResult OTPBlackListLog(string CellPhone)
        {
            var result = _customerSecurityManageCommand.ListOTPLog(CellPhone);
            ViewBag.CellPhone = CellPhone;
            return View(result);
        }
        #endregion

        #region OTP黑名單匯出CSV        
        public ActionResult OTPBlackListExportCSV(string StartDate, string EndDate, string CellPhone, string IDNO, string Email)
        {            
            OTPBlackQryVM query = new OTPBlackQryVM {
                StartDate = StartDate,
                EndDate = EndDate,
                CellPhone = (CellPhone == "") ? null: CellPhone,
                IDNO = (IDNO == "") ? null : IDNO,
                Email = (Email == "") ? null : Email,
                PageSize = 65535
            };

            MemoryStream fileStream = _customerSecurityManageCommand.GetOTPBlackExport(query);

            string fileName = Server.UrlPathEncode(string.Format("OTPBlacklist_{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss")));

            fileStream.Flush();
            fileStream.Position = 0;

            return File(fileStream, "application/ms-excel", fileName);
                       
        }
        #endregion

        #region OTP黑名單Log匯出CSV
        public ActionResult OTPBlackLogExportCSV(string CellPhone)
        {   
            MemoryStream fileStream = _customerSecurityManageCommand.GetOTPBlackLogExport(CellPhone);

            string fileName = Server.UrlPathEncode(string.Format("OTPBlacklistRecord_{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss")));

            fileStream.Flush();
            fileStream.Position = 0;

            return File(fileStream, "application/ms-excel", fileName);
        }      
        #endregion

        #endregion

        #region 提領限制黑名單相關

        #region 新增提領限制黑名單
        [UserLoginAuth(MappingMethod = "TakeCashLimitIndex", Action = MappingMethodAction.Edit)]
        public ActionResult TakeCashLimitAdd()
        {
            return View();
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "TakeCashLimitIndex", Action = MappingMethodAction.Edit)]
        public ActionResult TakeCashLimitAdd(TakeCashLimitAddOrUpdateModel model)
        {
            model.ModifyUser = CurrentUser.Account;
            model.Status = 1;
            model.IsAdd = 1;
            model.RealIP = RealIP;
            model.ProxyIP = ProxyIP;

            var result = _customerSecurityManageCommand.AddOrUpdateTakeCashLimitList(model);

            if (Request.IsAjaxRequest())
            {

                if (result.RtnCode == 1)
                {
                    return Json(new { result.RtnMsg, result.RtnCode, success = true, ICPMID = model.ICPMID }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(result);
                }
            }
            else
            {
                return RedirectAndAlert(Url.Action("TakeCashLimitIndex", new { }), "成功");
            }
        }
        #endregion


        #region 新增/解鎖/封鎖提領限制黑名單
        [UserLoginAuth(MappingMethod = "TakeCashLimitIndex", Action = MappingMethodAction.Edit)]
        public ActionResult TakeCashLimitUpdate(string ICPMID, int Status)
        {
            ViewBag.ICPMID = ICPMID;
            ViewBag.Status = Status;
            return View();
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "TakeCashLimitIndex", Action = MappingMethodAction.Edit)]
        public ActionResult TakeCashLimitUpdate(TakeCashLimitAddOrUpdateModel model)
        {
            model.ModifyUser = CurrentUser.Account;
            model.ProxyIP = ProxyIP;
            model.RealIP = RealIP;

            var result = _customerSecurityManageCommand.AddOrUpdateTakeCashLimitList(model);

            if (Request.IsAjaxRequest())
            {
                return Json(result);
            }
            else
            {
                return RedirectAndAlert(Url.Action("TakeCashLimitIndex", new { }), "成功");
            }
        }
        #endregion 

        #region 提領限制黑名單列表
        [UserLoginAuth(MappingMethod = "TakeCashLimitIndex", Action = MappingMethodAction.Query)]
        public ActionResult TakeCashLimitIndex()
        {
            var viewModel = new TakeCashLimitQryVM
            {
                StartDate = DateTime.Now.ToString("yyyy-MM-01"),
                EndDate = DateTime.Now.ToString("yyyy-MM-dd")
            };
            return View(viewModel);
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "TakeCashLimitIndex", Action = MappingMethodAction.Query)]
        public ActionResult TakeCashLimitQuery(TakeCashLimitQryVM query)
        {
            ViewBag.QueryModel = query;

            var list = _customerSecurityManageCommand.ListTakeCashLimitList(query);
            return PagedListView(list, query);
        }
        #endregion

        #region 提領限制黑名單Log列表
        [UserLoginAuth(MappingMethod = "TakeCashLimitIndex", Action = MappingMethodAction.Query)]
        public ActionResult ListTakeCashLimitListLog(string MID)
        {
            var result = _customerSecurityManageCommand.ListTakeCashLimitListLog(MID);
            ViewBag.MID = MID;
            return View("TakeCashLimitListLog", result);
        }
        #endregion

        #region 提領限制黑名單匯出CSV
        public ActionResult TakeCashLimitListExportCSV(string StartDate, string EndDate, string CellPhone, string Email, string IDNO, string ICPMID)
        {
            TakeCashLimitQryVM query = new TakeCashLimitQryVM
            {
                PageSize = 65535,
                ICPMID = (ICPMID == "") ? null : ICPMID,
                StartDate = StartDate,
                EndDate = EndDate,
                CellPhone = (CellPhone == "") ? null : CellPhone,
                IDNO = (IDNO == "") ? null : IDNO,
                Email = (Email == "") ? null : Email
            };

            MemoryStream fileStream = _customerSecurityManageCommand.GetTakeCashLimitListExport(query);

            string fileName = Server.UrlPathEncode(string.Format("WithdrawBlacklist_{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss")));

            fileStream.Flush();
            fileStream.Position = 0;

            return File(fileStream, "application/ms-excel", fileName);
            
        }

        public ActionResult TakeCashLimitListLogExportCSV(string MID)
        {
            MemoryStream fileStream = _customerSecurityManageCommand.GetTakeCashLimitListLogExport(MID);

            string fileName = Server.UrlPathEncode(string.Format("WithdrawBlacklistRecord_{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss")));

            fileStream.Flush();
            fileStream.Position = 0;

            return File(fileStream, "application/ms-excel", fileName);
            
        }        

        #endregion

        #endregion

        #region 註冊同IP預警名單相關


        #region 註冊同IP預警名單列表
        [UserLoginAuth(MappingMethod = "RegistIPBlackIndex", Action = MappingMethodAction.Query)]
        public ActionResult RegistIPBlackIndex()
        {
            return View();
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "RegistIPBlackIndex", Action = MappingMethodAction.Query)]
        public ActionResult RegistIPBlackQuery(RegistIPBlackQryModel query)
        {
            ViewBag.QueryModel = query;

            var list = _customerSecurityManageCommand.ListRegistIPList(query);
            return PagedListView(list, query);
        }
        #endregion

        #region 註冊同IP預警名單明細        
        [UserLoginAuth(MappingMethod = "RegistIPBlackIndex", Action = MappingMethodAction.Query)]
        public ActionResult ListRegistIPListLog(string IP)
        {
            var result = _customerSecurityManageCommand.ListRegistIPListLog(IP);
            ViewBag.IP = IP;
            return View("RegistIPBlackListLog", result);
        }
        #endregion        

        #region 註冊同IP加入黑名單
        [UserLoginAuth(MappingMethod = "RegistIPBlackIndex", Action = MappingMethodAction.Edit)]
        public ActionResult AddRegistIPBlack(string IP)
        {
            ViewBag.IP = IP;
            return View("RegistIPBlackAdd");
        }

        [HttpPost]
        [UserLoginAuth(MappingMethod = "RegistIPBlackIndex", Action = MappingMethodAction.Edit)]
        public ActionResult AddRegistIPBlack(IPBlackAddModel model)
        {
            model.CreateUser = CurrentUser.Account;
            model.Status = 1;

            var result = _customerSecurityManageCommand.AddIPBlackList(model);

            return Json(result);             

        }
        #endregion

        #region 註冊同IP黑名單匯出CSV
        public ActionResult RegistBlackListExportCSV(string IP)
        {
            string UserID = CurrentUser.Account;

            RegistIPBlackQryModel query = new RegistIPBlackQryModel
            {
                PageSize = 65535,
                IP = IP
            };

            MemoryStream fileStream = _customerSecurityManageCommand.GetRegistBlackListExport(query);

            string fileName = Server.UrlPathEncode(string.Format("SameIPList_{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss")));

            fileStream.Flush();
            fileStream.Position = 0;

            return File(fileStream, "application/ms-excel", fileName);            
        }        

        #endregion

        #region 註冊同IP黑名單Log匯出CSV
        public ActionResult RegistIPBlackListExportLogCSV(string IP)
        {
            MemoryStream fileStream = _customerSecurityManageCommand.GetRegistIPBlackListLogExport(IP);

            string fileName = Server.UrlPathEncode(string.Format("SameIPRegistList_{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss")));

            fileStream.Flush();
            fileStream.Position = 0;

            return File(fileStream, "application/ms-excel", fileName);
            
        }
        #endregion

        #endregion

        
    }
}
