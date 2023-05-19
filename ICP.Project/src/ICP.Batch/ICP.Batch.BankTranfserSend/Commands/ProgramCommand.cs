using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ICP.Batch.BankTranfserSend.Commands
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

        public ProgramCommand(
            ILogger<ProgramCommand> logger,
            LibBankTransferService libBankTransferService,
            FirstBankService firstBankService,
            PaymentService paymentService
            )
        {
            _logger = logger;
            _libBankTransferService = libBankTransferService;
            _firstBankService = firstBankService;
            _paymentService = paymentService;
        }

        public void exec()
        {
            var listResult = _paymentService.ListBankTransferSend();
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
            _logger.Info($"{list.Count} 筆資料待送驗");

            int iSuccess = 0;
            int iError = 0;

            list.ForEach(t =>
            {
                bool success = false;
                try
                {
                    var result = UpdateBankTransferSend(t, setting);

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

            _logger.Info($"{iSuccess} 筆資料送驗成功");
            _logger.Info($"{iError} 筆資料送驗失敗");
        }

        private BaseResult UpdateBankTransferSend(BankTransferSendModel data, BankTransferSettingModel setting)
        {
            var result = new BaseResult();
            result.SetError();

            // fxml model
            var model = _paymentService.CreateB2B001Model(data, setting);

            // fxml send
            var fxmlResult = _firstBankService.Exec_B2B001(model, data.TransferID);
            if (!fxmlResult.IsSuccess)
            {
                _logger.Error($"Exec_B2B001 failure: TransferID: {data.TransferID}, RtnMsg: {fxmlResult.RtnMsg}");
                result.SetError(fxmlResult);
                return result;
            }

            // update db
            var updateResult = _paymentService.UpdateBankTransferSend(data.TransferID, fxmlResult.RtnData);
            if (!updateResult.IsSuccess)
            {
                _logger.Error($"UpdateBankTransferSend failure: TransferID: {data.TransferID}, RtnMsg: {updateResult.RtnMsg}");
                result.SetError(updateResult);
                return result;
            }

            result.SetSuccess();
            return result;
        }
    }
}