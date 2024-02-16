using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Helpers;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Models.Consts;
using ICP.Modules.Mvc.Admin.Models;
using ICP.Modules.Mvc.Admin.Models.ViewModels;
using ICP.Modules.Mvc.Admin.Repositories;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Services
{
    public class SuspenseMainService
    {
        SuspenseMainRepository _suspenseMainRepository;
        ConfigRepository _configRepository;
        NetworkHelper _networkHelper;

        public SuspenseMainService(
            SuspenseMainRepository suspenseMainRepository,
            ConfigRepository configRepository
            )
        {
            _suspenseMainRepository = suspenseMainRepository;
            _configRepository = configRepository;
            _networkHelper = new NetworkHelper();
        }

        /// <summary>
        /// 新增會員黑名單
        /// </summary>
        /// <param name="model"></param>
        /// <param name="CreateUser"></param>
        /// <param name="RealIP"></param>
        /// <param name="ProxyIP"></param>
        /// <returns></returns>
        public DataResult<long> AddSuspenseMain(SuspenseMain model, string CreateUser, long RealIP, long ProxyIP)
        {
            return _suspenseMainRepository.AddSuspenseMain(model, CreateUser, RealIP, ProxyIP);
        }

        /// <summary>
        /// 判斷查詢格式
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult VerifyQueryModel(QuerySuspenseMainVM model)
        {
            var result = new BaseResult();
            result.SetError();

            if (!string.IsNullOrWhiteSpace(model.CellPhone))
            {
                if (!Regex.IsMatch(model.CellPhone, RegexConst.CellPhone))
                {
                    result.RtnMsg = "請輸入正確的手機號碼";
                    return result;
                }
            }

            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                if (!Regex.IsMatch(model.Email, RegexConst.Email))
                {
                    result.RtnMsg = "請輸入正確的電子郵件";
                    return result;
                }
            }

            if (!string.IsNullOrWhiteSpace(model.IDNO))
            {
                if (!Regex.IsMatch(model.IDNO, RegexConst.IDNO) && !Regex.IsMatch(model.IDNO, RegexConst.UniformID))
                {
                    result.RtnMsg = "請輸入正確的身分證字號";
                    return result;
                }
            }

            result.SetSuccess();
            return result;
        }

        /// <summary>
        /// 取得會員黑名單列表
        /// </summary>
        /// <returns></returns>
        public List<SuspenseMainVM> ListSuspenseMain(QuerySuspenseMainVM model)
        {
            return _suspenseMainRepository.ListSuspenseMain(model);
        }

        /// <summary>
        /// 取得懲處方式列表
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Category"></param>
        /// <returns></returns>
        public List<SuspenseSetting> ListSuspenseSetting()
        {
            return _suspenseMainRepository.ListSuspenseSetting();
        }

        /// <summary>
        /// 取得會員黑名單紀錄列表
        /// </summary>
        /// <param name="SuspenseID"></param>
        /// <returns></returns>
        public List<SuspenseMainLogVM> ListSuspenseMainLog(long SuspenseID)
        {
            return _suspenseMainRepository.ListSuspenseMainLog(SuspenseID);
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
            return _suspenseMainRepository.UpdateSuspenseMain(model, Modifier, RealIP, ProxyIP);
        }

        /// <summary>
        /// 取得單筆會員黑名單
        /// </summary>
        /// <param name="SuspenseID"></param>
        /// <returns></returns>
        public SuspenseMain GetSuspenseMain(long SuspenseID)
        {
            return _suspenseMainRepository.GetSuspenseMain(SuspenseID);
        }

        /// <summary>
        /// 驗證更新資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult VerifyUpdateModel(UpdateSuspenseMainVM model)
        {
            var result = new BaseResult();
            result.SetError();

            if (string.IsNullOrWhiteSpace(model.Note))
            {
                result.RtnMsg = "請輸入停權記錄說明";
                return result;
            }

            result.SetSuccess();
            return result;
        }

        /// <summary>
        /// 取得有功能權限的使用者信箱
        /// </summary>
        /// <param name="ControllerName"></param>
        /// <param name="MethodName"></param>
        /// <param name="Action"></param>
        /// <returns></returns>
        public List<string> ListUserEmailByFunctionCategory(string ControllerName, string MethodName, int Action)
        {
            return _suspenseMainRepository.ListUserEmailByFunctionCategory(ControllerName, MethodName, Action);
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
            return _suspenseMainRepository.UnlockSuspenseMain(SuspenseID, Modifier, RealIP, ProxyIP);
        }

        public BaseResult CloseMemberAccount(long MID, string Modifier)
        {
            string memberDomain = _configRepository.MemberDomain;

            string url = $"{memberDomain}/api/Member/Admin/CloseMemberAccount";

            var model = new
            {
                MID,
                Modifier
            };

            int timeoutSecond = 5;
            var apiResult = _networkHelper.DoRequestWithJson(url, JsonConvert.SerializeObject(model), timeoutSecond, null, null);

            return JsonConvert.DeserializeObject<BaseResult>(apiResult);
        }
    }
}
