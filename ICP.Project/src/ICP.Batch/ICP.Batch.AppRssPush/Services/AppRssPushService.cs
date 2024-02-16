using System.Collections.Generic;
using ICP.Batch.AppRssPush.Models;
using ICP.Batch.AppRssPush.Repositories;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Helpers;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Repositories.MemberRepositories;
using Newtonsoft.Json;

namespace ICP.Batch.AppRssPush.Services
{
    public class AppRssPushService
    {
        private readonly AppRssPushRepositories _appRssPushRepositories;
        private readonly ConfigService _configService;
        private MemberConfigCyptRepository _memberConfigCyptRepository;

        public AppRssPushService(
            AppRssPushRepositories appRssPushRepositories,
            ConfigService configService,
            MemberConfigCyptRepository memberConfigCyptRepository)
        {
            _appRssPushRepositories = appRssPushRepositories;
            _configService = configService;
            _memberConfigCyptRepository = memberConfigCyptRepository;
        }

        #region 公開

        /// <summary>
        /// 取得並更新待推播清單
        /// </summary>
        /// <returns></returns>
        public List<AppRssPushContent> GetAppRssPushList()
        {
            return _appRssPushRepositories.GetAppRssPushList();
        }

        /// <summary>
        /// 推播字串
        /// </summary>
        /// <param name="contents"></param>
        /// <returns></returns>
        public string AppRssPushStr(AppRssPushContent contents)
        {
            return GenerateAppRssPushStr(contents);
        }

        /// <summary>
        /// AppRss推播
        /// </summary>
        /// <param name="appRssPushStr"></param>
        /// <returns></returns>
        public string AppRssPush(string appRssPushStr)
        {
            var request = DoRequest(appRssPushStr);

            return request;
        }

        /// <summary>
        /// 存入回傳結果
        /// </summary>
        /// <param name="request"></param>
        public BaseResult UpdateAppRssPush(string request)
        {
            BaseResult result = new BaseResult();
            result.SetError();

            if (request != null && request != "Error")
            {
                return UpdateAppRssPushList(ResponseStrToContent(request));
            }

            return result;
        }

        #endregion

        #region 不公開

        /// <summary>
        /// 組成推播字串
        /// </summary>
        /// <param name="contents"></param>
        /// <returns></returns>
        private string GenerateAppRssPushStr(AppRssPushContent contents)
        {
            var requestModel = _appRssPushRepositories.GetAppRssRequest(contents, ConfigService.SourceName);
            
            requestModel.mask =
                _memberConfigCyptRepository.MD5_AppRssPushMask(contents.Traceid + ConfigService.SourceName+contents.OPMID).ToLower();

            var jsonStr = JsonConvert.SerializeObject(requestModel);//,
                /*Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });*/

            return _memberConfigCyptRepository.Encrypt_AppRssPushData(jsonStr);
        }

        /// <summary>
        /// 送出推播
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private string DoRequest(string args)
        {
            NetworkHelper network = new NetworkHelper
            {
                DefaultTimeout = ConfigService.WebRequestTimeout
            };
            return network.DoRequest(_configService.Url, args, "application/x-www-form-urlencoded", 15, null, null);
        }

        /// <summary>
        /// 轉換ResponseStr
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static AppRssResponseContent ResponseStrToContent(string content)
        {
            AppRssResponseContent response = JsonConvert.DeserializeObject<AppRssResponseContent>(content);
            if (response != null)
            {
                //S：處理完成，F：處理失敗
                if (response.status == "S")
                {
                    response.SaveStatus = (int) AppRssPushEnum.EnableAndPush;
                }
                else if (response.status == "F")
                {
                    response.SaveStatus = (int) AppRssPushEnum.Fail;
                }
                else
                {
                    response.SaveStatus = (int) AppRssPushEnum.Fail;
                }
            }
            else
            {
                response.SaveStatus =(int) AppRssPushEnum.Fail;
                response.message = content;
            }


            return response;
        }

        /// <summary>
        /// 更新推播狀態
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private BaseResult UpdateAppRssPushList(AppRssResponseContent content)
        {
            return _appRssPushRepositories.UpdateAppRssPushList(content.traceid, content.SaveStatus, content.message);
        }

        #endregion
    }
}