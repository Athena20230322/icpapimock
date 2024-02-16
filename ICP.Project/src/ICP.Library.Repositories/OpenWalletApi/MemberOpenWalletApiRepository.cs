using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace ICP.Library.Repositories.OpenWalletApi
{
    using Infrastructure.Core.Models;
    using Infrastructure.Core.Extensions;
    using Infrastructure.Core.Helpers;
    using Models.OpenWalletApi;

    public class MemberOpenWalletApiRepository
    {
        NetworkHelper _networkHelper;

        public MemberOpenWalletApiRepository()
        {
            _networkHelper = new NetworkHelper();
        }

        private DataResult<T> CallApi<T>(string url, object obj) where T : OpenWalletBaseResult, new()
        {
            string json;

            T model;

            try
            {
                json = "{}";
                model = JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception)
            {
                model = new T();
                model.errorCode = "ex";
            }

            return modelToResult(model);
        }

        private DataResult<T> modelToResult<T>(T model) where T: OpenWalletBaseResult
        {
            var result = new DataResult<T>();
            result.SetSuccess(model);

            if (!string.IsNullOrEmpty(model.errorCode))
            {
                result.SetCode(0);
            }

            return result;
        }

        public DataResult<AccessTokenResult> GetAccessToken(string AuthV)
        {
            string url = "";

            var request = new AccessTokenRequest();
            request.code = AuthV;

            //return CallApi<AccessTokenResult>(url, request);

            //假資料
            var model = new AccessTokenResult
            {
                access_token = Guid.NewGuid().ToString().Replace("-", string.Empty),
                expires = DateTime.Now.AddDays(1).ToString("yyyyMMddhhmmss")
            };
            var result = new DataResult<AccessTokenResult>();
            result.SetSuccess(model);
            return result;
        }

        public DataResult<AccessTokenResult> RefreshAccessToken(string AccessToken, string mid)
        {
            string url = "";

            var request = new OpenWalletRequest();
            request.access_token = AccessToken;
            request.mid = mid;

            //return CallApi<AccessTokenResult>(url, request);

            //假資料
            var model = new AccessTokenResult
            {
                access_token = request.access_token,
                expires = DateTime.Now.AddDays(1).ToString("yyyyMMddhhmmss")
            };
            var result = new DataResult<AccessTokenResult>();
            result.SetSuccess(model);
            return result;
        }

        public DataResult<QueryMemberMIDResult> QueryMemberMID(string AccessToken)
        {
            string url = "";

            var request = new QueryMemberMIDRequest();
            request.access_token = AccessToken;

            //return CallApi<QueryMemberMIDResult>(url, request);

            //假資料
            var model = new QueryMemberMIDResult
            {
                mid = Guid.NewGuid().ToString().Replace("-", string.Empty)
            };
            var result = new DataResult<QueryMemberMIDResult>();
            result.SetSuccess(model);
            return result;
        }

        public DataResult<QueryMemberInfoResult> QueryMemberInfo(string AccessToken, string mid)
        {
            string url = "";

            var request = new OpenWalletRequest();
            request.access_token = AccessToken;
            request.mid = mid;

            //return CallApi<QueryMemberInfoResult>(url, request);

            //假資料
            var model = new QueryMemberInfoResult
            {
                phone = "09" + DateTime.Now.ToString("ddHHmmss")
            };
            var result = new DataResult<QueryMemberInfoResult>();
            result.SetSuccess(model);
            return result;
        }

        public DataResult<QueryMobileBarCodeResult> QueryMobileBarCode(string AccessToken, string mid)
        {
            string url = "";

            var request = new OpenWalletRequest();
            request.access_token = AccessToken;
            request.mid = mid;

            //return CallApi<QueryMobileBarCodeResult>(url, request);

            //假資料
            var model = new QueryMobileBarCodeResult
            {
                mobile_barcode = string.Empty
            };
            var result = new DataResult<QueryMobileBarCodeResult>();
            result.SetSuccess(model);
            return result;
        }
    }
}