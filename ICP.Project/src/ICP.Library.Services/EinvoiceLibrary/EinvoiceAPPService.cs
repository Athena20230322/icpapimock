using System;
using System.Collections.Generic;
using Castle.Core.Internal;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.EinvoiceLibrary;
using ICP.Library.Models.EinvoiceLibrary.DTO;
using ICP.Library.Models.EinvoiceLibrary.Enum;
using ICP.Library.Repositories.EinvoiceLibrary;
using ICP.Library.Repositories.MemberRepositories;
using ICP.Library.Services.MemberServices;
using static System.Decimal;
using CarrierBarcodeDTO = ICP.Library.Services.Einvoice.CarrierBarcodeDTO;

namespace ICP.Library.Services.EinvoiceLibrary
{
    /// <summary>
    /// 電子發票APP站台服務
    /// </summary>
    public class EinvoiceAPPService
    {
        private EinvoiceRepository _einvoiceRepository;
        private EinvoiceService _einvoiceService;
        private CarrierEinvoiceService _carrierEinvoiceService;
        private MemberConfigCyptRepository _configCyptRepository;
        private MemberConfigRepository _memberConfigRepository;
        private ILogger<EinvoiceAPPService> _logger;
        private LibMemberInfoService _libMemberInfo; //測試用更改流程取會員資料

        public EinvoiceAPPService(
            EinvoiceRepository einvoiceRepository,
            EinvoiceService einvoiceService,
            MemberConfigCyptRepository configCyptRepository,
            CarrierEinvoiceService carrierEinvoiceService, 
            MemberConfigRepository memberConfigRepository, ILogger<EinvoiceAPPService> logger, 
            LibMemberInfoService libMemberInfo)
        {
            _einvoiceRepository = einvoiceRepository;
            _einvoiceService = einvoiceService;
            _configCyptRepository = configCyptRepository;
            _carrierEinvoiceService = carrierEinvoiceService;
            _memberConfigRepository = memberConfigRepository;
            _logger = logger;
            _libMemberInfo = libMemberInfo;
        }

        /// <summary>
        /// 新增下載發票表頭的預定排程
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public BaseResult AddInvTitleDownloadBatch(string carrierNum)
        {
            return _einvoiceRepository.AddTitlePush(carrierNum);
        }

        #region APP API

        /// <summary>
        /// 取得電子發票載具頁面所需資訊
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public GetEInvoiceCarrierInfoResult GetEInvoiceCarrierInfo(long mid)
        {
            return _einvoiceRepository.GetEInvoiceCarrierInfo(mid);
        }

        /// <summary>
        /// 取得發票清單
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="EinvoicePeriod"></param>
        /// <returns></returns>
        public QueryEinvoiceListResult QueryEinvoiceList(long mid, string EinvoicePeriod)
        {
            decimal amount = 0;
            int count = 0;

            var result = new QueryEinvoiceListResult
            {
                EinvoiceData = _einvoiceRepository.QueryInvList(mid, EinvoicePeriod)
            };
            

            result.EinvoiceData.ForEach(item =>
            {
                count += 1;
                amount += StringToDecimal(item.EinvoiceSaleAmount);
                item.EinvoiceCreateDate = Convert.ToDateTime(item.EinvoiceCreateDate).ToString("yyyy-MM-dd HH:mm:ss");
            });
            
            //小數點暫時轉換INT
            result.EinvoiceTotalAmount = ToInt32(amount);
            result.EinvoiceTotalCount = count;

            return result;
        }

        /// <summary>
        /// 取得發票明細與載具資料
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="einvoicePeriod"></param>
        /// <param name="einvoiceNum"></param>
        /// <returns></returns>
        public CarrierInvDetailModel GetCarrierInvDetail(long mid, string einvoicePeriod, string einvoiceNum)
        {
            return _einvoiceRepository.GetCarrierInvDetail(mid,einvoicePeriod,einvoiceNum);
        }

        /// <summary>
        /// 取得發票明細
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="einvoiceNum"></param>
        /// <param name="einvoicePeriod"></param>
        /// <returns></returns>
        public QueryInvDetailModel QueryInvDetail(long mid, string einvoicePeriod, string einvoiceNum)
        {
            return _einvoiceRepository.QueryInvDetail(mid, einvoicePeriod, einvoiceNum);
        }

        /// <summary>
        /// 將電子發票相關字串解密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string Decrypt_InvoiceData(string value)
        {
            return _configCyptRepository.Decrypt_InvoiceData(value);
        }

