using Autofac.Features.Metadata;
using ICP.Batch.AccountLink.Commands;
using ICP.Library.Models.AccountLinkApi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ICP.Batch.AccountLink.Factories
{
    /// <summary>
    /// 各銀行AccountLink的工廠模式
    /// </summary>
    public class BankFactory
    {
        private readonly IEnumerable<Meta<Func<BaseCommand>>> _baseCommand = null;

        public BankFactory(IEnumerable<Meta<Func<BaseCommand>>> baseCommand)
        {
            _baseCommand = baseCommand;
        }

        /// <summary>
        /// 建立實體
        /// </summary>
        /// <param name="bankType"></param>
        /// <returns></returns>
        public BaseCommand Create(BankType bankType)
        {
            return _baseCommand.FirstOrDefault(x => bankType.Equals(x.Metadata[nameof(bankType)]))?.Value();
        }
    }
}
