using AutoMapper;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Utils;
using ICP.Library.Models.AccountLinkApi;
using ICP.Library.Services.AccountLinkApi;
using ICP.Modules.Mvc.Member.Models.ACLink;
using ICP.Modules.Mvc.Member.Services;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Web;

namespace ICP.Modules.Mvc.Member.Commands
{
    public class ACLinkCommand
    {
        private ACLinkService _acLinkService = null;
        private ACLinkCommonService _commonService = null;
        private readonly ILogger _logger = null;

        public ACLinkCommand(
            ACLinkService acLinkService,
            ACLinkCommonService commonService,
            ILogger<ACLinkCommand> logger)
        {
            _acLinkService = acLinkService;
            _commonService = commonService;
            _logger = logger;
        }

        #region 綁定AccountLink結果頁
        /// <summary>
        /// 取得中國信託指定的回應訊息
        /// </summary>
        /// <param name="rtnCode"></param>
        /// <returns></returns>
        public string GetChinaTrustRtnMsg(string rtnCode)
        {
            string rtnMsg = _acLinkService.GetChinaTrustRtnMsg(rtnCode);

            return rtnMsg;
        }
        #endregion

        #region 中國信託
        /// <summary>
        /// 送至ACLinkApi申請綁定
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<ACLinkBindModel> ChinaTrustACLinkApply(ChinaTrustACLinkApplyReq model)
        {
            var result = new DataResult<ACLinkBindModel>();
            result.SetError();

            _logger.Info($"[ChinaTrustACLinkApply][Input] {JsonConvert.SerializeObject(model)}");

            // 組成送出資料
            ACLinkBindModel acLinkBindModel = Mapper.Map<ACLinkBindModel>(model);
            var getPostData = _acLinkService.ACLinkBindPostData(acLinkBindModel);
            if (!getPostData.IsSuccess)
            {
                result.SetCode(7424);
                return result;
            }
            result = getPostData;

            _logger.Info($"[組成送出資料] postData:{JsonConvert.SerializeObject(result)}");

            return result;
        }

        /// <summary>
        /// 送至ACLinkApi綁定
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<ACLinkBindModel> ChinaTrustACLinkBind(ChinaTrustACLinkApplyReq model)
        {
            var result = new DataResult<ACLinkBindModel>();
            result.SetError();

            _logger.Info($"[ChinaTrustACLinkBind][Input] {JsonConvert.SerializeObject(model)}");

            // 組成送出資料
            ACLinkBindModel acLinkBindModel = Mapper.Map<ACLinkBindModel>(model);
            var getPostData = _acLinkService.ACLinkBindPostData(acLinkBindModel);
            if (!getPostData.IsSuccess)
            {
                result.SetCode(7424);
                return result;
            }
            result = getPostData;

            _logger.Info($"[組成送出資料] postData:{JsonConvert.SerializeObject(result)}");

            return result;
        }

        #endregion

    }
}
