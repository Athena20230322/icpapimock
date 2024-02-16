using ICP.Infrastructure.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Exceptions
{
    public class BaseResultException : Exception
    {
        private readonly string _message = null;

        public BaseResultException(BaseResult result)
        {
            _message = JsonConvert.SerializeObject(result);
        }

        public override string Message => _message;
    }
}
