using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Models.Consts;
using ICP.Library.Models.EinvoiceLibrary;
using ICP.Library.Models.EinvoiceLibrary.Enum;
using ICP.Library.Repositories.EinvoiceLibrary;
using ICP.Library.Repositories.MemberRepositories;
using ICP.Library.Services.Einvoice;
using Newtonsoft.Json;

namespace ICP.Library.Services.EinvoiceLibrary
{
    /// <summary>
    /// 電子發票服務
    /// </summary>
    public class CarrierEinvoiceService
    {
        private EinvoiceRepository _einvoiceRepository;
        private MemberConfigCyptRepository _configCyptRepository;
        private EinvoiceService _einvoiceService;

        public CarrierEinvoiceService(
            EinvoiceRepository einvoiceRepository,
            MemberConfigCyptRepository configCyptRepository,
            EinvoiceService einvoiceService)
        {
            _einvoiceRepository = einvoiceRepository;
            _configCyptRepository = configCyptRepository;
            _einvoiceService = einvoiceService;
        }

        #region Download

        /// <summary>
        /// 下載發票表頭
        /// </summary>
        /// <param name="cardNo"></param>
        /// <param name="cardEncrypt"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        public BaseResult DownloadInvTitle(string cardNo, string cardEncrypt, DateTime startDate, int delayTime = 1000,
            string batchNo = null)
        {

            while (startDate.Date < DateTime.Now.Date)
            {

                var endDate = startDate.AddMonths(1);
                if (endDate > DateTime.Now.Date)
                {
                    endDate = DateTime.Now;
                }

                var apiResult = _einvoiceService.GetCarrierInvTitle(new CarrierInvTitleDTO
                {
                    CardEncrypt = cardEncrypt,
                    CardNo = cardNo,
                    StartDate = startDate.ToString("yyyy/MM/dd"),
                    EndDate = endDate.ToString("yyyy/MM/dd"),
                    OnlyWinningInv = "N",
                    Timeout = 60
                });
                // 財政部掛掉直接跳出
                if (apiResult.Code != "200")
                {
                    return new BaseResult()
                    {
                        RtnCode = 0,
                        RtnMsg = apiResult.Msg
                    };
                }

                addOrUpdateInvTitle(cardNo, apiResult.Details, batchNo);
                startDate = endDate.AddDays(1);
                Thread.Sleep(delayTime);
            }

            _einvoiceRepository.UpdateCarrierDownload(cardNo);

            return new BaseResult().SetSuccess();
        }

