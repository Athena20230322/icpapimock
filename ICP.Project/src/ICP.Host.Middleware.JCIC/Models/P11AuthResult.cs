using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICP.Host.Middleware.JCIC.Models
{
    public class P11AuthResult : BaseResult
    {
        public short IsPass { get; set; }

        public string Answer { get; set; }

        public string Result { get; set; }
    }
}