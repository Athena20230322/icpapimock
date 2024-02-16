using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ICP.Modules.API.TinyURL.Commands;

namespace ICP.Modules.API.TinyURL.Controllers
{
    public class TinyURLController :Controller
    {
        private TinyURLCommand _tinyURLCommand = null;

        public TinyURLController(TinyURLCommand tinyURLCommand)
        {
            _tinyURLCommand = tinyURLCommand;
        }

        #region 產生短網址
        public ActionResult Add(string url)
        {
            var tinyURL = _tinyURLCommand.GenerateTinyURL(url);
            return Content(tinyURL);
        }
        #endregion


        public ActionResult GetUrl(string url)
        {
            var tinyURL = _tinyURLCommand.EnTinyURL(url);
            return Content(tinyURL);
        }
    }
}
