using Newtonsoft.Json;

namespace ICP.Infrastructure.Core.Models
{
    public class BaseResult
    {
        [JsonIgnore]
        public bool IsSuccess
        {
            get
            {
                return RtnCode == 1;
            }
            set
            {
                RtnCode = value ? 1 : 0;
            }
        }

        public virtual int RtnCode { get; set; }

        public virtual string RtnMsg { get; set; }
    }
}
