using System;

namespace ICP.Batch.AuthTeenagers.Models
{
    public class AuthTeenagersData
    {
        public long MID { get; set; }

        public byte IDNOStatus { get; set; }

        public string IDNO { get; set; }

        public DateTime IssueDate { get; set; }

        public byte ObtainType { get; set; }

        public string IssueLocationID { get; set; }

        public byte IsPicture { get; set; }

        public DateTime Birthday { get; set; }
    }
}
