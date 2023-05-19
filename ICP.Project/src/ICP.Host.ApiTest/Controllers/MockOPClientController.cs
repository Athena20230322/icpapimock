using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ICP.Host.ApiTest.Controllers
{
    using Infrastructure.Core.Models.Consts;
    using Commands;

    public class MockOPClientController : Controller
    {
        MockOPClientCommand _mockOPClientCommand;

        public MockOPClientController(
            MockOPClientCommand mockOPClientCommand
            )
        {
            _mockOPClientCommand = mockOPClientCommand;
        }

        // GET: OPClient
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
            string filePath = Server.MapPath("~/App_Data/MockOPClientApi.json");

            string json = System.IO.File.ReadAllText(filePath);

            return Content(json, MimeTypes.ApplicationJson);
        }

        /// <summary>
        /// 將 json 依 api 規則進行加密
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult CallApi(string url, string json)
        {
            string result = _mockOPClientCommand.CallApi(url, json);

            return Content(result, MimeTypes.ApplicationJson);
        }
    }
}