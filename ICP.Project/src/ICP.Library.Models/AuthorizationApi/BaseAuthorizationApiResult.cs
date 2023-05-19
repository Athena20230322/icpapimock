using ICP.Infrastructure.Core.Models.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.AuthorizationApi
{
    public class BaseAuthorizationApiResult
    {
        public string Timestamp
        {
            get
            {
                return DateTime.Now.ToString(FormatConst.DateTime);
            }
        }
    }
}
