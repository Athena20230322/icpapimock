using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MerchantModels
{
    public class MerchantDataModel
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 0: 個人賣家 or 公司賣家, 1: 特店會員
        /// </summary>
        public int MerchantType { get; set; }

        /// <summary>
        /// 商家名稱
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 商家狀態, 0:停用, 1:啟用, 2:可登入看帳號, 3:刪除
        /// </summary>
        public int States { get; set; }

        /// <summary>
        /// 統一編號
        /// </summary>
        public string UnifiedBusinessNo { get; set; }

        /// <summary>
        /// 登記地址(帳單發票地址)
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 網站名稱
        /// </summary>
        public string WebSiteName { get; set; }

        /// <summary>
        /// 網站網址
        /// </summary>
        public string WebSiteURL { get; set; }

        /// <summary>
        /// 負責人姓名
        /// </summary>
        public string CName { get; set; }

        /// <summary>
        /// 財務聯絡人(contact)
        /// </summary>
        public string ContactPerson { get; set; }

        /// <summary>
        /// 財務聯絡人Email
        /// </summary>
        public string ContactEmail { get; set; }

        /// <summary>
        /// 財務聯絡人電話
        /// </summary>
        public string ContactPhone { get; set; }

        /// <summary>
        /// 財務聯絡人手機
        /// </summary>
        public string ContactCellPhone { get; set; }

        /// <summary>
        /// 歐付寶電子發票接收email
        /// </summary>
        public string EinvoiceEmail { get; set; }

        /// <summary>
        /// OTP電話簡訊驗證, 1:啟用  0:未啟用
        /// </summary>
        public int OTPValidateStates { get; set; }

        /// <summary>
        /// 免費提領次數
        /// </summary>
        public int FreeBankTransferCount { get; set; }

        /// <summary>
        /// 提領手續費
        /// </summary>
        public decimal TransferHandlingCharge { get; set; }

        /// <summary>
        /// 0: 電子發票, 1: 電子計算機發票(紙本)
        /// </summary>
        public string ChargeInvoiceType { get; set; }

        /// <summary>
        /// 開發票週期(D:日開 W:週開 M:月開 N:不開)
        /// </summary>
        public string InvoicePeriod { get; set; }

        /// <summary>
        /// 新增日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 到期日(expire)
        /// </summary>
        public DateTime ExpiryDate { get; set; }
    }
}
