using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Extensions
{
    using Infrastructure.Core.Frameworks.ResultMapper;
    using Infrastructure.Core.Models;

    public static class BaseResultExtension
    {
        public static BaseResult SetError(this BaseResult result)
        {
            result.SetCode(0);
            return result;
        }

        public static BaseResult SetFatalError(this BaseResult result)
        {
            result.SetCode(9999);
            return result;
        }

        public static BaseResult SetSuccess(this BaseResult result)
        {
            result.SetCode(1);
            return result;
        }

        public static BaseResult ToBaseResult(this BaseResult result)
        {
            return new BaseResult
            {
                RtnCode = result.RtnCode,
                RtnMsg = result.RtnMsg
            };
        }       

        public static BaseResult SetCode(this BaseResult result, int rtnCode, params object[] args)
        {
            var resultHelper = new ResultMapper();

            string msg = resultHelper.GetResultMsg(rtnCode);
            if (args != null && args.Any())
            {
                msg = string.Format(msg, args);
            }

            result.RtnCode = rtnCode;
            result.RtnMsg = msg;

            return result;
        }

        public static BaseResult SetFormatError(this BaseResult result, string msg)
        {
            result.RtnCode = 1000;
            result.RtnMsg = msg;
            return result;
        }

        public static BaseResult SetError(this BaseResult result, BaseResult errorResult)
        {
            result.RtnCode = errorResult.RtnCode;
            result.RtnMsg = errorResult.RtnMsg;
            return result;
        }

        public static BaseResult SetResult(this BaseResult result1, BaseResult result2)
        {
            result1.RtnCode = result2.RtnCode;
            result1.RtnMsg = result2.RtnMsg;
            return result1;
        }
    }
}
