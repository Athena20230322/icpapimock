using ICP.Infrastructure.Core.Models;
using System;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{
    public class MemberAuthIDNOModel : BaseListModel
    {
        public string ICPMID { get; set; }

        public string CName { get; set; }

        public DateTime? Birthday { get; set; }

        public string IssueLocationName { get; set; }

        public long MID { get; set; }

        public string IDNO { get; set; }

        public DateTime? IssueDate { get; set; }

        public string ObtainType { get; set; }

        public byte? AuthStatus { get; set; }

        public byte? PaperAuthStatus { get; set; }

        public string AuthMsg { get; set; }

        public DateTime? AuthDate { get; set; }

        public string FilePath1 { get; set; }

        public string FilePath2 { get; set; }

        public string Modifier { get; set; }

        public DateTime ModifyDate { get; set; }
    }
}
