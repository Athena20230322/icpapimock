using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Repositories.OpenWalletApi
{
    using Infrastructure.Core.Models;
    using Infrastructure.Abstractions.DbUtil;
    using Models.OpenWalletApi.WebUIApi;
    using ICP.Library.Models.OpenWalletApi.Enums;
    using ICP.Infrastructure.Core.Extensions;
    using ICP.Library.Repositories.MemberRepositories;

    public class OPWebUIApiRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;
        MemberConfigCyptRepository _configCyptRepository;

        public OPWebUIApiRepository(
            IDbConnectionPool dbConnectionPool,
            MemberConfigCyptRepository configCyptRepository
            )
        {
            _dbConnectionPool = dbConnectionPool;
            _configCyptRepository = configCyptRepository;
        }

        /// <summary>
        /// 產生 Mask
        /// </summary>
        /// <param name="webUIApiMethodType"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<string> GenerateMask(WebUIApiMethodType webUIApiMethodType, object model)
        {
            var result = new DataResult<string>();
            result.SetError();

            GeneratePropertiesObject gen = null;

            if (webUIApiMethodType == WebUIApiMethodType.Login)
            {
                #region ICP001 icash會員登入驗證
                gen =
                    new GeneratePropertiesObject<LoginWebUIRequest>((LoginWebUIRequest)model)
                    .Add(t => t.LoginID)
                    .Add(t => t.PassWord)
                    .Add(t => t.TimeStamp)
                    ;
                #endregion
            }
            else if (webUIApiMethodType == WebUIApiMethodType.GetUserData)
            {
                #region ICP003 查詢未成年身分資料
                gen =
                    new GeneratePropertiesObject<GetUserDataWebUIRequest>((GetUserDataWebUIRequest)model)
                    .Add(t => t.Token)
                    .Add(t => t.TimeStamp)
                    ;
                #endregion
            }
            else if (webUIApiMethodType == WebUIApiMethodType.AgreeRegister)
            {
                #region ICP004 同意未成年註冊
                gen =
                    new GeneratePropertiesObject<AgreeRegisterWebUIRequest>((AgreeRegisterWebUIRequest)model)
                    .Add(t => t.Token)
                    .Add(t => t.MID)
                    .Add(t => t.UserID)
                    .Add(t => t.TimeStamp)
                    ;
                #endregion
            }
            else
            {
                result.RtnMsg = $"GenerateMask WebUIApiMethodType {webUIApiMethodType.ToString()} handle not defined";
                return result;
            }

            //所有的INPUT欄位依序組合
            string valueString = gen.ToValueString();

            //由字串{Md5加密前綴} + 所有的INPUT欄位依序組合 + {Md5加密後綴}轉成MD5
            string Mask = _configCyptRepository.MD5_OPWebUIApiMask(valueString);

            if (Mask != null) Mask = Mask.ToLower();

            result.SetSuccess(Mask);
            return result;
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
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_OPWeb_UpdateOPWebTokenExpired_IU";

            var args = new
            {
                MID,
                RealIP,
                ProxyIP
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 取得 OPWebToken
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <returns></returns>
        public MemberOPWebToken GetOPWebToken(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_OPWeb_GetOPWebToken_S";

            var args = new
            {
                MID,
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<MemberOPWebToken>(sql, args);
        }

        /// <summary>
        /// 檢查 OPWebToken
        /// </summary>
        /// <param name="MID">會員編號</param>
        /// <returns></returns>
        public DataResult<long> CheckOPWebToken(string OPAccessToken)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_OPWeb_CheckOPWebToken_S";

            var args = new
            {
                OPAccessToken
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<DataResult<long>>(sql, args);
        }
    }
}