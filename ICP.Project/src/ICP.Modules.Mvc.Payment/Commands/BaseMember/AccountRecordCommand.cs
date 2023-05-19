using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Payment.Models.BaseMember;
using ICP.Modules.Mvc.Payment.Services.BaseMember;
using System.Collections.Generic;

namespace ICP.Modules.Mvc.Payment.Commands.BaseMember
{
    /// <summary>
    /// 帳戶紀錄
    /// </summary>
    public class AccountRecordCommand
    {
        AccountRecordService _accountRecordService = null;

        public AccountRecordCommand(AccountRecordService accountRecordService)
        {
            _accountRecordService = accountRecordService;
        }

        /// <summary>
        /// 查詢帳戶紀錄列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<List<AccountRecordDbRes>> ListAccountRecord(AccountRecordReq request)
        {
            return _accountRecordService.ListAccountRecord(request);
        }
    }
}
