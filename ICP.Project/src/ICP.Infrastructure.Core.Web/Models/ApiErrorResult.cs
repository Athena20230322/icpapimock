using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Web.Models
{
    public class ApiErrorResult : BaseResult
    {
        public string RequestId { get; set; }
    }
}
