namespace ICP.Modules.Api.AccountLink.Models.Cathay
{
    /// <summary>
    /// 國泰世華-綁定記錄
    /// </summary>
    public class BankRecordInfoModel
    {
        /// <summary>
        /// 綁定之銀行帳號
        /// </summary>
        /// <remarks>需加密</remarks>
        public string BnkActNo { get; set; }

        /// <summary>
        /// 綁定時間
        /// </summary>
        public string BindTime { get; set; }

        /// <summary>
        /// 目前狀態
        /// </summary>
        /// <remarks>01:申請 02:已綁定 03:取消綁定</remarks>
        public string ActStstus { get; set; }

    }
}
