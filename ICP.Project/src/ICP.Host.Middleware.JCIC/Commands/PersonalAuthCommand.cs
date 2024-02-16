using System.Linq;

namespace ICP.Host.Middleware.JCIC.Commands
{
    using Models;
    using Newtonsoft.Json;
    using Host.Middleware.JCIC.Services;
    using Infrastructure.Abstractions.Logging;
    using Infrastructure.Core.Models;

    public class PersonalAuthCommand
    {
        ILogger<PersonalAuthCommand> _logger;
        PersonalAuthService _personalAuthService;

        public PersonalAuthCommand(
            ILogger<PersonalAuthCommand> logger,
            PersonalAuthService personalAuthService
            )
        {
            _logger = logger;
            _personalAuthService = personalAuthService;
        }

        /// <summary>
        /// P33 驗證
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public P33AuthResponse ExecP33Auth(P33Auth model)
        {
            P33AuthResponse result = new P33AuthResponse();

            _logger.Info($"P33 Request: {JsonConvert.SerializeObject(model)}");

            var logResult = _personalAuthService.AddAuthP33Log(model);

            BaseResult baseResult = _personalAuthService.VerifyP33Auth(model);
            if (!baseResult.IsSuccess)
            {
                result.Return_Code = baseResult.RtnCode;
                result.Return_Msg = baseResult.RtnMsg;
            }
            else
            {
                result = _personalAuthService.P33Query(model);

                result.LogID = logResult.LogID;
            }
            _personalAuthService.UpdateAuthP33Log(result);

            _logger.Info($"P33 Response: {JsonConvert.SerializeObject(result)}");

            return result;
        }

        /// <summary>
        /// P11 驗證
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public P11AuthResponse ExecP11Auth(P11Auth model)
        {
            P11AuthResponse result = new P11AuthResponse();

            _logger.Info($"P11 Request: {JsonConvert.SerializeObject(model)}");

            var logResult = _personalAuthService.AddAuthP11Log(model);
            if (!logResult.IsSuccess)
            {
                result.Return_Code = logResult.RtnCode;
                result.Return_Msg = logResult.RtnMsg;
                return result;
            }

            BaseResult baseResult = _personalAuthService.VerifyP11Auth(model);
            if (!baseResult.IsSuccess)
            {
                result.Return_Code = baseResult.RtnCode;
                result.Return_Msg = baseResult.RtnMsg;
            }
            else
            {
                int[] specCode = new int[] { 102, 103, 104 };
                if (specCode.Contains(logResult.RtnCode))
                {
                    result.Return_Code = logResult.RtnCode;
                    result.Return_Msg = logResult.RtnMsg;
                    result.Result = logResult.RtnMsg;
                }
                else
                {
                    result = _personalAuthService.P11Query(model);

                    result.LogID = logResult.LogID;
                }
            }
            _personalAuthService.UpdateAuthP11Log(result);

            _logger.Info($"P11 Response: {JsonConvert.SerializeObject(result)}");

            return result;
        }
    }
}