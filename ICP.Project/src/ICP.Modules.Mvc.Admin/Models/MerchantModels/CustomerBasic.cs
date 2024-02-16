using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.MerchantModels
{
    /// <summary>
    /// 特店基本檔
    /// </summary>
    public class CustomerBasic
    {
        /// <summary>
        /// 特店編號(流水號)
        /// </summary>
        public long CustomerID { get; set; }

        /// <summary>
        /// 特店身份
        /// 1:公司特店 2:個人戶
        /// </summary>
        [Required(ErrorMessage = "請選擇特店身份")]
        [Display(Name = "特店身份")]
        public byte CustomerType { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// ICASH 電支帳號
        /// </summary>
        public string ICPMID { get; set; }

        /// <summary>
        /// 登入帳號
        /// </summary>
        [Required(ErrorMessage = "帳號不可空白")]
        [Display(Name = "登入帳號")]
        //[StringLength()]
        public string Account { get; set; }

        /// <summary>
        /// 金鑰綁定Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 合約範本
        /// 1.ec交易2.ec+行動支付3.行動支付
        /// </summary>
        [Required(ErrorMessage = "請選擇合約範本")]
        [Display(Name = "合約範本")]
        public byte Template { get; set; }

        /// <summary>
        /// 合約類別
        /// 0:公版,1:紙本
        /// </summary>
        [Required(ErrorMessage = "請選擇合約類別")]
        [Display(Name = "合約類別")]
        public byte ContractType { get; set; }

        /// <summary>
        /// 商品類型(and運算法)
        /// 1:實體 
        /// 2:虛擬商品(點數/服務) ,
        /// 4:遞延商品(課程/SPA) 
        /// 8:商品代收 
        /// 16: 商品代售
        /// </summary>
        [Required(ErrorMessage = "請選擇商品類型")]
        [Display(Name = "商品類型")]
        public byte CommoditiyType { get; set; }

        /// <summary>
        /// 收款方式(and運算法)
        /// 1:帳戶餘額
        /// 2:連結帳戶
        /// </summary>
        [Required(ErrorMessage = "請選擇收款方式")]
        [Display(Name = "收款方式")]
        public byte GatewayItem { get; set; }

        /// <summary>
        /// 儲值代收
        /// 0: 否 1: 是
        /// </summary>
        [Display(Name = "儲值代收")]
        public byte isDeposit { get; set; }

        /// <summary>
        /// 過件狀態
        /// 0:未過件
        /// 1:已過件
        /// </summary>
        public byte CustomerStatus { get; set; }

        /// <summary>
        /// 審核狀態
        /// </summary>
        [Required(ErrorMessage = "請選擇審核狀態")]
        public byte AuditStatus { get; set; }

        /// <summary>
        /// 審核備註
        /// </summary>
        [Required(ErrorMessage = "請輸入審核備註")]
        [StringLength(200)]
        [Display(Name = "審核備註")]
        public string AuditNote { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        [StringLength(200)]
        [Display(Name = "進件備註")]
        public string Remark { get; set; }

        /// <summary>
        ///  業務SaleID
        /// </summary>
        [Required(ErrorMessage = "請選擇負責業務")]
        [Display(Name = "負責業務")]
        public int SalesUserID { get; set; }      
    }
}