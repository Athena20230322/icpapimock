using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.ExecSendMistake.Commands
{
    using Infrastructure.Abstractions.Logging;
    using Services;

    public class SMSCommand
    {
        private readonly ILogger _logger = null;

        SMSService _service;

        public SMSCommand(SMSService service, ILogger<SMSCommand> logger)
        {
            _service = service;
            _logger = logger;
        }

        public void Start()
        {
            try
            {
                var list = _service.ListMistakeTemp();

                _service.ShortSmsSubmitMistake(list);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, null);
            }
        }
    }
}
