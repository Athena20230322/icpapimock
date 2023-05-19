using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{
    public class MemberInfoManage : BaseListModel
    {
        
        //IDENTITY(899999999,-1), 會員編號, 我們系統的唯一識別碼
        [Required(ErrorMessage = "必須輸入")]
        [Display(Name = "會員編號")]
        public long MID { get; set; }

        //中文姓名
        [Required(ErrorMessage = "必須輸入")]
        [StringLength(60, ErrorMessage = "長度不可超過60")]
        [Display(Name = "中文姓名")]
        public String CName { get; set; }

        //合作廠商代號(保留)
        [StringLength(10, ErrorMessage = "長度不可超過10")]
        [Display(Name = "合作廠商代號")]
        public String PartnerID { get; set; }

        //合作廠商會員識別碼(保留)
        [StringLength(50, ErrorMessage = "長度不可超過50")]
        [Display(Name = "合作廠商會員識別碼")]
        public String PartnerAccount { get; set; }

        //驗證方式, 採AND運算, 1: Email驗證, 2: 簡訊驗證, 4: 身份證驗證
        [Required(ErrorMessage = "必須輸入")]
        [Display(Name = "驗證方式")]
        public short AuthType { get; set; }

        //會員類型, 採用AND運算, 1: 一般會員, 2: 商務會員, 4:商家
        [Required(ErrorMessage = "必須輸入")]
        [Display(Name = "會員類型")]
        public ushort MemberType { get; set; }

        //防盜卡編號
        [Required(ErrorMessage = "必須輸入")]
        [Display(Name = "防盜卡編號")]
        public long EncryptCardID { get; set; }

        //防盜卡使用狀態 0:預設值 , 1:已啟用
        [Required(ErrorMessage = "必須輸入")]
        [Display(Name = "防盜卡使用狀態")]
        public byte EncryptCardStatus { get; set; }

        //最後登入日期
        [Display(Name = "最後登入日期")]
        public DateTime LastLoginDate { get; set; }

        //建立日期
        [Required(ErrorMessage = "必須輸入")]
        [Display(Name = "建立日期")]
        public DateTime CreateDate { get; set; }

        //會員狀態 0:初始 , 1:正常, 2:停權
        [Required(ErrorMessage = "必須輸入")]
        [Display(Name = "會員狀態")]
        public byte MemberStatus { get; set; }

        //身份證字號
        [StringLength(12, ErrorMessage = "長度不可超過12")]
        [Display(Name = "身份證字號")]
        public String IDNO { get; set; }

        //統一編號
        [StringLength(10, ErrorMessage = "長度不可超過10")]
        [Display(Name = "統一編號")]
        public String UnifiedBusinessNo { get; set; }

        //護照號碼
        [StringLength(20, ErrorMessage = "長度不可超過20")]
        [Display(Name = "護照號碼")]
        public String PassportNo { get; set; }

        //公司名稱
        [StringLength(40, ErrorMessage = "長度不可超過40")]
        [Display(Name = "公司名稱")]
        public String CompanyName { get; set; }

        //生日
        [Display(Name = "生日")]
        public DateTime Birthday { get; set; }

        //住址
        [StringLength(200, ErrorMessage = "長度不可超過200")]
        [Display(Name = "住址")]
        public String Address { get; set; }

        //郵遞區號
        [StringLength(5, ErrorMessage = "長度不可超過5")]
        [Display(Name = "郵遞區號")]
        public String ZipCode { get; set; }

        //電子信箱
        [StringLength(100, ErrorMessage = "長度不可超過100")]
        [Display(Name = "電子信箱")]
        public String Email { get; set; }

        //市內電話
        [StringLength(20, ErrorMessage = "長度不可超過20")]
        [Display(Name = "市內電話")]
        public String Phone { get; set; }

        //手機號碼
        [StringLength(15, ErrorMessage = "長度不可超過15")]
        [Display(Name = "手機號碼")]
        public String CellPhone { get; set; }

        //性別, 0: 女, 1: 男
        [Display(Name = "性別")]
        public string Sex { get; set; }

        //公司聯絡人姓名
        [StringLength(60, ErrorMessage = "長度不可超過60")]
        [Display(Name = "公司聯絡人姓名")]
        public String ContactPerson { get; set; }

        //公司聯絡人電子信箱
        [StringLength(100, ErrorMessage = "長度不可超過100")]
        [Display(Name = "公司聯絡人電子信箱")]
        public String ContactEmail { get; set; }

        //公司聯絡人市內電話
        [StringLength(20, ErrorMessage = "長度不可超過20")]
        [Display(Name = "公司聯絡人市內電話")]
        public String ContactPhone { get; set; }

        //公司聯絡人手機號碼
        [StringLength(15, ErrorMessage = "長度不可超過15")]
        [Display(Name = "公司聯絡人手機號碼")]
        public String ContactCellPhone { get; set; }

        //認證日期
        [Display(Name = "認證日期")]
        public DateTime AuthDate { get; set; }

        //註冊日期
        [Display(Name = "註冊日期")]
        public DateTime RegDate { get; set; }

        //安全提問編號
        [Display(Name = "安全提問編號")]
        public short SecurityID { get; set; }

        //會員自訂安全提問
        [StringLength(200, ErrorMessage = "長度不可超過200")]
        [Display(Name = "會員自訂安全提問")]
        public String MemberSecurityQuestion { get; set; }

        //安全提問密碼
        [StringLength(200, ErrorMessage = "長度不可超過200")]
        [Display(Name = "安全提問密碼")]
        public String SecurityAnswer { get; set; }

        //會員密碼
        [StringLength(50, ErrorMessage = "長度不可超過50")]
        [Display(Name = "會員密碼")]
        public String Pwd { get; set; }

        //交易密碼
        [StringLength(50, ErrorMessage = "長度不可超過50")]
        [Display(Name = "交易密碼")]
        public String TradePwd { get; set; }

        //網站名稱
        [StringLength(100, ErrorMessage = "長度不可超過100")]
        [Display(Name = "網站名稱")]
        public String WebSiteName { get; set; }

        //商家名稱
        [StringLength(50, ErrorMessage = "長度不可超過50")]
        [Display(Name = "商家名稱")]
        public String MerchantName { get; set; }

        public String Occupation { get; set; }

        #region 客戶管理的會員管理顯示
        //手機驗證狀態與時間
        public string CellphoneAuthStatus { get; set; }
        public DateTime CellphoneAuthDate { get; set; }

        //email驗證狀態與時間
        public string EmailAuthStatus { get; set; }
        public DateTime EmailAuthDate { get; set; }

        //身分證驗證狀態與時間
        public string IDNOAuthStatus { get; set; }
        public DateTime IDNOAuthDate { get; set; }

        //餘額
        public string Balance { get; set; }

        //凍結餘額
        public string FreezeCash { get; set; }
        //可用餘額
        public string AvailableCash { get; set; }

        //身分證圖檔
        public String FilePath1 { get; set; }
        public String FilePath2 { get; set; }
        public String FilePath3 { get; set; }
        public String FilePath4 { get; set; }
        public String FilePath5 { get; set; }
        public String FilePath6 { get; set; }
        public String FilePath7 { get; set; }
        public String FilePath8 { get; set; }
        public String FilePath9 { get; set; }

        //身分證其他資訊
        public string _issueyDate { get; set; }
        [Column("IssueDate", TypeName = "datetime")]
        [Display(Name = "發證日期")]
        public string IssueDate
        {
            get { return TaiwanCalendar(_issueyDate); }
            set { _issueyDate = value; }
        }
        public string IssueDateYY { get; set; }
        public string IssueDateMM { get; set; }
        public string IssueDateDD { get; set; }

        public string IssueLocationName { get; set; }
        public string IssueLocationID { get; set; }

        public int ObtainType { get; set; }
        //領取類別名稱
        [Display(Name = "領取類別名稱")]
        public string ObtainTypeName
        {
            get
            {
                switch (ObtainType.ToString())
                {
                    case "1":
                        return "初發";
                    case "2":
                        return "補發";
                    case "3":
                        return "換發";
                    default:
                        return "";
                }

            }

        }

        //憑證
        public string CertIsActive { get; set; }
        public DateTime CertCreateDate { get; set; }
        public DateTime CertModifyDate { get; set; }

        //手機動態密碼鎖
        public string OTPIsActive { get; set; }
        public DateTime OTPCreateDate { get; set; }
        public DateTime OTPModifyDate { get; set; }

        //玉山實名認證
        public string ESUNVAccNo { get; set; }
        public DateTime ESUNVAccNoAuthDate { get; set; }
        #endregion

        protected string DateTimeFormat(string sDateTime, string sFormat)
        {
            DateTime _datetime;

            if (DateTime.TryParse(sDateTime, out _datetime))
                return _datetime.ToString(sFormat);
            else
                return null;
        }

        protected string TaiwanCalendar(string sDateTime)
        {
            DateTime s;

            if (DateTime.TryParse(sDateTime, out s))
            {
                System.Globalization.TaiwanCalendar twC = new System.Globalization.TaiwanCalendar();
                return twC.GetYear(s) + s.ToString("/MM/dd");
            }

            return "";
        }

        public new int TotalCount { get; set; }

        public string StatusName { get; set; }

        //玉山儲值帳戶
        public string TopUpAcc { get; set; }

        //玉山儲值帳戶使用狀態
        public int TopUpStatus { get; set; }

        //玉山儲值帳戶開通日期
        public DateTime TopUpAuthDate { get; set; }

        //紅利點數
        public int BonusCash { get; set; }

        //購物金
        public int ShoppingCash { get; set; }

        //是否接受廣告行銷 預設1:接受 0:拒絕
        //private int? _AcceptAdMarking = 1;
        //public int? AcceptAdMarking
        //{
        //    get
        //    { return _AcceptAdMarking; }
        //    set
        //    { _AcceptAdMarking = value == null ? 1 : value; }
        //}
        public List<string> BAValiationList { set; get; }
        public List<string> BAValiationReasonList { set; get; }

        public int AcceptAdMarking { get; set; }

        public MemberInfoManage()
        {
            AcceptAdMarking = 1;
        }

        //海外會員:統一證號
        public string UniformID { get; set; }

        //海外會員:護照號碼
        public string OverSeasIDNO { get; set; }

        //護照英文名子
        public string OverSeasENname { get; set; }

        //從商務會員轉回個人會員次數
        public int TransferPersonalTimes { get; set; }

        //最後轉回個人會員日期
        public DateTime LastTransferDate { get; set; }

        //儲值驗證類別
        public int TopUpAuthType { get; set; }

        //登入代號
        [Display(Name = "會員帳號")]
        public string UserCode { get; set; }

        //會員等級
        public byte LevelID { get; set; }

        //備註
        [StringLength(50, ErrorMessage = "長度不可超過50")]
        public string Remark { get; set; }

        //公司類型
        public byte CompanyType { get; set; }

        /// <summary>
        /// 付款成功通知
        /// </summary>
        public int PaySuccNotify { get; set; }

        /// <summary>
        /// 信用卡授權通知
        /// </summary>
        public int CreditAuthNotify { get; set; }

        /// <summary>
        /// 內部後台特店代號
        /// </summary>
        public int CustomerID { get; set; }


        /// <summary>
        /// 是否只修改姓名 0:否 1:是
        /// </summary>
        public int IsCName { get; set; }


        /// <summary>
        /// 提領上限額度
        /// </summary>
        public int CreditCashUsedLimit { get; set; }


        /// <summary>
        /// 信用卡最低保留款
        /// </summary>
        public int MinimumCreditCash { get; set; }

        public DateTime? UniformIusseDate { get; set; }

        /// <summary>
        /// 推廣商名稱
        /// </summary>
        public string PartnerName { get; set; }

        /// <summary>
        /// 業務人員
        /// </summary>
        public string SalesUserCName { get; set; }

        public string WebSiteURL { get; set; }

        //設立日期
        [Display(Name = "設立日期")]
        public DateTime IncorporationDate { get; set; }
        //資本額
        public long CapitalAmount { get; set; }
        //登記地址
        public string CompanyAddress { get; set; }
        public string CompanyZipCode { get; set; }
        public string CompanyCity { get; set; }

        /// <summary>
        /// 國籍編號
        /// </summary>
        public string NationalityName { get; set; }

        /// <summary>
        /// 風險評分
        /// </summary>
        public string RiskScoreLevel { get; set; }

        /// <summary>
        /// 行業名稱
        /// </summary>
        public string MCCName { get; set; }

        /// <summary>
        /// 稅籍編號
        /// </summary>
        public string PrincipalTaxNo { get; set; }
        
    }
}
