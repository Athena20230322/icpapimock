using System;
using ICP.Batch.InvoiceCarrierDetail.Services;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Services.MailLibrary;

namespace ICP.Batch.InvoiceCarrierDetail.Commands
{
    public class InvoiceCarrierDetailCommand
    {
        ILogger<InvoiceCarrierDetailCommand> _logger;
        
        private InvoiceCarrierDetailService _carrierDetailService;
        private MailSendService _mailSendService;

        public InvoiceCarrierDetailCommand(
            ILogger<InvoiceCarrierDetailCommand> logger, 
            InvoiceCarrierDetailService invoiceCarrierService, 
            MailSendService mailSendService)
        {
            _logger = logger;
            _carrierDetailService = invoiceCarrierService;
            _mailSendService = mailSendService;
        }
        /// <summary>
        /// 執行排程
        /// </summary>
        /// <returns></returns>
        public void exec()
        {
            //電子發票載具明細下載更新
            execWithTryCatch(InvoiceCarrierDetail);
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
                _logger.Warning($"電子發票載具明細[例外訊息]:{ex}");

               // _mailSendService.SendErrorMail("[電子發票明細][明細下載更新] 排程錯誤", ex.ToString());

            }
        }
        /// <summary>
        /// 電子發票載具表頭下載更新
        /// </summary>
        private void InvoiceCarrierDetail()
        {
            BaseResult result =new BaseResult();

            _logger.Info("電子發票明細表頭[下載開始]");
            result=_carrierDetailService.InvoiceCarrierDetail();
            _logger.Info($"電子發票載具明細[執行結果]:{result.RtnMsg.ToString()}");

            if (result == null || result.RtnCode != 0)
            {
                _logger.Warning($"電子發票載具明細[例外訊息]:Code:{result.RtnCode} Msg:{result.RtnMsg.ToString()}");

               // _mailSendService.SendErrorMail("[電子發票載具][明細下載更新] 排程錯誤",$"Code:{result.RtnCode} Msg:{result.RtnMsg.ToString()}");
            }


        }
    }
}