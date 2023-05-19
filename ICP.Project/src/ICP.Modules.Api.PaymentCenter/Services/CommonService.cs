using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Utils;
using ICP.Library.Services.ValidateService;
using System;
using System.Collections.Generic;
using System.Text;

namespace ICP.Modules.Api.PaymentCenter.Services
{
    public static class CommonService
    {
        /// <summary>
        /// 驗證欄位
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">傳入需驗證的物件</param>
        /// <returns></returns>
        public static BaseResult Validate<T>(T model)
        {
            var result = new BaseResult();

            StringBuilder errorMsg = new StringBuilder();
            List<string> errList = new List<string>();

            errList.AddRange(Validator.Validate(model));

            foreach (var item in errList)
            {
                errorMsg.Append(item.ToString() + " ");
        }

            if (string.IsNullOrWhiteSpace(errorMsg.ToString()))
            {
                result.SetSuccess();
            }
            else
            {
                result.SetCode(7019, errorMsg);
            }

            return result;
        }

        public static bool CheckMacValue
            (
                object requestModel, 
                string checkMacValue,
                Func<object, string, string, string> generateCheckMacValue
            )
        {
            var genCheckMacValue = generateCheckMacValue(requestModel, GlobalConfigUtil.SYS_HashKey, GlobalConfigUtil.SYS_HashIV);
            return genCheckMacValue.Equals(checkMacValue);
        }

        /// <summary>
        /// 交易結果設定
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DataResult<T> SetResult<T>
            (
                object rtnObj = null,
                object tradeResponseModel = null
            )
        {
            var result = new DataResult<T>();

            if (tradeResponseModel != null)
                result.SetSuccess((T)tradeResponseModel);

            if (rtnObj is int) result.SetCode((int)rtnObj);
            else if (rtnObj is BaseResult) result.SetError((BaseResult)rtnObj);

            return result;
        }
    }
}
