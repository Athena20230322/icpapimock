using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    using ICP.Library.Models.MemberModels;
    using Library.Models.AuthorizationApi;

    public class GetLoginInfoResult: BaseAuthorizationApiResult
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// OPEN WALLET會員帳號
        /// </summary>
        public string OpwMID { get; set; }

        /// <summary>
        /// icashpay電支帳號
        /// </summary>
        public string IcpMID { get; set; }

        /// <summary>
        /// 登入TokenID
        /// </summary>
        public string LoginTokenID { get; set; }

        /// <summary>
        /// LoginTokenID有效時間
        /// </summary>
        public string TokenExpireTime { get; set; }

        /// <summary>
        /// 最後登入時間 格式：2019/01/01 00:00:00
        /// </summary>
        public string LastLoginDate { get; set; }

        /// <summary>
        /// 登入帳號
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 會員姓名
        /// </summary>
        public string CName { get; set; }

        /// <summary>
        /// 會員手機號碼
        /// </summary>
        public string CellPhone { get; set; }

        /// <summary>
        /// 會員電子郵件帳號
        /// </summary>
        public string EMail { get; set; }

        /// <summary>
        /// 身份證字號(海外會員為空值)
        /// </summary>
        public string Idno { get; set; }

        /// <summary>
        /// 是否是海外會員
        /// </summary>
        public bool isOverSea { get; set; }

        /// <summary>
        /// 居留證號 False：否, True：是
        /// </summary>
        public string UniformID { get; set; }

        /// <summary>
        /// 手機條碼載具號碼(在OPEN WALLET綁定的)
        /// </summary>
        public string CarrierNum { get; set; }

        /// <summary>
        /// 縣市+鄉鎮市區
        /// </summary>
        public string AreaID { get; set; }

        /// <summary>
        /// 地址(巷弄路段號)
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 是否為未成年會員 False：否, True：是
        /// </summary>
        public bool IsMinor { get; set; }

        /// <summary>
        /// 是否為員工預設帳號 False：否, True：是
        /// </summary>
        public bool IsDefaultAct { get; set; }

        /// <summary>
        /// 手機號碼驗證狀態 False：未驗證, True：已驗證
        /// </summary>
        public bool IsCellPhoneAuth { get; set; }

        /// <summary>
        /// 電子信箱驗證狀態 False：未驗證, True：已驗證
        /// </summary>
        public bool IsEmailAuth { get; set; }

        /// <summary>
        /// 身分驗證狀態
        /// 0：未驗證
        /// 1：待驗證
        /// 2：驗證中
        /// </summary>
        public int IDNOAuthStatus { get; set; }

        /// <summary>
        /// P33驗證狀態
        /// 0：P33未驗證、未通過
        /// 1：P33已驗證
        /// </summary>
        public int IsP33Auth { get; set; }

        /// <summary>
        /// 銀行帳戶驗證狀態
        /// 0：未驗證
        /// 1：已驗證
        /// 2：驗證失敗
        /// </summary>
        public int BankAuthStatus { get; set; }

        /// <summary>
        /// 是否有設定圖型鎖 False：否, True：是
        /// </summary>
        public bool IsExistsSetGraphicLock { get; set; }

        /// <summary>
        /// 圖形鎖設定引導狀態
        /// </summary>
        public bool GraphicLockStatus { get; set; }

        /// <summary>
        /// 指紋/Face ID密碼
        /// 若使用者未開啟指紋認證/Face ID則回傳 null
        /// </summary>
        public string FingerPrintPwd { get; set; }

        /// <summary>
        /// 會員等級
        /// 10:驗過手機
        /// 12:驗過身份驗證(P33、P11一起驗過, 或後台人工驗證)
        /// 13:驗過金融工具(銀行帳戶)
        /// 14:驗過自然人或臨櫃驗證(預留參數)
        /// 31:特店
        /// </summary>
        public byte LevelID { get; set; }

        /// <summary>
        /// 會員類別
        /// 01: 一類會員
        /// 02: 二類會員
        /// </summary>
        public string MemberClass { get; set; }

        /// <summary>
        /// 是否有設定過暱稱
        /// False：否, True：是
        /// </summary>
        public bool IsNickName { get; set; }

        /// <summary>
        /// 暱稱
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 推薦碼
        /// </summary>
        public string RCCode { get; set; }

        /// <summary>
        /// 會員同意項目
        /// </summary>
        public List<MemberAgreeResult> AgreeItems { get; set; }

        /// <summary>
        /// 固定儲值條碼 (中獎發票獎金儲值)
        /// </summary>
        public string Barcode { get; set; }
    }
}