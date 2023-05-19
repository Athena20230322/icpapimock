using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core
{
    public static class CustomJsonSerializerSettings
    {
        static readonly Models.Converter.DecimalConverter decimalConverter = new Models.Converter.DecimalConverter();
        static readonly List<JsonConverter> jsonConvertersList = new List<JsonConverter>() { decimalConverter };

        public static JsonSerializerSettings IgnoreException = new JsonSerializerSettings()
        {
            Error = (serializer, err) => err.ErrorContext.Handled = true,
        };

        public static JsonSerializerSettings IgnoreNullValue = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        public static JsonSerializerSettings DecimalConvert = new JsonSerializerSettings()
        {
            Converters = jsonConvertersList
        };
    }
}
