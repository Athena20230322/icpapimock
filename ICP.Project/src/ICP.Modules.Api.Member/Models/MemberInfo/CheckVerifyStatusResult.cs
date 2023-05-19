using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    using Library.Models.AuthorizationApi;

    public class CheckVerifyStatusResult: BaseAuthorizationApiResult
    {
        /// <summary>
        /// 是否有設定安全密碼
        /// </summary>
        public bool IsPwdPass { get; set; }

        /// <summary>
        /// 是否有設定圖型鎖
        /// </summary>
        public bool GraphicLock { get; set; }

        /// <summary>
        /// 是否有通過身份證驗證
        /// </summary>
        public bool IsIDNOPass { get; set; }

        /// <summary>
        /// 是否通過未成年驗證
        /// </summary>
        public bool IsTeenagersPass { get; set; }

        /// <summary>
        /// 登入密碼設定狀態
		/// 0：設定完成
		/// 1：需重設登入密碼(員工預設密碼)
		/// 2：超過一年未修改
		/// 3：超過一年未修改且略過密碼修改又超過一年
        /// </summary>
        public int LoginPwdStatus { get; set; }

        /// <summary>
        /// 安全密碼設定狀態
		/// 0：設定完成
		/// 1：需重設安全密碼
		/// 2：超過一年未修改
		/// 3：超過一年未修改且略過密碼修改又超過一年
        /// </summary>
        public int SecPwdStatus { get; set; }

        /// <summary>
        /// 圖形鎖設定引導狀態
		/// 0：設定完成
		/// 1：需重設圖形密碼
		/// 2：超過一年未修改
		/// 3：超過一年未修改且略過密碼修改又超過一年
        /// </summary>
        public int GraphicLockPwdStatus { get; set; }

        public string SecPwdRtnMsg { get; set; }

        public int NextStep { get; set; }

        /// <summary>
        /// 是否為員工預設帳號
        /// </summary>
        public bool IsDefaultAct { get; set; }
    }
}