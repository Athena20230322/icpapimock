using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models
{
    using Infrastructure.Core.Models;

    public class UserGroupQuery: PageModel
    {
        public string UserGroupName { get; set; }

        public byte? Visible { get; set; } = 1;
    }
}
