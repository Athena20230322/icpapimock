using ICP.Infrastructure.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models
{
    public class ICPPaymentResult : DataResult
    {
        [JsonIgnore]
        public override int RtnCode { get => base.RtnCode; set => base.RtnCode = value; }

        [JsonIgnore]
        public override string RtnMsg { get => base.RtnMsg; set => base.RtnMsg = value; }

        [JsonIgnore]
        public override object RtnData { get => base.RtnData; set => base.RtnData = value; }

        public int StatusCode { get => base.RtnCode; }

        public string StatusMsg { get => base.RtnMsg; }

        public string EncData { get => base.RtnData as string; }
    }
}
