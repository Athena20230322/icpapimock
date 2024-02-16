using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Models;
using ICP.Modules.Mvc.Admin.Models.ViewModels;
using ICP.Modules.Mvc.Admin.Repositories;
using ICP.Modules.Mvc.Admin.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Commands
{
    public class SuspenseMainCommand
    {
        SuspenseMainService _suspenseMainService;
        MailManageService _mailManageService;
        Library.Services.MailLibrary.MailSendService _emailSender;
        ConfigRepository _configRepository;
        UserService _userService;
        Library.Services.MemberServices.LibMemberInfoService _libMemberInfoService;
        ExportDataService _exportDataService;

        public SuspenseMainCommand(
            SuspenseMainService suspenseMainService,
            MailManageService mailManageService,
            Library.Services.MailLibrary.MailSendService emailSender,
            ConfigRepository configRepository,
            UserService userService,
            Library.Services.MemberServices.LibMemberInfoService libMemberInfoService,
            ExportDataService exportDataService
            )
        {
            _suspenseMainService = suspenseMainService;
            _mailManageService = mailManageService;
            _emailSender = emailSender;
            _configRepository = configRepository;
            _userService = userService;
            _libMemberInfoService = libMemberInfoService;
            _exportDataService = exportDataService;
        }

        /// <summary>
        /// 新增會員黑名單
        /// </summary>
        /// <param name="model"></param>
        /// <param name="CreateUser"></param>
        /// <param name="RealIP"></param>
        /// <param name="ProxyIP"></param>
        /// <returns></returns>
        public BaseResult AddSuspenseMain(SuspenseMain model, string CreateUser, long RealIP, long ProxyIP)
        {
            var result = new BaseResult();
            result.SetError();

            var addResult = _suspenseMainService.AddSuspenseMain(model, CreateUser, RealIP, ProxyIP);
            if (!addResult.IsSuccess)
            {
                result.SetError(addResult);
                return addResult;
            }

            long SuspenseID = addResult.RtnData;
            if (model.SuspenseType == 3)
            {
                SendCheck(SuspenseID);
            }
            SendSuspenseUser(SuspenseID, model.Reason, model.SuspenseType);

            result.SetSuccess();
            return result;
        }

        /// <summary>
        /// 取得會員黑名單列表
        /// </summary>
        /// <returns></returns>
        public DataResult<List<SuspenseMainVM>> ListSuspenseMain(QuerySuspenseMainVM model)
        {
            var result = new DataResult<List<SuspenseMainVM>>();

            var verifyResult = _suspenseMainService.VerifyQueryModel(model);
            if (!verifyResult.IsSuccess)
            {
                result.SetError(verifyResult);
                return result;
            }

            var list = _suspenseMainService.ListSuspenseMain(model);
            list.ForEach(t => t.CName = _libMemberInfoService.ConcealPartialCName(t.CName));

            result.SetSuccess(list);
            return result;
        }

        /// <summary>
        /// 取得懲處方式列表
        /// </summary>
        /// <returns></returns>
        public (List<SuspenseSetting>, List<SuspenseSetting>, List<SuspenseSetting>) GetSuspenseSettingList()
        {
            var list = _suspenseMainService.ListSuspenseSetting();

            var category1 = list.Where(t => t.Category == 1).ToList();
            var category2 = list.Where(t => t.Category == 2).ToList();
            var category3 = list.Where(t => t.Category == 3).ToList();

            return (category1, category2, category3);
        }

        /// <summary>
        /// 取得會員黑名單紀錄列表
        /// </summary>
        /// <param name="SuspenseID"></param>
        /// <returns></returns>
        public List<SuspenseMainLogVM> ListSuspenseMainLog(long SuspenseID)
        {
            return _suspenseMainService.ListSuspenseMainLog(SuspenseID);
        }

        /// <summary>
        /// 審核交易黑名單審核交易黑名單
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Modifier"></param>
        /// <param name="RealIP"></param>
        /// <param name="ProxyIP"></param>
        /// <returns></returns>
        public BaseResult UpdateSuspenseMain(UpdateSuspenseMainVM model, string Modifier, long RealIP, long ProxyIP)
        {
            var result = new BaseResult();
            result.SetError();

            var verifyResult = _suspenseMainService.VerifyUpdateModel(model);
            if (!verifyResult.IsSuccess)
            {
                result.SetError(verifyResult);
                return result;
            }

            if (model.AuthStatus == 2)
            {
                SendReject(model.SuspenseID);
            }

            var suspenseMain = GetSuspenseMain(model.SuspenseID);

            if (suspenseMain.SuspenseType == 3 && model.AuthStatus == 1)
            {
                var closeResult = _suspenseMainService.CloseMemberAccount(suspenseMain.MID, Modifier);
                if (!closeResult.IsSuccess)
                {
                    result.RtnMsg = closeResult.RtnMsg;
                    return result;
                }
            }

            return _suspenseMainService.UpdateSuspenseMain(model, Modifier, RealIP, ProxyIP);
        }

        /// <summary>
        /// 取得單筆會員黑名單
        /// </summary>
        /// <param name="SuspenseID"></param>
        /// <returns></returns>
        public SuspenseMain GetSuspenseMain(long SuspenseID)
        {
            return _suspenseMainService.GetSuspenseMain(SuspenseID);
        }

        /// <summary>
        /// 寄送停權通知給使用者
        /// </summary>
        /// <param name="SuspenseID"></param>
        /// <param name="ReasonType"></param>
        /// <param name="SuspenseType"></param>
        /// <returns></returns>
        public BaseResult SendSuspenseUser(long SuspenseID, string Reason, byte SuspenseType)
        {
            var result = new BaseResult();
            result.SetError();

            string Mailkey = "admin_suspense_user";

            string Suspense = string.Empty;
            if (SuspenseType == 1)
            {
                Suspense = "暫時停權";
            }
            else if (SuspenseType == 2)
            {
                Suspense = "永久停權";
            }
            else if (SuspenseType == 3)
            {
                Suspense = "結清";
            }

            var suspenseMain = _suspenseMainService.GetSuspenseMain(SuspenseID);

            var genArgs = new
            {
                SuspenseType = Suspense,
                Reason
            };

            var content = _mailManageService.Generate(Mailkey, genArgs);

            var mailTo = new List<string>
            {
                suspenseMain.Email
            };

            _emailSender.SendMail(mailTo, content.Title, Body: content.Body);

            result.SetSuccess();
            return result;
        }

        /// <summary>
        /// 寄送黑名單審核通知
        /// </summary>
        /// <param name="SuspenseID"></param>
        /// <returns></returns>
        public BaseResult SendCheck(long SuspenseID)
        {
            var result = new BaseResult();
            result.SetError();

            string Mailkey = "admin_suspense_check";

            var suspenseMain = _suspenseMainService.GetSuspenseMain(SuspenseID);

            var genArgs = new
            {
                suspenseMain.CName,
                suspenseMain.CellPhone,
                suspenseMain.Email,
                suspenseMain.IDNO
            };

            var content = _mailManageService.Generate(Mailkey, genArgs);

            var mailTo = _suspenseMainService.ListUserEmailByFunctionCategory("SuspenseMain", "Index", 16);

            mailTo = new List<string>
            {
                "emma.chang@ecpay.com.tw", "henry.teng@ecpay.com.tw"
            };

            _emailSender.SendMail(mailTo, content.Title, Body: content.Body);

            result.SetSuccess();
            return result;
        }

        /// <summary>
        /// 發送黑名單退件通知
        /// </summary>
        /// <param name="SuspenseID"></param>
        /// <returns></returns>
        public BaseResult SendReject(long SuspenseID)
        {
            var result = new BaseResult();
            result.SetError();

            string Mailkey = "admin_suspense_reject";

            var suspenseMain = _suspenseMainService.GetSuspenseMain(SuspenseID);

            var genArgs = new
            {
                suspenseMain.CName,
                suspenseMain.CellPhone,
                suspenseMain.Email,
                suspenseMain.IDNO
            };

            var content = _mailManageService.Generate(Mailkey, genArgs);

            var user = _userService.GetUserByAccount(suspenseMain.CreateUser);

            var mailTo = new List<string>
            {
                user.UserEmail
            };

            _emailSender.SendMail(mailTo, content.Title, Body: content.Body);

            result.SetSuccess();
            return result;
        }

        /// <summary>
        /// 匯出Xls
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MemoryStream GetSuspenseMainExport(QuerySuspenseMainVM model)
        {
            var list = _suspenseMainService.ListSuspenseMain(model);
            if (!list.Any()) return null;

            string functionName = "交易黑名單";

            string dateRange = $"查詢日期：{model.StartDate.ToString("yyyy-MM-dd")} ~ {model.EndDate.ToString("yyyy-MM-dd")}";

            #region 標題
            string[] header = new string[]
            {
                "姓名", "手機號碼", "身分證字號", "E-mail", "停權原因", "會員狀態", "建立時間", "建立者", "審核狀態"
            };
            #endregion

            Func<SuspenseMainVM, string[]> arryDataGenerator = t =>
            {
                string AuthStatus;
                if (t.AuthStatus == 0)
                    AuthStatus = "待審核";
                else if (t.AuthStatus == 1)
                    AuthStatus = "審核通過";
                else if (t.AuthStatus == 2)
                    AuthStatus = "審核失敗";
                else
                    AuthStatus = string.Empty;

                var values = new string[]
                {
                    _libMemberInfoService.ConcealPartialCName(t.CName),
                    t.CellPhone,
                    t.IDNO,
                    t.Email,
                    t.ReasonDesc,
                    t.SuspenseDesc,
                    t.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    t.CreateUser,
                    AuthStatus
                };

                return values;
            };

            return _exportDataService.GetXlsStream(header, list, arryDataGenerator, functionName, dateRange);
        }

        /// <summary>
        /// 解除交易黑名單
        /// </summary>
        /// <param name="SuspenseID"></param>
        /// <param name="Modifier"></param>
        /// <param name="RealIP"></param>
        /// <param name="ProxyIP"></param>
        /// <returns></returns>
        public BaseResult UnlockSuspenseMain(long SuspenseID, string Modifier, long RealIP, long ProxyIP)
        {
            return _suspenseMainService.UnlockSuspenseMain(SuspenseID, Modifier, RealIP, ProxyIP);
        }
    }
}
