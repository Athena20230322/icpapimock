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
    public class HomeController :Controller
    {
        private readonly TinyURLCommand _tinyURLCommand = null;


        public HomeController(TinyURLCommand tinyURLCommand)
        {
            _tinyURLCommand = tinyURLCommand;
        }
    
        public ActionResult Index(string tinyUrl)
        {
            var tinyURL = _tinyURLCommand.GetWebSiteURL(tinyUrl);

            if (string.IsNullOrWhiteSpace(tinyURL)) { return Content(("<script>alert('參數錯誤');</script>")); }

            Response.StatusCode = 301;
            Response.AppendHeader("Location", tinyURL);
            return new EmptyResult();
        }
    
    }
}
