using ICP.Infrastructure.Abstractions;
using ICP.Infrastructure.Abstractions.Logging;

namespace ICP.Batch.Demo.Commands
{
    public class DemoBatchCommand
    {
        private readonly ILogger _logger = null;

        public DemoBatchCommand(ILogger<DemoBatchCommand> logger)
        {
            _logger = logger;
            _logger.Trace("ctor");
        }

        public string Test()
        {
            _logger.Trace("method");
            return "Test";
        }
    }
}
