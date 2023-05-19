using System;

namespace ICP.Batch.PICChargeFeeResult.Commands
{
    using ICP.Batch.PICChargeFeeResult.Models;
    using ICP.Batch.PICChargeFeeResult.Models.Enums;
    using ICP.Batch.PICChargeFeeResult.PICWebService;
    using ICP.Batch.PICChargeFeeResult.Repositories;
    using ICP.Batch.PICChargeFeeResult.Services;
    using Infrastructure.Abstractions.Logging;
    using System.Collections.Generic;
    using System.Linq;

    public class PICChargeFeeResultCommand 
    {
        private readonly ChargeFeeRepository _chargeFeeRepository = null;
        private readonly ILogger<PICChargeFeeResultCommand> _logger;
        private readonly EMailNotifyService _eMailNotifyService;
        private readonly SellerJobReceiverServiceImplService _picWS;

        public PICChargeFeeResultCommand(
            ILogger<PICChargeFeeResultCommand> logger,
             EMailNotifyService eMailNotifyService,
             ChargeFeeRepository chargeFeeRepository) 
        {
            _logger = logger;
            _eMailNotifyService = eMailNotifyService;
            _chargeFeeRepository = chargeFeeRepository;

            _picWS = new SellerJobReceiverServiceImplService();
            _picWS.Url = ConfigService.PicWSUrl;
        }

        /// <summary>
        /// 執行排程
        /// </summary>
        /// <returns></returns>
        public void exec()
        {
            execWithTryCatch(ChargeFeeResult);
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
                _logger.Warning(ex.ToString());
                _eMailNotifyService.SendErrorEmail(ex);
            }
        }

        private void ChargeFeeResult()
        {
            _logger.Info($"開始 - 輪巡待開立手續費發票排程");

            // 取得 PIC需要的使用者資訊
            wsUser user = GetPicUserModel();

            List<iuo010OrdersHeadVO> invoiceSearchInfo = GetInvoiceSearchInfo();

            _logger.Warning($"搜尋時間 : { invoiceSearchInfo[0].InvoiceDateFrom} ~ {invoiceSearchInfo[0].InvoiceDateTo}, 搜尋類別 : {invoiceSearchInfo[0].BuyerType}");

            siuo010ResultWebVo PicResult = _picWS.SIUO010(user, invoiceSearchInfo.ToArray());

            if (PicResult != null && PicResult.Code == "0")
            {
                if (PicResult.invoices.ToList().Count > 0)
                {
                    PicResult.invoices.ToList().ForEach(Invoice =>
                    {
                        CF_InvoiceIssue_UpdateModel updateModel = new CF_InvoiceIssue_UpdateModel()
                        {
                            InvoiceNo = Invoice.orderNo,
                            InvoiceNumber = Invoice.invoiceNo,
                            InvoiceDate = Invoice.invoiceDate,
                            Issue_Status = Issue_StatusEnum.Success,
                            State = StateEnums.Done,
                            RtnCode = PicResult.Code,
                            RtnMsg = PicResult.Description,
                            Modifier = "PICChargeFeeResult"
                        };

                        _chargeFeeRepository.UpdateInvoiceIssueByPicResult(updateModel);

                    });
                }
                _logger.Warning($"更新手續費資料。共取得資料 : { PicResult.invoices.ToList().Count}");
            }
            else
            {
                _logger.Warning($"搜尋手續費資料錯誤。回傳代碼 : {PicResult.Code}, 回傳錯誤 : {PicResult.Description}");
            }
            _logger.Info($"結束 - 輪巡待開立手續費發票排程");
        }

        /// <summary>
        /// 取得 PIC需要的使用者資訊
        /// </summary>
        /// <returns></returns>
        private wsUser GetPicUserModel()
        {
            //需事先與統一資訊電子發票平台申請
            wsUser user = new wsUser();
            user.unifiedNo = ConfigService.Identifier;   //登入統編
            user.usrAcc = ConfigService.Account;   //登入帳號
            user.usrMima = ConfigService.Password;    //登入密碼

            return user;
        }

        /// <summary>
        /// 取得搜尋發票條件
        /// </summary>
        /// <returns></returns>
        private List<iuo010OrdersHeadVO> GetInvoiceSearchInfo()
        {
            List<iuo010OrdersHeadVO> InvoiceList = new List<iuo010OrdersHeadVO>();
            iuo010OrdersHeadVO invoiceSearchInfo = new iuo010OrdersHeadVO();

            //判斷 Config 是否有指定要跑哪一天，如果沒有預設今天和昨天
            if (!string.IsNullOrEmpty(ConfigService.InvoiceDateFrom)
                && !string.IsNullOrEmpty(ConfigService.InvoiceDateTo)
                && !string.IsNullOrEmpty(ConfigService.BuyerType))
            {
                invoiceSearchInfo.InvoiceDateFrom = ConfigService.InvoiceDateFrom;
                invoiceSearchInfo.InvoiceDateTo = ConfigService.InvoiceDateTo;
                invoiceSearchInfo.BuyerType = ConfigService.BuyerType;
            }
            else
            {
                invoiceSearchInfo.InvoiceDateFrom = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
                invoiceSearchInfo.InvoiceDateTo = DateTime.Now.ToString("yyyyMMdd");
                invoiceSearchInfo.BuyerType = "0";
            }

            InvoiceList.Add(invoiceSearchInfo);

            return InvoiceList;
        }


    }
}
