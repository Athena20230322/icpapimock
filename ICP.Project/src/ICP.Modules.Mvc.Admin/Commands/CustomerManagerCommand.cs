using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Commands
{
    using AutoMapper;
    using ICP.Infrastructure.Core.Extensions;
    using ICP.Library.Models.MemberModels;
    using ICP.Library.Services.MemberServices;
    using ICP.Modules.Mvc.Admin.Models.CustomerManager;
    using ICP.Modules.Mvc.Admin.Models.ViewModels;
    using Infrastructure.Core.Models;
    using Models;
    using Services;
    using System.Web;

    public class CustomerManagerCommand
    {
        CustomerManagerService _customerManagerService;
        LibMemberInfoService _libMemberInfoService;
        LibtMemberNotifyMessageService _libtMemberNotifyMessageService;
        LibMemberBankService _libMemberBankService;

        public CustomerManagerCommand(CustomerManagerService customerManagerService, LibMemberInfoService libMemberInfoService, LibtMemberNotifyMessageService libtMemberNotifyMessageService, LibMemberBankService libMemberBankService)
        {
            _customerManagerService = customerManagerService;
            _libMemberInfoService = libMemberInfoService;
            _libtMemberNotifyMessageService = libtMemberNotifyMessageService;
            _libMemberBankService = libMemberBankService;
        }

        public List<QueryMemberResultVM> QueryMember(QueryMemberVM queryMember)
        {
            int TotalCount = 0;
            var list = _customerManagerService.ListMember(queryMember, ref TotalCount);

            return list;
        }

        public BaseResult vaildQueryMember(QueryMemberVM queryMember)
        {
            var result = new BaseResult {
                RtnCode = 1,
                RtnMsg = "成功"
            };           

            if (queryMember == null)
            {
                result.RtnCode = 0 ;
                result.RtnMsg = "請輸入至少一種搜尋條件";
                return result;
            }

            return result;
        }

        public MemberDataModel GetMemberDetail(long id)
        {
            var result = _libMemberInfoService.GetMemberData(id);
            return result;
        }

        public string GetLevelIDName(int LevelID)
        {
            // 會員等級: 10 尚未註冊完成會員  12: 一類使用者 13 : 二類使用者  31: 特店
            string LevelTypeName = string.Empty;            
            
            switch (LevelID) 
            {
                case 10:
                    LevelTypeName = "尚未註冊完成會員";
                    break;                
                case 12:
                    LevelTypeName = "一類使用者";
                    break;
                case 13:
                    LevelTypeName = "二類使用者";
                    break;                
                case 31:
                    LevelTypeName = "特店會員";
                    break;
            }

            return LevelTypeName;
        }


        public List<MemberBankInfo> ListMemberOnBankAccount(long MID)
        {
            var result = _libMemberInfoService.ListMemberBankInfo(MID); 
            return result;
        }


        //public DateTime GetP33VerifyDate(long MID, string IDNO)
        //{
        //    var result = _customerManagerService.GetP33VerifyDate(MID, IDNO);
        //    return result;
        //}

        public MemberVerifyStatus GetMemberVerifyStatus(long MID)
        {
            var result = _customerManagerService.GetMemberVerifyStatus(MID);
            return result;
        }        

        public int GetOTPCount(string CellPhone)
        {
            var result = _customerManagerService.GetOTPCount(CellPhone);
            return result;
        }


        public MemberAuthIDNOVM GetMemberAuthIDNO(long MID, int exceptOverSea = 0)
        {
            var result = _customerManagerService.GetMemberAuthIDNO(MID, exceptOverSea);
            return result;
        }


        public List<AuthCNameListLogVM> ListAuthCNameListLog(long MID)
        {
            var result = _customerManagerService.ListAuthCNameListLog(MID);
            return result;
        }

        public List<AuthCellPhoneListLogVM> ListAuthCellPhoneListLog(long MID)
        {
            var result = _customerManagerService.ListAuthCellPhoneListLog(MID);
            return result;
        }

        public List<AuthMemberUpgradeListLogVM> ListMemberUpgradeListLog(long MID)
        {
            var result = _customerManagerService.ListMemberUpgradeListLog(MID);
            return result;
        }

        public UnLockSMSVM GetUnLockSMSData(long id)
        {
            var result = _customerManagerService.GetUnLockSMSData(id);
            return result;
        }

        public BaseResult UpdateUnLockSMS(long id, string ModifyUser)
        {
            var result = _customerManagerService.UpdateUnLockSMS(id, ModifyUser);
            return result;
        }

        public EditCellPhoneModel GetCellPhoneData(long id)
        {
            var MemberData = _libMemberInfoService.GetMemberData(id).detail;
            
            var result = new EditCellPhoneModel {
                CellPhone = MemberData.CellPhone,
                MID = MemberData.MID
            };

            return result;
        }


        public BaseResult UpdateCellPhone(EditCellPhoneModel model)
        {
            var result = _customerManagerService.UpdateCellPhone(model);
            return result;
        }

        public List<MemberNotifyMessageModel> MemberNotifyMessage(long id, ref int TotalCount, ref PageModel pageModel, byte? Status = 1)
        {         
            var result = _libtMemberNotifyMessageService.ListNotifyMessage(id, pageModel.PageNo, pageModel.PageSize, Status);

            var list = Mapper.Map<List<MemberNotifyMessageModel>>(result.Items);

            list.ForEach(t => { t.TotalCount = result.TotalCount; t.MID = id ; });

            if (list.Count == 0)
            {
                TotalCount = 0;
            }
            else
            {
                TotalCount = list[0].TotalCount;
            }

            return list;
        }


        public DataResult<MemberNotifyMessageDetail> MemberNotifyMessageDetail(long NotifyMessageID, long? id = null, byte? Status = 1)
        {
            var result = _libtMemberNotifyMessageService.GetNotifyMessage(NotifyMessageID, id, Status);
            
            return result;
        }


        public AuthFinancialResult GetAuthFinancial(long MID)
        {
            var result = _libMemberBankService.GetAuthFinancial(MID);
            return result;
        }

        #region 凍結款項相關

        #region 凍結款項列表
        public List<FreezeCoinsModel> ListFreezeCoins(QueryFreezeCoinsModel query)
        {
            int TotalCount = 0;
            var result = _customerManagerService.ListFreezeCoins(query, ref TotalCount);
            return result;
        }
        #endregion

        #region 凍結款項明細
        public List<FreezeCoinsModel> ListFreezeCoinsLog(long MID, long FreezeID)
        {
            var result = _customerManagerService.ListFreezeCoinsLog(MID, FreezeID);
            return result;
        }
        #endregion

        #region 新增凍結餘額資料
        public BaseResult AddFreezeCoins(AddFreezeCoinsModel model)
        {
            var result = _customerManagerService.AddFreezeCoins(model);
            return result;
        }
        #endregion

        #region 查詢帳戶金額
        public decimal GetUserCoinsOnBalanceByType(long MID, int Type = 0)
        {
            var result = _customerManagerService.GetUserCoinsOnBalanceByType(MID, Type);
            return result;
        }

        public DataResult<AddFreezeCoinsModel> GetUserCoinsOnBalanceData(long MID, int Type = 0)
        {
            return _customerManagerService.GetUserCoinsOnBalanceData(MID, Type);
        }
        #endregion

        #region 返還凍結餘額
        public BaseResult ReturnFreezeCoins(ReturnFreezeCoinsModel model)
        {
            var result = _customerManagerService.ReturnFreezeCoins(model);
            return result;
        }
        #endregion

        #region 解除凍結餘額
        public BaseResult ReleaseFreezeCoins(ReleaseFreezeCoinsModel model)
        {
            var result = _customerManagerService.ReleaseFreezeCoins(model);
            return result;
        }
        #endregion

        #endregion

        #region 匯入海外會員相關

        #region 海外會員資料列表
        public List<ListOverSeaUserResultModel> ListOverSeaUser(ListOverSeaUserQryVM model = null)
        {
            var result = _customerManagerService.ListOverSeaUser(model);
            return result;
        }
        #endregion

        #region 檢查匯入的檔案
        public BaseResult VaildCSVFile(HttpPostedFileBase fileData)
        {
            BaseResult result = new BaseResult();

            result = _customerManagerService.VaildCSVFile(fileData);

            return result;
            
        }

        public List<UploadOverSeaResultModel> UploadOverSeaFile(string FilePath, string Creator, long RealIP, long ProxyIP, ref int SuccessCounts, ref int FailCounts)
        {
            var result = _customerManagerService.UploadOverSeaFile(FilePath, Creator, RealIP, ProxyIP, ref SuccessCounts, ref FailCounts);
            return result;
        }
        #endregion




        //#region 檢查匯入CSV檔
        //public BaseResult UploadOverSeaFile(HttpPostedFileBase fileData, string Creator, long RealIP, long ProxyIP)
        //{
        //    var result = _customerManagerService.UploadOverSeaFile(fileData, Creator, RealIP, ProxyIP);
        //    return result;
        //}
        //#endregion

        #region 匯入資料建立帳號
        public BaseResult AddOverSeaMemberData(AddOverSeaMemberDataModel model)
        {
            var result = _customerManagerService.AddOverSeaMemberData(model);
            return result;
        }
        #endregion

        #region 驗證圖檔名稱、格式
        public BaseResult ValidFile(HttpPostedFileBase file, string Creator)
        {
            var result = _customerManagerService.ValidFile(file, Creator);           

            return result;
        }
        #endregion

        #region 上傳證件圖檔
        public BaseResult OverSeaFileUpload(OverSeaFileUploadModel model)
        {
            var result = new BaseResult();
            result = _customerManagerService.OverSeaFileUpload(model);
            return result;
        }
        #endregion

        //#region 上傳身分證/居留證正反面
        //public BaseResult UploadOverSeaImages(HttpPostedFileBase file, string Creator, OverSeaFileUploadModel model)
        //{
        //    BaseResult result = new BaseResult();
        //    List<string> realFilePathes = new List<string>();
        //    var uploadFilesResult = _customerManagerService.UploadOverSeaImages(file, ref realFilePathes);
        //    if (!uploadFilesResult.IsSuccess)
        //    {                
        //        result.SetError(uploadFilesResult);
        //        return result;
        //    }

        //    result.SetSuccess();
        //    return result;
        //}
        //#endregion

        #region 身份驗證確認
        public BaseResult UpdateUniformIDStatus(UpdateUniformIDStatusModel model)
        {
            var result = _customerManagerService.UpdateUniformIDStatus(model);
            return result;
        }
        #endregion

        #region 發送金融支付工具驗證請求(新增一元驗證資料) 
        public BaseResult AddBankTransfer(long MID)
        {
            var memberBankInfo = _libMemberBankService.ListMemberBankInfo(MID).First();

            ICP.Library.Models.MemberModels.MemberBankAccount bankAccount = new ICP.Library.Models.MemberModels.MemberBankAccount
            {
                MID = MID,
                BankCode = memberBankInfo.BankCode,
                BankAccount = memberBankInfo.BankAccount
            };

            var result = _libMemberBankService.AddBankTransferOnBankAccount(bankAccount);

            return result;
        }
        #endregion

        #endregion



    }
}
