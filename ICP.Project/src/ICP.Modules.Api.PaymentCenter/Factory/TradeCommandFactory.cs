using Autofac.Features.Metadata;
using ICP.Modules.Api.PaymentCenter.Enums;
using ICP.Modules.Api.PaymentCenter.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Factory
{
    public class TradeCommandFactory : ITradeCommandFactory
    {
        private readonly IEnumerable<Meta<Func<ITradeCommand>>> _tradeCommands = null;

        public TradeCommandFactory(IEnumerable<Meta<Func<ITradeCommand>>> tradeCommands)
        {
            _tradeCommands = tradeCommands;
        }

        public ITradeCommand Create(eTradeMode tradeMode)
        {
            return _tradeCommands.FirstOrDefault(x => tradeMode.Equals(x.Metadata[nameof(eTradeMode)]))?.Value();
        }
    }
}
