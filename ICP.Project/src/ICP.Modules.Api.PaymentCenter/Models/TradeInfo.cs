using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Models
{
    public class TradeInfo : BaseResult
    {
        /// <summary>
        /// 廠商編號
        /// </summary>
        public long MerchantID { get; set; }

        /// <summary>
        /// 廠商訂單編號
        /// </summary>
        public string MerchantTradeNo { get; set; }

        /// <summary>
        /// 虛擬帳號
        /// </summary>
        public string VirtualAccount { get; set; }

        /// <summary>
        /// 付款方式代碼
        /// </summary>
        public int PaymentTypeID { get; set; }

        /// <summary>
        /// 收單行名稱
        /// </summary>
        public int PaymentSubTypeID { get; set; }

        /// <summary>
        /// 交易流水號
        /// </summary>
        public long TradeID { get; set; }

        /// <summary>
        /// 訂單建立時間
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 付款狀態(0:未付款/1:付款成功/2:付款失敗/3:退款成功/4:退款失敗)
        /// </summary>
        public int PaymentStatus { get; set; }

        /// <summary>
        /// 付款時間
        /// </summary>
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// 交易編號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        public decimal TradeAMT { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        public string MpType { get; set; }

        /// <summary>
        /// 繳費來源
        /// </summary>
        public string MpName { get; set; }

        /// <summary>
        /// 回傳的交易金額
        /// </summary>
        public decimal MpReturnTradeAMT { get; set; }

        /// <summary>
        /// 回傳的虛擬帳號
        /// </summary>
        public string MpReturnVirtualAccount { get; set; }

        /// <summary>
        /// 回傳的交易編號
        /// </summary>
        public string MpReturnPaymentNo { get; set; }

        /// <summary>
        /// 錯誤代碼
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; }
    }
}
