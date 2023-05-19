using System;
using System.Text;

namespace ICP.Modules.Api.AccountLink.Services
{
    /// <summary>
    /// 其它共用方法
    /// </summary>
    public class CommonService
    {
        /// <summary>
        /// 16進位字串轉換
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetHexadecimal(string data)
        {
            byte[] byteSource = Encoding.Default.GetBytes(data);
            StringBuilder result = new StringBuilder();
            foreach (var item in byteSource)
            {
                result.Append(item.ToString("X2"));
            }
            return result.ToString();
        }

        /// <summary>
        /// 產生QueryString
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Obj2QueryString(object obj)
        {
            StringBuilder sb = new StringBuilder();

            var targetProps = obj.GetType().GetProperties();

            foreach (var targetProp in targetProps)
            {
                sb.Append($"{(string.IsNullOrEmpty(sb.ToString()) ? "" : "&")}{targetProp.Name}={Convert.ToString(targetProp.GetValue(obj, null))}");
            }

            return sb.ToString();
        }

    }
}
