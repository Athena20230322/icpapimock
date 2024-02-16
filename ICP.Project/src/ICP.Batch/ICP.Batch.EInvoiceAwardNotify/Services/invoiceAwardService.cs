using System.Collections.Generic;
using ICP.Batch.EInvoiceAwardNotify.Repositories;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Services.MemberServices;

namespace ICP.Batch.EInvoiceAwardNotify.Services
{
    public class invoiceAwardService
    {
        private readonly InvoiceAwardRepository _invoiceAwardRepository;
        private LibtMemberNotifyMessageService _libtMemberNotify;

        public invoiceAwardService(InvoiceAwardRepository invoiceAwardRepository, LibtMemberNotifyMessageService libtMemberNotify)
        {
            _invoiceAwardRepository = invoiceAwardRepository;
            _libtMemberNotify = libtMemberNotify;
        }

        public BaseResult InvoiceIssueAwardCheck(string createDate, string endDate, string yearMonth)
        {
            return _invoiceAwardRepository.InvoiceIssueAwardCheck(createDate, endDate, yearMonth);
        }

        /// <summary>
        /// 發送訊息中心
        /// </summary>
        /// <param name="carrierIdClear"></param>
        public void SendAwardNotify(string carrierIdClear)
        {
            long mid = 0;
            int push = 1;//0推播 1不推播
            mid = long.TryParse(carrierIdClear,out mid)?mid:0;

            string title=ConfigService.InvoiceAwardTitle;
            string content=ConfigService.InvoiceAwardContent;

            _libtMemberNotify.AddNotifyMessage(mid,title,content,0,push);
        }
    }
}