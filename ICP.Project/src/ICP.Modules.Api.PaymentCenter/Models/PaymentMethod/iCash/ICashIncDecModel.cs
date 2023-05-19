using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Models.PaymentMethod.iCash
{
    public class ICashIncDecModel
    {
        public string TradeNo { get; set; }

        public long MID { get; set; }

        public long MerchantID { get; set; }

        public int TradeModeID { get; set; }

        public int PaymentTypeID { get; set; }

        public int PaymentSubTypeID { get; set; }

        public string Notes { get; set; }

        public decimal Amount { get; set; }
    }
}
