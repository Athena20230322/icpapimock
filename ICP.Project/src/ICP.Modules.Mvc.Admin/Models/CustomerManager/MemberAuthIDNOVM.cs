using System;

namespace ICP.Modules.Mvc.Admin.Models.CustomerManager
{
    public class MemberAuthIDNOVM
    {
        public int AuthStatus { get; set; }

        public DateTime IssueDate { get; set; }

        public int ObtainType { get; set; }

        public string IssueLocationName { get; set; }

        public DateTime? ModifyDate { get; set; }
        public string UniformID { get; set; }

    }
}
