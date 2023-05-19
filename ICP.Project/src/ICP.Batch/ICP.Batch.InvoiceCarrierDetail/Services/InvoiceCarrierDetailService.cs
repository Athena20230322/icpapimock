using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ICP.Batch.InvoiceCarrierDetail.Repositories;
using ICP.Library.Models.EinvoiceLibrary.Enum;
using ICP.Library.Repositories.MemberRepositories;
using ICP.Library.Services.EinvoiceLibrary;
using Newtonsoft.Json;

namespace ICP.Batch.InvoiceCarrierDetail.Services
{
    using Infrastructure.Abstractions.Logging;
    using Infrastructure.Core.Extensions;
    using Infrastructure.Core.Models;

    //using Repositories;

    public class InvoiceCarrierDetailService
    {
        private readonly int _sleepTime = 1000;
        private readonly int _carrierGroup = -1;

        private InvoiceCarrierDetailRepository _carrierDetailRepository;
        private CarrierEinvoiceService _carrierEinvoiceService;
        private MemberConfigCyptRepository _configCyptRepository;
        private ILogger<InvoiceCarrierDetailService> _logger;

        public InvoiceCarrierDetailService(
            CarrierEinvoiceService carrierEinvoiceService,
            MemberConfigCyptRepository configCyptRepository,
            ILogger<InvoiceCarrierDetailService> logger,
            InvoiceCarrierDetailRepository carrierDetailRepository)
        {
            _carrierEinvoiceService = carrierEinvoiceService;
            _configCyptRepository = configCyptRepository;
            _logger = logger;
            _carrierDetailRepository = carrierDetailRepository;
        }

        public BaseResult InvoiceCarrierDetail()
        {
            BaseResult result = new BaseResult();
            var listDetail = _carrierDetailRepository.QueryCarrierDetail();

            if (listDetail == null || !listDetail.Any())
            {
                result.SetError();
                result.RtnMsg = "Not Found Any Data";
                return result;
            }

            var eivoiceBatchNo = _carrierEinvoiceService.GetBatchNo(BatchInvoiceType.DetailQuery);

            List<object> errorObjs = new List<object>();
            List<object> cardEncryptErrorObjs = new List<object>();
            var chValueTuple = new List<(long checkedLongType, bool checkedBoolType)>();
            int successCount = 0;

            listDetail.ForEach(item =>
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
                        result = _carrierEinvoiceService.DownloadInvDetail(
                                                                            item.EinvoicePeriod,
                                                                            item.EinvoiceNum,
                                                                            item.EinvoiceCreateDate.GetValueOrDefault(),
                                                                            item.CarrierNum,
                                                                            item.VerificationCode,
                                                                            eivoiceBatchNo?.BatchNo);
                        if (result.RtnCode != 1)
                        {
                            string json = JsonConvert.SerializeObject(result);
                            throw new Exception(json);
                        }
                    }
                    else
                    {
                        cardEncryptErrorObjs.Add(item);
                    }
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
            if (errorObjs.Any())
            {
                result.SetError();
                result.RtnMsg = ("[錯誤資料] " + JsonConvert.SerializeObject(errorObjs));
            }

            if (cardEncryptErrorObjs.Any())
            {
                result.SetError();
                result.RtnMsg = ("[驗證碼有誤] " + JsonConvert.SerializeObject(cardEncryptErrorObjs));
            }

            _carrierEinvoiceService.UpdateBatchNo(eivoiceBatchNo.BatchNo, BatchStatus.Finish);

            result.RtnCode = (errorObjs.Any()) ? 0 : 1;
            result.RtnMsg =
                $"批號：{eivoiceBatchNo?.BatchNo}, 成功：{successCount} 筆, 失敗：{errorObjs.Count} 筆, 驗證碼有誤：{cardEncryptErrorObjs.Count} 筆";

            return result;
        }
    }
}