using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ICP.Batch.InvoiceCarrier.Models;
using ICP.Library.Models.EinvoiceLibrary.Enum;
using ICP.Library.Repositories.MemberRepositories;
using ICP.Library.Services.EinvoiceLibrary;
using Newtonsoft.Json;

namespace ICP.Batch.InvoiceCarrier.Services
{
    using Infrastructure.Abstractions.Logging;
    using Infrastructure.Core.Extensions;
    using Infrastructure.Core.Models;
    using Repositories;

    public class InvoiceCarrierService
    {
        private readonly int _sleepTime = 1000;
        private readonly int _carrierGroup = 1;
        private InvoiceCarrierRepository _carrierRepository;
        private CarrierEinvoiceService _carrierEinvoiceService;
        private MemberConfigCyptRepository _configCyptRepository;
        private ILogger<InvoiceCarrierService> _logger;


        public InvoiceCarrierService(
            InvoiceCarrierRepository carrierRepository,
            CarrierEinvoiceService carrierEinvoiceService,
            MemberConfigCyptRepository configCyptRepository,
            ILogger<InvoiceCarrierService> logger)
        {
            _carrierRepository = carrierRepository;
            _carrierEinvoiceService = carrierEinvoiceService;
            _configCyptRepository = configCyptRepository;
            _logger = logger;
        }

        /// <summary>
        /// 電子發票載具表頭下載更新
        /// </summary>
        /// <returns></returns>
        public BaseResult InvoiceCarrier()
        {
            BaseResult result = new BaseResult();
            List<TitlePushModel> listPushTitle = _carrierRepository.ListTitlePush(_carrierGroup);

            if (listPushTitle == null || !listPushTitle.Any())
            {
                result.SetError();
                result.RtnMsg = "Not Found Any Data";
                return result;
            }

            var eivoiceBatchNo = _carrierEinvoiceService.GetBatchNo(BatchInvoiceType.TitleQuery);

            List<UpdateTitlePushModel> updateTitlePushModels = new List<UpdateTitlePushModel>();
            var chValueTuple = new List<(long checkedLongType, bool checkedBoolType)>();
            List<object> errorObjs = new List<object>();

            listPushTitle.ForEach(item =>
            {
                try
                {
                    bool isPass = chValueTuple.Any(x => item.MID == x.checkedLongType && x.checkedBoolType);
                    if (!isPass && chValueTuple.All(x => x.checkedLongType != item.MID))
                    {
                        isPass = _carrierEinvoiceService.CheckCarrierInfo(item.MID);
                        chValueTuple.Add((item.MID, isPass));
                    }

                    if (isPass)
                    {
                        result = _carrierEinvoiceService.DownloadInvTitle(item.CarrierNum, item.VerificationCode,
                            item.DownloadDate, _sleepTime, eivoiceBatchNo?.BatchNo);
                        ;
                        if (result.RtnCode != 1)
                        {
                            string json = JsonConvert.SerializeObject(result);
                            throw new Exception(json);
                        }
                    }

                    updateTitlePushModels.Add(new UpdateTitlePushModel
                    {
                        CarrierNum = item.CarrierNum,
                        DownloadDate = item.DownloadDate,
                        Status = (byte) (isPass ? 1 : 2)
                    });
                }
                catch (Exception ex)
                {
                    errorObjs.Add(new
                    {
                        Data = item,
                        Exception = ex.Message,
                        StackTrace = ex.StackTrace,
                        DT = DateTime.Now
                    });
                }
                finally
                {
                    Thread.Sleep(_sleepTime);
                }
            });

            if (updateTitlePushModels.Any())
            {
                // 分批更新避免資料量過大
                var list = updateTitlePushModels.Select((x, idx) => new {Idx = idx, Item = x})
                    .GroupBy(x => x.Idx / 100)
                    .Select(x => JsonConvert.SerializeObject(x.Select(y => y.Item)));

                foreach (var item in list)
                {
                    if (!item.TryParseToJson(out var jToken))
                    {
                        var updateResult = _carrierEinvoiceService.UpdateTitlePush(item);
                        if (updateResult.RtnCode != 1)
                        {
                            _logger.Error($"[更新錯誤] {updateResult.RtnMsg}, DATA：{item}");
                        }
                    }
                }
            }

            if (errorObjs.Any())
            {
                _logger.Error("[錯誤資料] " + JsonConvert.SerializeObject(errorObjs));
            }

            _carrierEinvoiceService.UpdateBatchNo(eivoiceBatchNo.BatchNo, BatchStatus.Finish);
            return result;
        }
    }
}