using Autofac.Features.Metadata;
using ICP.Modules.Api.Payment.Interface;
using ICP.Modules.Api.Payment.Models.Payment;
using System.Collections.Generic;
using System.Linq;

namespace ICP.Modules.Api.Payment.Factory
{
    public class TradeModeFactory : ITradeModeFactory
    {
        private readonly IEnumerable<Meta<ITradeMode>> _tradeModeManagers = null;

        public TradeModeFactory(IEnumerable<Meta<ITradeMode>> tradeModeManagers)
        {
            _tradeModeManagers = tradeModeManagers;
        }

        public ITradeMode Create(eTradeMode tradeMode)
        {
            return _tradeModeManagers.FirstOrDefault(x => tradeMode.Equals(x.Metadata[nameof(eTradeMode)]))?.Value;
        }
    }
}
