using System.Collections.Generic;
using ICP.Library.Models.EinvoiceLibrary;
using ICP.Library.Models.EinvoiceLibrary.Enum;
using ICP.Library.Repositories.MemberRepositories;

namespace ICP.Batch.InvoiceCarrierDetail.Repositories
{
    using Infrastructure.Abstractions.DbUtil;
    using Infrastructure.Core.Frameworks.DbUtil;
    //using Models;
    public class InvoiceCarrierDetailRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;
        private MemberConfigCyptRepository _configCyptRepository;

        public InvoiceCarrierDetailRepository(
            IDbConnectionPool dbConnectionPool, 
            MemberConfigCyptRepository configCyptRepository)
        {
            _dbConnectionPool = dbConnectionPool;
            _configCyptRepository = configCyptRepository;
        }

        /// <summary>
        /// 取得未下載明細的資訊
        /// </summary>
        /// <returns></returns>
        public List<EinvoiceByCarrierDetailModel> QueryCarrierDetail()
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC ausp_Member_WaitDownLoadCarrierDetailList_S";

            var result = db.Query<EinvoiceByCarrierDetailModel>(sql);
            result.ForEach(x => x.VerificationCode=Decrypt_InvoiceVerification(x.VerificationCode));

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