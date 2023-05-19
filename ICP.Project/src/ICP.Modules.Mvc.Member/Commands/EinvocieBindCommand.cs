using ICP.Library.Models.EinvoiceLibrary;
using ICP.Library.Services.EinvoiceLibrary;

namespace ICP.Modules.Mvc.Member.Commands
{
    public class EinvocieBindCommand
    {
        private EinvocieBindService _einvocieBindService;

        public EinvocieBindCommand(EinvocieBindService einvocieBindService)
        {
            _einvocieBindService = einvocieBindService;
        }

        /// <summary>
        /// 取得電子發票會員載具
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public GetEInvoiceCarrierInfoResultType GetEInvoiceCarrierInfo(long MID)
        {
            return _einvocieBindService.GetEInvoiceCarrierInfo(MID);
        }

        /// <summary>
        /// 驗證 發票是否能歸戶 並取得token
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="carryNum"></param>
        /// <returns></returns>
        public InvoiceBindReturn InvoiceBindProcess(long mid, string carryNum)
        {
            
            return _einvocieBindService.InvoiceBindProcess(mid, carryNum);
        }
    }
}