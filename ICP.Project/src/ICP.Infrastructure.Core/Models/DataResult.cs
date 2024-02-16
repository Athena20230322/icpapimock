using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Models
{
    public class DataResult : BaseResult
    {
        public virtual object RtnData { get; set; }
    }

    public class DataResult<T> : DataResult
    {
        public new T RtnData { get => (T)base.RtnData; set => base.RtnData = value; }
    }
}
