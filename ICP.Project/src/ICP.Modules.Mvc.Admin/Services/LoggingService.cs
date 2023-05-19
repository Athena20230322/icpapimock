using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Services
{
    using Repositories;
    using Models.Logging;

    public class LoggingService
    {
        #region 建構、倉儲
        LoggingRepository _loggingRepository;

        public LoggingService(
            LoggingRepository loggingRepository
            )
        {
            _loggingRepository = loggingRepository;
        }
        #endregion

        /// <summary>
        /// 訊息公告清單
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CustomerMutipleLogModel ListCustomerMutipleLog(long CustomerID) => _loggingRepository.ListCustomerMutipleLog(CustomerID);
    }
}
