namespace ICP.Modules.Mvc.Payment.Models.BaseMember
{
    /// <summary>
    /// 查詢帳戶明細
    /// </summary>
    public class AccountRecordDbReq
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 帳戶明細類型(0:所有 1:支付 2:退款 3:儲值 4:轉出 5:轉入 6:提領)
        /// </summary>
        public int AccRecordType { get; set; }

        /// <summary>
        /// 查詢起始日期
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 查詢結束日期
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 查詢關鍵字
        /// </summary>
        public string KeyWords { get; set; }

        /// <summary>
        /// 資料行檢核
        /// </summary>
        public int RowID { get; set; }
    }
}