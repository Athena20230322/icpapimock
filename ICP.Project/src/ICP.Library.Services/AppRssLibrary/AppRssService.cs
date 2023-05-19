using System;
using Castle.Core.Internal;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Library.Models.AppRssLibrary;
using ICP.Library.Services.MemberServices;

namespace ICP.Library.Services.AppRssLibrary
{
    using Infrastructure.Core.Models;
    using Library.Repositories.AppRssLibrary;
    using Repositories.SystemRepositories;

    public class AppRssService
    {
        private readonly AppRssRepository _appRssRepository;
        private readonly ConfigKeyValueRepository _configKeyValueRepository;
        private LibMemberInfoService _libMemberInfoService;
        private ILogger<AppRssService> _logger;

        public AppRssService(
            AppRssRepository appRssRepository,
            ConfigKeyValueRepository configKeyValueRepository,
            LibMemberInfoService libMemberInfoService, 
            ILogger<AppRssService> logger)
        {
            _appRssRepository = appRssRepository;
            _configKeyValueRepository = configKeyValueRepository;
            _libMemberInfoService = libMemberInfoService;
            _logger = logger;
        }

        /// <summary>
        /// 新增推播訊息
        /// </summary>
        /// <param name="mid">會員編號</param>
        /// <param name="title">推播訊息對話方塊之標題</param>
        /// <param name="subject">訊息主旨，最長80個全形字</param>
        /// <param name="notifyMessageId">通知訊息編號 Share_NotifyMessage_Detail.NotifyMessageID</param>
        /// <param name="hyperLink">點擊推播開啟指定URL頁面</param>
        /// <param name="functionid">點擊推播開APP指定頁</param>
        /// <param name="param">點擊推播開APP指定頁分支頁面</param>
        /// <param name="expireTime">推播訊息失效時間</param>
        /// <param name="priority">發送的優先順序，0為立即發送，其餘按數字由小至大順序發送</param>
        /// <returns></returns>
        public BaseResult AddAppRss(long mid, string subject, int notifyMessageId, string title = null,
            string hyperLink = null, string functionid = null, string param = null, DateTime? expireTime = null,
            int priority = 0)
        {
            BaseResult result = new BaseResult();
            if (hyperLink != null && functionid != null)
            {
                result.SetError();
                result.RtnMsg = $"{nameof(hyperLink)}與{nameof(functionid)}只能擇一發送";
                return result;
            }

            var infoMemberData = _libMemberInfoService.GetMemberData(mid);

            if (infoMemberData != null)
            {
                if (infoMemberData.basic.OPMID.IsNullOrEmpty())
                {
                    result.RtnMsg = "推播會員資料缺少OPEN WALLET MID";
                    _logger.Error(result.RtnMsg);
                    return result;
                }
            }
            else
            {
                result.RtnMsg = "無相關推播會員資料";
                _logger.Error(result.RtnMsg);
                return result; 
            }

            AppRssContent appContext = new AppRssContent
            {
                MID = mid,
                OPMID = infoMemberData.basic.OPMID,
                Title = title,
                Subject = subject,
                NotifyMessageID = notifyMessageId,

                ExpireTime = expireTime,
                Hyper_link = hyperLink,
                Functionid = functionid,
                Param = param,
                Identifier = _configKeyValueRepository.Environment_Code?.Substring(0, 1)
            };

            return _appRssRepository.AddAppRssContent(appContext);
        }
    }
}