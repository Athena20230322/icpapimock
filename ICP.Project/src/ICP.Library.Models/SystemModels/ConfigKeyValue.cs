using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.SystemModels
{
    /// <summary>
    /// 設定檔
    /// </summary>
    public class ConfigKeyValue
    {
        /// <summary>
        /// 設定代碼
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 設定值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 快取時間(分鐘)
        /// </summary>
        public int CacheTime { get; set; }

        /// <summary>
        /// 是否需要加解密
        /// </summary>
        public bool IsEncrypt { get; set; }
    }
}
