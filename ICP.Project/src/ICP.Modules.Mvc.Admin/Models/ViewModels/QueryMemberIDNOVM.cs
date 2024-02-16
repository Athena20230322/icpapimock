using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Enums;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{
    public class QueryMemberIDNOVM : PageModel
    {
        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public AuthStatusType Status { get; set; }

        public string CName { get; set; }

        public string IDNO { get; set; }

        public string ICPMID { get; set; }
    }
}
