using ICP.Library.Models.AuthorizationApi;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.Certificate
{
    public class BindMerchantCertRequest : BaseAuthorizationApiRequest
    {
        [Required(ErrorMessage = "{0} 為必填")]
        [Range(1, long.MaxValue, ErrorMessage = "{0} 格式不正確")]
        public long MerchantID { get; set; }

        [Required(ErrorMessage = "{0} 為必填")]
        public string Token { get; set; }
    }
}
