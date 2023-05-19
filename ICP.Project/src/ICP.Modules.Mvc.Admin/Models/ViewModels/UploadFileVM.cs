using ICP.Modules.Mvc.Admin.Attributes;
using System.Web;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{
    public class UploadFileVM
    {
        public HttpPostedFileBase MidFile { get; set; }
    }

    /// <summary>
    /// 圖片檔案
    /// </summary>
    public class UploadImageVM
    {
        [Image(Required = true)]
        public HttpPostedFileBase ImageFile { get; set; }
    }

    /// <summary>
    /// 圖片檔案(限制大小2M以下)
    /// </summary>
    /// <remarks>限制大小2M以下</remarks>
    public class UploadImageLimit2m
    {
        [Image(Required = true, MaxSize = 2097152)]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}
