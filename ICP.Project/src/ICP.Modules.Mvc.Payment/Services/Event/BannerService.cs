using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Utils;
using ICP.Modules.Mvc.Payment.Models.Event.Banner;
using ICP.Modules.Mvc.Payment.Repositories.Event;
using System.Collections.Generic;

namespace ICP.Modules.Mvc.Payment.Services.Event
{
    public class BannerService
    {
        private readonly BannerRepository _bannerRepository = null;

        public BannerService(BannerRepository bannerRepository)
        {
            _bannerRepository = bannerRepository;
        }

        /// <summary>
        /// 廣告清單
        /// </summary>
        /// <param name="siteID"></param>
        /// <returns></returns>
        public DataResult<List<ListBannerRes>> ListBanner(int siteID)
        {
            var result = new DataResult<List<ListBannerRes>>();

            try
            {
                List<ListBannerRes> list = _bannerRepository.ListBanner(siteID);
                list.ForEach(x =>
                {
                    x.ImagePath = GlobalConfigUtil.Host_Member_Domain + x.ImagePath;
                });

                result.SetSuccess(list);
            }
            catch
            {
                result.SetError();
            }

            return result;
        }

        /// <summary>
        /// 取得廣告
        /// </summary>
        /// <param name="bannerID"></param>
        /// <returns></returns>
        public DataResult<GetBannerRes> GetBanner(int bannerID)
        {
            var result = new DataResult<GetBannerRes>();

            try
            {
                GetBannerRes res = _bannerRepository.GetBanner(bannerID);
                res.ImagePath = string.IsNullOrEmpty(res.ImagePath) ? "" : GlobalConfigUtil.Host_Member_Domain + res.ImagePath;
                result.SetSuccess(res);
            }
            catch
            {
                result.SetError();
            }

            return result;
        }
    }
}
