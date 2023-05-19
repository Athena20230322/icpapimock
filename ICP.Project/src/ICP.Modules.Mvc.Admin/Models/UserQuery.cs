using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models
{
    using Infrastructure.Core.Models;

    public class UserQuery : PageModel
    {
        public int? UserID { get; set; }

        public int? DeptID { get; set; }

        public int? UserGroupID { get; set; }

        public string CName { get; set; }

        public byte? UserStatus { get; set; } = 1;

        public string Account { get; set; }
    }
}
