using ICP.Infrastructure.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Web.Helpers
{
    public class SaveFileHelper
    {
        ImageHelper _imageHelper;

        public SaveFileHelper()
        {
            _imageHelper = new ImageHelper();
        }

        public void SaveImgToModel<T>(string urlDir, string saveDir, string fileName, string base64Str, T model, Expression<Func<T, string>> memberLamda1)
        {
            if (string.IsNullOrWhiteSpace(base64Str)) return;

            var img = _imageHelper.Base64ToImage(base64Str);

            string url = $"{urlDir}/{fileName}";

            if (!System.IO.Directory.Exists(saveDir)) System.IO.Directory.CreateDirectory(saveDir);

            string filePath = System.IO.Path.Combine(saveDir, fileName);

            img.Save(filePath);

            var memberExpression = (MemberExpression)memberLamda1.Body;

            (memberExpression.Member as PropertyInfo).SetValue(model, url, null);
        }

        public void SaveFileToModel<T>(string urlDir, string saveDir, string fileName, System.Web.HttpPostedFileBase file, T model, Expression<Func<T, string>> memberLamda1)
        {
            if (file == null) return;

            if (!System.IO.Directory.Exists(saveDir)) System.IO.Directory.CreateDirectory(saveDir);

            string filePath = System.IO.Path.Combine(saveDir, fileName);

            string url = $"{urlDir}/{fileName}";

            file.SaveAs(filePath);

            var memberExpression = (MemberExpression)memberLamda1.Body;

            (memberExpression.Member as PropertyInfo).SetValue(model, url, null);
        }
    }
}
