using ICP.Host.ApiTest.Commands;
using ICP.Infrastructure.Core.Models.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ICP.Host.ApiTest.Controllers
{
    public class MockOPWebUIController : Controller
    {
        MockOPWebUICommand _mockOPWebUICommand;

        public MockOPWebUIController(
            MockOPWebUICommand mockOPWebUICommand
            )
        {
            _mockOPWebUICommand = mockOPWebUICommand;
        }

        // GET: OPWebUI
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 列出 api 清單
        /// </summary>
        /// <returns></returns>
        public ContentResult ListApi()
        {
            string filePath = Server.MapPath("~/App_Data/MockOPWebUIApi.json");

            string json = System.IO.File.ReadAllText(filePath);

            return Content(json, MimeTypes.ApplicationJson);
        }

        /// <summary>
        /// 將 json 依 api 規則進行加密
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult CallApi(string env, string url, string json)
        {
            string filePath = Server.MapPath("~/App_Data/EnvOPWebUIHost.json");

            string hostJson = System.IO.File.ReadAllText(filePath);

            var jHost = Newtonsoft.Json.Linq.JObject.Parse(hostJson);

            string doamin = jHost[env].ToString();

            var result = _mockOPWebUICommand.CallApi(doamin, url, json);

            return Content(result, MimeTypes.ApplicationJson);
        }
    }
}