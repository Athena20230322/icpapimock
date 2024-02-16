
namespace ICP.Batch.AccountLink.Models.First
{
    /// <summary>
    /// 交易明細資料(Bank -> icash)
    /// </summary>
    public class TradeModel
    {
        /// <summary>
        /// 交易序號(20)
        /// </summary>
        public string MSG_NO { get; set; }

        /// <summary>
        /// 交易日期時間(14)
        /// </summary>
        public string TRNS_TIME { get; set; }

        /// <summary>
        /// 平台代碼(4)
        /// </summary>
        public string EC_ID { get; set; }

        /// <summary>
        /// 平台會員代號(20)
        /// </summary>
        public string EC_USER { get; set; }

        /// <summary>
        /// 綁定實體帳號(14)
        /// </summary>
        public string LINK_ACNT { get; set; }

        /// <summary>
        /// 帳戶種類(3)
        /// </summary>
        /// <remarks>LINK_ACNT綁定實體帳戶取第4位及第5位的值</remarks>
        public string LINK_TYPE { get; set; }

        /// <summary>
        /// 交易代號(1)
        /// </summary>
        /// <remarks>P:付款 R:退款 N:提領</remarks>
        public string TRNS_CODE { get; set; }

        /// <summary>
        /// 訂單編號(24)
        /// </summary>
        public string TRNS_NO { get; set; }

        /// <summary>
        /// 交易說明(40)
        /// </summary>
        public string TRNS_NOTE { get; set; }

        /// <summary>
        /// 交易金額(40)
        /// </summary>
        public string TRNS_AMT { get; set; }

        /// <summary>
        /// 幣別(3)
        /// </summary>
        public string TRNS_CCY { get; set; }

        /// <summary>
        /// 交易類別(1)
        /// </summary>
        /// <remarks>P : 付款(1:付款 2:儲值)</remarks>
        /// <remarks>R:退款 N:提領(空白)</remarks>
        public string TRNS_TYPE { get; set; }
    }
}
