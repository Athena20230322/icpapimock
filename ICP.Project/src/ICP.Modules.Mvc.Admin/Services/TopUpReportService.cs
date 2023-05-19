using ICP.Modules.Mvc.Admin.Models;
using ICP.Modules.Mvc.Admin.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Services
{
    public class TopUpReportService
    {
        private readonly TopUpReportRepository _topUpReportRepository = null;

        public TopUpReportService(TopUpReportRepository topUpReportRepository)
        {
            _topUpReportRepository = topUpReportRepository;
        }

        /// <summary>
        /// 取得儲值明細資料清單
        /// </summary>
        /// <param name="topUpReportQueryCondition"></param>
        /// <returns></returns>
        public List<TopUpReportQueryResult> ListTopUpDetails(TopUpReportQueryCondition topUpReportQueryCondition)
        {
            return _topUpReportRepository.ListTopUpDetails(topUpReportQueryCondition);
        }
    }
}
