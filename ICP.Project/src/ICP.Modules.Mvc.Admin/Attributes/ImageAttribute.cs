using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Attributes
{
    /// <summary>
    /// 檢查上傳的檔案是否為圖片
    /// </summary>
    public class ImageAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly string[] _imgExt = new string[] { ".jpg", ".jpeg", ".png", ".bmp", ".svg", ".gif" };

        /// <summary>
        /// 檔案大小限制(單位: 位元組)
        /// </summary>
        public int MaxSize { get; set; }

        /// <summary>
        /// 是否必須上傳
        /// </summary>
        public bool Required { get; set; }

        public override bool IsValid(object value)
        {
            bool isPass = false;
            var file = value as HttpPostedFileBase;

            if (value == null || file == null || (Required && file.ContentLength == 0))
            {
                return isPass;
            }

            bool isImageType = file.ContentType.ToLower().Contains("image/");
            string ext = Path.GetExtension(file.FileName).ToLower();

            if (isImageType && _imgExt.Any(x => x == ext))
            {
                isPass = true;
            }

            //超過大小限制
            if (MaxSize > 0 && file.ContentLength > MaxSize)
            {
                isPass = false;
            }

            return isPass;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule
            {
                ValidationType = "image",
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName())
            };
            yield return rule;
        }

        public override string FormatErrorMessage(string name)
        {
            return this.ErrorMessage ?? "image error";
        }
    }
}
