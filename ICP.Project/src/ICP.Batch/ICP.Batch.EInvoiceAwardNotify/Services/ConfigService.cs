using System.Configuration;

namespace ICP.Batch.EInvoiceAwardNotify.Services
{
    public class ConfigService
    {
        /// <summary>
        ///版型 載具發票中獎通知 標題 
        /// </summary>
        public static string InvoiceAwardTitle
        {
            get
            {
                var retVal = ConfigurationManager.AppSettings["InvoiceAwardTitle"];
                return retVal ?? "";
            }
        }

        /// <summary>
        ///版型 載具發票中獎通知 內容 
        /// </summary>
        public static string InvoiceAwardContent
        {
            get
            {
                var retVal = ConfigurationManager.AppSettings["InvoiceAwardContent"];
                return retVal ?? "";
            }
        }
    }
}