using ICP.Modules.Api.PaymentCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ICP.Modules.Api.PaymentCenter.Services
{
    public class TradeDataService
    {
        public TradeRequestModel Deserialize(string data)
        {
            TradeRequestModel tradeRequestModel = null;
            try
            {
                tradeRequestModel = JsonConvert.DeserializeObject<TradeRequestModel>(data);
            }
            catch (Exception ex) { };

            return tradeRequestModel;
        }
    }
}
