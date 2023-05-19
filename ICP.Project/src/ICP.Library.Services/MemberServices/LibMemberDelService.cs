using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.MemberModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Services.MemberServices
{
    public class LibMemberDelService
    {
        ILogger<LibMemberDelService> _logger;

        public LibMemberDelService(
            ILogger<LibMemberDelService> logger
            )
        {
            _logger = logger;
        }

        /// <summary>
        /// 批次刪除會員
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public BaseResult BatchDeleteMember(List<MemberDeleteModel> list)
        {
            var result = new BaseResult();
            result.SetError();

            var errorResults = new List<BaseResult>();

            list.ForEach(model =>
            {
                var delResult = DeleteMember(model);
                if (!delResult.IsSuccess)
                {
                    errorResults.Add(delResult);
                }
            });

            if (errorResults.Count > 0)
            {
                string errMsg = "BatchDeleteMember:";

                errorResults.ForEach(t => 
                {
                    errMsg += Environment.NewLine + $"RtnCode: {t.RtnCode}, RtnMsg: {t.RtnMsg}";
                });

                result.RtnMsg = errMsg;
                return result;
            }

            result.SetSuccess();
            return result;
        }

        /// <summary>
        /// 刪除會員
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult DeleteMember(MemberDeleteModel model)
        {
            var result = new BaseResult();
            result.SetError();

            try
            {
                //todo: member delete
                //不重覆刪除, 但回結果為成功
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return result;
            }

            result.SetSuccess();
            return result;
        }
    }
}
