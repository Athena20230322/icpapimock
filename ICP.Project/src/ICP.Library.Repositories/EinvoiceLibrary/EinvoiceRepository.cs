using System;
using System.Collections.Generic;
using ICP.Infrastructure.Core.Extensions;
using ICP.Library.Models.EinvoiceLibrary;
using ICP.Library.Models.EinvoiceLibrary.Enum;
using ICP.Library.Repositories.MemberRepositories;

namespace ICP.Library.Repositories.EinvoiceLibrary
{
    using Infrastructure.Abstractions.DbUtil;
    using Infrastructure.Core.Models;

    public class EinvoiceRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;
        private MemberConfigCyptRepository _configCyptRepository;
        public EinvoiceRepository(
            IDbConnectionPool dbConnectionPool, 
            MemberConfigCyptRepository configCyptRepository)
        {
            _dbConnectionPool = dbConnectionPool;
            _configCyptRepository = configCyptRepository;
        }

        #region APP API

        /// <summary>
        /// 變更ICP會員手機條碼載具
        /// </summary>
        /// <param name="mid">ICP會員MID</param>
        /// <param name="carrierNum">載具號碼</param>
        /// <param name="cellPhone">會員手機</param>
        /// <param name="verificationCode"></param>
        /// <param name="isVerify"></param>
        /// <returns></returns>
        public BaseResult CellphoneCarrierAddCarrier(long mid, string carrierNum, string cellPhone,string verificationCode="",int isVerify=0 )
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC ausp_Member_CellphoneCarrier_AddCarrier_ID";

            var args = new
            {
                MID = mid,
                CarrierNum = carrierNum,
                CellPhone = cellPhone,
                VerificationCode=string.IsNullOrWhiteSpace(verificationCode)?null: Encrypt_InvoiceVerification(verificationCode),
                IsVerify=isVerify
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 取得電子發票載具頁面所需資訊
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public GetEInvoiceCarrierInfoResult GetEInvoiceCarrierInfo(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            GetEInvoiceCarrierInfoResult result = new GetEInvoiceCarrierInfoResult();
            string sql = "EXEC ausp_Member_CellphoneCarrier_GetEInvoiceCarrierInfo_S";

            var args = new
            {
                MID
            };

            sql += db.GenerateParameter(args);

            result.CarrierList = db.Query<GetEInvoiceCarrierInfoResultType>(sql, args);

            return result;
        }

        /// <summary>
        /// 取得會員手機條碼設定檔
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public CellPhoneCarrierDetail GetCellPhoneCarrierDetail(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC ausp_Member_CellphoneCarrier_GetCellPhoneCarrierDetail_S";

            var args = new
            {
                MID
            };

            sql += db.GenerateParameter(args);

            var result = db.QuerySingleOrDefault<CellPhoneCarrierDetail>(sql, args);
            if (result!=null)
            {
                result.VerificationCode =string.IsNullOrWhiteSpace(result.VerificationCode)?null:Decrypt_InvoiceVerification(result.VerificationCode);
            }
            
            return result;
        }

        /// <summary>
        /// 取得當期發票清單
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="invPeriod"></param>
        /// <param name="einvoicePeriod"></param>
        /// <returns></returns>
        public List<EinvoiceItemList> QueryInvList(long mid, string einvoicePeriod)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC ausp_Member_GetMember_EinvoiceList_S";

            var args = new
            {
                MID = mid,
                EinvoicePeriod = einvoicePeriod
            };
            sql += db.GenerateParameter(args);

            return db.Query<EinvoiceItemList>(sql, args);
        }

        /// <summary>
        /// 取得發票明細與載具資料
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="einvoicePeriod"></param>
        /// <param name="einvoiceNum"></param>
        /// <returns></returns>
        public CarrierInvDetailModel GetCarrierInvDetail(long mid, string einvoicePeriod, string einvoiceNum)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC ausp_Member_EinvoiceCarrierDetail_S";

            var args = new
            {
                MID = mid,
                EinvoiceNum = einvoiceNum,
                EinvoicePeriod = einvoicePeriod
            };
            sql += db.GenerateParameter(args);

            var result = db.QuerySingleOrDefault<CarrierInvDetailModel>(sql, args);
            if (result!=null)
            {
                result.VerificationCode =string.IsNullOrWhiteSpace(result.VerificationCode)?null:Decrypt_InvoiceVerification(result.VerificationCode);
            }

