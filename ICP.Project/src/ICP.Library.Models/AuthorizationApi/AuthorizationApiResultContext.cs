using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.AuthorizationApi
{
    public class AuthorizationApiResultContext
    {
        public BaseResult Result { get; set; }

        public string Signature { get; set; }
    }
}
