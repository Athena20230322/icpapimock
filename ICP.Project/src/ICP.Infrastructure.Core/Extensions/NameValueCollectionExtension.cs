using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Extensions
{
    public static class NameValueCollectionExtension
    {
        public static Dictionary<string, string> ToDictionary(this NameValueCollection collection)
        {
            return collection.AllKeys.ToDictionary(x => x, x => collection[x]);
        }
    }
}
