using ICP.Infrastructure.Core.Models;
using System;

namespace ICP.Modules.Mvc.Admin.Models
{
    public class QueryMemberIDNO : PageModel
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public byte? AuthStatus { get; set; }

        public byte? PaperAuthStatus { get; set; }

        public string CName { get; set; }

        public string IDNO { get; set; }
        
        public string ICPMID { get; set; }
    }
}
