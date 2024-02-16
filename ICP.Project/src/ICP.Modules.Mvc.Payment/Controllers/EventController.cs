using ICP.Infrastructure.Core.Web.Controllers;
using ICP.Modules.Mvc.Payment.Commands.Event;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Payment.Controllers
{
    /// <summary>
    /// 獎勵/支付優惠/Fin學堂
    /// </summary>
    public class EventController : BaseMvcController
    {
        private readonly BannerCommand _bannerCommand = null;
        private readonly ActivityCommand _activityCommand = null;

        public EventController(BannerCommand bannerCommand,
            ActivityCommand activityCommand)
        {
            _bannerCommand = bannerCommand;
            _activityCommand = activityCommand;
        }

        #region 支付優惠/Fin學堂
        public ActionResult Banner(int? id)
        {
            ViewBag.SiteID = id;
            return View();
        }

        [HttpPost]
        public ActionResult Banner(int id)
        {
            var result = _bannerCommand.ListBanner(id);
            if (!result.IsSuccess)
            {
                return RedirectAndAlert(Url.Action("Banner", new { id }), result.RtnMsg);
            }

            return PartialView("BannerList", result.RtnData);
        }

        public ActionResult BannerContent(int id)
        {
            var result = _bannerCommand.GetBanner(id);
            if (!result.IsSuccess)
            {
                return RedirectAndAlert(Url.Action("Banner"), result.RtnMsg);
            }

            return View(result.RtnData);
        }
        #endregion

        #region 獎勵
        public ActionResult Activity()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Activity(int type)
        {
            return PartialView("ActivityList");
        }      
        #endregion
    }
}
