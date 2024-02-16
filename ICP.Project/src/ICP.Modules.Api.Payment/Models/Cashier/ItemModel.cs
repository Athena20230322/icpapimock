namespace ICP.Modules.Api.Payment.Models.Cashier
{
    public class ItemModel
    {
        /// <summary>
        /// 序號
        /// </summary>
        public int ItemNo { get; set; }

        /// <summary>
        /// 活動代碼
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 活動成立數量
        /// </summary>
        public string Quantity { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; }
    }
}
