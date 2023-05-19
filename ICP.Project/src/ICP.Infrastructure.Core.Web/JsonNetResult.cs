using ICP.Infrastructure.Core.Models.Consts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ICP.Infrastructure.Core.Web
{
    public class JsonNetResult : JsonResult
    {
        public JsonSerializerSettings SerializerSettings { get; set; }
        public Formatting Formatting { get; set; }

        public JsonNetResult()
        {
            SerializerSettings = new JsonSerializerSettings();
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = !string.IsNullOrWhiteSpace(ContentType) ? ContentType : MimeTypes.ApplicationJson;

            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            if (Data != null)
            {
                using (var jsonTextWriter = new JsonTextWriter(response.Output))
                {
                    jsonTextWriter.Formatting = Formatting;

                    JsonSerializer serializer = JsonSerializer.Create(SerializerSettings);
                    serializer.Serialize(jsonTextWriter, Data);
                    jsonTextWriter.Flush();
                }
            }
        }
    }
}
