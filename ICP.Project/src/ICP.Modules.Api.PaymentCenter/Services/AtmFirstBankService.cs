using AutoMapper;
using ICP.Infrastructure.Core.Helpers;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.PaymentCenter.Models;
using ICP.Modules.Api.PaymentCenter.Repositories;
using ICP.Modules.Api.PaymentCenter.Repositories.ATM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Services
{
    public class AtmFirstBankService
    {
        private readonly AtmFirstBankRepository _atmFirstBankRepository = null;
        private readonly FirstATMRepository _firstATMRepository = null;
        private readonly QueryRepository _queryRepository = null;

        public AtmFirstBankService(AtmFirstBankRepository atmFirstBankRepository, FirstATMRepository firstATMRepository, QueryRepository queryRepository)
        {
            _atmFirstBankRepository = atmFirstBankRepository;
            _firstATMRepository = firstATMRepository;
            _queryRepository = queryRepository;
        }

        /// <summary>
        /// 取得銷帳資料內容
        /// </summary>
        /// <param name="atmFirstBankWriteOffDataReq"></param>
        /// <returns></returns>
        public List<string> GetWriteOffContents(AtmFirstBankWriteOffDataReq atmFirstBankWriteOffDataReq)
        {
            return atmFirstBankWriteOffDataReq.Content.Split(new char[] { '\r', '\n' }).ToList();
        }

        /// <summary>
        /// 轉型為銷帳物件
        /// </summary>
        /// <param name="lineContent"></param>
        /// <returns></returns>
        public AtmFirstBankWriteOffData TransferToWriteOffData(string lineContent)
        {
            return new AtmFirstBankWriteOffData()
            {
                CompanyAccount = lineContent.Substring(0, 11),
                TransDate = lineContent.Substring(11, 8),
                TransID = int.Parse(lineContent.Substring(19, 7)),
                TransNo = lineContent.Substring(26, 4),
                LenderAmt = TranserToDecimail(lineContent.Substring(30, 13), 13),
                DebitAmt = TranserToDecimail(lineContent.Substring(43, 13), 13),
                PNType = lineContent.Substring(56, 1),
                Balance = TranserToDecimail(lineContent.Substring(57, 15), 15),
                TransType = int.Parse(lineContent.Substring(72, 1)),
                VirtualAccount = lineContent.Substring(73, 16),
                RouteType = lineContent.Substring(89, 2)
            };

            decimal TranserToDecimail(string sourceText, int totalLen, int dotLen = 2)
            {
                decimal.TryParse(sourceText.Insert(totalLen - dotLen, "."), out decimal rtnDecimal);
                return rtnDecimal;
            }
        }

        /// <summary>
        /// 更新 PaymentCenter 訂單資料
        /// </summary>
        /// <param name="atmFirstBankWriteOffData"></param>
        /// <returns></returns>
        public TradeInfoAtm UpdatePaymentCenterAtmTrade(AtmFirstBankWriteOffData atmFirstBankWriteOffData)
        {
            return _atmFirstBankRepository.UpdateCenterAtmTrade(atmFirstBankWriteOffData);
        }

        /// <summary>
        /// 以銷帳資料查詢交易資料
        /// </summary>
        /// <param name="virtualAccount"></param>
        /// <returns></returns>
        public TradeInfo GetTradeInfoByWriteOffData(AtmFirstBankWriteOffData atmFirstBankWriteOffData)
        {
            TradeInfo tradeInfo = _queryRepository.GetTradeInfoByVirtualAccount(atmFirstBankWriteOffData.VirtualAccount);
            if (tradeInfo != null)
            {
                tradeInfo.MpName = "FIRST";
                tradeInfo.MpType = "ATM";
                tradeInfo.MpReturnTradeAMT = atmFirstBankWriteOffData.DebitAmt;
                tradeInfo.MpReturnVirtualAccount = atmFirstBankWriteOffData.VirtualAccount;
                tradeInfo.MpReturnPaymentNo = atmFirstBankWriteOffData.TransID.ToString();
                tradeInfo.ErrorCode = GetErrorCode();
            }
            return tradeInfo;

            int GetErrorCode()
            {
                if (tradeInfo.TradeID == 0)
                {
                    return 10100080;    // 無符合的訂單(虛擬帳號不符)
                }
                if (tradeInfo.MpReturnTradeAMT > 0 && tradeInfo.TradeAMT > 0 && tradeInfo.MpReturnTradeAMT != tradeInfo.TradeAMT)
                {
                    return 10100068;    // 銷帳金額不符
                }
                return 10100058;    // 其它原因
            }
        }

        /// <summary>
        /// 新增銷帳錯誤 log
        /// </summary>
        /// <param name="tradeInfo"></param>
        /// <returns></returns>
        public BaseResult AddWriteOffErrorLog(TradeInfo tradeInfo)
        {
            return _atmFirstBankRepository.AddWriteOffErrorLog(tradeInfo);
        }

        /// <summary>
        /// 更新 Payment 訂單資料
        /// </summary>
        /// <param name="tradeInfoAtm"></param>
        /// <returns></returns>
        public BaseResult UpdatePaymentAtmTrade(TradeInfoAtm tradeInfoAtm)
        {
            tradeInfoAtm.ReplyURL = "http://localhost:3312/api/Payment/ReceiveAtmServerReturn/Result";
            if (string.IsNullOrEmpty(tradeInfoAtm.ReplyURL))
            {
                return new BaseResult() { RtnCode = 9999 };
            }

            var postData = new
            {
                TradeID = tradeInfoAtm.TradeNo,
                JsonData = JsonConvert.SerializeObject(tradeInfoAtm)    // 需要加密編碼?
            };

            string response = new NetworkHelper().DoRequestWithJson(tradeInfoAtm.ReplyURL, JsonConvert.SerializeObject(postData), 300, null, null);

            return new BaseResult() { RtnCode = response.Equals("1|OK") ? 1 : 9999 };   // "1|OK" : "0|0-TradeError"
        }

        /// <summary>
        /// 通知銀行(AP to AP)
        /// </summary>
        /// <param name="tradeInfoAtm"></param>
        /// <returns></returns>
        public BaseResult NotifyBank(TradeInfoAtm tradeInfoAtm)
        {
            var atmNotifyModel = Mapper.Map<AtmNotifyModel>(tradeInfoAtm);
            atmNotifyModel.Amount = tradeInfoAtm.TradeAMT;
            atmNotifyModel.ApplyType = "N";

            return _firstATMRepository.NotifyBank(atmNotifyModel);
        }
    }
}
