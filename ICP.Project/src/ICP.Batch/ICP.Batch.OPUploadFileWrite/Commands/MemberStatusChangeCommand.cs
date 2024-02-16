using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.OPUploadFileWrite.Commands
{
    using Infrastructure.Abstractions.Logging;
    using Services;
    using Library.Services.MemberServices;

    public class MemberStatusChangeCommand
    {
        ILogger<MemberStatusChangeCommand> _logger;
        EMailNotifyService _eMailNotifyService;
        OPMemberDataWriteService _oPMemberDataWriteService;
        LibMemberAuthService _libMemberAuthService;

        public MemberStatusChangeCommand(
            ILogger<MemberStatusChangeCommand> logger,
            EMailNotifyService eMailNotifyService,
            OPMemberDataWriteService oPMemberDataWriteService,
            LibMemberAuthService libMemberAuthService
            )
        {
            _logger = logger;
            _eMailNotifyService = eMailNotifyService;
            _oPMemberDataWriteService = oPMemberDataWriteService;
            _libMemberAuthService = libMemberAuthService;
        }

        /// <summary>
        /// 執行排程
        /// </summary>
        /// <returns></returns>
        public void exec(DateTime? execTime = null)
        {
            if (execTime == null) execTime = DateTime.Now;

            //綁定/解綁 OP帳號通知產檔
            execWithTryCatch(generateFile, execTime.Value);
        }

        /// <summary>
        /// 使用 try_catch 執行
        /// </summary>
        /// <param name="action"></param>
        private void execWithTryCatch(Action<DateTime> action, DateTime execTime)
        {
            try
            {
                action(execTime);
            }
            catch (Exception ex)
            {
                _eMailNotifyService.SendErrorEmail(ex);
            }
        }

        /// <summary>
        /// 綁定/解綁 OP帳號通知產檔
        /// </summary>
        private void generateFile(DateTime execDate)
        {
            _logger.Info("綁定/解綁 OP帳號通知產檔 Begin");
            string logEndMsg = "綁定/解綁 OP帳號通知產檔 End";

            //取得執行日前一天資料
            var DataTime = execDate.AddDays(-1);
            var listResult = _oPMemberDataWriteService.ListBindOPAccountNotify_ReSendRecord(Source: 3, DataDate: DataTime);
            if (!listResult.IsSuccess)
            {
                _logger.Info(listResult.RtnMsg);
                _logger.Info(logEndMsg);
                return;
            }

            var list = listResult.RtnData;
            _logger.Info("準備檔案重送 {0} 筆", list.Count);

            //產生 txt
            var genTxtResult = _oPMemberDataWriteService.GenerateTxt_MemberStatusChangeFile(DataTime, list);
            if (!genTxtResult.IsSuccess)
            {
                _logger.Error(genTxtResult.RtnMsg);
                _logger.Info(logEndMsg);
                return;
            }

            //產生 zip
            string txtFilePath = genTxtResult.RtnData;
            var zipResult = _oPMemberDataWriteService.WriteZip_MemberStatusChangeFile(txtFilePath);
            if (!zipResult.IsSuccess)
            {
                _logger.Error(zipResult.RtnMsg);
                _logger.Info(logEndMsg);
                return;
            }

            string zipFilePath = zipResult.RtnData;
            //todo: Copy zip to UploadDir (SFTP or Local)

            //備份 txt, zip
            _oPMemberDataWriteService.Back_MemberStatusChangeFile(txtFilePath, zipFilePath);

            int failure = 0;
            int success = 0;

            list.ForEach(t =>
            {
                var notifyResult = _libMemberAuthService.BindOPAccountNotify(t.Type, t.MID, t.OPMID, t.ICPMID, t.RecordID);

                if (notifyResult.IsSuccess)
                    success++;
                else
                {
                    _logger.Error($"狀態回壓: Type:{t.Type}, MID:{t.MID}, OPMID:{t.OPMID}, ICPMID:{t.ICPMID}, RecordID:{t.RecordID}");
                    failure++;
                }
            });

            _logger.Info("狀態回壓成功 {0} 筆", success);
            _logger.Info("狀態回壓失敗 {0} 筆", failure);

            _logger.Info(logEndMsg);
        }
    }
}