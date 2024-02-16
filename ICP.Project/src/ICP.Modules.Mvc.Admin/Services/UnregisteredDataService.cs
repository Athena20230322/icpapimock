using ICP.Modules.Mvc.Admin.Models;
using ICP.Modules.Mvc.Admin.Models.ViewModels;
using ICP.Modules.Mvc.Admin.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Services
{
    public class UnregisteredDataService
    {
        UnregisteredDataRepository _unregisteredDataRepository;

        public UnregisteredDataService(UnregisteredDataRepository unregisteredDataRepository)
        {
            _unregisteredDataRepository = unregisteredDataRepository;
        }

        /// <summary>
        /// 取得被刪除的會員資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<UnregisteredData> ListUnregisteredData(QueryUnregisteredDataVM model)
        {
            return _unregisteredDataRepository.ListUnregisteredData(model);
        }

        /// <summary>
        /// 取得單筆被刪除的會員資料
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public UnregisteredData GetUnregisteredData(long MID)
        {
            return _unregisteredDataRepository.GetUnregisteredData(MID);
        }
    }
}
