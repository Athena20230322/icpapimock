using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Helpers;
using ICP.Infrastructure.Core.Utils;
using ICP.Infrastructure.Core.Web.Attributes;
using ICP.Infrastructure.Core.Web.Controllers;
using ICP.Library.Services.Payment;
using ICP.Modules.Api.Payment.Models.Cashier;
using ICP.Modules.Api.Payment.Models.ChargeBack;
using ICP.Modules.Api.Payment.Models.Test;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace ICP.Modules.Api.Payment.Controllers
{
    public class TestController : BaseApiController
    {
        private readonly PaymentCommonService _paymentCommonService = null;

        public TestController(
            PaymentCommonService paymentCommonService    
        )
        {
            _paymentCommonService = paymentCommonService;
        }

        [LogRequestByActionNameAttribute(LogTextResponse = true)]
        public ActionResult Test()
        {
            return Content(base.GetType().FullName);
        }

        public ActionResult RefundTrade(string TradeNo, string MerchantTradeNo, long MerchantID, decimal RefundAmt)
        {
            ChargeBackReq cashierRequest = new ChargeBackReq()
            {
                MerchantTradeNo = MerchantTradeNo,
                MerchantID = MerchantID,               
                Amount = RefundAmt,
                TransactionID = TradeNo,
                MerchantTradeDate = DateTime.Now,                
                CheckMacValue = "",                
            };
            
            string postUrl = "http://localhost:3312/api/Payment/Cashier/ChargeBack";

            string checkMacValue = _paymentCommonService.GenerateCheckMacValue(
                                        cashierRequest,
                                        GlobalConfigUtil.SYS_HashKey,
                                        GlobalConfigUtil.SYS_HashIV
                                    );

            cashierRequest.CheckMacValue = checkMacValue;

            PropertyInfo[] properties = cashierRequest.GetType().GetProperties();

            Dictionary<string, string> aryName = new Dictionary<string, string>();

            foreach (PropertyInfo pi in properties)
            {
                if (pi.CanRead)
                {
                    aryName.Add(pi.Name, pi.GetValue(cashierRequest, null) != null ? pi.GetValue(cashierRequest, null).ToString() : "");
                }
            }

            NetworkHelper networkHelper = new NetworkHelper();

            string response = networkHelper.DoRequestWithUrlEncode(postUrl, aryName);

            return Content(response);

        }

        public ActionResult CreateTrade(int TradeModeID = 1, int PaymentTypeID = 1, int PaymentSubTypeID = 1, string AccountID = "")
        {
            string paymentDomain = GlobalConfigUtil.Host_Payment_Domain;

            CashierTestRequest cashierRequest = new CashierTestRequest()
            {
                merchantTradeNo = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                MerchantID = 0,
                MID = 10010063,
                //walletId = "123456",
                amount = 100,
                merchantTradeDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                //CarrierType = "123456",
                TradeModeID = TradeModeID,
                PaymentTypeID = PaymentTypeID,
                PaymentSubTypeID = PaymentSubTypeID,
                //ItemList = JsonConvert.SerializeObject(new List<ItemModel>()
                //{
                //    new ItemModel(){
                //        ItemName = "ItemName",
                //        Quantity = "1",
                //        Remark = "Test"
                //    },
                //    new ItemModel(){
                //        ItemName = "ItemName2",
                //        Quantity = "2",
                //        Remark = "Test2"
                //    }
                //}),
                CheckMacValue = "",
                TradeType = 1,
                AccountID = AccountID,
                Account = ""
            };


            cashierRequest.ItemList.TryParseJsonToObj<List<ItemModel>>(out List<ItemModel> model);

            //var xEle = new XElement("ItemList",
            //    from items in model
            //    select new XElement("Item",
            //                 new XElement("ItemName", items.ItemName),
            //                   new XElement("Quantity", items.Quantity),
            //                   new XElement("Remark", items.Remark)
            //               ));

            

            string postUrl = "http://localhost:3312/api/Payment/Common/Cashier";

            string checkMacValue = _paymentCommonService.GenerateCheckMacValue(
                                        cashierRequest,
                                        GlobalConfigUtil.SYS_HashKey,
                                        GlobalConfigUtil.SYS_HashIV
                                    );

            cashierRequest.CheckMacValue = checkMacValue;

            PropertyInfo[] properties = cashierRequest.GetType().GetProperties();

            Dictionary<string, string> aryName = new Dictionary<string, string>();

            foreach (PropertyInfo pi in properties)
            {
                if (pi.CanRead)
                {
                    aryName.Add(pi.Name, pi.GetValue(cashierRequest, null) != null ? pi.GetValue(cashierRequest, null).ToString() : "");
                }
            }

            NetworkHelper networkHelper = new NetworkHelper();

            string response = networkHelper.DoRequestWithUrlEncode(postUrl, aryName);

            return Content(response);

        }
    }
}
