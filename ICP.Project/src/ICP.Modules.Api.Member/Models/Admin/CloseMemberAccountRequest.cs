using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Api.Member.Models.Admin
{
    public class CloseMemberAccountRequest
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        [Required]
        public long MID { get; set; }

        /// <summary>
        /// 最後修改人
        /// </summary>
        [Required]
        public string Modifier { get; set; }
    }
}
