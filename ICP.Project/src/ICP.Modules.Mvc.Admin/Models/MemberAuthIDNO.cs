using ICP.Infrastructure.Core.Models;
using System;

namespace ICP.Modules.Mvc.Admin.Models
{
    public class MemberAuthIDNO : BaseListModel
    {
        public string ICPMID { get; set; }

        public string CName { get; set; }

        public DateTime? Birthday { get; set; }

        public string IssueLocationID { get; set; }

        public string IssueLocationName { get; set; }

        public long MID { get; set; }

        public string IDNO { get; set; }

        public string UniformID { get; set; }

        public DateTime? IssueDate { get; set; }

        public byte? ObtainType { get; set; }

        public byte IsPicture { get; set; }

        public byte? AuthType { get; set; }

        public byte AuthStatus { get; set; }

        public byte PaperAuthStatus { get; set; }

        public string AuthMsg { get; set; }

        public DateTime? AuthDate { get; set; }

        public DateTime? PaperAuthDate { get; set; }

        public DateTime? UniformIssueDate { get; set; }

        public DateTime? UniformExpireDate { get; set; }

        public string UniformNumber { get; set; }

        public string FilePath1 { get; set; }

        public string FilePath2 { get; set; }

        public DateTime CreateDate { get; set; }

        public string Modifier { get; set; }

        public DateTime ModifyDate { get; set; }
    }
}
