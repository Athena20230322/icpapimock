using System;
using System.Collections.Generic;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Enums;
using ICP.Modules.Mvc.Admin.Models.MemberModels;
using ICP.Modules.Mvc.Admin.Models.ViewModels;
using ICP.Modules.Mvc.Admin.Services;

namespace ICP.Modules.Mvc.Admin.Commands
{
    public class MemberDeviceIdManageCommand
    {
        private MemberDeviceIdService _memberDeviceIdService;

        public MemberDeviceIdManageCommand(
            MemberDeviceIdService memberDeviceIdService
            )
        {
            _memberDeviceIdService = memberDeviceIdService;
        }

        /// <summary>
        /// 查詢 裝置ID封鎖列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<MemberDeviceIdModel> MemberDeviceIdManageList(MemberDeviceIdQuery model)
        {

            return _memberDeviceIdService.MemberDeviceIdManageList(model);
        }

        /// <summary>
        /// 增加 裝置ID狀態
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="status"></param>
        /// <param name="user"></param>
        /// <param name="memo"></param>
        /// <param name="realIp"></param>
        /// <param name="proxyIp"></param>
        /// <returns></returns>
        public BaseResult AddMemberDeviceId(string deviceId, int status, string user, string memo,long realIp,long proxyIp)
        {
            BaseResult result=new BaseResult();
            result.SetError();

            if ((Enum.IsDefined(typeof(MemberDeviceIdStatusType),status))==false)
            {
                return result;
            }

            result=_memberDeviceIdService.AddMemberDeviceId(deviceId, status, user, memo);
            if (result.IsSuccess)
            {
                _memberDeviceIdService.AddMemberDeviceIdLog(deviceId,user,realIp,proxyIp);
                result.SetSuccess();
            }
 
            return result;
        }

        /// <summary>
        /// 查詢 裝置ID封鎖歷程
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public List<MemberDeviceIdVM> MemberDeviceIdVmList(string deviceId)
        {
            return _memberDeviceIdService.MemberDeviceIdVmList(deviceId);
        }
    }
}