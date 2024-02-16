namespace ICP.Library.Models.EinvoiceLibrary
{
    /// <summary>
    /// 會員手機條碼設定檔
    /// </summary>
    public class CellPhoneCarrierDetail
    {
        /// <summary>
        /// 載具編號(流水號)
        /// </summary>
        public int CarrierID { get; set; }
        /// <summary>
        /// 會員編號
        /// </summary>
        public int MID { get; set; }
        /// <summary>
        /// 載具號碼
        /// </summary>
        public string CarrierNum { get; set; }
        /// <summary>
        /// 手機號碼
        /// </summary>
        public string Cellphone { get; set; }
        /// <summary>
        /// 驗證碼
        /// </summary>
        public string VerificationCode { get; set; }
        /// <summary>
        /// 手機載具驗證碼是否驗證通過
        /// </summary>
        public int IsVerify { get; set; }
        /// <summary>
        /// 是否綁定行動支付，0：取消綁定 1：已綁定 綁訂只允許一組
        /// </summary>
        public int IsDefault { get; set; }
        /// <summary>
        /// 綁定會員類別。0:OP 同步帳號，1=icash 創建大平台帳號，2=大平台撈取綁定帳號
        /// </summary>
        public int BindType { get; set; }

    }
}