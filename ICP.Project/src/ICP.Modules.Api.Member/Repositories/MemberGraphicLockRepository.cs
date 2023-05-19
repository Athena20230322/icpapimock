using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.Member.Models.MemberInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ICP.Modules.Api.Member.Repositories
{
    public class MemberGraphicLockRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public MemberGraphicLockRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        #region 取得圖型鎖相關資訊
        public MemberGraphicLock GetMemberGraphicLock(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberSecurity_GetGraphicLockByMID_S";

            var args = new
            {
                MID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<MemberGraphicLock>(sql, args);
        }
        #endregion

        #region 設定/更新 圖型鎖
        public BaseResult UpdateGraphicPassword(long MID, GraphicLockRerquest model, long realIP, long proxyIP, string DeviceID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberSecurity_UpdateGraphicPassword_SIU";

            var args = new
            {
                MID,
                model.OriPassword,
                model.NewPassword,
                realIP,
                proxyIP,
                DeviceID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);

        }
        #endregion

        #region M0010 檢查圖形密碼
        /// <summary>
        ///驗證APP 圖型鎖
        /// </summary>
        public BaseResult CheckGraphicLock(long MID, string GraphicPwd, long realIP = 0, long ProxyIP = 0)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberSecurity_CheckGraphicLockPassword_SIU";

            var args = new
            {
                MID,
                GraphicPwd,
                realIP,
                ProxyIP
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
        #endregion

        #region 檢查 圖型鎖 是否跟原密碼一樣
        /// <summary>
        /// 檢查 圖型鎖 是否跟原密碼一樣
        /// </summary>
        /// <param name="GraphicPwd">圖型鎖</param>
        /// <param name="mid">會員編號</param>
        /// <returns></returns>
        public bool CheckOldGraphicPwdSame(string GraphicPwd, long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberSecurity_CheckOldGraphicPwdSame_S";

            var args = new
            {
                MID,
                GraphicPwd
            };

            bool bResult = false;

            sql += db.GenerateParameter(args);
            int rtnCode = db.Execute(sql, args);

            if (rtnCode == 1)
            {
                bResult = true;
            }

            return bResult;
        }
        #endregion

        //#region 忘記圖型鎖重設
        ///// <summary>
        ///// 忘記圖型鎖重設
        ///// </summary>
        ///// <param name="GraphicPwd">圖型鎖</param>
        ///// <param name="mid">會員編號</param>
        ///// <returns></returns>
        //public bool UpdateGraphicPwdOnReset(long MID, string GraphicPwd, long realIP = 0, long ProxyIP = 0)
        //{
        //    var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

        //    string sql = "EXEC ausp_Member_MemberSecurity_UpdateGraphicPwdOnReset_U";

        //    var args = new
        //    {
        //        MID,
        //        GraphicPwd,
        //        realIP,
        //        ProxyIP
        //    };

        //    bool bResult = false;

        //    BaseResult rtnModel = db.QuerySingleOrDefault<BaseResult>(sql, args);

        //    if (rtnModel.RtnCode == 1)
        //    {
        //        bResult = true;
        //    }

        //    return bResult;
        //}
        //#endregion

        #region  更新略過修改圖型鎖日期
        public BaseResult UpdateGraphicPwdIgnorDate(long MID, long realIP = 0, long ProxyIP = 0)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberSecurity_UpdateGraphicPwdIgnorDate_U";

            var args = new
            {
                MID,
                realIP,
                ProxyIP
            };

            BaseResult RtnModel = new BaseResult();

            sql += db.GenerateParameter(args);

            RtnModel = db.QuerySingleOrDefault<BaseResult>(sql, args);

            return RtnModel;
        }
        #endregion

        #region 取得使用圖型鎖記錄(未設定過圖型鎖)
        public GetAppGraphicDataLog GetMemberGraphicDataLog(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberSecurity_GetAppGraphicLog_S";

            var args = new
            {
                MID
            };

            var model = db.QuerySingleOrDefault<GetAppGraphicDataLog>(sql, args);

            return model;
        }
        #endregion

        /// <summary>
        /// 更新圖型密碼鎖狀態
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult UpdateGraphicStatus(GraphicLockStatusModel model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "ausp_Member_MemberSecurity_UpdateGraphicStatus_IU";

            sql += db.GenerateParameter(model);            

            return db.QuerySingleOrDefault<BaseResult>(sql, model);

        }
    }
}