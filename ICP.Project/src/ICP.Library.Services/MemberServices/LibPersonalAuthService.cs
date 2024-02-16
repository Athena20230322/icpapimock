using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Services.MemberServices
{
    using Library.Models.MemberModels;
    using Infrastructure.Core.Models;
    using Infrastructure.Core.Helpers;
    using ICP.Library.Repositories.MemberRepositories;
    using ICP.Infrastructure.Core.Extensions;

    public class LibPersonalAuthService
    {
        PersonalAuth.PersonalAuthSoapClient _personalAuthSoapClient;
        MemberAuthRepository _memberAuthRepository;
		MemberConfigRepository _configRepository;


        public LibPersonalAuthService(
            PersonalAuth.PersonalAuthSoapClient personalAuthSoapClient,
            MemberAuthRepository memberAuthRepository,
			MemberConfigRepository configRepository
            )
        {
            _personalAuthSoapClient = personalAuthSoapClient;
            _memberAuthRepository = memberAuthRepository;
            _configRepository = configRepository;
        }

        /// <summary>
        /// 驗證身份證發證日期
        /// </summary>
        /// <param name="dt">發證日期</param>
        /// <returns></returns>
        public BaseResult VerifyIssueDate(DateTime dt)
        {
            var result = new BaseResult();
            result.SetError();

            if (dt < new DateTime(94 + 1911, 12, 21))
            {
                result.RtnMsg = "最小發證日期為 94年12月21號";
                return result;
            }

            result.SetSuccess();
            return result;
        }

        /// <summary>
        /// P33 驗證
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="IDNO"></param>
        /// <param name="UserName"></param>
        /// <param name="Source"></param>
        /// <returns></returns>
        public PersonalAuth.P33AuthResult VerifyP33Auth(P33Auth model)
        {
            if (!_configRepository.ProductMode)
            {
                return new PersonalAuth.P33AuthResult
                {
                    RtnCode = 1,
                    RtnMsg = "",
                    IsPass = 1,
                    DataCount = 0,
                    DataList = string.Empty,
                    IsSuccess = true
                };
            }

            var reqData = new PersonalAuth.P33Auth
            {
                MID = model.MID,
                IDNO = model.IDNO,
                UserName = model.UserName,
                Source = model.Source,
                RealIP = model.RealIP,
                ProxyIP = model.ProxyIP
            };
            return _personalAuthSoapClient.P33Auth(reqData);
        }

        /// <summary>
        /// P11 驗證
        /// </summary>
        /// <returns></returns>
        public PersonalAuth.P11AuthResult VerifyP11Auth(P11Auth model)
        {
            if (!_configRepository.ProductMode)
            {
                return new PersonalAuth.P11AuthResult
                {
                    RtnCode = 1,
                    IsPass = 1,
                    Answer = "Y",
                    Result = "與檔存資料相符",
                    IsSuccess = true
                };
            }

            var reqData = new PersonalAuth.P11Auth
            {
                MID = model.MID,
                IDNO = model.IDNO,
                IssueDate = ConvertHelper.ToSimpleTaiwanDate(model.IssueDate),
                IssueType = model.ObtainType,
                BirthDate = ConvertHelper.ToSimpleTaiwanDate(model.BirthDay),
                PicFree = model.IsPicture,
                IssueLoc = model.IssueLocationID,
                Source = model.Source,
                UserName = model.UserName,
                RealIP = model.RealIP,
                ProxyIP = model.ProxyIP
            };
            return _personalAuthSoapClient.P11Auth(reqData);
        }

        /// <summary>
        /// P33 驗證及更新DB
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult AddAuthP33(P33Auth model)
        {
            var result = new P33AuthResult();
            result.SetError();

            var p33Result = VerifyP33Auth(model);
            if (!p33Result.IsSuccess)
            {
                result.RtnCode = p33Result.RtnCode;
                result.RtnMsg = p33Result.RtnMsg;
                return result;
            }

            var p33AuthResult = new P33AuthResult
            {
                RtnCode = p33Result.RtnCode,
                RtnMsg = p33Result.RtnMsg,
                MID = model.MID,
                IDNO = model.IDNO,
                AuthStatus = (byte)p33Result.IsPass
            };

            var addResult = _memberAuthRepository.AddAuthP33(p33AuthResult);
            if (!addResult.IsSuccess)
            {
                result.SetError(addResult);
                return result;
            }

            result = p33AuthResult;
            return result;
        }

        /// <summary>
        /// P11 驗證及更新DB
        /// </summary>
        /// <param name="model"></param>
        /// <param name="filePath1"></param>
        /// <param name="filePath2"></param>
        /// <returns></returns>
        public P11AuthResult AddAuthP11(P11Auth model)
        {
            var result = new P11AuthResult();
            result.SetError();

            var p11Result = VerifyP11Auth(model);
            if (!p11Result.IsSuccess)
            {
                result.RtnCode = p11Result.RtnCode;
                result.RtnMsg = p11Result.RtnMsg;
                return result;
            }

            byte authStatus = Convert.ToByte(p11Result.IsPass);
            var updateResult = _memberAuthRepository.UpdateAuthIDNOStatus(model.MID, authStatus);
            if (!updateResult.IsSuccess)
            {
                result.SetError(updateResult);
                return result;
            }

            result = AutoMapper.Mapper.Map<P11AuthResult>(model);
            result.RtnCode = p11Result.RtnCode;
            result.RtnMsg = p11Result.RtnMsg;
            result.AuthStatus = authStatus;
            return result;
        }
    }
}
