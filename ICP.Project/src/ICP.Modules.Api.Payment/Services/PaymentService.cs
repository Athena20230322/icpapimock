using AutoMapper;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.MemberModels;
using ICP.Library.Models.Payment;
using ICP.Library.Repositories.Payment;
using ICP.Modules.Api.Payment.Models.AccountLimitInfo;
using ICP.Modules.Api.Payment.Models.Payment;
using ICP.Modules.Api.Payment.Repositories;
using System;
using MemberInfoRepository = ICP.Library.Repositories.MemberRepositories.MemberInfoRepository;

namespace ICP.Modules.Api.Payment.Services
{
    public class PaymentService
    {
        private readonly ILogger _logger = null;
        private readonly PaymentTradeRepository _paymentTradeRepository = null;
        private readonly Repositories.MemberInfoRepository _memberInfoRepository = null;
        private readonly MemberInfoRepository _libMemberInfoRepository = null;
        private readonly PaymentRepository _libPaymentRepository = null;
        
        public PaymentService(
            ILogger<PaymentService> logger,
            PaymentTradeRepository paymentTradeRepository,   
            Repositories.MemberInfoRepository memberInfoRepository,
            MemberInfoRepository libMemberInfoRepository,
            PaymentRepository libPaymentRepository
        )
        {
            _logger = logger;
            _paymentTradeRepository = paymentTradeRepository;           
            _memberInfoRepository = memberInfoRepository;
            _libMemberInfoRepository = libMemberInfoRepository;
            _libPaymentRepository = libPaymentRepository;
        }

        /// <summary>
        /// 驗證收付款額度
        /// </summary>
        /// <param name="mid">會員代碼</param>
        /// <param name="idType">角色(1:買家 2:賣家)</param>
        /// <param name="tradeModeID">交易模式(1:交易、2:儲值、3:轉帳、4:提領)</param>
        /// <param name="amount">交易金額</param>       
        /// <param name="tradetype">交易類型(1:EC、2:Mobile)</param>
        /// <param name="paymentTypeTypeID">交易付款方式</param>
        /// <param name="paymentSubTypeID">交易子付款方式</param>
        /// <returns></returns>
        public BaseResult ValidateTradeAmtLimit(long mid, int idType, int tradeModeID, decimal amount, int tradetype = 0, int paymentTypeID = 0, int paymentSubTypeID = 0)
        {
            BaseResult result = new BaseResult();
            result.SetError();

            DateTime now = DateTime.Now;
            //### 本日起始時間
            string onedayStartDate = now.Date.ToString("yyyy-MM-dd HH:mm:ss.fff");
            //### 本日結束時間(隔天00:00:00.000)
            string onedayEndDate = now.AddDays(1).Date.ToString("yyyy-MM-dd HH:mm:ss.fff");

            //### 本月起始時間
            string monthStartDate = now.AddDays(1 - now.Day).Date.ToString("yyyy-MM-dd HH:mm:ss.fff");
            //### 本月結束時間
            string monthEndDate = now.AddDays(1 - now.Day).AddMonths(1).Date.ToString("yyyy-MM-dd HH:mm:ss.fff");

            //### 撈出MID會員資訊
            MemberDataModel memberInfo = _libMemberInfoRepository.GetMemberData(mid);

            //### 用會員的等級(LawID)撈出額度上限
            int lawID = memberInfo != null && memberInfo.basic != null ? memberInfo.basic.LevelID : 0;
            MemberLawBasicModel memberLawBasic = _libMemberInfoRepository.GetMemberLawBasic(lawID);

            decimal monthlyPayAmount = 0;
            decimal monthlyTransAmount = 0;
            decimal monthlyTopupAmount = 0;
            decimal dayTransAmount = 0;

            _logger.Trace($"交易額度驗證開始。");

            try
            {
                //### 交易
                if (eTradeMode.Transaction == (eTradeMode)tradeModeID)
                {
                    //### 買家付款
                    if (idType == 1)
                    {
                        //### 月付款總金額
                        monthlyPayAmount = (GetTradeStatistics(monthStartDate, monthEndDate, mid, idType, (int)eTradeMode.Transaction));

                        //### 月轉出總金額
                        monthlyTransAmount = (GetTradeStatistics(monthStartDate, monthEndDate, mid, idType, (int)eTradeMode.Transfer));

                        if (monthlyPayAmount + monthlyTransAmount + amount > (memberLawBasic != null ? memberLawBasic.PaymentLimitAMT : 0))
                        {
                            result.SetCode(2048);
                            return result;
                        }
                    }
                }
                //### 儲值
                else if (eTradeMode.Topup == (eTradeMode)tradeModeID)
                {
                    //### 消費者儲值
                    if (idType == 1)
                    {
                        //### 月儲值總金額
                        monthlyTopupAmount = (GetTradeStatistics(monthStartDate, monthEndDate, mid, idType, (int)eTradeMode.Topup));

                        if (monthlyTopupAmount + amount > (memberLawBasic != null ? memberLawBasic.TopUpLimitAMT : 0))
                        {
                            result.SetCode(2049);
                            return result;
                        }
                    }
                }
                //### 轉帳
                else if (eTradeMode.Transaction == (eTradeMode)tradeModeID)
                {
                    //### 月轉入/轉出總金額
                    monthlyTransAmount = (GetTradeStatistics(monthStartDate, monthEndDate, mid, idType, (int)eTradeMode.Transfer));

                    //### 付款者轉出
                    if (idType == 1)
                    {
                        //### 日轉出總金額
                        dayTransAmount = (GetTradeStatistics(onedayStartDate, onedayEndDate, mid, idType, (int)eTradeMode.Transfer));

                        if (dayTransAmount + amount > (memberLawBasic != null ? memberLawBasic.DailyTransferOutLimitAMT : 0))
                        {
                            result.SetCode(2050);
                            return result;
                        }

                        if (monthlyTransAmount + amount > (memberLawBasic != null ? memberLawBasic.MonthlyTransferOutLimitAMT : 0))
                        {
                            result.SetCode(2051);
                            return result;
                        }
                    }
                    //### 收款者轉入
                    else if (idType == 2)
                    {
                        if (dayTransAmount + amount > (memberLawBasic != null ? memberLawBasic.TransferInLimitAMT : 0))
                        {
                            result.SetCode(2052);
                            return result;
                        }
                    }
                }

                result.SetSuccess();
            }
            catch (Exception ex)
            {
                _logger.Error($"交易額度驗證發生未預期錯誤, 錯誤訊息為={ ex.ToString() }");

                throw ex;
            }
            finally
            {
                _logger.Trace($"交易額度驗證結束, 結果代碼={ result.RtnCode }");
            }

            return result;
        }

