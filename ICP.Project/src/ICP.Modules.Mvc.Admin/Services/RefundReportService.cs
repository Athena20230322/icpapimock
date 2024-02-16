using ICP.Modules.Mvc.Admin.Models;
using ICP.Modules.Mvc.Admin.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Services
{
    public class RefundReportService
    {
        private readonly RefundReportRepository _refundReportRepository = null;

        public RefundReportService(RefundReportRepository refundReportRepository)
        {
            _refundReportRepository = refundReportRepository;
        }

        /// <summary>
        /// 取得退款明細資料
        /// </summary>
        /// <param name="refundReportQueryCondition"></param>
        /// <returns></returns>
        public List<RefundReportQueryResult> ListRefundDetail(RefundReportQueryCondition refundReportQueryCondition)
        {
            return _refundReportRepository.ListRefundDetail(refundReportQueryCondition);
        }
    }
}
