using System.Collections.Generic;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Models.MemberModels;
using ICP.Modules.Mvc.Admin.Models.ViewModels;
using ICP.Modules.Mvc.Admin.Repositories;

namespace ICP.Modules.Mvc.Admin.Services
{
    public class MemberDeviceIdService
    {
        private MemberDeviceIdManageRepository _deviceIdManageRepository;

        public MemberDeviceIdService(MemberDeviceIdManageRepository deviceIdManageRepository)
        {
            _deviceIdManageRepository = deviceIdManageRepository;
        }

        /// <summary>
        /// 查詢 裝置ID封鎖列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<MemberDeviceIdModel> MemberDeviceIdManageList(MemberDeviceIdQuery model)
        {
            return _deviceIdManageRepository.MemberDeviceIdManageList(model);
        }

        /// <summary>
        /// 增加 裝置ID封鎖
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="status"></param>
        /// <param name="user"></param>
        /// <param name="memo"></param>
        /// <returns></returns>
        public BaseResult AddMemberDeviceId(string deviceId, int status, string user, string memo)
        {
            return _deviceIdManageRepository.AddMemberDeviceId(deviceId, status, user, memo);
        }

        /// <summary>
        /// 查詢 裝置ID封鎖歷程
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public List<MemberDeviceIdVM> MemberDeviceIdVmList(string deviceId)
        {
            return _deviceIdManageRepository.MemberDeviceIdVmList(deviceId);
        }

        /// <summary>
        /// 新增 裝置ID修改歷程
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="user"></param>
        /// <param name="realIp"></param>
        /// <param name="proxyIp"></param>
        public void AddMemberDeviceIdLog(string deviceId, string user, long realIp, long proxyIp)
        {
            _deviceIdManageRepository.AddMemberDeviceIdLog(deviceId, user, realIp, proxyIp);
        }
    }
}