        /// <summary>
        /// 撈取訂單資訊
        /// </summary>
        /// <param name="queryOnlinechargeRequest"></param>
        /// <returns></returns>
        public DataResult<GetTradeInfoDbRes> GetTradeInfo(GetTradeInfoDbReq getTradeInfoDbReq)
        {
            DataResult<GetTradeInfoDbRes> result = new DataResult<GetTradeInfoDbRes>();
            result.SetCode(2025);

            GetTradeInfoDbRes paymentInfo = _libPaymentRepository.GetTradeInfo(getTradeInfoDbReq);

            if(paymentInfo != null)
            {
                result.SetSuccess(paymentInfo);
            }

            return result;
        }

        /// <summary>
        /// 取得日期區間交易模式對應總金額
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="mid"></param>
        /// <param name="idType"></param>
        /// <param name="tradeModeID"></param>
        /// <param name="tradeType"></param>
        /// <param name="paymentTypeID"></param>
        /// <param name="paymentSubTypeID"></param>
        /// <returns></returns>
        public Decimal GetTradeStatistics(string startDate, string endDate, long mid, int idType, int tradeModeID, int tradeType = 0, int paymentTypeID = 0, int paymentSubTypeID = 0)
        {
            return _paymentTradeRepository.GetTradeStatistics(startDate, endDate, mid, idType, tradeModeID, tradeType, paymentTypeID, paymentSubTypeID);
        }

        /// <summary>
        /// 新增條碼
        /// </summary>
        /// <param name="addBarcodeRequest"></param>
        /// <returns></returns>
        public AddBarcodeDBRes AddBarcode(AddBarcodeDBReq addBarcodeRequest)
        {
            return _libPaymentRepository.AddBarcode(addBarcodeRequest);
        }

        /// <summary>
        /// 取得交易限額資料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<AccountLimitInfoRes> GetAccountLimitInfo(long mid)
        {
            var result = new DataResult<AccountLimitInfoRes>();

            var dbReq = new AccountLimitInfoDbReq
            {
                MID = mid
            };

            var dbResult = _memberInfoRepository.GetAccountLimitInfo(dbReq);

            if (dbResult == null)
            {
                result.SetCode(0);
                return result;
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AccountLimitInfoDbRes, AccountLimitInfoRes>()
                    .ForMember(res => res.Timestamp, dbRes => dbRes.Ignore());
            });

            result.SetSuccess();
            result.RtnData = config.CreateMapper().Map<AccountLimitInfoRes>(dbResult);


            return result;
        }
    }
}
