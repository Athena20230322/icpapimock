namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class CellphoneCarrierModel
    {
        public int RtnCode { get; set; }
        public string RtnMsg { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        public int MID { get; set; }

        /// <summary>
        /// 載具號碼 存入固定長度為8且格式為1碼斜線「/」加上由7碼數字及大小寫字母組成。
        /// </summary>
        public string CarrierNum { get; set; }
        /// <summary>
        /// 手機號碼
        /// </summary>
        public string OPCellPhone { get; set; }
    }
}