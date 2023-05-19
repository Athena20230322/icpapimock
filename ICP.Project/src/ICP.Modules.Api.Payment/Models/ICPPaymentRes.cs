using ICP.Infrastructure.Core.Models;
using Newtonsoft.Json;

namespace ICP.Modules.Api.Payment.Models
{
    public class ICPPaymentRes : DataResult
    {
        //[JsonProperty(PropertyName = "StatusCode")]
        [JsonIgnore]
        public override int RtnCode { get => base.RtnCode; }

        public string StatusCode { get => base.RtnCode.ToString().PadLeft(4, '0'); }

        [JsonProperty(PropertyName = "StatusMessage")]
        public override string RtnMsg { get => base.RtnMsg; }

        [JsonProperty(PropertyName = "EncData")]
        public override object RtnData { get => base.RtnData; }       
    }
}
