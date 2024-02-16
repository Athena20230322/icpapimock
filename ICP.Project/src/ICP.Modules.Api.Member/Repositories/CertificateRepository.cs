using Autofac.Extras.DynamicProxy;
using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Frameworks.AOP;
using ICP.Infrastructure.Core.Frameworks.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.Member.Models.Certificate;

namespace ICP.Modules.Api.Member.Repositories
{
    public class CertificateRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public CertificateRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        public RsaCertDTO GetRsaCert(long certId)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC [ICP_Member].[dbo].[ausp_Certificate_GetRSACert_S]";

            var args = new
            {
                CertId = certId
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<RsaCertDTO>(sql, args);
        }

        [EnableDbProxy]
        public virtual DataResult<long> AddClientRsaCert(AddClientRsaCertDTO dto)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC [ICP_Member].[dbo].[ausp_Certificate_AddClientRSACert_IU]";
            sql += db.GenerateParameter(dto);

            return db.QuerySingleOrDefault<DataResult<long>>(sql, dto);
        }

        [EnableDbProxy]
        public virtual DataResult<long> AddClientAesCert(AddClientAesCertDTO dto)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC [ICP_Member].[dbo].[ausp_Certificate_AddClientAesCert_IU]";
            sql += db.GenerateParameter(dto);

            return db.QuerySingleOrDefault<DataResult<long>>(sql, dto);
        }

        public DataResult<long> GetDefaultRsaCertId()
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC [ICP_Member].[dbo].[ausp_Certificate_GetDefaultRsaCertId_S]";

            return db.QuerySingleOrDefault<DataResult<long>>(sql);
        }

        [EnableDbProxy]
        public virtual DataResult<long> UpdateClientAesCertExpireDT(long clientCertId)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC [ICP_Member].[dbo].[ausp_Certificate_UpdateClientAesCertExpireDT_U]";

            var args = new
            {
                ClientCertId = clientCertId
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<DataResult<long>>(sql, args);
        }

        [EnableDbProxy]
        public virtual BaseResult UpdateClientCertFromMerchant(long clientCertId, long merchantID, string token)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC [ICP_Member].[dbo].[ausp_Certificate_UpdateClientCertFromMerchant_U]";

            var args = new
            {
                ClientCertId = clientCertId,
                MerchantID = merchantID,
                Token = token
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        [EnableDbProxy]
        public virtual BaseResult UpdateClientCertFromMember(long ClientCertId, long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_Certificate_UpdateClientCertFromMember_U";

            var args = new
            {
                ClientCertId,
                MID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
    }
}
