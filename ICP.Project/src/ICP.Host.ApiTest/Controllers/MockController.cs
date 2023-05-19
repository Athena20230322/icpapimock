using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace ICP.Host.ApiTest.Controllers
{
    using Infrastructure.Core.Models.Consts;
    using Services;

    using ICP.Library.Repositories.ManageBank;
    using ICP.Library.Models.ManageBank.FirstBank;
    using System.Xml;

    public class MockController : Controller
    {
        FirstBankRepository _firstBankRepository;

        public MockController(
            FirstBankRepository firstBankRepository
            )
        {
            _firstBankRepository = firstBankRepository;
        }

        // GET: Mock
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetCertStatus(string env)
        {
            var _service = new MockService(env);

            return Json(new
            {
                _service.MID,
                _service.UserCode
            });
        }

        /// <summary>
        /// 將 json 依 api 規則進行加密
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CallNormalApi(string env, string host, string url, string json, string fileCols, HttpPostedFileBase[] files = null)
        {
            JToken jToken = null;

            var _service = new MockService(env);
            int delag = 0;
            var fileList = this.GetFiles(files, fileCols);

            string result = _service.CallNormalApi(env, host, url, json, ref delag, fileList);

            //登入
            if (url.EndsWith("/UserCodeLogin"))
            {
                jToken = JToken.Parse(result);
                if (jToken["RtnCode"].Value<int>() == 1)
                {
                    var RtnData = jToken["EncData"];
                    string UserCode = RtnData["UserCode"].Value<string>();
                    long MID = RtnData["MID"].Value<long>();
                    _service.UpdateMockSetting(env, UserCode, MID);
                }
            }
            //登出
            else if (url.EndsWith("/UserCodeLogout"))
            {
                jToken = JToken.Parse(result);
                if (jToken["RtnCode"].Value<int>() == 1)
                {
                    //移除設定
                    _service.RemoveMockSetting(env);
                }
            }
            //註冊
            if (url.EndsWith("/CheckRegisterAuthSMS"))
            {
                jToken = JToken.Parse(result);
                if (jToken["RtnCode"].Value<int>() == 1)
                {
                    var RtnData = jToken["EncData"];
                    string UserCode = RtnData["UserCode"].Value<string>();
                    long MID = RtnData["MID"].Value<long>();
                    _service.UpdateMockSetting(env, UserCode, MID);
                }
            }

            return Json(new { delag, result });
        }

        /// <summary>
        /// 將 json 依 api 規則進行加密，包含上傳檔案
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CallNormalApiWithFile(string env, string host, string url, string json, HttpPostedFileBase[] files)
        {
            JToken jToken = null;

            var _service = new MockService(env);
            int delag = 0;
            string result = _service.CallNormalApi(env, host, url, json, ref delag);

            return Json(new { delag, result });
        }

        /// <summary>
        /// 列出 api 清單
        /// </summary>
        /// <returns></returns>
        public ContentResult ListApi()
        {
            string filePath = Server.MapPath("~/App_Data/MockApi.json");

            string json = System.IO.File.ReadAllText(filePath);

            return Content(json, MimeTypes.ApplicationJson);
        }

        private List<Models.File> GetFiles(HttpPostedFileBase[] files, string fileCols)
        {
            if (files == null) return null;

            List<Models.File> list = new List<Models.File>();

            foreach (HttpPostedFileBase file in files)
            {
                if (file == null) return list;

                list.Add(new Models.File
                {
                    FileName = file.FileName,
                    InputStream = file.InputStream,
                    ContentType = file.ContentType
                });
            }

            if (string.IsNullOrWhiteSpace(fileCols)) return list;

            string[] cols = fileCols.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim()).Where(t => !string.IsNullOrEmpty(t)).ToArray();

            if (cols.Length == 0) return list;

            for (int i = 0; i < cols.Length && i < list.Count; i++)
            {
                list[i].UploadName = cols[i];
            }

            return list;
        }

        public ActionResult AppRedirect()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AppRedirect(string env, string url)
        {
            var _service = new MockService(env);

            string redirectUrl = _service.AppRedirect(env, url);

            return Redirect(redirectUrl);
        }
    }
}