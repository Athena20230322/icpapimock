using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.TinyURL.Repositories
{
    using Infrastructure.Core.Utils;

    public abstract class BaseConfigRepository
    {
        #region 不公開
        protected string GetAppSetting(string key)
        {
            return GlobalConfigUtil.GetAppSetting(key);
        }

        //單執行序使用 Dictionary, 多執行序請用 ConcurrentDictionary
        private Dictionary<string, string> _dict;

        protected string GetKeyApiValue(string key)
        {
            if (_dict.ContainsKey(key)) return _dict[key];

            string value = GlobalConfigUtil.GetKeyApiValue(key);

            //if (value != null)
            _dict.Add(key, value);

            return value;
        }
        #endregion

        public BaseConfigRepository()
        {
            _dict = new Dictionary<string, string>();
        }
    }
}
