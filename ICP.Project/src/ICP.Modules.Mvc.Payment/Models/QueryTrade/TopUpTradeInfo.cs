namespace ICP.Modules.Mvc.Payment.Models.QueryTrade
{
    public class TopUpTradeInfo : TradeInfo
    {
        /// <summary>
        /// 銀行帳戶編號(系統定義流水號)
        /// </summary>
        public long AccountID { get; set; }

        /// <summary>
        /// 虛擬帳號
        /// </summary>
        public string VirtualAccount { get; set; }

        /// <summary>
        /// 虛擬帳號有效時間
        /// </summary>
        public string ExpireDate { get; set; }

        /// <summary>
        /// 虛擬帳號是否已過期 (True:已過期, False:未過期)
        /// </summary>
        public bool IsExpired { get; set; }

        /// <summary>
        /// 儲值取消旗標 (0:不可取消, 1:可取消)
        /// </summary>
        public int CancelFlag { get; set; }
    }
}