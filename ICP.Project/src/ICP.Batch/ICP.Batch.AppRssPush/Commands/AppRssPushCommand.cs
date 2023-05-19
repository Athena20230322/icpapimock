using System;

namespace ICP.Batch.AppRssPush.Commands
{
    using Infrastructure.Abstractions.Logging;
    using Services;
    public class AppRssPushCommand
    {
        private readonly ILogger _logger;
        private readonly AppRssPushService _appRssPushService;
        public AppRssPushCommand(
            AppRssPushService appRssPushService,
            ILogger<AppRssPushCommand> logger
        )
        {
            _logger = logger;
            _appRssPushService = appRssPushService;
        }

        public void Start()
        {

            int success = 0;
            int error = 0;
            _logger.Info("發送AppRss推播開始");
            try
            {
                var list = _appRssPushService.GetAppRssPushList();
                list.ForEach(t =>
                    {
                        string appRssPushStr = _appRssPushService.AppRssPushStr(t);

                        _logger.Info($"發送RssID:{t.RssID} 發送內容:{appRssPushStr}");

                        var appRssPushResponseStr = _appRssPushService.AppRssPush(appRssPushStr);

                        _logger.Info($"回傳RssID:{t.RssID} 回傳內容:{appRssPushResponseStr}");

                        var result = _appRssPushService.UpdateAppRssPush(appRssPushResponseStr);

                        if (result.IsSuccess)
                        {
                            success++;
                        }
                        else
                        {
                            error++;
                        }
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "發送AppRss推播失敗");
            }
            _logger.Info($"AppRss推送成功:{success}");
            _logger.Info($"AppRss推送失敗:{error}");
        }
    }
}