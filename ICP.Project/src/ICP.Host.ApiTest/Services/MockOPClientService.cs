using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace ICP.Host.ApiTest.Services
{
    using ICP.Infrastructure.Core.Extensions;
    using ICP.Infrastructure.Core.Models;
    using ICP.Library.Repositories.MemberRepositories;
    using Library.Repositories.OpenWalletApi;

    public class MockOPClientService
    {
        OPClientApiRepository _oPClientApiRepository;
        MemberConfigCyptRepository _configCyptRepository;

        public MockOPClientService(
            OPClientApiRepository oPClientApiRepository,
            MemberConfigCyptRepository configCyptRepository
            )
        {
            _oPClientApiRepository = oPClientApiRepository;
            _configCyptRepository = configCyptRepository;
        }

        private DataResult<string> GetMemberAuthVCode(string AuthV, ref bool mock)
        {
            var result = new DataResult<string>();
            result.SetError();

            string AuthVCode = null;

            if (AuthV.Length > 128)
            {
                try
                {
                    string json = _configCyptRepository.Decrypt_CustomOpenWalletEncData(AuthV);

                    var jObj = JObject.Parse(json);

                    AuthVCode = jObj.Value<string>("code");
                }
                catch { }
            }
            else if (_configCyptRepository.AllowMock)
            {
                mock = true;
                AuthVCode = AuthV;
            }

            if (string.IsNullOrWhiteSpace(AuthVCode))
            {
                return result;
            }

            result.SetSuccess(AuthVCode);
            return result;
        }

        public BaseResult GetAccessToken(string json)
        {
            var jObj = JObject.Parse(json);

            string authV = jObj.Value<string>("authV");

            bool mock = false;
            var getMemberAuthVCodeResult = GetMemberAuthVCode(authV, ref mock);
            if (!getMemberAuthVCodeResult.IsSuccess)
            {
                return getMemberAuthVCodeResult;
            }

            string code = getMemberAuthVCodeResult.RtnData;

            var apiResult = _oPClientApiRepository.GetAccessToken(code);

            return apiResult;
        }

        public BaseResult QueryMemberMID(string json)
        {
            var jObj = JObject.Parse(json);

            string access_token = jObj.Value<string>("access_token");

            var apiResult = _oPClientApiRepository.QueryMemberMID(access_token);

            return apiResult;
        }

        public BaseResult QueryMemberInfo(string json)
        {
            var jObj = JObject.Parse(json);

            string access_token = jObj.Value<string>("access_token");
            string mid = jObj.Value<string>("mid");

            var apiResult = _oPClientApiRepository.QueryMemberInfo(access_token, mid);

            return apiResult;
        }

        public BaseResult RefreshAccessToken(string json)
        {
            var jObj = JObject.Parse(json);

            string access_token = jObj.Value<string>("access_token");
            string mid = jObj.Value<string>("mid");

            var apiResult = _oPClientApiRepository.RefreshAccessToken(access_token, mid);

            return apiResult;
        }

        public BaseResult QueryMobileBarCode(string json)
        {
            var jObj = JObject.Parse(json);

            string access_token = jObj.Value<string>("access_token");
            string mid = jObj.Value<string>("mid");

            var apiResult = _oPClientApiRepository.QueryMobileBarCode(access_token, mid);

            return apiResult;
        }
    }
}