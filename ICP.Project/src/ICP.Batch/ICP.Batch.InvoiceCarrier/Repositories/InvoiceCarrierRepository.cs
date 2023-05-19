using System.Collections.Generic;
using System.Linq;
using ICP.Library.Models.EinvoiceLibrary;
using ICP.Library.Models.EinvoiceLibrary.Enum;
using ICP.Library.Repositories.MemberRepositories;

namespace ICP.Batch.InvoiceCarrier.Repositories
{
    using Infrastructure.Abstractions.DbUtil;
    using Infrastructure.Core.Frameworks.DbUtil;
    using Models;

    public class InvoiceCarrierRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;
        private MemberConfigCyptRepository _configCyptRepository;

        public InvoiceCarrierRepository(IDbConnectionPool dbConnectionPool, 
            MemberConfigCyptRepository configCyptRepository)
        {
            _dbConnectionPool = dbConnectionPool;
            _configCyptRepository = configCyptRepository;
        }

        /// <summary>
        /// 取得電子發票表頭下載推播表
        /// </summary>
        /// <returns></returns>
        public List<TitlePushModel> ListTitlePush(int dataType)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_Member_EInvoiceCarrier_ListTitlePush_SU";

            var args = new
            {
                DataType=dataType
            };
            sql += db.GenerateParameter(args);

            var result = db.Query<TitlePushModel>(sql, args).ToList();
            result.ForEach(x =>x.VerificationCode= Decrypt_InvoiceVerification(x.VerificationCode));

            return result;
        }

        /// <summary>
        /// 加密電子發票載具驗證碼字串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string Encrypt_InvoiceVerification(string str)
        {
            return _configCyptRepository.Encrypt_InvoiceVerification(str);
        }

        /// <summary>
        /// 解密電子發票載具驗證碼字串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string Decrypt_InvoiceVerification(string str)
        {
            return _configCyptRepository.Decrypt_InvoiceVerification(str);
        }
    }
}