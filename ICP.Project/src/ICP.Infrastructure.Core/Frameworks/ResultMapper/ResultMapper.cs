using ICP.Infrastructure.Abstractions.ResultMapper;
using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Frameworks.ResultMapper
{
    public class ResultMapper
    {
        private static IDictionary<int, string> _results = null;

        public void Init(IDictionary<int, string> results)
        {
            _results = results;
        }

        public string GetResultMsg(int rtnCode)
        {
            return _results.TryGetValue(rtnCode, out string msg) ? msg : _results[9999];
        }
    }
}
