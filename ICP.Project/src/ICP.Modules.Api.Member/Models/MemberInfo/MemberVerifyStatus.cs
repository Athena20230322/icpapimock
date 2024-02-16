using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    using Library.Models.AuthorizationApi;

    public class MemberVerifyStatus
    {
        /// <summary>
        /// 手機號碼驗證狀態
        /// </summary>
        public bool IsCellPhoneAuth { get; set; }

        /// <summary>
        /// 電子信箱驗證狀態
        /// </summary>
        public bool IsEmailAuth { get; set; }

        /// <summary>
        /// P33驗證狀態
        /// </summary>
        public bool IsP33Auth { get; set; }

        /// <summary>
        /// 是否有通過身份證驗證
        /// </summary>
        public bool IsIDNOPass { get; set; }

        /// <summary>
        /// 身分驗證狀態 0：未驗證 1：待驗證 2：驗證中
        /// </summary>
        public int IDNOAuthStatus { get; set; }

        /// <summary>
        /// 銀行帳戶驗證狀態
        /// </summary>
        public int BankAuthStatus { get; set; }

        /// <summary>
        /// 是否有設定安全密碼
        /// </summary>
        public bool IsPwdPass { get; set; }

        /// <summary>
        /// 是否有設定圖型鎖
        /// </summary>
        public bool IsGraphicLock { get; set; }

        /// <summary>
        /// 登入密碼設定狀態
        /// </summary>
        public int LoginPwdStatus { get; set; }

        /// <summary>
        /// 安全密碼設定狀態
        /// </summary>
        public int SecPwdStatus { get; set; }

        /// <summary>
        /// 圖形鎖設定引導狀態
        /// </summary>
        public int GraphicLockStatus { get; set; }

        /// <summary>
        /// 指紋/FaceID密碼
        /// </summary>
        public string FingerPrintPwd { get; set; }

        /// <summary>
        /// 是否通過未成年驗證
        /// </summary>
        public bool IsTeenagersPass { get; set; }

        /// <summary>
        /// 是否是海外會員
        /// </summary>
        public bool IsOverSea { get; set; }

        /// <summary>
        /// 是否為員工預設帳號
        /// </summary>
        public bool IsDefaultAct { get; set; }
    }
}