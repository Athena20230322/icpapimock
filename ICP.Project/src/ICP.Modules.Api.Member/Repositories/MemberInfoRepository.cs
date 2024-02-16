using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Repositories
{
    using Infrastructure.Abstractions.DbUtil;
    using Infrastructure.Core.Frameworks.DbUtil;
    using Infrastructure.Core.Models;
    using Library.Models.MemberModels;
    using Models.MemberInfo;

    public class MemberInfoRepository
    {
        IDbConnectionPool _dbConnectionPool;

        public MemberInfoRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        public MemberAppToken GetMemberAppToken(string OPMID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_GetAppToken_S";

            var args = new
            {
                OPMID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<MemberAppToken>(sql, args);
        }

        public MemberAppToken GetMemberAppToken(long AppTokenID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_GetAppToken_S";

            var args = new
            {
                AppTokenID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<MemberAppToken>(sql, args);
        }

        public MemberAppToken GetMemberAppTokenByMID(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_GetAppTokenByMID_S";

            var args = new
            {
                MID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<MemberAppToken>(sql, args);
        }

        public MemberAppToken GetMemberAppTokenByAuthV(string AuthV)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_GetAppTokenByAuthV_S";

            var args = new
            {
                AuthV
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<MemberAppToken>(sql, args);
        }

        public MemberAppToken GetMemberAppTokenByAccount(string Account)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_Mock_GetAppTokenByAccount_S";

            var args = new
            {
                Account
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<MemberAppToken>(sql, args);
        }

        [EnableDbProxy]
        public virtual BaseResult UpdateMemberAppTokenExpired(long AppTokenID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_UpdateAppTokenExpired_U";

            var args = new
            {
                AppTokenID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        [EnableDbProxy]
        public virtual BaseResult UpdateMemberAppToken(AddMemberAppToken model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "ausp_Member_MemberInfo_UpdateAppToken_IU";

            var args = new
            {
                model.OPMID,
                model.AuthV,
                model.OPAccessToken,
                model.OPExpired,
                model.OPCellPhone,
                model.OPMobileBarcode,
                model.OPErrorCode,
                model.OPErrorMessage
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        public MemberVerifyStatus GetMemberVerifyStatus(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_GetVerifyStatus_S";

            var args = new
            {
                MID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<MemberVerifyStatus>(sql, args);
        }

        /// <summary>
        /// M0013 取得密碼設定開關狀態
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public GetPasswordStatusResult GetPasswordStatus(long mid)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "ausp_Member_MemberSecurity_GetPasswordStatus_S";

            var args = new
            {
                mid
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<GetPasswordStatusResult>(sql, args);

        }

        #region 安全密碼相關

        #region 驗證安全密碼
        /// <summary>
        /// 驗證安全密碼
        /// </summary>
        public BaseResult Check_Member_Security_CheckSecPwd(long MID, string PayPwd, long realIP, long proxyIP)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "ausp_Member_MemberSecurity_CheckPayPwd_SIU";

            var args = new
            {
                MID,
                PayPwd,
                realIP,
                proxyIP
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
        #endregion

        /// <summary>
        /// 檢查安全密碼是否正確
        /// </summary>
        /// <param name="loginPassword">安全密碼</param>
        /// <param name="mid">會員編號</param>
        /// <returns></returns>
        public bool CheckOldPayPasswordSame(string PayPassword, long mid)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "ausp_Member_MemberSecurity_CheckOldPayPasswordSame_S";

            bool bResult = false;

            var args = new
            {
                PayPassword,
                mid
            };

            sql += db.GenerateParameter(args);

            
            int rtnCode = db.QuerySingleOrDefault<BaseResult>(sql, args).RtnCode;                        

            if (rtnCode == 1)
            {
                bResult = true;
            }

            return bResult;
        }

        /// <summary>
        /// 取得會員目前安全密碼
        /// </summary>
        /// <param name="loginPassword">安全密碼</param>
        /// <param name="mid">會員編號</param>
        /// <returns></returns>
        public string GetOriSecPWD(long mid)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "ausp_Member_MemberSecurity_GetSecPWD_S";

            var args = new
            {
                mid
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<string>(sql, args);
        }


        /// <summary>
        /// 更新安全密碼
        /// </summary>
        /// <param name="model">TradePassword Model</param>
        /// <param name="mid">會員代碼</param>
        /// <param name="email">回傳的Email</param>
        /// <returns></returns>
        public bool UpdateSecPassword(SecPassword model, long mid, long realIP, long proxyIP, ref string email)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            bool bResult = false;

            string oriPayPassword = model.OriSecPassword;
            string newPayPassword = model.NewSecPassword;

            string sql = "ausp_Member_MemberSecurity_UpdateSecPassword_U";

            var args = new
            {
                mid,
                oriPayPassword,
                newPayPassword,
                realIP,
                proxyIP
            };

            sql += db.GenerateParameter(args);

            var RtnModel = db.QuerySingleOrDefault<SecPasswordResult>(sql, args);
            if (RtnModel.RtnCode == 1)
            {
                email = RtnModel.Email;
                bResult = true;
            }

            return bResult;
        }

        #endregion

        #region 登入密碼相關

        /// <summary>
        /// 檢查登入密碼是否正確
        /// </summary>
        /// <param name="loginPassword">登入密碼</param>
        /// <param name="mid">會員編號</param>
        /// <returns></returns>
        public bool CheckLoginPassword(string loginPassword, long mid)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);


            bool bResult = false;

            string sql = "ausp_Member_MemberSecurity_CheckLoginPassword_S";

            var args = new
            {
                loginPassword,
                mid
            };

            sql += db.GenerateParameter(args);

            int rtnCode = db.Execute(sql, args);

            if (rtnCode == 1)
            {
                bResult = true;
            }

            return bResult;
        }

        /// <summary>
        /// 檢查登入密碼是否跟原登入密碼相同 
        /// </summary>
        /// <param name="loginPassword">登入密碼</param>
        /// <param name="mid">會員編號</param>
        /// <returns></returns>
        public BaseResult CheckOldLoginPasswordSame(string loginPassword, long mid)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);


            var result = new BaseResult();

            string sql = "ausp_Member_MemberSecurity_CheckOldLoginPasswordSame_S";

            var args = new
            {
                loginPassword,
                mid
            };

            sql += db.GenerateParameter(args);

            result = db.QuerySingleOrDefault<BaseResult>(sql, args);
            

            return result;
        }

        /// <summary>
        /// 取得會員目前安全密碼
        /// </summary>
        /// <param name="loginPassword">安全密碼</param>
        /// <param name="MID">會員編號</param>
        /// <returns></returns>
        [EnableDbProxy]
        public string GetOriLoginPWD(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "ausp_Member_MemberSecurity_GetLoginPWD_S";

            var args = new
            {
                MID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<string>(sql, args);
        }


        /// <summary>
        /// 更新登入密碼
        /// </summary>
        /// <param name="model">TradePassword Model</param>
        /// <param name="mid">會員代碼</param>
        /// <param name="email">回傳的Email</param>
        /// <returns></returns>
        public BaseResult UpdateLoginPassword(LoginPassword model, long mid, long realIP, long proxyIP, ref string email)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string oriLoginPassword = model.OriLoginPassword;
            string newLoginPassword = model.NewLoginPassword;

            string sql = "ausp_Member_MemberSecurity_UpdateLoginPassword_U";

            var args = new
            {
                mid,
                oriLoginPassword,
                newLoginPassword,
                realIP,
                proxyIP
            };

            sql += db.GenerateParameter(args);

            var RtnModel = db.QuerySingleOrDefault<LoginPasswordResult>(sql, args);
            if (RtnModel.IsSuccess)
            {
                email = RtnModel.Email;
            }

            return RtnModel;
        }

        #endregion

        #region 修改Email
        //##  修改Email
        public BaseResult UpdateEmailAddress(long MID, string Email)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            BaseResult RtnModel = new BaseResult();

            string sql = "ausp_Member_MemberInfo_UpdateEmail_U";

            var args = new
            {
                MID,
                Email
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
        #endregion

        #region 更新聊天會員資料
        //##  更新聊天會員資料
        public BaseResult UpdateNickName(long MID, string NickName)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);


            BaseResult RtnModel = new BaseResult();

            string sql = "ausp_Member_MemberInfo_UpdateNickName_U";

            var args = new
            {
                MID,
                NickName
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
        #endregion


        //##  M0038 更新略過修改密碼日期
        public BaseResult UpdatePwdIgnorDate(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            BaseResult RtnModel = new BaseResult();

            string sql = "ausp_Member_MemberSecurity_UpdatePwdIgnorDate_U";

            var args = new
            {
                MID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
        

        #region M0039_記錄略過修改安全密碼

        public BaseResult UpdateIgnoreModifyPayPwdDate(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            BaseResult RtnModel = new BaseResult();


            string sql = "ausp_Member_MemberSecurity_UpdateIgnoreModifyPayPwdDate_U";

            var args = new
        {
                MID
            };

            sql += db.GenerateParameter(args);            

            return db.QuerySingleOrDefault<BaseResult>(sql, args);

            
        }

        #endregion

        public List<MemberBankDetail> ListBankDetail()
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_ListMemberBankDetail_S";

            return db.Query<MemberBankDetail>(sql).ToList();
        }

        public WithdrawBalanceInfo GetBankTransferInfo(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_GetBankTransferInfo_S";

            var args = new
            {
                MID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<WithdrawBalanceInfo>(sql, args);
        }


        public BaseResult CheckEmail(string Email, long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_CheckEmail_S";

            var args = new
            {
                Email,
                MID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        public BaseResult UpdateMemberDetail(long MID, string CName, MemberDetailModel detailModel)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_UpdateMemberDetail_U";

            var args = new
            {
                MID,
                CName,
                detailModel.Birthday,
                detailModel.NationalityID,
                detailModel.AreaID,
                detailModel.Address,
                detailModel.Email,
                detailModel.IDNO,
                detailModel.UniformID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
        
        [EnableDbProxy]
        public virtual BaseResult DeleteMemberBankAccount(long MID, byte Category, string BankCode, long AccountID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_DeleteBankAccount_IU";

            var args = new
            {
                MID,
                Category,
                BankCode,
                AccountID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        public BaseResult AddAuthTeenagers(long MID, long RealIP, long ProxyIP)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberAuth_AddAuthTeenagers_I";

            var args = new
            {
                MID,
                RealIP,
                ProxyIP
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        public BaseResult AddAuthTeenagersLegalDetail(long MID, LegalRepData legalRepData, long RealIP, long ProxyIP)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberAuth_AddAuthTeenagersLegalDetail_I";

            var args = new
            {
                MID,
                ICPMID = legalRepData.LegalRepIcpMID,
                CName = legalRepData.LegalRepName,
                LegalType = legalRepData.LegalRepType,
                RealIP,
                ProxyIP
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }


        public BaseResult UpdateAccountResult(long mid, string Account, string CreateUser, long RealIP, long ProxyIP)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_UpdateAccount_U";

            var args = new
            {
                mid,
                Account,
                CreateUser,
                RealIP,
                ProxyIP
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
        

        /// <summary>
        /// 查詢OP會員與ICP會員手機條碼載具
        /// </summary>
        /// <param name="RtnOPMID"></param>
        /// <returns></returns>
        public CellphoneCarrierModel GetCarrierDetail(string RtnOPMID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            var sql = "EXEC ausp_Member_MemberInfo_GetMIDDetailByOpMID";

            var args = new
            {
                RtnOPMID
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<CellphoneCarrierModel>(sql, args);
        }

        /// <summary>
        /// 變更ICP會員手機條碼載具
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public BaseResult AddCarrierDetail(CellphoneCarrierModel model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            var sql = "EXEC ausp_Member_CellphoneCarrier_AddCarrier_ID";

            var args = new
            {   
                model.MID,
                CellPhone=model.OPCellPhone,
                model.CarrierNum
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql,args);
        }

        /// <summary>
        /// 取得會員餘額
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public UserCoins GetUserCoins(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Coins);

            string sql = "EXEC ausp_Coins_UserCoin_GetUserCoins_SI";

            var args = new
            {
                MID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<UserCoins>(sql, args);
        }

        /// <summary>
        /// 確認是否有未完成的提領資料
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public bool CheckBankTransfer(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);

            string sql = "EXEC ausp_Payment_ManageBank_CheckBankTransferByMID_S";

            var args = new
            {
                MID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<bool>(sql, args);
        }

        /// <summary>
        /// 會員結清
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="Modifier"></param>
        /// <param name="RealIP"></param>
        /// <param name="ProxyIP"></param>
        /// <returns></returns>
        public BaseResult CloseMemberAccount(long MID, byte Source, string Modifier, long RealIP, long ProxyIP)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_CloseMemberAccount_IU";

            var args = new
            {
                MID,
                Source,
                Modifier,
                RealIP,
                ProxyIP
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 確認會員是否有待撥款項
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public bool CheckUnAllocateTrade(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);

            string sql = "EXEC ausp_Payment_Trade_CheckUnAllocateTrade_S";

            var args = new
            {
                MID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<bool>(sql, args);
        }

        /// <summary>
        /// 取得法定代理人資料
        /// </summary>
        /// <param name="TeenagersMID"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public List<TeenagersLegalDetail> ListTeenagersLegalDetail(long TeenagersMID, byte Status = 1)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_Teenagers_ListTeenagersLegalDetailByTeenMID_S";

            var args = new
            {
                TeenagersMID,
                Status
            };

            sql += db.GenerateParameter(args);

            return db.Query<TeenagersLegalDetail>(sql, args).ToList();
        }
    }

}
