using ICP.Infrastructure.Core.Models;
using System;

namespace ICP.Modules.Mvc.Admin.Models
{
    public class QueryMemberBankAccount : PageModel
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public byte? AccountStatus { get; set; }

        public byte? PaperAuthStatus { get; set; }

        public string CName { get; set; }

        public string ICPMID { get; set; }
    }
}
