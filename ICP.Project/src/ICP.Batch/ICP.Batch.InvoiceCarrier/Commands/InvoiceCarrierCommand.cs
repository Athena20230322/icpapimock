using System;
using ICP.Infrastructure.Core.Extensions;


namespace ICP.Batch.InvoiceCarrier.Commands
{
    using ICP.Library.Services.MailLibrary;
    using Infrastructure.Abstractions.Logging;
    using Infrastructure.Core.Models;
    using Services;

    public class InvoiceCarrierCommand
    {
        ILogger<InvoiceCarrierCommand> _logger;
        
        private InvoiceCarrierService _invoiceCarrierService;
        private MailSendService _mailSendService;

        public InvoiceCarrierCommand(
            ILogger<InvoiceCarrierCommand> logger,
            InvoiceCarrierService invoiceCarrierService, 
            MailSendService mailSendService)
        {
            _logger = logger;
            _invoiceCarrierService = invoiceCarrierService;
            _mailSendService = mailSendService;
        }

        /// <summary>
        /// 執行排程
        /// </summary>
        /// <returns></returns>
        public void exec()
        {
            //電子發票載具表頭下載更新
            execWithTryCatch(InvoiceCarrier);
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
                _logger.Warning($"電子發票載具表頭[例外訊息]:{ex}");

                //_mailSendService.SendErrorMail("[電子發票載具][表頭下載更新] 排程錯誤", ex.ToString());

            }
        }
        /// <summary>
        /// 電子發票載具表頭下載更新
        /// </summary>
        private void InvoiceCarrier()
        {
            BaseResult result =new BaseResult();
            result.SetError();

            _logger.Info("電子發票載具表頭[下載開始]");
            result=_invoiceCarrierService.InvoiceCarrier();
            _logger.Info($"電子發票載具表頭[執行結果]:{result.RtnMsg.ToString()}");

            if (result == null || result.RtnCode != 1)
            {
                _logger.Warning($"電子發票載具表頭[例外訊息]:Code:{result.RtnCode} Msg:{result.RtnMsg.ToString()}");
                
                //_mailSendService.SendErrorMail("[電子發票載具][表頭下載更新] 排程錯誤",$"Code:{result.RtnCode} Msg:{result.RtnMsg.ToString()}");
            }


        }
    }
}