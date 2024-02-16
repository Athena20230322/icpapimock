using System;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class TeenagersLegalDetail
    {
        public long MID { get; set; }

        public long TeenagersMID { get; set; }

        public string CName { get; set; }

        public byte LegalType { get; set; }

        public byte Status { get; set; }

        public DateTime? AgreeDate { get; set; }

        public string IDNOFile1 { get; set; }

        public string IDNOFile2 { get; set; }

        public string FilePath1 { get; set; }

        public string FilePath2 { get; set; }

        public string FilePath3 { get; set; }

        public string FilePath4 { get; set; }

        public string FilePath5 { get; set; }

        public string FilePath6 { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? ModifyDate { get; set; }

        public string Modifier { get; set; }
    }
}
