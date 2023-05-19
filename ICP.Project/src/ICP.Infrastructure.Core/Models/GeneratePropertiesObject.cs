using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Linq.Expressions;
using System.Collections.Specialized;
using System.Web;

namespace ICP.Infrastructure.Core.Models
{
    public class GeneratePropertiesObject
    {
        protected object _obj { get; set; }

        protected Dictionary<string, object> _dict { get; set; }

        public NameValueCollection GenerateNameValueCollection()
        {
            var nv = new NameValueCollection();

            foreach (var item in _dict)
            {
                string value = item.Value != null ? item.Value.ToString() : null;
                nv.Add(item.Key, value);
            }

            return nv;
        }

        public GeneratePropertiesObject Add(string key, string value)
        {
            _dict.Add(key, value);

            return this;
        }

        public string ToQueryString(bool urlencode = true, bool containempty = true)
        {
            var nv = GenerateNameValueCollection();

            if (!containempty)
            {
                var keys = nv.AllKeys.ToList();

                keys.ForEach(key => 
                {
                    if (string.IsNullOrEmpty(nv[key]))
                    {
                        nv.Remove(key);
                    }
                });
            }

            List<string> list;

            if (urlencode)
            {
                list = nv.AllKeys.Select(key => $"{HttpUtility.UrlEncode(key)}={HttpUtility.UrlEncode(nv[key])}").ToList();
            }
            else
            {
                list = nv.AllKeys.Select(key => $"{key}={nv[key]}").ToList();
            }

            return string.Join("&", list);
        }


        public string ToValueString()
        {
            string str = string.Empty;

            foreach (var item in _dict)
            {
                string value = item.Value != null ? item.Value.ToString() : string.Empty;
                str += value;
            }

            return str;
        }
    }

    public class GeneratePropertiesObject<T>: GeneratePropertiesObject
    {
        public T model
        {
            get
            {
                return (T)_obj;
            }
        }

        public GeneratePropertiesObject(T obj)
        {
            _obj = obj;

            _dict = new Dictionary<string, object>();
        }

        public GeneratePropertiesObject<T> Add<TResult>(Expression<Func<T, TResult>> selector)
        {
            var body = (MemberExpression)selector.Body;
            var func = selector.Compile();

            string propNmae = body.Member.Name;
            var value = func((T)_obj);

            _dict.Add(propNmae, value);

            return this;
        }
    }
}
