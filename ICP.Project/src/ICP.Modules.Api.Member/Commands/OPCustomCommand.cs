using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICP.Modules.Api.Member.Services;

namespace ICP.Modules.Api.Member.Commands
{
    using ICP.Infrastructure.Core.Extensions;
    using ICP.Infrastructure.Core.Models;
    using ICP.Library.Models.OpenWalletApi.CustomReceiveApi;

    public class OPCustomCommand
    {
        private MemberInfoService _memberInfoService;
        public OPCustomCommand(MemberInfoService memberInfoService)
        {
            _memberInfoService = memberInfoService;
        }

        public DataResult<BaseCustomReceiveApiResult> NoticeMemberDelete(NoticeMemberDeleteRequest request)
        {
            var result = new DataResult<BaseCustomReceiveApiResult>();
            result.SetError();

            var rtnData = new BaseCustomReceiveApiResult();
            //todo: MemberDelete

            result.SetSuccess(rtnData);
            return result;
        }

        public DataResult<BaseCustomReceiveApiResult> NoticeMobileBarcode(NoticeMobileBarcodeRequest request)
        {
            var result = new DataResult<BaseCustomReceiveApiResult>();
            var rtnData = new BaseCustomReceiveApiResult();
            BaseResult baseResult =new BaseResult();

            rtnData.Code = "01";
            result.SetError();

            baseResult=_memberInfoService.openWallet_NoticeMobileBarcode(request);

            if (baseResult.RtnCode == 0)
            {
                rtnData.Code = "00";

            }
            result.SetSuccess(rtnData);

            return result;
        }
    }
}
