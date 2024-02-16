using System;

namespace ICP.Modules.Mvc.Admin.Models.Finance
{
    /// <summary>
    /// 帳務類型
    /// </summary>
    public class TradeModeModel
    {
        /// <summary>
        /// 帳務類型編號
        /// </summary>
        public int TradeModeID { get; set; }

        /// <summary>
        /// 帳務類型名稱
        /// </summary>
        public string TradeModeCName { get; set; }
    }

    /// <summary>
    /// 交易類型
    /// </summary>
    public class TradeTypeModel
    {
        /// <summary>
        /// 交易類型編號
        /// </summary>
        public int PaymentTypeID { get; set; }

        /// <summary>
        /// 交易類型名稱
        /// </summary>
        public string PaymentNotes { get; set; }
    }

    /// <summary>
    /// 交易子類型
    /// </summary>
    public class TradeSubTypeModel
    {
        /// <summary>
        /// 交易子類型編號
        /// </summary>
        public int PaymentSubTypeID { get; set; }

        /// <summary>
        /// 交易子類型名稱
        /// </summary>
        public string PaymentSubTypeNotes { get; set; }
    }
}
