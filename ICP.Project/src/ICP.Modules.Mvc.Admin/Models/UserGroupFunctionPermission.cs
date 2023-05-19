using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models
{
    public class UserGroupFunctionPermission
    {
        public int FunctionGroupID { get; set; }

        public int FunctionID { get; set; }

        public int ActionSum { get; set; }
    }
}
