using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.OpenWalletApi.Enums;
using ICP.Library.Models.OpenWalletApi.WebUIApi;
using ICP.Library.Repositories.MemberRepositories;
using ICP.Library.Repositories.OpenWalletApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Authorization.Services
{
    public class OPWebUIAuthorizeService : BaseIdentifyService<OPWebUIAuthorizeService>
    {
        private readonly ILogger _logger = null;
        MemberConfigCyptRepository _configCyptRepository;
        OPWebUIApiRepository _oPWebUIApiRepository;

        public OPWebUIAuthorizeService(
            ILogger<OPWebUIAuthorizeService> logger,
            MemberConfigCyptRepository configCyptRepository,
            OPWebUIApiRepository oPWebUIApiRepository
            ) : base(logger)
        {
            _logger = logger;
            _configCyptRepository = configCyptRepository;
            _oPWebUIApiRepository = oPWebUIApiRepository;
        }

        public DataResult<WebUIApiMethodType> MethodNameToType(string methodName)
        {
            var result = new DataResult<WebUIApiMethodType>();
            result.SetError();

            WebUIApiMethodType rtnData;

            if (!Enum.TryParse(methodName, out rtnData) || rtnData == WebUIApiMethodType.None)
            {
                result.RtnMsg = $"{methodName} no declare MethodType";
            }

            result.SetSuccess(rtnData);
            return result;
        }

        /// <summary>
        /// 驗證雜湊碼
        /// </summary>
        /// <param name="methodType">API 方法</param>
        /// <param name="model">API 資料</param>
        /// <param name="mask">API 雜湊碼</param>
        /// <returns></returns>
        public BaseResult ValidMask(WebUIApiMethodType methodType, object model, string mask)
        {
            var result = new BaseResult();
            result.SetError();

            var genModelMaskResult = _oPWebUIApiRepository.GenerateMask(methodType, model);
            if (!genModelMaskResult.IsSuccess)
            {
                result.SetError(genModelMaskResult);
                return result;
            }

            string modelMask = genModelMaskResult.RtnData;

            if (mask != null) mask = mask.ToLower();

            if (modelMask != mask)
            {
                return result;
            }

            result.SetSuccess();
            return result;
        }
    }
}
