using ICP.Infrastructure.Abstractions.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Authorization.Models
{
    public class UserDataModel
    {
        public long MID { get; set; }

        public string LoginToken { get; set; }

        public bool IsWebView { get; set; }
    }
}
