namespace ICP.Host.APIService.Models
{
    public class MistakeRtnModel
    {
        public string MessageId { get; set; }

        public string dstaddr { get; set; }

        public string dlvtime { get; set; }

        public string donetime { get; set; }

        public int statuscode { get; set; }

        public string statusstr { get; set; }

        public int StatusFlag { get; set; }
    }
}