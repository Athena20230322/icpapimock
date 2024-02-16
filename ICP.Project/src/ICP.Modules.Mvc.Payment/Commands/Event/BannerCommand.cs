using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Payment.Models.Event.Banner;
using ICP.Modules.Mvc.Payment.Services.Event;
using System.Collections.Generic;

namespace ICP.Modules.Mvc.Payment.Commands.Event
{
    public class BannerCommand
    {
        private readonly BannerService _bannerService = null;

        public BannerCommand(BannerService bannerService)
        {
            _bannerService = bannerService;
        }

        /// <summary>
        /// 廣告清單
        /// </summary>
        /// <param name="siteID"></param>
        /// <returns></returns>
        public DataResult<List<ListBannerRes>> ListBanner(int siteID)
        {
            return _bannerService.ListBanner(siteID);
        }

        /// <summary>
        /// 取得廣告
        /// </summary>
        /// <param name="bannerID"></param>
        /// <returns></returns>
        public DataResult<GetBannerRes> GetBanner(int bannerID)
        {
            return _bannerService.GetBanner(bannerID);
        }

    }
}
