using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.OpenWalletApi.Enums;
using ICP.Library.Models.OpenWalletApi.WebUIApi;
using ICP.Library.Repositories.OpenWalletApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Services.OpenWalletApi
{
    public class OPWebUIApiService
    {
        OPWebUIApiRepository _oPWebUIApiRepository;

        public OPWebUIApiService(
            OPWebUIApiRepository oPWebUIApiRepository
            )
        {
            _oPWebUIApiRepository = oPWebUIApiRepository;
        }

        /// <summary>
        /// 更新 OPWebToken
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <returns></returns>
        public BaseResult UpdateOPWebTokenExpired(long MID, long RealIP, long ProxyIP)
        {
            return _oPWebUIApiRepository.UpdateOPWebTokenExpired(MID, RealIP, ProxyIP);
        }

        /// <summary>
        /// 取得 OPWebToken
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <returns></returns>
        public DataResult<MemberOPWebToken> GetOPWebToken(long MID)
        {
            var result = new DataResult<MemberOPWebToken>();
            result.SetError();

            var rtnData = _oPWebUIApiRepository.GetOPWebToken(MID);
            if (rtnData == null)
            {
                return result;
            }

            result.SetSuccess(rtnData);
            return result;
        }

        /// <summary>
        /// 檢查 OPWebToken
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <returns></returns>
        public DataResult<long> CheckOPWebToken(string OPAccessToken)
        {
            if (string.IsNullOrWhiteSpace(OPAccessToken))
            {
                var result = new DataResult<long>();
                result.SetError();
                return result;
            }

            return _oPWebUIApiRepository.CheckOPWebToken(OPAccessToken);
        }
    }
}