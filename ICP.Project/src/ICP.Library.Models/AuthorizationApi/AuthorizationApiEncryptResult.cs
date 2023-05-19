using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.AuthorizationApi
{
    public class AuthorizationApiEncryptResult : BaseResult
    {
        public string EncData { get; set; }
    }
}
