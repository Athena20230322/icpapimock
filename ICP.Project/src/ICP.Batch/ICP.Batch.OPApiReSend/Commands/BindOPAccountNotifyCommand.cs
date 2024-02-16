using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.OPApiReSend.Commands
{
    using Infrastructure.Abstractions.Logging;
    using Services;
    using Library.Services.MemberServices;

    public class BindOPAccountNotifyCommand
    {
        ILogger<BindOPAccountNotifyCommand> _logger;
        EMailNotifyService _eMailNotifyService;
        OPApiReSendService _oPApiReSendService;
        LibMemberAuthService _libMemberAuthService;

        public BindOPAccountNotifyCommand(
            ILogger<BindOPAccountNotifyCommand> logger,
            EMailNotifyService eMailNotifyService,
            OPApiReSendService oPApiReSendService,
            LibMemberAuthService libMemberAuthService
            )
        {
            _logger = logger;
            _eMailNotifyService = eMailNotifyService;
            _oPApiReSendService = oPApiReSendService;
            _libMemberAuthService = libMemberAuthService;
        }

        /// <summary>
        /// 執行排程
        /// </summary>
        /// <returns></returns>
        public void exec()
        {
            //綁定/解綁 OP帳號通知重送
            execWithTryCatch(BindOPAccountNotify_ReSendRecord);
        }

        /// <summary>
        /// 使用 try_catch 執行
        /// </summary>
        /// <param name="action"></param>
        private void execWithTryCatch(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                _eMailNotifyService.SendErrorEmail(ex);
            }
        }

        /// <summary>
        /// 綁定/解綁 OP帳號通知重送
        /// </summary>
        private void BindOPAccountNotify_ReSendRecord()
        {
            _logger.Info("綁定/解綁 OP帳號通知重送 Begin");

            var list = _oPApiReSendService.ListBindOPAccountNotify_ReSendRecord(Source: 2);

            _logger.Info("重送 {0} 筆", list.Count);

            int failure = 0;
            int success = 0;

            list.ForEach(t =>
            {
                var notifyResult = _libMemberAuthService.BindOPAccountNotify(t.Type, t.MID, t.OPMID, t.ICPMID, t.RecordID);

                if (notifyResult.IsSuccess)
                    success++;
                else
                    failure++;
            });

            _logger.Info("成功 {0} 筆", success);
            _logger.Info("失敗 {0} 筆", failure);

            _logger.Info("綁定/解綁 OP帳號通知重送 End");
        }
    }
}
