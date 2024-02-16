using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace ICP.Infrastructure.Core.Extensions
{
    public static class StringExtension
    {
        public static bool TryParseToJson(this string str, out JToken jToken)
        {
            jToken = null;

            try
            {
                jToken = JToken.Parse(str);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool TryParseJsonToObj<T>(this string str, out T obj)
        {
            obj = default(T);

            if (!str.TryParseToJson(out JToken jToken))
            {
                return false;
            }

            try
            {
                obj = jToken.ToObject<T>();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool TryParseJsonToObj(this string str, Type objectType, out object obj)
        {
            obj = null;

            if (!str.TryParseToJson(out JToken jToken))
            {
                return false;
            }

            try
            {
                obj = jToken.ToObject(objectType);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool TryParseToXml(this string str, out XmlDocument xmlDocument)
        {
            try
            {
                xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(str);
                return true;
            }
            catch
            {
                xmlDocument = null;
                return false;
            }
        }       

        public static string ToBase64(this string str, int codePage = 65001)
        {
            byte[] input = Encoding.GetEncoding(codePage).GetBytes(str);
            return Convert.ToBase64String(input);
        }

        public static string RevertBase64(this string str, int codePage = 65001)
        {
            byte[] input = Convert.FromBase64String(str);
            return Encoding.GetEncoding(codePage).GetString(input);
        }

        public static string Right(this string str, int length)
        {
            length = Math.Max(length, 0);

            if (str.Length > length)
            {
                return str.Substring(str.Length - length, length);
            }
            else
            {
                return str;
            }
        }
    }
}
