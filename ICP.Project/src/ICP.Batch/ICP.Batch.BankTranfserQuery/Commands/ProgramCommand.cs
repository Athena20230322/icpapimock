using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ICP.Batch.BankTranfserQuery.Commands
{
    using Infrastructure.Abstractions.Logging;
    using Infrastructure.Core.Extensions;
    using Infrastructure.Core.Models;
    using Library.Services.ManageBank;
    using Library.Services.Payment;
    using Models;
    using Services;
    using ICP.Library.Models.ManageBank.FirstBank;

    public class ProgramCommand
    {
        ILogger _logger;
        LibBankTransferService _libBankTransferService;
        FirstBankService _firstBankService;
        PaymentService _paymentService;
        MemberBankService _memberBankService;

        public ProgramCommand(
            ILogger<ProgramCommand> logger,
            LibBankTransferService libBankTransferService,
            FirstBankService firstBankService,
            PaymentService paymentService,
            MemberBankService memberBankService
            )
        {
            _logger = logger;
            _libBankTransferService = libBankTransferService;
            _firstBankService = firstBankService;
            _paymentService = paymentService;
            _memberBankService = memberBankService;
        }

        public void exec()
        {
            var listResult = _paymentService.ListBankTransferQuery();
            if (!listResult.IsSuccess)
            {
                _logger.Info("無資料");
                return;
            }

            var settingResult = _libBankTransferService.GetBankTransferSetting();
            if (!settingResult.IsSuccess)
            {
                _logger.Error(settingResult.RtnMsg);
                return;
            }
            var setting = settingResult.RtnData;

            var list = listResult.RtnData;
            _logger.Info($"{list.Count} 筆資料待查詢");

            int iSuccess = 0;
            int iError = 0;

            list.ForEach(t =>
            {
                bool success = false;
                try
                {
                    var result = UpdateBankTransferQuery(t, setting);

                    success = result.IsSuccess;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, $"{JsonConvert.SerializeObject(t)}");
                }

                if (success)
                    iSuccess++;
                else
                    iError++;
            });

            _logger.Info($"{iSuccess} 筆資料查詢成功");
            _logger.Info($"{iError} 筆資料查詢失敗");
        }

        private BaseResult UpdateBankTransferQuery(BankTransferQueryModel data, BankTransferSettingModel setting)
        {
            var result = new BaseResult();
            result.SetError();

            // fxml model
            var model = _paymentService.CreateB2B002Model(data, setting);

            // fxml send
            var fxmlResult = _firstBankService.Exec_B2B002(model, data.TransferID);
            if (!fxmlResult.IsSuccess)
            {
                _logger.Error($"Exec_B2B002 failure: TransferID: {data.TransferID}, RtnMsg: {fxmlResult.RtnMsg}");
                result.SetError(fxmlResult);
                return result;
            }

            // update bankTranfser
            var updateResult = _paymentService.UpdateBankTransferQuery(data.TransferID, fxmlResult.RtnData);
            if (!updateResult.IsSuccess)
            {
                _logger.Error($"UpdateBankTransferQuery failure: TransferID: {data.TransferID}, RtnMsg: {updateResult.RtnMsg}");
                result.SetError(updateResult);
                return result;
            }

            // update bankAccount
            _memberBankService.UpdateMemberBankAccountStatus(data, fxmlResult.RtnData);

            result.SetSuccess();
            return result;
        }
    }
}