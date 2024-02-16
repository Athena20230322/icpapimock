using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Models
{
    public class BaseListModel : ValidatableObject
    {
        public int TotalCount { get; set; }
    }
}
