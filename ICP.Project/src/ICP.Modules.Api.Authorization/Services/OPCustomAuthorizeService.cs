using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ICP.Modules.Api.Authorization.Services
{
    using Infrastructure.Abstractions.Logging;
    using Infrastructure.Core.Extensions;
    using Infrastructure.Core.Models;
    using Library.Models.OpenWalletApi.Enums;
    using Library.Models.Enums;
    using Library.Repositories.OpenWalletApi;

    public class OPCustomAuthorizeService : BaseIdentifyService<OPCustomAuthorizeService>
    {
        private readonly ILogger _logger = null;
        private readonly OPCustomApiRepository _oPCustomApiRepository = null;

        public OPCustomAuthorizeService(
            ILogger<OPCustomAuthorizeService> logger,
            OPCustomApiRepository oPCustomApiRepository
            ) : base(logger)
        {
            _logger = logger;
            _oPCustomApiRepository = oPCustomApiRepository;
        }

        public DataResult<CustomApiMethodType> MethodNameToType(string methodName)
        {
            var result = new DataResult<CustomApiMethodType>();
            result.SetError();

            CustomApiMethodType rtnData;

            if (!Enum.TryParse(methodName, out rtnData) || rtnData == CustomApiMethodType.None)
            {
                result.RtnMsg = $"{methodName} no declare MethodType";
            }

            result.SetSuccess(rtnData);
            return result;
        }

        public BaseResult AddCustomAPILog(
            TransType transType,
            CustomApiMethodType methodType,
            string EncData,
            long? MID = null,
            string StatusCode = null,
            string StatusMessage = null,
            long RealIP = 0,
            long ProxyIP = 0
            )
        {
            return _oPCustomApiRepository.AddCustomAPILog(transType, methodType, EncData, MID, StatusCode, StatusMessage, RealIP, ProxyIP);
        }

        public BaseResult ValidMask(CustomApiMethodType methodType, object model, string mask)
        {
            var result = new BaseResult();
            result.SetError();

            var genModelMaskResult = _oPCustomApiRepository.GenerateMask(methodType, model);
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
