using ICP.Infrastructure.Core.Frameworks.ResultMapper;
using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Extensions
{
    public static class DataResultExtension
    {
        public static DataResult<T> SetSuccess<T>(this DataResult<T> result, T data)
        {
            result.SetSuccess();
            result.RtnData = data;
            return result;
        }

        public static DataResult<T> SetError<T>(this DataResult<T> result, BaseResult errorResult)
        {
            result.RtnCode = errorResult.RtnCode;
            result.RtnMsg = errorResult.RtnMsg ?? new ResultMapper().GetResultMsg(result.RtnCode);
            return result;
        }

        public static DataResult<T> SetError<T>(this DataResult<T> result, DataResult<T> errorResult)
        {
            result.RtnCode = errorResult.RtnCode;
            result.RtnMsg = errorResult.RtnMsg;
            result.RtnData = errorResult.RtnData;
            return result;
        }
    }
}