        /// <summary>
        /// 下載發票明細
        /// </summary>
        /// <param name="invPeriod"></param>
        /// <param name="invNum"></param>
        /// <param name="invDate"></param>
        /// <param name="cardNo"></param>
        /// <param name="cardEncrypt"></param>
        /// <param name="batchNo"></param>
        /// <returns></returns>
        public BaseResult DownloadInvDetail(string invPeriod, string invNum, DateTime invDate, string cardNo,
            string cardEncrypt, string batchNo = null)
        {
            return _carrierEinvoiceService.DownloadInvDetail(invPeriod, invNum, invDate, cardNo, cardEncrypt, batchNo);
        }

        /// <summary>
        /// 驗證手機條碼+驗證碼是否正確
        /// </summary>
        /// <param name="mid">會員編號</param>
        /// <param name="carrierNumber">手機條碼載具號碼</param>
        /// <param name="verificationCode">手機條碼載具驗證碼</param>
        /// <returns></returns>
        public BaseResult AuthCellPhoneCarrier(long mid, string carrierNumber, string verificationCode)
        {
            BaseResult result = new BaseResult();
            CellPhoneCarrierDetail cellPhoneCarrierDetail= new CellPhoneCarrierDetail();
            result.SetError();
            string cellphone = "";

            cellPhoneCarrierDetail = _einvoiceRepository.GetCellPhoneCarrierDetail(mid);

            if (cellPhoneCarrierDetail == null)
            {
                _logger.Info("手機明細內並無資訊");
                var memberData=_libMemberInfo.GetMemberData(mid);
                cellphone = memberData.detail.CellPhone;
                
            }
            else
            {
                if (cellPhoneCarrierDetail.Cellphone.IsNullOrEmpty())
                {
                    var memberData=_libMemberInfo.GetMemberData(mid);
                    cellphone = memberData.detail.CellPhone;
                }
                else
                {
                    cellphone = cellPhoneCarrierDetail.Cellphone;
                }
            }
            
            var verificationCodeDbCode = cellPhoneCarrierDetail?.VerificationCode;
            

            //如果verificationCode與cellPhoneCarrier內的verificationCode都為空，就直接傳回錯誤
            if (!(string.IsNullOrWhiteSpace(verificationCodeDbCode) &&
                  string.IsNullOrWhiteSpace(verificationCode)))
            {
                CarrierBarcodeDTO barcodeDto = new CarrierBarcodeDTO
                {
                    PhoneNo = cellphone,
                    VerificationCode = string.IsNullOrWhiteSpace(verificationCode)
                        ? verificationCodeDbCode
                        : verificationCode,//傳入驗證碼為NULL則用DB驗證碼驗證
                    CardNo =carrierNumber
                };

                //打財政部Eivoice站台
                try
                {
                    var barcodeResult = _einvoiceService.GetCarrierBarcode(barcodeDto);
                    _einvoiceRepository.AddLogCarrierVerifyCode(mid, carrierNumber, string.IsNullOrWhiteSpace(barcodeResult.VerificationCode)?verificationCode:barcodeResult.VerificationCode,
                        barcodeResult.Code, barcodeResult.Msg);

                    
                    switch (barcodeResult.Code)
                    {
                        case "910":
                            result.RtnMsg = "手機條碼或手機條碼驗證碼錯誤，請確認後重新輸入";
                            return result;
                        case "919":
                            result.RtnMsg = "手機條碼或手機條碼驗證碼錯誤，請確認後重新輸入";
                            return result;
                    }

                    
                    if (!(string.IsNullOrWhiteSpace(barcodeResult.CardNo)))
                    {
                        _einvoiceRepository.CellphoneCarrierAddCarrier(mid, barcodeResult.CardNo, barcodeResult.PhoneNo, barcodeResult.VerificationCode,(int)EinvoiceVerifyStatus.Enable);
                        AddInvTitleDownloadBatch(barcodeResult.CardNo);
                        result.SetSuccess();
                    }
                }
                catch(Exception ex)
                {
                    _logger.Error(ex,"財政部Eivoice站台錯誤");
                    return result;
                }
            }

            return result;
        }

        #endregion

        /// <summary>
        /// 反序列化發票明細商品
        /// </summary>
        /// <param name="EinvoiceItemDetail"></param>
        /// <returns></returns>
        public List<EinvoiceItemDetail> deserializeEinvoiceItem(string einvoiceItemDetail)
        {
            return _einvoiceRepository.deserializeEinvoiceItem(einvoiceItemDetail);
        }

        /// <summary>
        /// 轉換StringToDecimal
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private decimal StringToDecimal(string str)
        {
            decimal ChangeDecimal;

            if (TryParse(str, out ChangeDecimal) == true)
            {
                return ChangeDecimal;
            }
            else
            {
                return ChangeDecimal;
            }
        }
    }
}