        /// <summary>
        /// 下載發票表頭(僅載一天)
        /// </summary>
        /// <param name="cardNo"></param>
        /// <param name="cardEncrypt"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public BaseDataModel DownloadInvTitleOneDate(string cardNo, string cardEncrypt, DateTime date,
            string batchNo = null)
        {
            var apiResult = _einvoiceService.GetCarrierInvTitle(new CarrierInvTitleDTO
            {
                CardEncrypt = cardEncrypt,
                CardNo = cardNo,
                StartDate = date.ToString("yyyy/MM/dd"),
                EndDate = date.ToString("yyyy/MM/dd"),
                OnlyWinningInv = "N"
            });

            // 財政部掛掉直接跳出
            if (apiResult.Code != "200")
            {
                return new BaseDataModel
                {
                    RtnCode = 0,
                    RtnMsg = apiResult.Msg
                };
            }

            addOrUpdateInvTitle(cardNo, apiResult.Details, batchNo);
            return new BaseDataModel
            {
                RtnCode = 1,
                RtnData = new Dictionary<string, object>
                {
                    {
                        typeof(TilteDetail).FullName, apiResult.Details
                    }
                }
            };
        }

        /// <summary>
        /// 下載發票明細
        /// </summary>
        /// <param name="invNum"></param>
        /// <param name="invDate"></param>
        /// <param name="cardNo"></param>
        /// <param name="cardEncrypt"></param>
        /// <param name="batchNo"></param>
        /// <returns></returns>
        public BaseResult DownloadInvDetail(string invPeriod, string invNum, DateTime invDate, string cardNo,
            string cardEncrypt, string batchNo = null)
        {
            BaseResult result = new BaseResult();
            result.SetError();

            var apiResult = _einvoiceService.GetCarrierInvDetail(new CarrierInvDetailDTO
            {
                CardEncrypt = cardEncrypt,
                CardNo = cardNo,
                InvDate = invDate.ToString("yyyy/MM/dd"),
                InvNum = invNum
            });

            var arrInvStatus = new string[] {"2", "已確認"};
            // 查無此發票 or 已作廢, 重撈表頭並確定是真的無此發票則將發票狀態壓掉
            if (apiResult.Code == "915")
            {
                var checkResult = checkInvTitleIsExist(invPeriod, invNum, invDate, cardNo, cardEncrypt, batchNo);

                result.RtnCode = 200;
                result.RtnMsg = checkResult.RtnMsg;

                return result;
            }
            else if (apiResult.Code != "200")
            {
                result.SetSuccess();
                result.RtnMsg = apiResult.Msg;
                return result;
            }
            else if (!arrInvStatus.Contains(apiResult.InvStatus))
            {
                result.RtnCode = 100;
                result.RtnMsg = JsonConvert.SerializeObject(apiResult);
                return result;
            }

            addInvDetail(cardNo, apiResult.InvPeriod, invNum, apiResult.Details, batchNo);
            result.SetSuccess();
            return result;
        }

        /// <summary>
        /// 下載附掛載具
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="cardNo"></param>
        /// <param name="cardEncrypt"></param>
        /// <returns></returns>
        public Task<BaseResult> DownloadCarrierTypeAsync(long mid, string cardNo, string cardEncrypt)
        {
            return Task.Factory.StartNew(() =>
            {
                var result = new BaseResult();
                result.SetError();
                try
                {
                    var apiResult = _einvoiceService.GetCarrierUnderType(new CarrierUnderTypeDTO
                    {
                        CardEncrypt = cardEncrypt,
                        CardNo = cardNo
                    });

                    if (apiResult.Code != "200")
                    {
                        return result;
                    }

                    var json = JsonConvert.SerializeObject(apiResult.Carriers ?? new Carrier[] { });
                    result = _einvoiceRepository.AddOrUpdateEinvoiceCarrierType(mid, cardNo, json);
                }
                catch (Exception ex)
                {
                    result.RtnMsg = $"CarrierEinvoiceService_DownloadCarrierTypeAsync:{ex}";
                    return result;
                }

                return result;
            });
        }

        #endregion

        #region Check

        /// <summary>
        /// 檢查我方載具資料是否與財政部一致
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public bool CheckCarrierInfo(long mid)
        {
            bool isPass = true;
            var memberCarrier = _einvoiceRepository.GetCellPhoneCarrierDetail(mid);
            if (memberCarrier == null)
            {
                return true;
            }

            // 一天只檢查一次，避免財政部掛掉
            var lastCarrierVerifyCodeLog = _einvoiceRepository.GetVerifyCodeLog(mid);
            if (lastCarrierVerifyCodeLog != null &&
                lastCarrierVerifyCodeLog.CreateDate > DateTime.Now.Date &&
                memberCarrier.IsVerify == 1)
            {
                return isPass;
            }

            
            CarrierBarcodeDTO result = new CarrierBarcodeDTO
            {
                PhoneNo = memberCarrier.Cellphone,
                VerificationCode = memberCarrier.VerificationCode,
                CardNo = memberCarrier.CarrierNum
            };
            var apiBarcode = _einvoiceService.GetCarrierBarcode(result);

            // 財政部異常
            if (apiBarcode.Code != "200" && apiBarcode.Code != "910")
            {
                return false;
            }

            int isVerify = (apiBarcode.CardNo != memberCarrier.CarrierNum) ? 0 : 1;
            if (isVerify != memberCarrier.IsVerify)
            {
                memberCarrier.IsVerify = isVerify;
                _einvoiceRepository.UpdateMemberCarrier(memberCarrier);
            }

            if (isVerify == 1)
            {
                DownloadCarrierTypeAsync(mid, memberCarrier.CarrierNum, memberCarrier.VerificationCode);

                // 新增檢查記錄
                _einvoiceRepository.AddLogCarrierVerifyCode(mid, memberCarrier.CarrierNum, memberCarrier.VerificationCode);
            }

            return (isVerify == 1);
        }

        #endregion

        #region EinvoiceRepository

        /// <summary>
        /// 更新已處理之載具
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public BaseResult UpdateTitlePush(string json)
        {
            return _einvoiceRepository.UpdateTitlePush(json);
        }

        /// <summary>
        /// 更新批號狀態
        /// </summary>
        /// <param name="BatchNo"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int UpdateBatchNo(string BatchNo, BatchStatus status)
        {
            return _einvoiceRepository.UpdateBatchNo(BatchNo, status);
        }

        /// <summary>
        /// 取得批號
        /// </summary>
        /// <returns></returns>
        public EinvoiceBatchNoModel GetBatchNo(BatchInvoiceType invoiceType)
        {
            return _einvoiceRepository.GetBatchNo(invoiceType);
        }

        #endregion

        #region 私有類

        /// <summary>
        /// 新增或更新發票表頭
        /// </summary>
        /// <param name="cardNo"></param>
        /// <param name="details"></param>
        /// <param name="batchNo"></param>
        private void addOrUpdateInvTitle(string cardNo, IEnumerable<TilteDetail> details, string batchNo = null)
        {
            if (string.IsNullOrWhiteSpace(cardNo))
            {
                throw new ArgumentNullException(cardNo);
            }
            else if (details == null)
            {
                return;
            }

            var arrInvStatus = new string[] {"2", "已確認"};
            foreach (var item in details.Where(x =>
                arrInvStatus.Contains(x.InvStatus) && Regex.IsMatch(x.InvPeriod, RegexConst.InvPeriod)))
            {
                _einvoiceRepository.InsertCarrierTitle(new EinvoiceCarrierTitleModel
                {
                    amount = item.Amount,
                    cardNo = cardNo,
                    cardType = item.CardType,
                    donateMark = item.DonateMark,
                    invDonatable = item.InvDonatable,
                    invNum = item.InvNum,
                    invoiceTime = item.InvoiceTime,
                    invPeriod = item.InvPeriod,
                    invStatus = item.InvStatus,
                    rowNum = item.RowNum,
                    sellerAddress = item.SellerAddress ?? string.Empty,
                    sellerBan = item.SellerBan,
                    sellerName = item.SellerName,
                    invDatedate = item.InvDate.Date,
                    invDateday = item.InvDate.Day,
                    invDatehours = item.InvDate.Hours,
                    invDateminutes = item.InvDate.Minutes,
                    invDatemonth = item.InvDate.Month,
                    invDateseconds = item.InvDate.Seconds,
                    invDatetime = item.InvDate.Time,
                    invDateyear = item.InvDate.Year,
                    invDatetimezoneOffset = item.InvDate.TimezoneOffset,
                    BatchNo = (batchNo ?? string.Empty),
                    RealCardNo = item.CardNo,
                    Currency = item.Currency
                });

                DateTime dtEinvCreate;
                string einvoiceCreateDate =
                    $"{(int.Parse(item.InvDate.Year) + 1911)}/{item.InvDate.Month}/{item.InvDate.Date} {item.InvoiceTime}";

                _einvoiceRepository.InsertEinvoiceByCarrier(new EinvoiceByCarrierModel
                {
                    EinvoiceItemDetail = "[]",
                    EinvoiceCreateDate = DateTime.TryParse(einvoiceCreateDate, out dtEinvCreate)
                        ? dtEinvCreate
                        : DateTime.Parse("1911/01/01"),
                    InvNum = item.InvNum,
                    InvPeriod = item.InvPeriod
                });
            }
        }

        /// <summary>
        /// 檢查發票是否存在，若不存在則修改狀態
        /// </summary>
        /// <param name="invNum"></param>
        /// <param name="invDate"></param>
        /// <param name="cardNo"></param>
        /// <param name="cardEncrypt"></param>
        /// <param name="batchNo"></param>
        /// <returns></returns>
        private BaseResult checkInvTitleIsExist(string invPeriod, string invNum, DateTime invDate, string cardNo,
            string cardEncrypt, string batchNo = null)
        {
            string rtnMsg = JsonConvert.SerializeObject(new {cardNo, invPeriod, invNum});
            var dlTitleResult = DownloadInvTitleOneDate(cardNo, cardEncrypt, invDate, batchNo);
            if (dlTitleResult.RtnCode != 1 || dlTitleResult.RtnData == null ||
                !dlTitleResult.RtnData.ContainsKey(
                    typeof(TilteDetail).FullName ?? throw new InvalidOperationException()))

            {
                rtnMsg += string.Format("ApiResult = {0}, 無法確認發票是否存在", dlTitleResult.RtnMsg);
                return new BaseResult
                {
                    RtnCode = 200,
                    RtnMsg = rtnMsg
                };
            }

            var details = (dlTitleResult.RtnData[typeof(TilteDetail).FullName] as TilteDetail[]);
            if (details == null || !details.Any(x => x.InvPeriod == invPeriod && x.InvNum == invNum))
            {
                _einvoiceRepository.UpdateEinvoiceByCarrierStatus(cardNo, invPeriod, invNum,
                    EinvoiceStatusType.Disable);
                rtnMsg += "查無此發票, 並已修改狀態";

                return new BaseResult()
                {
                    RtnCode = 300,
                    RtnMsg = rtnMsg
                };
            }

            rtnMsg += "發票狀態正常";
            return new BaseResult()
            {
                RtnCode = 1,
                RtnMsg = rtnMsg
            };
        }

        /// <summary>
        /// 新增發票明細
        /// </summary>
        /// <param name="cardNo"></param>
        /// <param name="invPeriod"></param>
        /// <param name="invNum"></param>
        /// <param name="details"></param>
        /// <param name="batchNo"></param>
        private void addInvDetail(string cardNo, string invPeriod, string invNum, IEnumerable<Detail> details,
            string batchNo = null)
        {
            if (string.IsNullOrWhiteSpace(cardNo))
            {
                throw new ArgumentNullException(cardNo);
            }
            else if (string.IsNullOrWhiteSpace(invPeriod))
            {
                throw new ArgumentNullException(invPeriod);
            }
            else if (string.IsNullOrWhiteSpace(invNum))
            {
                throw new ArgumentNullException(invNum);
            }
            else if (details == null)
            {
                return;
            }

            foreach (var detail in details)
            {
                _einvoiceRepository.InsertCarrierDetail(new EinvoiceCarrierDetailModel
                {
                    amount = detail.Amount,
                    BatchNo = (batchNo ?? string.Empty),
                    description = detail.Description,
                    invNum = invNum,
                    InvPeriod = invPeriod,
                    quantity = detail.Quantity,
                    rowNum = detail.RowNum,
                    unitPrice = detail.UnitPrice
                });
            }

            string detailJson = JsonConvert.SerializeObject(details.Select(detail => new InvDetailItemModel
            {
                description = detail.Description,
                quantity = detail.Quantity,
                unitPrice = detail.UnitPrice,
                amount = detail.Amount
            }));

            _einvoiceRepository.UpdateEinvoiceByCarrier(invPeriod, invNum, detailJson);
        }

        #endregion
    }
}