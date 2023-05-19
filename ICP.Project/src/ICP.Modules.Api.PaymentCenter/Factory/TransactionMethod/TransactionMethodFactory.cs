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
    public class TransactionMethodFactory : ITransactionMethodFactory
    {
        private readonly IEnumerable<Meta<Func<ITransactionMethod>>> _transactionMethods = null;

        public TransactionMethodFactory(IEnumerable<Meta<Func<ITransactionMethod>>> transactionMethods)
        {
            _transactionMethods = transactionMethods;
        }

        public ITransactionMethod Create(int tradeMode)
        {
            return _transactionMethods.FirstOrDefault(x => ((eTradeMode)tradeMode).Equals(x.Metadata[nameof(eTradeMode)]))?.Value();
        }
    }
}
