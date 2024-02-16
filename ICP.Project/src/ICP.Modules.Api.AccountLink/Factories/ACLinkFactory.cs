using Autofac.Features.Metadata;
using ICP.Library.Models.AccountLinkApi.Enums;
using ICP.Modules.Api.AccountLink.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ICP.Modules.Api.AccountLink.Factories
{
    /// <summary>
    /// 各銀行AccountLink的工廠模式
    /// </summary>
    public class ACLinkFactory
    {
        private readonly IEnumerable<Meta<Func<BaseACLinkCommand>>> _baseACLinkCommand = null;

        public ACLinkFactory(IEnumerable<Meta<Func<BaseACLinkCommand>>> baseACLinkCommand)
        {
            _baseACLinkCommand = baseACLinkCommand;
        }

        /// <summary>
        /// 建立實體
        /// </summary>
        /// <param name="bankType"></param>
        /// <returns></returns>
        public BaseACLinkCommand Create(BankType bankType)
        {
            return _baseACLinkCommand.FirstOrDefault(x => bankType.Equals(x.Metadata[nameof(bankType)]))?.Value();
        }
    }
}
