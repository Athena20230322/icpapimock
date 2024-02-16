using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Modules.Api.Payment.Interface;
using ICP.Modules.Api.Payment.Models.Cashier;
using ICP.Modules.Api.Payment.Models.Payment;
using ICP.Modules.Api.Payment.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ICP.Modules.Api.Payment.Services.PaymentType
{
    public abstract class BasePaymentType : IPaymentType
    {
        public ILogger _logger = null;
        public PaymentTradeRepository _paymentTradeRepository = null;

        public BasePaymentType(
            ILogger logger,
            PaymentTradeRepository paymentTradeRepository
        )
        {
            _logger = logger;
            _paymentTradeRepository = paymentTradeRepository;
        }

        public virtual AddTradeDbRes AddTrade(AddTradeDBReq addTradeDBReq)
        {
            _logger.Trace($"新增訂單開始，MerchantTradeNo={ addTradeDBReq.MerchantTradeNo }，TradeModeID={ ((eTradeMode)addTradeDBReq.TradeModeID).ToString() }，PaymentType={((ePaymentType)addTradeDBReq.PaymentTypeID).ToString()}");

            AddTradeDbRes result = new AddTradeDbRes();
            result.SetError();

            //### ItemList Json轉成XML再塞DB
            addTradeDBReq.ItemList.TryParseJsonToObj<List<ItemModel>>(out List<ItemModel> model);

            if (model != null && model.Count() > 0)
            {
                var xEle = new XElement("ItemList",
                    from items in model
                    select new XElement("Item",
                                    new XElement("ItemName", items.ItemName),
                                    new XElement("Quantity", items.Quantity),
                                    new XElement("Remark", items.Remark)
                               ));

                addTradeDBReq.ItemList = xEle.ToString();
            }

            result = _paymentTradeRepository.AddTrade(addTradeDBReq);

            result.SetCode(result.RtnCode);

            _logger.Trace($"新增訂單結束，MerchantTradeNo={ addTradeDBReq.MerchantTradeNo }，訊息代碼={ result.RtnCode }");

            return result;
        }

        public virtual UpdateTradeDBRes UpdateTrade(UpdateTradeDBReq updateTradeDBReq)
        {
            _logger.Error($"更新訂單開始，愛金卡交易編號(TransactionID)={ updateTradeDBReq.TradeNo }, TradeID={updateTradeDBReq.TradeID}, RtnCode={updateTradeDBReq.RtnCode}");

            UpdateTradeDBRes result = _paymentTradeRepository.UpdateTrade(updateTradeDBReq);

            result.SetCode(result.RtnCode);

            _logger.Error($"更新訂單結束，愛金卡交易編號(TransactionID)={ updateTradeDBReq.TradeNo }，訊息代碼={ result.RtnCode }");

            return result;
        }
    }
}
