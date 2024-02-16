using ICP.Host.ApiTest.Commands;
using ICP.Infrastructure.Core.Models.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ICP.Host.ApiTest.Controllers
{
    public class MockOPCustomReceiveController : Controller
    {
        MockOPCustomReceiveCommand _mockOPCustomReceiveCommand;

        public MockOPCustomReceiveController(
            MockOPCustomReceiveCommand mockOPCustomReceiveCommand
            )
        {
            _mockOPCustomReceiveCommand = mockOPCustomReceiveCommand;
        }

        
        // GET: OPCustom
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
            string filePath = Server.MapPath("~/App_Data/MockOPCustomReceiveApi.json");

            string json = System.IO.File.ReadAllText(filePath);

            return Content(json, MimeTypes.ApplicationJson);
        }

        /// <summary>
        /// 將 json 依 api 規則進行加密
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CallApi(string env, string url, string json)
        {
            string filePath = Server.MapPath("~/App_Data/EnvOPCustomReceiveHost.json");

            string hostJson = System.IO.File.ReadAllText(filePath);

            var jHost = Newtonsoft.Json.Linq.JObject.Parse(hostJson);

            string doamin = jHost[env].ToString();

            var result = _mockOPCustomReceiveCommand.CallApi(doamin, url, json);

            return Json(result);
        }
    }
}