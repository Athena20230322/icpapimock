using System;
using System.Web;

namespace ICP.Modules.Api.AccountLink.Services
{
    /// <summary>
    /// 快取設定
    /// </summary>
    public class CacheService
    {
        /// <summary>
        /// 取得快取
        /// </summary>
        /// <typeparam name="T">類別型態指定</typeparam>
        /// <param name="cacheName">快取名稱</param>
        /// <returns>傳回 type T 的物件</returns>
        public T Get<T>(string cacheName)
        {
            return (T)this.Get(cacheName);
        }

        /// <summary>
        /// 取得快取
        /// </summary>
        /// <param name="cacheName">快取名稱</param>
        /// <returns></returns>
        public Object Get(string cacheName)
        {
            return HttpContext.Current.Cache[cacheName];
        }

        /// <summary>
        /// 設定快取
        /// </summary>
        /// <param name="obj">設定的物件</param>
        /// <param name="cacheName">快取名稱</param>
        /// <param name="durationSeconds">快取時間(秒)</param>
        public void Set(Object obj, string cacheName, int durationSeconds)
        {
            HttpContext.Current.Cache.Remove(cacheName);
            HttpContext.Current.Cache.Insert
                (
                    cacheName,
                    obj,
                    null,
                    DateTime.Now.AddSeconds(durationSeconds),
                    TimeSpan.Zero
                );
        }
    }
}
