using Autofac.Extras.DynamicProxy;

namespace ICP.Library.Repositories.MemberApi
{
    using Infrastructure.Core.Frameworks.DbUtil;
    using Infrastructure.Abstractions.DbUtil;
    using Infrastructure.Core.Models;
    using Library.Models.MemberApi.Certificate;

    public class CertificateMemberApiRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public CertificateMemberApiRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        public virtual ClientRsaCertDTO GetClientRsaCert(long clientCertId)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC [ICP_Member].[dbo].[ausp_Certificate_GetClientRSACert_S]";

            var args = new
            {
                ClientCertId = clientCertId
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<ClientRsaCertDTO>(sql, args);
        }

        public ClientAesCertDTO GetClientAesCert(long clientCertId, long rsaKeyClientCertId)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC [ICP_Member].[dbo].[ausp_Certificate_GetClientAesCert_S]";

            var args = new
            {
                ClientCertId = clientCertId,
                RsaKeyClientCertId = rsaKeyClientCertId
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<ClientAesCertDTO>(sql, args);
        }

        [EnableDbProxy]
        public BaseResult UpdateClientCertExpired(long ClientCertId, long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC [ICP_Member].[dbo].[ausp_Member_Certificate_UpdateClientCertExpired_U]";

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
