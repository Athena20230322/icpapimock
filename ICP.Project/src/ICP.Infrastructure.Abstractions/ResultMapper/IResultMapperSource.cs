using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Abstractions.ResultMapper
{
    public interface IResultMapperSource : IDisposable
    {
        IDictionary<int, string> GetResults();
    }
}
