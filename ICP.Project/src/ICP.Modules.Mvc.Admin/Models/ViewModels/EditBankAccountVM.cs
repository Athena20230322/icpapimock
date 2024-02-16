using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.ValidationAttributes;
using ICP.Modules.Mvc.Admin.Models.MemberModels;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{
    public class EditBankAccountVM : ValidatableObject
    {
        /// <summary>
        /// 銀行帳號資料
        /// </summary>
        [ValidateObjectAttribute]
        [Display(Name = "銀行帳號資料")]
        public UpdateMemberBankAccount MemberBankAccount { get; set; }

        /// <summary>
        /// 上傳圖檔一
        /// </summary>
        public HttpPostedFileBase FileUpload1 { get; set; }
    }
}
