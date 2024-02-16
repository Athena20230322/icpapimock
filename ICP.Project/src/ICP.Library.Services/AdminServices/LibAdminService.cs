using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Services.AdminServices
{
    using Models.AdminModels;
    using Repositories.AdminRepositories;

    public class LibAdminService
    {
        AdminRepository _adminRespository;

        public LibAdminService(AdminRepository adminRepository)
        {
            _adminRespository = adminRepository;
        }

        /// <summary>
        /// 取得App功能開關
        /// </summary>
        /// <param name="AppName"></param>
        /// <param name="FunctionID"></param>
        /// <param name="FunctionName"></param>
        /// <returns></returns>
        public List<AppFunctionSwitch> ListAppFunctionSwitch(string AppName = null, byte? FunctionID = null, string FunctionName = null)
        {
            return _adminRespository.ListAppFunctionSwitch(AppName, FunctionID, FunctionName);
        }
    }
}
