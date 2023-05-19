using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Models.CustomerSecurityManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Repositories
{
    public class CustomerSecurityManageRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        private IDbConnection db;

        public CustomerSecurityManageRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;

            db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);
        }

        #region IP黑名單相關

        #region IP黑名單資料列表查詢
        public List<IPBlackListModel> ListIPBlackList(IPBlackQryModel query)
        {
            string sql = "EXEC ausp_Admin_ListMemberBlackIP_S";

            sql += db.GenerateParameter(query);
            return db.Query<IPBlackListModel>(sql, query);
        }
        #endregion

        #region 新增IP黑名單
        public BaseResult AddIPBlackList(IPBlackAddModel model)
        {
            string sql = "EXEC ausp_Admin_AddIP_BlackList_I";

            var args = new
            {
                model.IP,
                model.CreateUser,
                Status = 1,
                model.LockMemo,
                model.RealIP,
                model.ProxyIP
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
        #endregion

        #region 鎖定/解鎖IP黑名單
        public BaseResult UpdateIPBlackList(IPBlackUpdateModel model)
        {
            string sql = "EXEC ausp_Admin_UpdateIP_BlackList_U";

            var args = new
            {
                model.Sn,
                model.Status,
                model.Modifier,
                model.Memo,
                model.RealIP,
                model.ProxyIP
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
        #endregion

        #region IP黑名單歷程
        public List<IPBlackListLogModel> ListIPBlackListLog(string IP)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);

            string sql = "EXEC ausp_Admin_ListIPBlackListLogRecord_S";
            var args = new
            {
                IP
            };
            sql += db.GenerateParameter(args);
            return db.Query<IPBlackListLogModel>(sql, args);
        }
        #endregion

        #endregion

        #region 身份證黑名單相關

        #region 新增/解鎖/封鎖身份證黑名單
        public BaseResult AddOrUpdateIDNOBlackList(IDNOBlackAddOrUpdateModel model)
        {
            string sql = "EXEC ausp_Admin_AddBlockIDNO_SIU";
            sql += db.GenerateParameter(model);
            return db.QuerySingleOrDefault<BaseResult>(sql, model);
        }
        #endregion

        #region 身份證黑名單列表
        public List<IDNOBlackListLogModel> ListIDNOBlackList(IDNOBlackQryModel query)
        {
            string sql = "EXEC ausp_Admin_ListBlockIDNO_S";

            sql += db.GenerateParameter(query);
            return db.Query<IDNOBlackListLogModel>(sql, query);
        }
        #endregion

        #region 身份證黑名單Log列表
        public List<IDNOBlackListLogModel> ListIDNOBlackListLog(string IDNO)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);

            string sql = "EXEC ausp_ListBlockIDNOLog_S";
            var args = new
            {
                IDNO
            };
            sql += db.GenerateParameter(args);
            return db.Query<IDNOBlackListLogModel>(sql, args);
        }
        #endregion

        #endregion

        #region OTP黑名單相關

        #region 列表-[OTP黑名單]
        public List<OTPBlackListModel> ListBlackOTP(OTPBlackQryModel model)
        {

            string sql = "EXEC ausp_Admin_ListOTPBlackList_S";

            sql += db.GenerateParameter(model);
            return db.Query<OTPBlackListModel>(sql, model);
        }
        #endregion

        #region 新增/鎖定-[OTP黑名單]
        public BaseResult AddBlackOTP(OTPBlackLockModel model)
        {
            string sql = "EXEC ausp_Admin_AddLockOTP_IU";

            sql += db.GenerateParameter(model);
            return db.QuerySingleOrDefault<BaseResult>(sql, model);
        }
        #endregion

        #region 解鎖-[OTP黑名單]
        public BaseResult UnLockBlackOTP(OTPBlackUnLockModel model)
        {
            string sql = "EXEC ausp_Admin_UnLockOTP_U";

            sql += db.GenerateParameter(model);
            return db.QuerySingleOrDefault<BaseResult>(sql, model);
        }
        #endregion

        #region 列表-[OTP黑名單歷程紀錄]
        public List<OTPBlackListLogModel> ListOTPLog(string CellPhone)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);

            string sql = "EXEC ausp_Admin_ListOTPLog_S";
            var args = new
            {
                CellPhone
            };
            sql += db.GenerateParameter(args);
            return db.Query<OTPBlackListLogModel>(sql, args);
        }
        #endregion

        #endregion

        #region 提領限制黑名單相關

        #region 新增/解鎖/封鎖提領限制黑名單
        public BaseResult AddOrUpdateTakeCashLimitList(TakeCashLimitAddOrUpdateModel model)
        {
            string sql = "EXEC ausp_Admin_AddTakeCashLimitList_SIU";
            sql += db.GenerateParameter(model);
            return db.QuerySingleOrDefault<BaseResult>(sql, model);
        }
        #endregion

        #region 提領限制黑名單列表
        public List<TakeCashLimitListLogModel> ListTakeCashLimitList(TakeCashLimitQryModel query)
        {
            string sql = "EXEC ausp_Admin_ListTakeCashLimitList_S";

            sql += db.GenerateParameter(query);
            return db.Query<TakeCashLimitListLogModel>(sql, query);
        }
        #endregion

        #region 提領限制黑名單Log列表
        public List<TakeCashLimitListLogModel> ListTakeCashLimitListLog(string MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);

            string sql = "EXEC ausp_ListTakeCashLimitLog_S";
            var args = new
            {
                MID
            };
            sql += db.GenerateParameter(args);
            return db.Query<TakeCashLimitListLogModel>(sql, args);
        }
        #endregion

        #endregion


        #region 註冊同IP預警名單相關

        #region 註冊同IP預警名單列表
        public List<RegistIPListLogModel> ListRegistIPList(RegistIPBlackQryModel model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);

            string sql = "EXEC ausp_Admin_ListIP_WarningList_S";
            sql += db.GenerateParameter(model);
            return db.Query<RegistIPListLogModel>(sql, model);
        }
        #endregion

        #region 註冊同IP預警名單明細
        public List<RegistIPListLogModel> ListRegistIPListLog(string RealIP)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);

            string sql = "EXEC ausp_Admin_ListRegisterLogRecord_S";
            var args = new
            {
                RealIP
            };
            sql += db.GenerateParameter(args);
            return db.Query<RegistIPListLogModel>(sql, args);
        }
        #endregion

        #endregion


    }
}
