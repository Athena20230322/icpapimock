using ICP.Infrastructure.Abstractions.FilterProxy;
using ICP.Infrastructure.Core.Helpers;
using ICP.Infrastructure.Core.Utils;
using ICP.Infrastructure.Core.Web.Attributes;
using ICP.Infrastructure.Core.Web.Controllers;
using ICP.Library.Services.Payment;
using ICP.Modules.Api.Payment.Models.Cashier;
using ICP.Modules.Mvc.Payment.Models.Cashier;
using ICP.Modules.Mvc.Payment.Models.ChargeBack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using CashierReq = ICP.Modules.Mvc.Payment.Models.Cashier.CashierReq;

namespace ICP.Modules.Mvc.Payment.Controllers
{
    public class MockController : BaseMvcController
    {
        private readonly PaymentCommonService _paymentCommonService = null;

        public MockController(PaymentCommonService paymentCommonService)
        {
            _paymentCommonService = paymentCommonService;
        }

        public ActionResult CreateOrder()
        {
            CashierReq cashierReq = new CashierReq()
            {
                MerchantID = 10010063,
                MID = 10010067,
                MerchantTradeNo = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                MerchantTradeDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                Amount = 100,
                PaymentSubTypeID = 1,
                ItemList = JsonConvert.SerializeObject(new List<ItemModel>()
                {
                    new ItemModel(){
                        ItemName = "Remark1",
                        Quantity = "1",
                        Remark = "Test"
                    },
                    new ItemModel(){
                        ItemName = "Remark2",
                        Quantity = "2",
                        Remark = "Test2"
                    }
                }),
                ItemAmt = 100,
                BonusAmt = 10,
                DebitPoint = 10,
            };

            return View(cashierReq);
        }

        [HttpPost]
        public string CreateOrder(CashierReq cashierReq)
        {
            cashierReq.CheckMacValue = _paymentCommonService.GenerateCheckMacValue(cashierReq, GlobalConfigUtil.SYS_HashKey, GlobalConfigUtil.SYS_HashIV);

            var dict = cashierReq.GetType()
                                   .GetProperties()
                                   .Where(x => x.CanRead)
                                   .ToDictionary(x => x.Name, x => x.GetValue(cashierReq, null)?.ToString());

            var sortDict = dict.OrderBy(x => x.Key);
            var list = sortDict.Select(x => $"{x.Key}={x.Value}");
            string queryString = string.Join("&", list);            

            string postUrl = GlobalConfigUtil.Host_Payment_Domain + "/api/Payment/Common/Cashier";

            NetworkHelper networkHelper = new NetworkHelper();

            string response = networkHelper.DoRequest(postUrl, queryString, "application/x-www-form-urlencoded", 0, null, null);
            //string response = networkHelper.DoRequestWithJson(postUrl, JsonConvert.SerializeObject(cashierReq));

            return response;
        }

        public ActionResult ChargeBack()
        {
            ChargeBackReq chargeBackReq = new ChargeBackReq()
            {
                MerchantID = 10010063,
                MID = 10010067,
                MerchantTradeDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                Amount = 100,
                BonusAmt = 10,
                DebitPoint = 10,
            };

            return View(chargeBackReq);
        }

        [HttpPost]
        public string ChargeBack(ChargeBackReq chargeBackReq)
        {
            chargeBackReq.CheckMacValue = _paymentCommonService.GenerateCheckMacValue(chargeBackReq, GlobalConfigUtil.SYS_HashKey, GlobalConfigUtil.SYS_HashIV);

            var dict = chargeBackReq.GetType()
                                   .GetProperties()
                                   .Where(x => x.CanRead)
                                   .ToDictionary(x => x.Name, x => x.GetValue(chargeBackReq, null)?.ToString());

            var sortDict = dict.OrderBy(x => x.Key);
            var list = sortDict.Select(x => $"{x.Key}={x.Value}");
            string queryString = string.Join("&", list);

            string postUrl = GlobalConfigUtil.Host_Payment_Domain + "/api/Payment/Common/ChargeBack";

            NetworkHelper networkHelper = new NetworkHelper();

            string response = networkHelper.DoRequest(postUrl, queryString, "application/x-www-form-urlencoded", 0, null, null);
            //string response = networkHelper.DoRequestWithJson(postUrl, JsonConvert.SerializeObject(cashierReq));

            return response;
        }


        [AllowAnonymous]
        [ActionFilterProxy(ProxyType = ProxyType.AuthorizationApi)]
        public string GetOnlineOrderInfo(CashierReq request)
        {
            return Request.QueryString.ToString();
        }
    }
}
