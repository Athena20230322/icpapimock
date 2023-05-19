using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Services
{
    using Infrastructure.Core.Models;
    using Modules.Mvc.Admin.Repositories;
    using Models;
    using ICP.Modules.Mvc.Admin.Models.ViewModels;
    using ICP.Library.Repositories.MemberRepositories;
    using ICP.Modules.Mvc.Admin.Models.CustomerManager;
    using ICP.Library.Services.MemberServices;
    using ICP.Infrastructure.Core.Extensions;
    using System.Web;
    using System.IO;
    using ICP.Library.Services.AdminServices;
    using ICP.Library.Models.MemberModels;
    using ICP.Infrastructure.Core.Models.Consts;
    using System.Text.RegularExpressions;
    using ICP.Library.Repositories;

    public class CustomerManagerService
    {
        CustomerManagerRepository _customerManagerRepository;
        MemberConfigCyptRepository _configCyptRepository;
        LibMemberInfoService _libMemberInfoService;
        Library.Repositories.MemberRepositories.MemberConfigRepository _configRepository;
        Library.Repositories.MemberRepositories.MemberAuthRepository _memberAuthRepository;
        LibAdminService _libAdminService;
        LibPersonalAuthService _libPersonalAuthService;
        LibMemberBankService _libMemberBankService;
        CommonRepository _commonRepository;

        public CustomerManagerService(
            CustomerManagerRepository customerManagerRepository, 
            MemberConfigCyptRepository configCyptRepository, 
            LibMemberInfoService libMemberInfoService,
            Library.Repositories.MemberRepositories.MemberConfigRepository configRepository,
            Library.Repositories.MemberRepositories.MemberAuthRepository memberAuthRepository,
            LibAdminService libAdminService,
            LibPersonalAuthService libPersonalAuthService,
            LibMemberBankService libMemberBankService,
            CommonRepository commonRepository
            )
        {
            _customerManagerRepository = customerManagerRepository;
            _configCyptRepository = configCyptRepository;
            _libMemberInfoService = libMemberInfoService;
            _configRepository = configRepository;
            _memberAuthRepository = memberAuthRepository;
            _libAdminService = libAdminService;
            _libPersonalAuthService = libPersonalAuthService;
            _libMemberBankService = libMemberBankService;
            _commonRepository = commonRepository;
        }

        public List<QueryMemberResultVM> ListMember(QueryMemberVM queryMember, ref int TotalCount)
        {
            //帳號加密
            
            queryMember.Account = (queryMember.Account!=null) ? _configCyptRepository.Encrypt_UserCode(queryMember.Account): queryMember.Account;

            var list = _customerManagerRepository.ListMember(queryMember);

            //解密帳號
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Account = _configCyptRepository.Decrypt_UserCode(list[i].Account);
            }


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


        //public List<MemberBankAccountVM> ListMemberOnBankAccount(long MID)
        //{
        //    var result = _customerManagerRepository.ListMemberOnBankAccount(MID);

        //    return result;
        //}

        //public DateTime GetP33VerifyDate(long MID, string IDNO)
        //{
        //    var result = _customerManagerRepository.GetP33VerifyDate(MID, IDNO);
        //    return result;
        //}

        public MemberVerifyStatus GetMemberVerifyStatus(long MID)
        {
            return _customerManagerRepository.GetMemberVerifyStatus(MID);
        }

        public int GetOTPCount(string CellPhone)
        {
            var result = _customerManagerRepository.GetOTPCount(CellPhone);
            return result;
        }

        public MemberAuthIDNOVM GetMemberAuthIDNO(long MID, int exceptOverSea = 0)
        {
            var result = _customerManagerRepository.GetMemberAuthIDNO(MID, exceptOverSea);
            return result;
        }

        public List<AuthCNameListLogVM> ListAuthCNameListLog(long MID)
        {
            var result = _customerManagerRepository.ListAuthCNameListLog(MID);
            return result;
        }

        public List<AuthCellPhoneListLogVM> ListAuthCellPhoneListLog(long MID)
        {
            var result = _customerManagerRepository.ListAuthCellPhoneListLog(MID);
            return result;
        }

        public List<AuthMemberUpgradeListLogVM> ListMemberUpgradeListLog(long MID)
        {
            var result = _customerManagerRepository.ListMemberUpgradeListLog(MID);
            return result;
        }


        public UnLockSMSVM GetUnLockSMSData(long MID)
        {
            var result = new UnLockSMSVM();
            string CellPhone = _libMemberInfoService.GetMemberData(MID).detail.CellPhone;
            result.CellPhone = CellPhone;
            result.OTPCounts = _customerManagerRepository.GetOTPCount(CellPhone);
            result.UnLockSMSLogs = _customerManagerRepository.ListUnlockSMSListLog(MID);            
            return result;
        }

        public BaseResult UpdateUnLockSMS(long id, string ModifyUser)
        {
            var result = _customerManagerRepository.UpdateUnLockSMS(id, ModifyUser);
            return result;
        }

        public BaseResult UpdateCellPhone(EditCellPhoneModel model)
        {
            var result = _customerManagerRepository.UpdateCellPhone(model);
            return result;
        }

        #region 凍結款項相關

        #region 凍結款項列表
        public List<FreezeCoinsModel> ListFreezeCoins(QueryFreezeCoinsModel query, ref int TotalCount)
        {
            var result = _customerManagerRepository.ListFreezeCoins(query);

            if (result.Count == 0)
            {
                TotalCount = 0;
            }
            else
            {
                TotalCount = result[0].TotalCount;
            }

            return result;
        }
        #endregion

        #region 凍結款項明細
        public List<FreezeCoinsModel> ListFreezeCoinsLog(long MID, long FreezeID)
        {
            var result = _customerManagerRepository.ListFreezeCoinsLog(MID, FreezeID);
            return result;
        }
        #endregion

        #region 新增凍結餘額資料
        public BaseResult AddFreezeCoins(AddFreezeCoinsModel model)
        {
            var result = _customerManagerRepository.AddFreezeCoins(model);
            return result;
        }
        #endregion

        #region 查詢帳戶金額
        public decimal GetUserCoinsOnBalanceByType(long MID, int Type = 0)
        {
            var result = _customerManagerRepository.GetUserCoinsOnBalanceByType(MID, Type);
            return result;
        }

        public DataResult<AddFreezeCoinsModel> GetUserCoinsOnBalanceData(long MID, int Type = 0)
        {
            var result = new DataResult<AddFreezeCoinsModel>();
            result.SetError();
            decimal TotalCash = GetUserCoinsOnBalanceByType(MID, 0);

            result.RtnData = new AddFreezeCoinsModel
            {
                TotalCash = TotalCash,
                MID = MID,
            };

            if (result.RtnData == null)
            {
                return result;
            }

            result.SetSuccess(result.RtnData);
            return result;
        }
        #endregion

        #region 返還凍結金額
        public BaseResult ReturnFreezeCoins(ReturnFreezeCoinsModel model)
        {
            FreezeCoinsModel dataModel = new FreezeCoinsModel
            {
                MID = model.MID,
                Remark = model.Remark,
                Status = model.Status,
                FreezeID = model.FreezeID,
                Creator = model.Creator,
                FreezeCash = model.FreezeCash,
                RtnICPMID = model.RtnICPMID
            };

            var result = _customerManagerRepository.UpdateFreezeCoinsStatus(dataModel);
            return result;
        }
        #endregion

        #region 解除凍結金額
        public BaseResult ReleaseFreezeCoins(ReleaseFreezeCoinsModel model)
        {
            FreezeCoinsModel dataModel = new FreezeCoinsModel
            {
                MID = model.MID,
                Remark = model.Remark,
                Status = model.Status,
                FreezeID = model.FreezeID,
                Creator = model.Creator,
                FreezeCash = model.FreezeCash,
                RtnICPMID = model.RtnICPMID
            };

            var result = _customerManagerRepository.UpdateFreezeCoinsStatus(dataModel);
            return result;
        }
        #endregion

        #endregion


        #region 匯入海外會員相關

        #region 海外會員資料列表
        public List<ListOverSeaUserResultModel> ListOverSeaUser(ListOverSeaUserQryVM model = null)
        {
            var result = _customerManagerRepository.ListOverSeaUser(model);
            return result;
        }
        #endregion


        public BaseResult VaildCSVFile(HttpPostedFileBase fileData)
        {
            BaseResult result = new BaseResult();
            //### 上傳檔案限制
            int fileSize = 5;
            string[] contentTypeArray = new string[] { "application/vnd.ms-excel" };
            string OverSeaPath = _configRepository.OverSeaPath;
            string fileDirPath = System.Web.HttpContext.Current.Server.MapPath(OverSeaPath) + string.Format("{0:yyyyMM}", DateTime.Now) + "\\";
            string realFilePath = "";
            string FileName = "";

            //### 上傳檔案
            result = UploadFile(fileData, contentTypeArray, fileSize, fileDirPath, OverSeaPath, ref realFilePath);

            if (result.RtnCode != 1)
            {
                return result;
            }

            FileName = realFilePath.Split('/').Last();

            result.RtnMsg = System.Web.HttpContext.Current.Server.MapPath(OverSeaPath + string.Format("{0:yyyyMM}", DateTime.Now) + "/" + FileName);

            return result;

        }

        public List<UploadOverSeaResultModel> UploadOverSeaFile(string FilePath, string Creator, long RealIP, long ProxyIP, ref int SuccessCounts, ref int FailCounts)
        { 
            FileInfo theFileInfo;
            
            theFileInfo = new FileInfo(FilePath);

            string line = string.Empty;
            string LastRecord = string.Empty;

            //## 設定Big5 編碼 讀取檔案
            System.Text.Encoding encode = System.Text.Encoding.GetEncoding("big5");
            StreamReader sr = new StreamReader(theFileInfo.FullName, encode);

            //先將隱藏的符號過濾
            string txtcontent = sr.ReadToEnd().Replace("\n", "");

            string[] d = txtcontent.Split('\r');

            List<UploadOverSeaResultModel> FailData = new List<UploadOverSeaResultModel>();

            int _successCounts = 0;
            int _failCounts = 0;
            string _batchNo = DateTime.Now.ToString("yyyyMMddHHmmss");

            try
            {
                //第一筆資料視為欄位說明，需過濾掉
                bool isFirstRow = true;
                foreach (var item in d)
                {
                    if (isFirstRow) { isFirstRow = false; continue; }
                    int resultInt = 0;
                    DateTime resultDT = new DateTime();
                    string _errorMsg = string.Empty;
                    if (item.Length > 0)
                    {                        
                        AddOverSeaMemberDataModel model = new AddOverSeaMemberDataModel();//str.Replace("\n",string.Empty);
                        string[] dataLine = item.Split(',');

                        //解析資料                        
                        //檔案欄位：手機號碼, 個人姓名, 國籍代碼, 郵遞區號, 地址, OPMID, 電子郵件, 居留證字號, 證號核發日期, 居留期限, 背面流水號, 銀行代號, 分行代碼, 銀行帳號
                        if (dataLine[0] != "") { model.CellPhone = dataLine[0]; }
                        if (dataLine[1] != "") { model.CName = dataLine[1]; }
                        if (dataLine[2] != "")
                        {
                            if (!int.TryParse(dataLine[2], out resultInt))
                            {
                                _errorMsg += "國籍格式有誤、";                                
                            }
                            else
                            {
                                model.NationalID = Convert.ToInt64(dataLine[2]);
                            }                            
                        }
                        if (dataLine[3] != "")
                        {
                            model.ZipCode = dataLine[3];
                            // 取得AreaID
                            CountryTownIDModel countryTown = _customerManagerRepository.GetAreaID(model.ZipCode);

                            if (countryTown != null)
                            {
                                model.AreaID = countryTown.TownID;
                            }
                            else
                            {
                                _errorMsg += "郵遞區號格式有誤、";                                
                            }                            
                        }
                        if (dataLine[4] != "") { model.Address = dataLine[4]; }
                        if (dataLine[5] != "") { model.OPMID = dataLine[5]; }
                        if (dataLine[6] != "") { model.Email = dataLine[6]; }
                        if (dataLine[7] != "")
                        {   
                            string reg = RegexConst.UniformID;
                            Regex regHtml = new Regex(reg);
                            Match mHtml = regHtml.Match(dataLine[7]);

                            if (!mHtml.Success)
                                _errorMsg += "居留證字號格式有誤、";
                            else
                            {
                                model.UniformID = dataLine[7];
                            }                            
                        }
                        if (dataLine[8] != "")
                        {
                            if (!DateTime.TryParse(dataLine[8], out resultDT))
                            {
                                _errorMsg += "居留證號核發日期格式有誤、";                                
                            }
                            else
                            {
                                model.UniformIssueDate = Convert.ToDateTime(dataLine[8]);
                            }                            
                        }
                        if (dataLine[9] != "")
                        {
                            if (!DateTime.TryParse(dataLine[9], out resultDT))
                            {
                                _errorMsg += "居留期限格式有誤、";                                
                            }
                            else
                            {
                                model.UniformExpireDate = Convert.ToDateTime(dataLine[9]);
                            }                            
                        }
                        if (dataLine[10] != "") { model.UniformNumber = dataLine[10]; }
                        if (dataLine[11] != "") { model.BankCode = dataLine[11]; }
                        if (dataLine[12] != "") { model.BankBranchCode = dataLine[12]; }
                        if (dataLine[13] != "") { model.BankAccount = dataLine[13].Trim(';'); }
                                                
                        model.CreateUser = Creator;
                        model.ProxyIP = ProxyIP;
                        model.RealIP = RealIP;

                        //資料型態有問題的不寫資料庫
                        if (!string.IsNullOrEmpty(_errorMsg))
                        {
                            UploadOverSeaResultModel failList = new UploadOverSeaResultModel
                            {
                                UniformID = dataLine[7],
                                ErrorMsg = _errorMsg.Substring(0,_errorMsg.Length-1)
                            };

                            FailData.Add(failList);
                            _failCounts += 1;
                            continue;
                        }

                        

                        string account = model.UniformID.Substring(0, 1) + model.UniformID.Substring(2, 4);
                        string pwd = model.UniformID.Substring(1, 1) + model.UniformID.Substring(6, 4) + 1;

                        //居留證第一碼 + 數字前4碼 + 1 (APP檢核需要6碼)
                        model.Account = _configCyptRepository.Encrypt_UserCode(account + 1);
                        //居留證第二碼 + 數字後4碼 + 1 (APP檢核需要6碼)
                        model.Pwd = _configCyptRepository.Hash_UserPwd(pwd);
                        model.BatchNo = _batchNo;

                        //記錄匯入資料Log
                        model.Status = 0;
                        _customerManagerRepository.AddMemberForeignBasicLog(model);

                        //驗證資料格式&規則
                        if (!model.IsValid())
                        {

                            UploadOverSeaResultModel failList = new UploadOverSeaResultModel
                            {
                                UniformID = model.UniformID,
                                ErrorMsg = model.GetFirstErrorMessage()
                            };

                            //記錄匯入資料Log
                            model.Status = 2;
                            _customerManagerRepository.AddMemberForeignBasicLog(model);
                            FailData.Add(failList);
                            _failCounts += 1;
                            
                        }
                        else
                        {                            
                            var vaildResult = VaildOverSeaMemberDataModel(model);

                            //預設帳號重複處理
                            int i = 2;
                            while (vaildResult.RtnCode == -1)
                            {                                
                                model.Account = _configCyptRepository.Encrypt_UserCode(account + i);
                                vaildResult = VaildOverSeaMemberDataModel(model);
                                i++;
                            };       

                            if (vaildResult.IsSuccess)
                            {     
                                //建立會員資料
                                var addDataResult = _customerManagerRepository.AddOverSeaMemberData(model);
                                if (addDataResult.RtnCode != 1)
                                {
                                    UploadOverSeaResultModel failList = new UploadOverSeaResultModel
                                    {
                                        UniformID = model.UniformID,
                                        ErrorMsg = addDataResult.RtnMsg
                                    };
                                    
                                    //記錄匯入資料Log
                                    model.Status = 2;
                                    _customerManagerRepository.AddMemberForeignBasicLog(model);
                                    FailData.Add(failList);
                                    _failCounts += 1;
                                }
                                else
                                {
                                    long MID = Convert.ToInt64(addDataResult.RtnMsg);
                                    model.MID = MID;
                                    var result = new BaseResult();
                                    // 新增 P11.P33 驗證
                                    #region 查功能開關
                                    string appName = "member";
                                    var appFunctionSwitchs = _libAdminService.ListAppFunctionSwitch(appName);
                                    var query = appFunctionSwitchs.Where(t => (t.FunctionID == 1) && t.Status == 0).ToList();
                                    if (query.Any())
                                    {
                                        UploadOverSeaResultModel failList = new UploadOverSeaResultModel
                                        {
                                            UniformID = model.UniformID,
                                            ErrorMsg = result.RtnMsg
                                        };                                 

                                        //記錄匯入資料Log
                                        model.Status = 2;
                                        _customerManagerRepository.AddMemberForeignBasicLog(model);
                                        FailData.Add(failList);
                                        _failCounts += 1;
                                    }                                    
                                    #endregion

                                    #region P33 驗證
                                    var p33Auth = new P33Auth
                                    {
                                        MID = MID,
                                        IDNO = model.UniformID,
                                        Source = 2,
                                        RealIP = RealIP,
                                        ProxyIP = ProxyIP
                                    };
                                    var p33Result = _libPersonalAuthService.AddAuthP33(p33Auth);

                                    if (!p33Result.IsSuccess)
                                    {        
                                        UploadOverSeaResultModel failList = new UploadOverSeaResultModel
                                        {
                                            UniformID = model.UniformID,
                                            ErrorMsg = p33Result.RtnMsg
                                        };
                                            
                                        //記錄匯入資料Log
                                        model.Status = 2;
                                        _customerManagerRepository.AddMemberForeignBasicLog(model);
                                        FailData.Add(failList);
                                        _failCounts += 1;

                                    }
                                    #endregion

                                    #region P11 驗證 (海外會員直接塞資料不需驗證P11)
                                    var p11Auth = new AddOverSeaP11Model
                                    {
                                        MID = MID,
                                        UniformID = model.UniformID,
                                        UniformExpireDate = model.UniformExpireDate,
                                        UniformIssueDate = model.UniformIssueDate,
                                        UniformNumber = model.UniformNumber,
                                        AuthType = 2,
                                        FilePath1 = null,
                                        FilePath2 = null,
                                        Source = 1,
                                        RealIP = RealIP,
                                        ProxyIP = ProxyIP,
                                        Modifier = Creator
                                    };
                                    var p11Result = _customerManagerRepository.AddOverSeaMemberP11(p11Auth);

                                    if (!p11Result.IsSuccess)
                                    {
                                        UploadOverSeaResultModel failList = new UploadOverSeaResultModel
                                        {
                                            UniformID = model.UniformID,
                                            ErrorMsg = p11Result.RtnMsg
                                        };
                                                                                
                                        //記錄匯入資料Log
                                        model.Status = 2;
                                        _customerManagerRepository.AddMemberForeignBasicLog(model);
                                        FailData.Add(failList);
                                        _failCounts += 1;
                                    }
                                    #endregion

                                    #region 新增銀行帳號資料
                                    ICP.Library.Models.MemberModels.MemberBankAccount BankModel = new ICP.Library.Models.MemberModels.MemberBankAccount {
                                        MID = MID,
                                        Category = 0,
                                        BankCode = model.BankCode,
                                        BankBranchCode = model.BankBranchCode,
                                        BankAccount = model.BankAccount,
                                        INDTAccount = null,
                                        isDefault = 1,
                                        AuthCategory = 0,
                                        AuthType = 1,
                                        PaperAuthStatus = 0,
                                        FilePath1 = null,
                                        FilePath2 = null,
                                        CreateUser = model.CreateUser,
                                        Source = 1
                                    };
                                    var addBankAccountResult = _libMemberBankService.AddMemberBankAccount(BankModel);
                                    if (addBankAccountResult.RtnCode != 1)
                                    {
                                        UploadOverSeaResultModel failList = new UploadOverSeaResultModel
                                        {
                                            UniformID = model.UniformID,
                                            ErrorMsg = addBankAccountResult.RtnMsg
                                        };
                                        
                                        //記錄匯入資料Log
                                        model.Status = 2;
                                        _customerManagerRepository.AddMemberForeignBasicLog(model);
                                        FailData.Add(failList);
                                        _failCounts += 1;
                                    }

                                    #endregion
                                                                        
                                    //記錄匯入資料Log
                                    model.Status = 1;
                                    _customerManagerRepository.AddMemberForeignBasicLog(model);
                                    _successCounts += 1;
                                }
                            }
                            else
                            {
                                // 資料有問題，記錄居留證、錯誤訊息
                                UploadOverSeaResultModel failList = new UploadOverSeaResultModel
                                {
                                    UniformID = model.UniformID,
                                    ErrorMsg = vaildResult.RtnMsg
                                };
                                
                                //記錄匯入資料Log
                                model.Status = 2;
                                _customerManagerRepository.AddMemberForeignBasicLog(model);
                                FailData.Add(failList);
                                _failCounts += 1;                                
                            }

                        }
                                             
                    }

                    
                }                

                SuccessCounts = _successCounts;
                FailCounts = _failCounts;   
                
            }
            catch (Exception ex)
            {

            }
            finally
            {
                //sr.Close();
            }

            return FailData;
        }

        

        #region 驗證匯入資料
        public BaseResult VaildOverSeaMemberDataModel(AddOverSeaMemberDataModel model)
        {
            var result = new BaseResult();
            result.SetError();

            // 檢查
            // 1. 手機號碼、OPMID、電子郵件、居留證字號、預設帳號 是否可使用
            // 2. 銀行代號、分行代碼格式，是否存在
            result = _customerManagerRepository.VaildOverSeaMemberDataModel(model);
            if (result.RtnCode != 1)
            {
                return result;
            }

            result.SetSuccess();
            return result;
        }
        #endregion


        #region 匯入資料建立帳號
        public BaseResult AddOverSeaMemberData(AddOverSeaMemberDataModel model)
        {
            var result = _customerManagerRepository.AddOverSeaMemberData(model);
            return result;
        }
        #endregion


        public BaseResult ValidFile(System.Web.HttpPostedFileBase file, string Creator)
        {
            //檢查檔案名稱
            // 1. 檔案類型
            // 2. 檔案名稱長度
            // 3. 檔案名稱是否符合 居留證號_1、居留證號_2、居留證號_3
            // 4. 居留證是否已存在DB
            // 5. 上傳圖檔
            var result = new BaseResult();
            result.SetError();

            string[] contentTypeArray = new string[] { "image/jpeg", "image/gif", "image/pjpeg", "image/png" }; //上傳檔案為jpg、png檔
            if (!contentTypeArray.Contains(file.ContentType))
            {
                result.RtnMsg = "圖片格式錯誤，請確認上傳檔案為jpg、png檔";
                return result;                
            }

            string FileName = file.FileName;
            string realFileName = FileName.Split('.')[0];
            string realFilePath = string.Empty;

            if (realFileName.Length != 12)
            {
                result.RtnMsg = FileName + "檔名規則有誤，請確認後重新上傳";
                return result;
            }

            string UniformID = realFileName.Substring(0, 10);
            string FileType = realFileName.Substring( realFileName.Length-2 , 2);
            string Type = FileType.Substring(1, 1);

            if (FileType != "_1" && FileType != "_2" && FileType != "_3")
            {
                result.RtnMsg = "檔名規則有誤，請確認後重新上傳";
                return result;
            }

            var CheckIdnoRepeat = _memberAuthRepository.CheckIdnoRepeat(UniformID, 0, true);
            if (CheckIdnoRepeat != null && CheckIdnoRepeat.CanUse != false)
            {
                result.RtnMsg = "此居留證不存在";
                return result;
            }

            var UploadImage = UploadOverSeaImages(file, ref realFilePath);
            if (UploadImage.RtnCode != 1)
            {
                return UploadImage;
            }
            else
            {
                //上傳完圖片就更新資料
                OverSeaFileUploadModel model = new OverSeaFileUploadModel
                {
                    UniformID = UniformID,
                    FilePath1 = (Type == "1") ? realFilePath : null,
                    FilePath2 = (Type == "2") ? realFilePath : null,
                    FilePath3 = (Type == "3") ? realFilePath : null,
                    Modifier = Creator
                };
                var updateResult = OverSeaFileUpload(model);
                if (updateResult.RtnCode != 1)
                {
                    return updateResult;
                }
                
            }

            result.SetSuccess();
            return result;



        }

        #region 上傳證件圖檔
        public BaseResult OverSeaFileUpload(OverSeaFileUploadModel model)
        {
            var result = _customerManagerRepository.OverSeaFileUpload(model);
            return result;
        }
        #endregion

        public BaseResult UploadOverSeaImages(System.Web.HttpPostedFileBase file, ref string realFilePath)
        {
            BaseResult result = new BaseResult();

            int fileSize = 5;

            string[] contentTypeArray = new string[] { "image/jpeg", "image/gif", "image/pjpeg", "image/png" };

            string FileName = file.FileName;
            string realFileName = FileName.Split('.')[0];
            string BankPath = _configRepository.Path_BankAccount;
            string IDNOPath = _configRepository.IDNOPath;
            string FileType = realFileName.Substring(realFileName.Length - 2, 2);
            int Type = Convert.ToInt32( FileType.Substring(1, 1) );
            string fileDirPath = string.Empty;
            string savePath = (Type == 1 || Type == 2) ? (_configRepository.IDNOPath + string.Format("{0:yyyyMM}", DateTime.Now) + "/") : (_configRepository.Path_BankAccount + string.Format("{0:yyyyMM}", DateTime.Now) + "/"); 
            fileDirPath = (Type==1 || Type==2)?(System.Web.HttpContext.Current.Server.MapPath(IDNOPath) + string.Format("{0:yyyyMM}", DateTime.Now) + "/"):(System.Web.HttpContext.Current.Server.MapPath(BankPath) + string.Format("{0:yyyyMM}", DateTime.Now) + "/") ;
            
            result = UploadFileImg(file, contentTypeArray, fileSize, fileDirPath, savePath, ref realFilePath);
                     
            return result;
        }

        private BaseResult UploadFileImg(System.Web.HttpPostedFileBase file, string[] contentTypeArray, int fileSize, string fileDirPath, string savePath, ref string realFilePath)
        {           

            var result = new BaseResult();
            result.SetError();

            string saveFilePath = savePath;
            string realFileName = Guid.NewGuid().ToString() + DateTime.Now.ToString("_yyyyMMddHHmmss") + Path.GetExtension(file.FileName);
                       
            realFilePath = Path.Combine(fileDirPath, realFileName);
                        
            if (!Directory.Exists(fileDirPath))
            {
                Directory.CreateDirectory(fileDirPath);
            }

            file.SaveAs(realFilePath);

            realFilePath = saveFilePath + realFileName;

            return new BaseResult
            {
                RtnCode = 1,
                RtnMsg = "成功"
            };
        }


        #region 身份驗證確認
        public BaseResult UpdateUniformIDStatus(UpdateUniformIDStatusModel model)
        {
            var result = _customerManagerRepository.UpdateUniformIDStatus(model);
            return result;
        }
        #endregion


        /// <summary>
        /// 上傳檔案
        /// </summary>
        /// <param name="files"></param>
        /// <param name="realFilePathes"></param>
        /// <returns></returns>
        public BaseResult UploadAuthIDNOFiles(System.Web.HttpPostedFileBase[] files, ref List<string> realFilePathes)
        {
            BaseResult result = new BaseResult();

            int fileSize = 5;

            string[] contentTypeArray = new string[] { "image/jpeg", "image/gif", "image/pjpeg", "image/png" };

            string IDNOPath = _configRepository.IDNOPath;

            string fileDirPath = System.Web.HttpContext.Current.Server.MapPath(IDNOPath) + string.Format("{0:yyyyMM}", DateTime.Now) + "/";

            foreach (var file in files)
            {
                string realFilePath = string.Empty;

                result = UploadFile(file, contentTypeArray, fileSize, fileDirPath, IDNOPath, ref realFilePath);
                realFilePathes.Add(realFilePath);
            }

            return result;
        }

        private BaseResult UploadFile(System.Web.HttpPostedFileBase file, string[] contentTypeArray, int fileSize, string fileDirPath, string configPath, ref string realFilePaths)
        {
            fileSize = 1024 * 1024 * fileSize;

            if (!contentTypeArray.Contains(file.ContentType))
            {
                return new BaseResult
                {
                    RtnCode = 0,
                    RtnMsg = string.Format("無法上傳檔案，檔案格式不接受：{0}", file.FileName)
                };
            }

            if (file.ContentLength > fileSize)
            {
                return new BaseResult
                {
                    RtnCode = 0,
                    RtnMsg = string.Format("無法上傳檔案，檔案大小限制{0}MB以下", fileSize)
                };
            }

            string realFileName = Guid.NewGuid().ToString() + DateTime.Now.ToString("_yyyyMMddHHmmss") + Path.GetExtension(file.FileName);
                        
            realFilePaths = Path.Combine(configPath, realFileName);

            if (!Directory.Exists(fileDirPath))
            {
                Directory.CreateDirectory(fileDirPath);
            }

            file.SaveAs(Path.Combine(fileDirPath, realFileName));

            return new BaseResult
            {
                RtnCode = 1,
                RtnMsg = "成功"
            };
        }
                      

        #endregion
    }
}
