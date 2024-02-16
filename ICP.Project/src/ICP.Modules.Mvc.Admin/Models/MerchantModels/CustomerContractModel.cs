using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.MerchantModels
{
    /// <summary>
    /// 特店合約
    /// </summary>
    public class CustomerContractModel
    {
        /// <summary>
        /// 特店編號
        /// </summary>
        public long CustomerID { get; set; }

        /// <summary>
        /// 特店類別代碼
        /// </summary>
        public int MCCCode { get; set; }

        /// <summary>
        /// 實際商品銷售
        /// </summary>
        public string ActualProductSell { get; set; }

        /// <summary>
        /// 安裝設定費
        /// </summary>
        public decimal SetupFee { get; set; }

        /// <summary>
        /// 年費
        /// </summary>
        public decimal AnnualFee { get; set; }

        /// <summary>
        /// 年費合約啟始日
        /// </summary>
        public DateTime? AnnualFeeStartDate { get; set; }

        /// <summary>
        /// 年費合約結束日
        /// </summary>
        public DateTime? AnnualFeeEndDate { get; set; }

        /// <summary>
        /// 來年續約費用
        /// </summary>
        public decimal AnnualNextYearFee { get; set; }

        /// <summary>
        /// 免費提領次數
        /// </summary>
        public short FreeBankTransferCount { get; set; }

        /// <summary>
        /// 提領手續費
        /// </summary>
        public decimal TransferHandlingCharge { get; set; }

        /// <summary>
        /// 帳戶餘額最低保留款
        /// </summary>
        public decimal MinimunRetention { get; set; }

        /// <summary>
        /// 30 天提領額度限制
        /// </summary>
        public decimal TransferUsedLimit { get; set; }

        /// <summary>
        /// 歸檔編號
        /// </summary>
        public string ArchivingNo { get; set; }
    }
}