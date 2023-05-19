using ICP.Infrastructure.Core.Models.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.MerchantModels
{
    /// <summary>
    /// 特店明細
    /// </summary>
    public class CustomerDetailModel
    {
        /// <summary>
        /// 特店編號
        /// </summary>
        public long CustomerID { get; set; }

        /// <summary>
        /// 網站名稱
        /// </summary>
        [Required(ErrorMessage = "商店名稱不可空白")]
        [StringLength(100)]
        public string WebSiteName { get; set; }

        /// <summary>
        /// 網站網址
        /// </summary>
        [Required(ErrorMessage = "網址不可空白！")]
        [StringLength(100)]
        public string WebSiteURL { get; set; }

        /// <summary>
        /// 交付天期
        /// 1: 三天內
        /// 2: 三 ~五 天
        /// 3: 五-十天
        /// 4: 十天以上
        /// </summary>
        [Required]
        [Display(Name = "交付天期")]
        public byte DeliveryDayType { get; set; }

        /// <summary>
        /// 銀行種類
        /// </summary>
        [Required]
        [Display(Name = "銀行種類")]
        public byte BankTypeID { get; set; }

        /// <summary>
        /// 銀行代碼
        /// </summary>
        [Required]
        [Display(Name = "銀行")]
        public string BankCode { get; set; }

        /// <summary>
        /// 銀行分行代碼
        /// </summary>
        [Required]
        [Display(Name = "銀行分行")]
        public string BankBranchCode { get; set; }

        /// <summary>
        /// 銀行帳號
        /// </summary>
        [Required(ErrorMessage = "銀行帳號不可空白")]
        [Display(Name = "銀行帳號")]
        public string BankAccount { get; set; }

        /// <summary>
        /// 銀行帳號戶名
        /// </summary>
        [Required]
        [Display(Name = "銀行帳號戶名")]
        [StringLength(40)]
        public string BankAccountName { get; set; }

        /// <summary>
        /// 提領規則 0:不啟用 1:啟用
        /// </summary>
        [Required]
        [Display(Name = "提領規則")]
        public bool TransferSchedule { get; set; }

        /// <summary>
        /// 排程種類 1:每日 2:每周 3:隔週 4:每月
        /// </summary>
        [Required(ErrorMessage = "請選擇結算方式")]
        [Display(Name = "排程種類")]
        public byte ScheduleType{ get; set; }

        /// <summary>
        /// 排程細項
        /// 日:0
        /// 周: 1234567
        /// 月: 1,5,10,15,20,25,0
        /// 自訂: 1-30
        /// </summary>
        [Required]
        [Display(Name = "排程細項")]
        public byte ScheduleValue   { get; set; }

        /// <summary>
        /// 轉帳額度類型
        /// 1: 固定 2:餘額
        /// </summary>
        [Required]
        [Display(Name = "轉帳額度類型")]
        public byte AMTransferType { get; set; }

        /// <summary>
        /// 最低金額
        /// </summary>
        [Required]
        [Display(Name = "最低金額")]
        public decimal TransferAmount { get; set; }

        /// <summary>
        /// 設立日期
        /// </summary>
        [Required(ErrorMessage = "設立日期不可空白！")]
        [Display(Name = "設立日期")]
        public DateTime? IncorporationDate { get; set; }

        /// <summary>
        /// 資本額
        /// </summary>
        [Required(ErrorMessage = "資本額不可空白！")]
        [Display(Name = "資本額")]
        public decimal CapitalAmount { get; set; }

        /// <summary>
        /// 公司名稱
        /// </summary>
        [Required(ErrorMessage = "公司名稱不可空白！")]
        [Display(Name = "公司名稱")]
        [StringLength(50)]
        public string CompanyName { get; set; }

        /// <summary>
        /// 公司類別 
        /// 0: 預設值 
        /// 1:政府機關
        /// 2:法人
        /// 3:行號
        /// 4:其他團體
        /// </summary>
        [Required(ErrorMessage = "公司類別不可空白！")]
        [Display(Name = "公司類別")]
        public byte CompanyType { get; set; }

        /// <summary>
        /// 統一編號
        /// </summary>
        [Required(ErrorMessage = "統一編號不可空白！")]
        [Display(Name = "統一編號")]
        public string UnifiedBusinessNo { get; set; }

        /// <summary>
        /// 負責人類型 
        /// 0:自然人 1:法人
        /// </summary>
        [Required]
        [Display(Name = "負責人類型")]
        public byte PrincipalType { get; set; }

        /// <summary>
        /// 身分證字號
        /// </summary>
        [Required(ErrorMessage = "身分證字號不可空白！")]
        [Display(Name = "身分證字號")]
        [RegularExpression(RegexConst.IDNO, ErrorMessage = "身分證字號格式輸入錯誤！")]
        public string IDNO { get; set; }

        /// <summary>
        /// 統一證號/居留證號
        /// </summary>
        [Required(ErrorMessage = "統一證號/居留證號不可空白！")]
        [Display(Name = "統一證號/居留證號")]
        [RegularExpression(RegexConst.UniformID, ErrorMessage = "居留證號碼格式輸入錯誤！")]
        public string UniformID { get; set; }

        /// <summary>
        /// 護照號碼
        /// </summary>
        [Required(ErrorMessage = "護照號碼不可空白！")]
        [Display(Name = "護照號碼")]
        //[RegularExpression(RegexConst., ErrorMessage = "護照號碼格式輸入錯誤！")]
        public string OverSeasIDNO { get; set; }

        /// <summary>
        /// 負責人/個人姓名
        /// </summary>
        [Required(ErrorMessage = "姓名不可空白！")]
        [Display(Name = "負責人/個人姓名")]
        [StringLength(40)]
        public string CName { get; set; }

        /// <summary>
        /// 英文負責人姓名
        /// </summary>
        [Required(ErrorMessage = "英文姓名不可空白！")]
        [Display(Name = "英文負責人姓名")]
        [StringLength(100)]
        public string CName_EN { get; set; }

        /// <summary>
        /// 負責人生日
        /// </summary>
        [Required(ErrorMessage = "生日不可空白！")]
        [Display(Name = "負責人生日")]
        public DateTime? PrincipalBirthday { get; set; }

        /// <summary>
        /// 國籍流水號
        /// </summary>
        [Required(ErrorMessage = "國籍不可空白！")]
        [Display(Name = "國籍流水號")]
        public int NationalityID { get; set; }

        /// <summary>
        /// 職業ID
        /// </summary>
        [Required(ErrorMessage = "職業不可空白！")]
        [Display(Name = "職業ID")]
        public int OccupationID { get; set; }

        /// <summary>
        /// 負責人公司名稱
        /// </summary>
        [Required(ErrorMessage = "負責人公司名稱不可空白！")]
        [Display(Name = "負責人公司名稱")]
        [StringLength(100)]
        public string PrincipalCompanyName { get; set; }

        /// <summary>
        /// 負責人公司公司統編
        /// </summary>
        [Required(ErrorMessage = "負責人公司統一編號不可空白！")]
        [Display(Name = "負責人公司公司統編")]
        [RegularExpression(RegexConst.UnifiedBusinessNo)]
        public string PrincipalUnifiedBusinessNo { get; set; }

        /// <summary>
        /// 公司/個人聯絡電話
        /// </summary>
        [Required(ErrorMessage = "聯絡電話不可空白！")]
        [Display(Name = "公司/個人聯絡電話")]
        [RegularExpression(RegexConst.Tel, ErrorMessage = "電話格式錯誤！")]
        public string CompanyTel { get; set; }

        /// <summary>
        /// 公司/個人傳真號碼
        /// </summary>
        [Required(ErrorMessage = "傳真號碼不可空白！")]
        [RegularExpression(RegexConst.Tel, ErrorMessage = "傳真電話格式錯誤！")]
        [Display(Name = "公司/個人傳真號碼")]
        public string CompanyFax { get; set; }

        /// <summary>
        /// 公司通訊地址郵遞區號
        /// </summary>
        [Required(ErrorMessage = "郵遞區號不可空白！")]
        [Display(Name = "公司通訊地址郵遞區號")]
        public string CompanyZipCode { get; set; }

        /// <summary>
        /// 登記地址(帳單發票地址)
        /// </summary>
        [Required(ErrorMessage = "地址不可空白！")]
        [StringLength(100)]
        [Display(Name = "登記地址")]
        public string Address { get; set; }

        /// <summary>
        /// 發票地址郵遞區號
        /// </summary>
        [Required(ErrorMessage = "郵遞區號不可空白！")]
        [Display(Name = "發票地址郵遞區號")]
        public string InvoiceZipCode { get; set; }

        /// <summary>
        /// 發票地址
        /// </summary>
        [Required(ErrorMessage = "地址不可空白！")]
        [StringLength(100)]
        [Display(Name = "發票地址")]
        public string InvoiceAddress { get; set; }

        /// <summary>
        /// 財務聯絡人
        /// </summary>
        [Required(ErrorMessage = "姓名不可空白！")]
        [StringLength(10)]
        [Display(Name = "財務聯絡人")]
        public string ContactPerson { get; set; }

        /// <summary>
        /// 財務聯絡人
        /// </summary>
        [Required(ErrorMessage = "姓名不可空白！")]
        [StringLength(100)]
        [Display(Name = "財務聯絡人英文姓名")]
        public string ContactPerson_EN { get; set; }


        /// <summary>
        /// 財務聯絡人Email
        /// </summary>
        [Required(ErrorMessage = "Email不可空白！")]
        [RegularExpression(RegexConst.Email, ErrorMessage = "請輸入正確E-mail！")]
        [StringLength(100)]
        [Display(Name = "財務聯絡人Email")]
        public string ContactEmail { get; set; }

        /// <summary>
        /// 財務聯絡人電話
        /// </summary>
        [Required(ErrorMessage = "電話不可空白！")]
        [RegularExpression(RegexConst.Tel, ErrorMessage = "電話格式錯誤！")]
        [Display(Name = "財務聯絡人電話")]
        public string ContactPhone { get; set; }

        /// <summary>
        /// 財務聯絡人手機
        /// </summary>
        [Required(ErrorMessage = "手機不可空白！")]
        [RegularExpression(RegexConst.CellPhone, ErrorMessage = "請輸入手機")]
        [Display(Name = "財務聯絡人手機")]
        public string ContactCellPhone { get; set; }

        /// <summary>
        /// 業務連絡人姓名
        /// </summary>
        [Required(ErrorMessage = "姓名不可空白！")]
        [StringLength(10)]
        [Display(Name = "業務連絡人姓名")]
        public string SalesContactPerson { get; set; }

        /// <summary>
        /// 業務聯絡人Email
        /// </summary>
        [Required(ErrorMessage = "Email不可空白！")]
        [RegularExpression(RegexConst.Email, ErrorMessage = "請輸入正確E-mail！")]
        [StringLength(100)]
        [Display(Name = "業務聯絡人Email")]
        public string SalesContactEmail { get; set; }

        /// <summary>
        /// 業務聯絡人電話
        /// </summary>
        [Required(ErrorMessage = "電話不可空白！")]
        [RegularExpression(RegexConst.Tel, ErrorMessage = "電話格式錯誤！")]
        [Display(Name = "業務聯絡人電話")]
        public string SalesContactPhone { get; set; }

        /// <summary>
        /// 業務聯絡人手機
        /// </summary>
        [Required(ErrorMessage = "手機不可空白！")]
        [RegularExpression(RegexConst.CellPhone, ErrorMessage = "請輸入手機")]
        [Display(Name = "業務聯絡人手機")]
        public string SalesContactCellPhone { get; set; }

        /// <summary>
        /// 電子發票接收email
        /// </summary>
        [Required(ErrorMessage = "Email不可空白！")]
        [RegularExpression(RegexConst.Email, ErrorMessage = "請輸入正確E-mail！")]
        [StringLength(100)]
        [Display(Name = "電子發票接收Email")]
        public string EinvoiceEmail { get; set; }
    }
}
