using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.AccountLink.Models.ChinaTrust
{
    public class ACLinkPayModel
    {
        /// <summary>
        /// 訂單編號
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 業種(01: 電子支付)
        /// </summary>
        public string MerchantType { get; set; }

        /// <summary>
        /// 業者統編
        /// </summary>
        public string MerchantId { get; set; }

        /// <summary>
        /// 電子支付帳戶
        /// </summary>
        public string UserNo { get; set; }

        /// <summary>
        /// 身分證字號
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 交易類別 (0:消費扣款,1:繳費(代收代付),2:儲值,9:沖正)
        /// </summary>
        public string TrxType { get; set; }

        /// <summary>
        /// 轉出帳號
        /// </summary>
        public string PayerAccount { get; set; }

        /// <summary>
        /// 轉入帳號
        /// </summary>
        public string PayeeAccount { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        public int TrxAmt { get; set; }

        /// <summary>
        /// 交易地點
        /// </summary>
        public string TrxShopName { get; set; }

        /// <summary>
        /// 原交易序號
        /// </summary>
        public string OriginalReferenceNo { get; set; }

        /// <summary>
        /// 交易序號
        /// </summary>
        public string TrxNo { get; set; }

        /// <summary>
        /// 交易日期時間
        /// </summary>
        public string TrxTime { get; set; }
    }
}