            return result;
        }

        /// <summary>
        /// 取得發票明細
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="einvoiceNum"></param>
        /// <param name="einvoicePeriod"></param>
        /// <returns></returns>
        public QueryInvDetailModel QueryInvDetail(long mid, string einvoicePeriod, string einvoiceNum)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC ausp_Member_GetMember_EinvoiceDetail_S";

            var args = new
            {
                MID = mid,
                EinvoiceNum = einvoiceNum,
                EinvoicePeriod = string.IsNullOrWhiteSpace(einvoicePeriod) ? null : einvoicePeriod
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<QueryInvDetailModel>(sql, args);
        }

        #endregion

        #region Log與批號

        /// <summary>
        /// 取得批號
        /// </summary>
        /// <returns></returns>
        public EinvoiceBatchNoModel GetBatchNo(BatchInvoiceType invoiceType)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC ausp_AddMember_Einvoice_BatchNo_I";

            var args = new
            {
                BatchInvoiceType=invoiceType
            };
            
            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<EinvoiceBatchNoModel>(sql, args);
        }

        /// <summary>
        /// 更新批號狀態
        /// </summary>
        /// <param name="BatchNo"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int UpdateBatchNo(string BatchNo, BatchStatus status)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC asup_Member_Einvoice_BatchNo_U";

            var args = new
            {
                BatchNo,
                Status = status
            };
            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<int>(sql, args);
        }

        /// <summary>
        /// 取得載具Log資料By MID
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public CarrierVerifyCodeLogModel GetVerifyCodeLog(long mid)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);
            string sql = "EXEC ausp_Member_CarrierVerifyCodeLogList_S";
            var args = new
            {
                mid
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<CarrierVerifyCodeLogModel>(sql, args);
        }

        /// <summary>
        /// 新增 檢查載具資料Log
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="carrierNum"></param>
        /// <param name="verificationCode"></param>
        /// <param name="rtnCode"></param>
        /// <param name="rtnMsg"></param>
        /// <returns></returns>
        public BaseResult AddLogCarrierVerifyCode(long MID, string carrierNum, string verificationCode,
            string rtnCode = null,
            string rtnMsg = null)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);
            string sql = "EXEC ausp_Member_CarrierVerifyCode_AddLog_I";

            var args = new
            {
                MID=MID,
                CarrierNum=carrierNum,
                VerificationCode=string.IsNullOrWhiteSpace(verificationCode)?null:Encrypt_InvoiceVerification(verificationCode),
                RtnCode=rtnCode,
                RtnMsg=rtnMsg
            };
            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        #endregion

        #region CarrierTitle

        /// <summary>
        /// 新增或更新載具類型相關資料
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="carrierNum"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public BaseResult AddOrUpdateEinvoiceCarrierType(long mid, string carrierNum, string json)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC ausp_Member_AddOrUpdateEinvoiceCarrierType_SUI";

            var args = new
            {
                MID = mid,
                CarrierNum = carrierNum,
                JsonValue = json
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        ///  新增電子發票表頭下載推播表
        /// </summary>
        /// <param name="carrierNum"></param>
        /// <returns></returns>
        public BaseResult AddTitlePush(string carrierNum)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC ausp_Member_EInvoiceCarrier_AddTitlePush_IS";

            var args = new
            {
                CarrierNum = carrierNum,
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 新增載具表頭
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int InsertCarrierTitle(EinvoiceCarrierTitleModel model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC asup_Member_EinvoiceCarrierTitle_I";

            #region 參數

            var args = new
            {
                invNum=model.invNum,
                rowNum=model.rowNum,
                cardType=model.cardType,
                cardNo=model.cardNo,
                sellerName=model.sellerName,
                invStatus=model.invStatus,
                invDonatable=model.invDonatable,
                amount=model.amount,
                invPeriod=model.invPeriod,
                donateMark=model.donateMark,
                sellerBan=model.sellerBan,
                sellerAddress=model.sellerAddress,
                invoiceTime=model.invoiceTime,
                invDateyear=model.invDateyear,
                invDatemonth=model.invDatemonth,
                invDatedate=model.invDatedate,
                invDateday=model.invDateday,
                invDatehours=model.invDatehours,
                invDateminutes=model.invDateminutes,
                invDateseconds=model.invDateseconds,
                invDatetime=model.invDatetime,
                invDatetimezoneOffset=model.invDatetimezoneOffset,
                BatchNo=model.BatchNo,
                RealCardNo=model.RealCardNo,
                Currency=model.Currency
            };

            #endregion
            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<int>(sql, args);
        }

        /// <summary>
        /// 更新已處理之載具
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public BaseResult UpdateTitlePush(string json)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Share);
            string sql = "EXEC ausp_EinvoiceCarrier_UpdateTitlePush_U";

            var args = new
            {
                JsonValue = json
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 更新簽到表時間
        /// </summary>
        /// <param name="carrierNum"></param>
        /// <param name="lastDownloadDate"></param>
        /// <returns></returns>
        public int UpdateCarrierDownload(string carrierNum, DateTime? lastDownloadDate = null)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC ausp_Member_UpdateInvoiceCarrierDownload_U";

            var args = new
            {
                CarrierNum = carrierNum,
                LastDownloadDate = lastDownloadDate
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<int>(sql, args);
        }

        /// <summary>
        /// 更新載具部分資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult UpdateMemberCarrier(CellPhoneCarrierDetail model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC ausp_Member_UpdateMemberCellphoneCarrier_U";

            var args = new
            {
                MID = model.MID,
                CarrierID = model.CarrierID,
                VerificationCode = string.IsNullOrWhiteSpace(model.VerificationCode)?null:Encrypt_InvoiceVerification(model.VerificationCode),
                IsVerify = model.IsVerify
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        #endregion

        #region CarrierDetail

        /// <summary>
        /// 新增載具明細
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int InsertCarrierDetail(EinvoiceCarrierDetailModel model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC asup_Member_EinvoiceCarrierDetail_I";

            var args = new
            {
                #region 參數
                model.invNum,
                model.rowNum,
                model.description,
                model.quantity,
                model.unitPrice,
                model.amount,
                model.BatchNo,
                model.InvPeriod
                #endregion
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<int>(sql, args);
        }

        /// <summary>
        /// 新增載具發票資料
        /// </summary>
        /// <param name="detailJson"></param>
        /// <returns></returns>
        public int InsertEinvoiceByCarrier(EinvoiceByCarrierModel model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC asup_Member_EinvoiceByCarrier_I";

            var args = new
            {
                model.EinvoiceItemDetail,
                model.InvNum,
                model.EinvoiceCreateDate,
                model.InvPeriod
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<int>(sql, args);
        }

        /// <summary>
        /// 更新 EinvoiceByCarrier 發票明細
        /// </summary>
        /// <param name="invPeriod"></param>
        /// <param name="invNum"></param>
        /// <param name="detailJson"></param>
        public int UpdateEinvoiceByCarrier(string invPeriod, string invNum, string detailJson)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC ausp_Member_EinvoiceCarrierDetail_U";

            var args = new
            {
                EinvoiceItemDetail = detailJson,
                InvNum = invNum,
                InvPeriod = invPeriod
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<int>(sql, args);
        }

        /// <summary>
        /// 更新載具發票狀態
        /// </summary>
        /// <param name="carrierNum"></param>
        /// <param name="einvoicePeriod"></param>
        /// <param name="einvoiceNum"></param>
        /// <param name="einvoiceStatusType"></param>
        /// <returns></returns>
        public BaseResult UpdateEinvoiceByCarrierStatus(string carrierNum, string einvoicePeriod, string einvoiceNum,
            EinvoiceStatusType einvoiceStatusType)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC ausp_Member_UpdateMemberEinvoiceByCarrierStatus_U";

            var args = new
            {
                CarrierNum = carrierNum,
                EinvoicePeriod = einvoicePeriod,
                EinvoiceNum = einvoiceNum,
                EinvoiceStatus = (int)einvoiceStatusType
            };
            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        #endregion

        #region 載具歸戶

        /// <summary>
        /// 取得電子發票會員載具
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public GetEInvoiceCarrierInfoResultType GetEInvoiceCarrierInfoByICP(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            GetEInvoiceCarrierInfoResult result = new GetEInvoiceCarrierInfoResult();
            string sql = "EXEC ausp_Member_CellphoneCarrier_GetEInvoiceCarrierInfo_S";

            var args = new
            {
                MID,
                GetCarrierType=2
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<GetEInvoiceCarrierInfoResultType>(sql, args);
        }

        /// <summary>
        /// 驗證是否可歸戶並回傳歸戶資訊
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="carruerNum"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public InvoiceBindReturn GetInvoiceBindToken(long mid, string carruerNum, string token)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC ausp_Member_UpdateCarruerToken_SIU";

            var args = new
            {
                MID=mid,
                CarruerNum=carruerNum,
                token=token
            };
            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<InvoiceBindReturn>(sql, args);
        }

        #endregion

        #region Tools

        /// <summary>
        /// 反序列化發票明細商品
        /// </summary>
        /// <param name="EinvoiceItemDetail"></param>
        /// <returns></returns>
        public List<EinvoiceItemDetail> deserializeEinvoiceItem(string einvoiceItemDetail)
        {
            int i = 0;
            List<InvDetailItemModel> invDetail = new List<InvDetailItemModel>();
            List<EinvoiceItemDetail> list = new List<EinvoiceItemDetail>();

            einvoiceItemDetail.TryParseJsonToObj(out invDetail);
            
            foreach (var item in invDetail)
            {
                item.unitPrice = item.unitPrice ?? "0";
                EinvoiceItemDetail detail = new EinvoiceItemDetail
                {
                    description = item.description,
                    quantity = Int32.TryParse(item.quantity,out i)?i:i,
                    unitPrice = Int32.TryParse(item.unitPrice,out i)?i:i,//避免小數點問題?
                    amount =0
                };
                #region 小計四捨五入取小數點第一位

                //預設值
                item.amount = "0";

                decimal unitPrice = 0;
                decimal quantity = 0;
                if (decimal.TryParse(item.unitPrice, out unitPrice) &&
                    decimal.TryParse(item.quantity, out quantity))
                {
                    decimal amount = Math.Round((unitPrice * quantity), 1, MidpointRounding.AwayFromZero);
                    detail.amount = Convert.ToInt32(amount);
                }
                #endregion

                list.Add(detail);
            }

            return list;
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
        #endregion
    }
}