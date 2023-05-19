using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using ICP.Modules.Api.Member.Repositories;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Helpers;
using ICP.Modules.Api.Member.Models.MemberInfo;

namespace ICP.Modules.Api.Member.Services
{
    public class MemberGraphicLockService
    {
        private readonly MemberGraphicLockRepository _memberGraphicLockRepository = null;
        private readonly ILogger _logger = null;

        public MemberGraphicLockService(
            MemberGraphicLockRepository memberGraphicLockRepository,
            ILogger<MemberGraphicLockService> logger)
        {
            _memberGraphicLockRepository = memberGraphicLockRepository;
            _logger = logger;
        }

        #region 取得圖型鎖相關資訊
        public MemberGraphicLock GetMemberGraphicLock(long MerchantID)
        {            
            return _memberGraphicLockRepository.GetMemberGraphicLock(MerchantID);
        }
        #endregion

        #region 設定/更新 圖型驗證鎖
        public BaseResult UpdateGraphicPassword(long MID, GraphicLockRerquest model, long RealIP, long ProxyIP, string DeviceID)
        {                                            
            return _memberGraphicLockRepository.UpdateGraphicPassword(MID, model, RealIP, ProxyIP, DeviceID);
        }
        #endregion

        #region 驗證APP圖型鎖
        /// <summary>
        /// 檢查APP圖型鎖是否正確
        /// </summary>
        /// <param name="MID">會員代號</param>=
        /// <param name="PayPwd">圖型鎖</param>
        /// <returns></returns>
        public BaseResult CheckGraphicLock(long MID, string GraphicPwd, long longRealIP, long longProxyIP)
        {       
            return _memberGraphicLockRepository.CheckGraphicLock(MID, GraphicPwd, longRealIP, longProxyIP);
        }

        #endregion

        #region 檢查 圖型鎖 是否正確
        /// <summary>
        /// 檢查 圖型鎖 是否正確
        /// </summary>
        /// <param name="GraphicPwd">圖型鎖</param>
        /// <param name="mid">會員編號</param>
        /// <returns></returns>
        public bool CheckOldGraphicPwdSame(string GraphicPwd, long MID)
        {
            return _memberGraphicLockRepository.CheckOldGraphicPwdSame(GraphicPwd, MID);
        }
        #endregion

        #region  M0040 記錄略過修改圖形密碼
        public BaseResult UpdateGraphicPwdIgnorDate(long MID, long RealIP, long ProxyIP)
        {
            return _memberGraphicLockRepository.UpdateGraphicPwdIgnorDate(MID, RealIP, ProxyIP);
        }
        #endregion

        #region 取得使用圖型鎖記錄(未設定過圖型鎖)
        public GetAppGraphicDataLog GetMemberGraphicDataLog(long MID)
        {
            return _memberGraphicLockRepository.GetMemberGraphicDataLog(MID);
        }
        #endregion

        #region 驗證圖形鎖
        public bool AuthGraphicLockProcess(string GraphicPwd, string Vers, ref string errorMessage)
        {
            bool isCheck = true;

            errorMessage = "您的圖形鎖不符合規則";

            if (string.IsNullOrWhiteSpace(GraphicPwd)) { isCheck = false; }

            return isCheck;
        }
        #endregion


        public DataResult<GraphicLockRerquest> ValidGraphicPwdRepeat(long mid, string graphicPwd, string oriGraphicPwd)
        {
            var result = new DataResult<GraphicLockRerquest>();
            result.SetError();

            GraphicLockRerquest GraphicPwdModel =
                new GraphicLockRerquest()
                {
                    //MID = mid,
                    OriPassword = oriGraphicPwd != null ? oriGraphicPwd : string.Empty,
                    NewPassword = graphicPwd,
                    //PwdUpgradeDate = null
                };

            //### 不可與前一次相同
            if (GraphicPwdModel.OriPassword == GraphicPwdModel.NewPassword)
            {
                result.SetCode(200003, "圖形密碼"); // 您輸入的新圖形密碼與原圖形密碼相同
                return result;
            }

            result.SetSuccess(GraphicPwdModel);
            return result;
        }

        public BaseResult GraphicPwdConfirm(ChangeGraphicLockRequest request)
        {
            var result = new BaseResult();
            result.SetError();

            if (request.NewGraphicPwd != request.ConfirmGraphicPwd)
            {
                result.SetCode(200004, "圖形密碼"); // 圖形密碼兩次輸入不同，請重新輸入
            }
            else
            {
                result.SetSuccess();
            }

            return result;
        }

        public BaseResult UpdateGraphicLockStatus(long mid, string DeviceID, bool? status, long realIP, long poxyIP)
        {
            var result = new BaseResult();
            result.SetError();
            bool dbStatus = GetGraphicLockStatus(mid);

            GraphicLockStatusModel model = new GraphicLockStatusModel
            {
                DeviceID = DeviceID,
                MID = mid,
                RealIP = realIP,
                ProxyIP = poxyIP,
                Status = (status == null) ? dbStatus : status.GetValueOrDefault()
            };

            result = _memberGraphicLockRepository.UpdateGraphicStatus(model);
            if (result.RtnCode != 1)
            {
                result.SetCode(0);
                return result;
            }

            result.SetSuccess();
            return result;

        }

        /// <summary>
        /// 取得圖型鎖密碼狀態
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public bool GetGraphicLockStatus(long mid)
        {
            var model = _memberGraphicLockRepository.GetMemberGraphicLock(mid);            
            return (model != null && model.PasswordSwitch == true);
        }
    }
}