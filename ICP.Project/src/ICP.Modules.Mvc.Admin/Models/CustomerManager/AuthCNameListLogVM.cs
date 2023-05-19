using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerManager
{
    public class AuthCNameListLogVM
    {
        public string OCName { get; set; }

        public string CName { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreateUser { get; set; }
    }
}
