using ICP.Batch.AccountLink.Services;
using ICP.Infrastructure.Abstractions.Logging;

namespace ICP.Batch.AccountLink.Commands
{
    public abstract class BaseCommand
    {
        protected ILogger _logger = null;
        protected EMailNotifyService _eMailNotifyService = null;

        public abstract void exec();
    }
}
