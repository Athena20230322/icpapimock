using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.ValidationAttributes;
using ICP.Modules.Mvc.Admin.Models.MemberModels;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{
    public class EditAuthIDNOVM : ValidatableObject
    {
        /// <summary>
        /// 身份證資料
        /// </summary>
        [ValidateObjectAttribute]
        [Display(Name = "身份證資料")]
        public UpdateMemberAuthIDNO AuthIDNO { get; set; }

        /// <summary>
        /// 上傳圖檔一
        /// </summary>
        public HttpPostedFileBase FileUpload1 { get; set; }

        /// <summary>
        /// 上傳圖檔二
        /// </summary>
        public HttpPostedFileBase FileUpload2 { get; set; }
    }
}
