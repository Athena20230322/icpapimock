using ICP.Infrastructure.Abstractions.Logging;
using ICP.Modules.Api.Member.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Commands
{
    public class CommonCommand
    {
        private readonly CommonService _commonService = null;
        private readonly ILogger _logger = null;

        public CommonCommand(
            CommonService commonService,
            ILogger<MemberInfoCommand> logger)
        {
            _commonService = commonService;
        }


    }
}
