using ICP.Infrastructure.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Web.Models
{
    public class AppResult : DataResult
    {
        [JsonIgnore]
        public override object RtnData { get => base.RtnData; set => base.RtnData = value; }

        public string EncData { get => base.RtnData as string; }
    }
}
