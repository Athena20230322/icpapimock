namespace ICP.Library.Models.MemberModels
{
    public class BankBranchCodeModel
    {
        /// <summary>
        /// 銀行代碼
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 銀行分行代碼
        /// </summary>
        public string BankBranchCode { get; set; }

        /// <summary>
        /// 銀行類別編號
        /// </summary>
        public byte BankTypeID { get; set; }

        /// <summary>
        /// 銀行類別名稱
        /// </summary>
        public string BankTypeName { get; set; }

        /// <summary>
        /// 銀行名稱
        /// </summary>
        public string BankName { get; set; }
    }
}
