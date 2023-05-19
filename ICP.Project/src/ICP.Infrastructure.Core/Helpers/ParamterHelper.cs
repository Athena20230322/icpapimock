using System;
using System.Collections.Generic;
using System.Web;

namespace ICP.Infrastructure.Core.Helpers
{
    public class ParamterHelper
    {
        /// <summary>
        /// 將Model轉換成paramter字串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string ModelToParameter<T>(T model, bool useUrlEncode = true)
        {
            List<string> paramterts = new List<string>();
            Type type = typeof(T);
            var paras = type.GetProperties();

            foreach (var para in paras)
            {
                var obj = para.GetValue(model, null);
                var value = (obj ?? "").ToString();
                var key = para.Name;

                paramterts.Add(SpellParamter(key, value, useUrlEncode));
            }
            return string.Join("&", paramterts.ToArray());
        }
        /// <summary>
        /// 將Dictionary轉換成paramter字串
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static string DictionaryToParamter(IDictionary<string, string> dict, bool useUrlEncode = true)
        {
            List<string> paramterts = new List<string>();
            foreach (var data in dict)
            {
                paramterts.Add(SpellParamter(data.Key, data.Value, useUrlEncode));
            }
            return string.Join("&", paramterts.ToArray());
        }

        /// <summary>
        /// 轉成參數
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string SpellParamter(string key, string value, bool useUrlEncode)
        {
            string str = useUrlEncode ? HttpUtility.UrlEncode(value) : value;
            return string.Format("{0}={1}", key, str);
        }

        public static string GetPatamterKey<TModel>(TModel model)
            where TModel : class,new()
        {
            List<string> parasKey = new List<string>();
            var paras = model.GetType().GetProperties();
            foreach (var para in paras)
            {
                parasKey.Add("@" + para.Name);
            }
            return string.Join(",", parasKey);
        }
    }
}