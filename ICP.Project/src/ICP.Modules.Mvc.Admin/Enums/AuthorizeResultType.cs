using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Enums
{
    public enum AuthorizeResultType
    {
        Success = 0,
        Logout,
        UserStatus,
        NoPermission,
        LoginToken,
        PwdExpired,
        FuncMaintain
    }
}
