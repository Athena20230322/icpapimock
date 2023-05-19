using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
namespace ICP.Modules.Mvc.Admin.App_Start.Profiles
{
    using ICP.Modules.Mvc.Admin.Models.PaymentStatistics.TimingMonitor;
    using Infrastructure.Core.Models;
    using Models;
    using Models.ViewModels;

    public class SystemAdminProfile: Profile
    {
        public SystemAdminProfile()
        {
            CreateMap<FunctionCatagory, FrameMenuItem>();
            CreateMap<PermissionModel, PermissionViewModel>();
            CreateMap<Library.Models.MailLibrary.MailContent, Models.MailLibrary.MailContentModel>();
            CreateMap<Library.Models.MailLibrary.NotifyContent, Models.MailLibrary.NotifyContentModel>();
            CreateMap<Library.Models.MemberModels.MemberNotifyMessage, Models.CustomerManager.MemberNotifyMessageModel>();
            CreateMap<DataResult<Library.Models.MemberModels.MemberNotifyMessageDetail>, Models.CustomerManager.MemberNotifyMessageDetailModel>();
            CreateMap<QryTimingMonitorVM, TimingMonitorDbReq>();
        }
    }
}
