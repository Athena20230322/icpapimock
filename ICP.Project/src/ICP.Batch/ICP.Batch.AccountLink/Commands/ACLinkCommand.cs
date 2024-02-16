using ICP.Batch.AccountLink.Factories;
using ICP.Library.Models.AccountLinkApi.Enums;

namespace ICP.Batch.AccountLink.Commands
{
    public class ACLinkCommand
    {
        private readonly BankFactory _bankFactory = null;
        private BaseCommand _baseCommand = null;

        public ACLinkCommand(
            BankFactory bankFactory
            )
        {
            _bankFactory = bankFactory;
        }

        /// <summary>
        /// 指定來源銀行
        /// </summary>
        /// <param name="bankType"></param>
        public void CreateBank(BankType bankType)
        {
            _baseCommand = _bankFactory.Create(bankType);
        }

        public void exec()
        {
            _baseCommand?.exec();
        }
    }
}
