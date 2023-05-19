using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.Certificate
{
    public class AppRedirectRequest : ValidatableObject
    {
        [Required(ErrorMessage = "{0} 欄位不能為空")]
        [Range(0, long.MaxValue, ErrorMessage = "{0} 欄位格式不正確")]
        public long EncKeyID { get; set; }

        [Required(ErrorMessage = "{0} 欄位不能為空")]
        public string Signature { get; set; }

        [Required(ErrorMessage = "{0} 欄位不能為空")]
        public string EncData { get; set; }
    }
}
