using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Repositories
{
    using ICP.Infrastructure.Core.Frameworks.DbUtil;
    using ICP.Modules.Mvc.Admin.Models.ViewModels;
    using Infrastructure.Abstractions.DbUtil;
    using Infrastructure.Core.Models;
    using Models;

    public class AppManagementRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public AppManagementRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 新增APP設定
        /// </summary>
        /// <param name="model">APP設定</param>
        /// <param name="Creator">建立者</param>
        /// <returns></returns>
        [EnableDbProxy]
        public virtual BaseResult AddAPPSetting(APPSetting model, string Creator)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);

            string sql = "EXEC ausp_Admin_App_AddAPPSettingList_I ";

            var args = new
            {
                model.VersionNo,
                model.XMLData,
                model.TestXMLData,
                model.ReleaseNote,
                model.TestMID,
                Creator
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<DataResult<long>>(sql, args);
        }

        /// <summary>
        /// 取得APP設定
        /// </summary>
        /// <param name="VersionNo">版本號</param>
        /// <returns></returns>
        public APPSetting GetAPPSetting(byte VersionNo)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);

            string sql = "EXEC ausp_Admin_App_GetAPPSettingList_S ";

            var args = new
            {
                VersionNo
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<APPSetting>(sql, args);
        }

        /// <summary>
        /// 取得目前最大版本號
        /// </summary>
        /// <returns></returns>
        public byte GetAPPSettingMaxVersion()
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);

            string sql = "EXEC ausp_Admin_App_GetAPPSettingMaxVersion_S";

            return db.QuerySingleOrDefault<byte>(sql);
        }

        /// <summary>
        /// 取得APP設定清單
        /// </summary>
        /// <returns></returns>
        public List<APPSettingQueryResult> ListAPPSetting()
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);

            string sql = "EXEC ausp_Admin_App_ListAPPSettingList_S";

            return db.Query<APPSettingQueryResult>(sql);
        }

        /// <summary>
        /// 發佈APP設定
        /// </summary>
        /// <param name="VersionNo">版本號</param>
        /// <param name="Modifier">發佈者</param>
        /// <returns></returns>
        [EnableDbProxy]
        public virtual BaseResult PublishAPPSetting(byte VersionNo, string Modifier)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);

            string sql = "EXEC ausp_Admin_App_PublishAPPSettingList_IU ";

            var args = new
            {
                VersionNo,
                Modifier
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 更新APP設定
        /// </summary>
        /// <param name="model">APP設定</param>
        /// <param name="Modifier">修改者</param>
        /// <returns></returns>
        [EnableDbProxy]
        public virtual BaseResult UpdateAPPSetting(APPSetting model, string Modifier)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);

            string sql = "EXEC ausp_Admin_App_UpdateAPPSettingList_IU ";

            var args = new
            {
                model.VersionNo,
                model.TestXMLData,
                model.ReleaseNote,
                model.TestMID,
                Modifier
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 取得修改歷程
        /// </summary>
        /// <param name="VersionNo">版本號</param>
        /// <returns></returns>
        public List<APPSettingLog> ListAPPXMLSettingLog(byte VersionNo)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);

            string sql = "EXEC ausp_Admin_ListAPPSettingListLog_S ";

            var args = new
            {
                VersionNo
            };
            sql += db.GenerateParameter(args);
            return db.Query<APPSettingLog>(sql, args);
        }
    }
}
