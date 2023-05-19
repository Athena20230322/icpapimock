using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Helpers;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.MemberApi.Certificate;
using ICP.Modules.Api.Authorization.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Authorization.Services
{
    public class IdentifyService: BaseIdentifyService<IdentifyService>
    {
        private readonly ILogger _logger = null;

        public IdentifyService(ILogger<IdentifyService> logger): base(logger)
        {
            _logger = logger;
        }

        public DataResult<ICPRequestHeaders> ValidHeaders(NameValueCollection headers)
        {
            var result = new DataResult<ICPRequestHeaders>();
            result.SetError();

            _logger.Trace("準備取得 X-iCP-EncKeyID");
            if (!long.TryParse(headers["X-iCP-EncKeyID"], out long encKeyID))
            {
                result.SetCode(1016);
                _logger.Warning(result.RtnMsg);
                return result;
            }
            _logger.Trace("取得 X-iCP-EncKeyID 成功");

            _logger.Trace("準備取得 X-iCP-Signature");
            string signature = headers["X-iCP-Signature"];
            if (string.IsNullOrWhiteSpace(signature))
            {
                result.SetCode(1017);
                _logger.Warning(result.RtnMsg);
                return result;
            }
            _logger.Trace("取得 X-iCP-Signature 成功");

            result.SetSuccess(new ICPRequestHeaders
            {
                EncKeyID = encKeyID,
                Signature = signature
            });
            return result;
        }
    }
}
