using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Payment.Models.QueryTrade
{
    public class TopUpCashTradeDbRes
    {
        /// <summary>
        /// 訂單流水號
        /// </summary>
        public long TradeID { get; set; }

        /// <summary>
        /// 訂單建立時間
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 訂單狀態
        /// </summary>
        public int TradeStatus { get; set; }

        /// <summary>
        /// 儲值金額
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 付款狀態
        /// </summary>
        public int PaymentStatus { get; set; }

        /// <summary>
        /// 付款日期
        /// </summary>
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// 付款方式代碼
        /// </summary>
        public int PaymentTypeID { get; set; }

        /// <summary>
        ///  付款方式子類別代碼
        /// </summary>
        public int PaymentSubTypeID { get; set; }

        /// <summary>
        /// 交易識別碼(條碼)
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 商店編號
        /// </summary>
        public string StoreID { get; set; }

        /// <summary>
        /// 商店名稱
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// 通路名稱
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 儲值取消旗標 (0:不可取消, 1:可取消)
        /// </summary>
        public int CancelFlag { get; set; }
    }
}
