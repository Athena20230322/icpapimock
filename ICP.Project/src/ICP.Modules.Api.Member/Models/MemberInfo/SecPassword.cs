using ICP.Library.Models.AuthorizationApi;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class SecPassword : BaseAuthorizationApiRequest
    {
        [Display(Name = "安全密碼")]
        public string OriSecPassword { get; set; }


        [Display(Name = "新安全密碼")]
        public string NewSecPassword { get; set; }

        [Display(Name = "確認新安全密碼")]
        public string ConfirmSecPassword { get; set; }

        //// 交易密碼強度
        //public int pswstrength { get; set; }

        //public string PayPwdUpgradeDate { get; set; }
    }
}
