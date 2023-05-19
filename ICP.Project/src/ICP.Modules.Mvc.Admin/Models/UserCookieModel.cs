using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models
{
    public class UserCookieModel
    {
        public int UserID { get; set; }

        public string LoginToken { get; set; }
    }
}
