using AutoMapper;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Utils;
using ICP.Modules.Mvc.Admin.Models.Announcement;
using ICP.Modules.Mvc.Admin.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace ICP.Modules.Mvc.Admin.Services
{
    public class AnnouncementService
    {
        AnnouncementRepository _announcementRepository;

        public AnnouncementService(AnnouncementRepository announcementRepository)
        {
            _announcementRepository = announcementRepository;
        }

        /// <summary>
        /// 訊息公告清單
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<ListAnnounceDbRes> ListContent(ListAnnounceDbReq req)
        {
            return _announcementRepository.ListContent(req);
        }

        /// <summary>
        /// 公告類別清單
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<ListCategoryDbRes> ListCategory(ListCategoryDbReq req)
        {
            return _announcementRepository.ListCategory(req);
        }

        /// <summary>
        /// 設定上傳檔案URL&絕對路徑
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public object SetFileUrl(string path)
        {
            object obj = new
            {
                url = GlobalConfigUtil.Host_Member_Domain + path.TrimStart('~').ToLower(),
                path = path.TrimStart('~').ToLower()
            };

            return obj;
        }

        /// <summary>
        /// 新增公告
        /// </summary>
        /// <param name="account"></param>
        /// <param name="realIP"></param>
        /// <param name="proxyIP"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<int> AddContent(string account, long realIP, long proxyIP, ModifyAnnounceVM model)
        {
            var result = new DataResult<int>();

            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ModifyAnnounceVM, AddContentDbReq>());
                var mapper = config.CreateMapper();
                AddContentDbReq req = mapper.Map<AddContentDbReq>(model);

                req.Modifier = account;
                req.AnnounceStatus = 1;
                req.StartDate = Convert.ToDateTime(model.StartDate + " " + model.StartDateTime);
                req.EndDate = DateTime.MaxValue;
                req.AnnounceContent = SetContent(req.AnnounceContent);
                if (model.IsTop == 1)
                {
                    req.IsTopStartDate = Convert.ToDateTime(model.IsTopStartDate + " " + model.IsTopStartDateTime);
                    req.IsTopEndDate = Convert.ToDateTime(model.IsTopEndDate + " " + model.IsTopEndDateTime);
                }

                result = _announcementRepository.AddContent(req);

                AddContentLog(account, realIP, proxyIP, result.RtnData, req);
            }
            catch
            {
                result.SetError();
            }

            return result;
        }

        /// <summary>
        /// 新增圖片
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult AddImage(ModifyAnnounceVM model)
        {
            BaseResult result = new BaseResult();
            result.SetSuccess();

            foreach (string filePath in model.ImagePathList)
            {
                AddImageDbReq req = new AddImageDbReq()
                {
                    NID = model.NID,
                    Path = filePath
                };

                var addResult = _announcementRepository.AddImage(req);
                if (!addResult.IsSuccess)
                {
                    result = addResult;
                }
            }

            return result;
        }

        /// <summary>
        /// 新增MID名單
        /// </summary>
        /// <param name="account"></param>
        /// <param name="nID"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public BaseResult AddMidList(string account, int nID, List<MidFileModel> list)
        {
            BaseResult result = new BaseResult();
            result.SetSuccess();

            foreach (var item in list)
            {
                int fileId = 0; //檔案編號
                List<long> fileList = new List<long>();

                #region 讀取CSV文件
                var listResult = OpenCSV(item.FileName);
                if (!listResult.IsSuccess)
                {
                    result = listResult;
                    continue;
                }
                fileList = listResult.RtnData;
                #endregion

                #region 新增MID名單檔案
                AddMidFileDbReq fileReq = new AddMidFileDbReq()
                {
                    NID = nID,
                    FileName = item.FileName,
                    Status = item.Status,
                    Modifier = account
                };

                var addResult = _announcementRepository.AddMidFile(fileReq);
                if (!addResult.IsSuccess)
                {
                    result = addResult;
                }
                #endregion

                fileId = addResult.RtnData;

                #region 新增MID名單
                foreach (long id in fileList)
                {
                    long mid = 0;

                    //電支帳號
                    if (id.ToString().Length == 16)
                    {
                        mid = _announcementRepository.GetMID(null, id.ToString()).RtnData;
                    }
                    else
                    {
                        mid = id;
                    }

                    if (mid > 0)
                    {
                        AddMidDbReq addMidReq = new AddMidDbReq()
                        {
                            FileID = fileId,
                            MID = mid
                        };

                        var addMidResult = _announcementRepository.AddMid(addMidReq);
                        if (!addMidResult.IsSuccess)
                        {
                            result = addMidResult;
                        }
                    }
                }
                #endregion
            }

            return result;
        }

        /// <summary>
        /// 讀取CSV文件
        /// </summary>
        /// <param name="filePath">CSV文件路徑</param>
        public DataResult<List<long>> OpenCSV(string filePath)
        {
            var result = new DataResult<List<long>>();
            List<long> list = new List<long>();

            try
            {
                filePath = HttpContext.Current.Server.MapPath(filePath);
                string[] lines = File.ReadAllLines(filePath, Encoding.Default);
                list = lines.Select(a => long.Parse(a)).ToList();
                result.SetSuccess(list);
            }
            catch
            {
                result.SetError();
                result.RtnMsg = "檔案內容異常";
            }

            return result;
        }

        /// <summary>
        /// 取得公告
        /// </summary>
        /// <param name="nID"></param>
        /// <returns></returns>
        public DataResult<GetContentDbRes> GetContent(int nID)
        {
            var result = new DataResult<GetContentDbRes>();

            try
            {
                GetContentDbRes res = _announcementRepository.GetContent(nID);
                result.SetSuccess(res);
            }
            catch
            {
                result.SetError();
            }

            return result;
        }

        /// <summary>
        /// 取得MID名單檔案
        /// </summary>
        /// <param name="nID"></param>
        /// <returns></returns>
        public DataResult<List<GetMidFileDbRes>> GetMidFile(int nID)
        {
            var result = new DataResult<List<GetMidFileDbRes>>();

            try
            {
                List<GetMidFileDbRes> res = _announcementRepository.GetMidFile(nID);
                result.SetSuccess(res);
            }
            catch
            {
                result.SetError();
            }

            return result;
        }

        /// <summary>
        /// 轉換ModifyAnnounceVM
        /// </summary>
        /// <param name="content"></param>
        /// <param name="midFileList"></param>
        /// <returns></returns>
        public DataResult<ModifyAnnounceVM> MappingAnnounceVM(GetContentDbRes content, List<GetMidFileDbRes> midFileList)
        {
            ModifyAnnounceVM model = new ModifyAnnounceVM();
            var result = new DataResult<ModifyAnnounceVM>();

            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<GetContentDbRes, ModifyAnnounceVM>());
                var mapper = config.CreateMapper();
                model = mapper.Map<ModifyAnnounceVM>(content);

                List<MidFileModel> CsvPathList = new List<MidFileModel>();
                foreach (var item in midFileList)
                {
                    CsvPathList.Add(new MidFileModel()
                    {
                        FileID = item.FileID,
                        FileName = item.FileName,
                        Status = item.Status
                    });
                }
                model.CsvPathList = CsvPathList;

                result.SetSuccess(model);
            }
            catch
            {
                result.SetError();
                result.RtnMsg = "資料轉換錯誤";
            }

            return result;
        }

        /// <summary>
        /// 修改公告
        /// </summary>
        /// <param name="account"></param>
        /// <param name="realIP"></param>
        /// <param name="proxyIP"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<int> EditContent(string account, long realIP, long proxyIP, ModifyAnnounceVM model)
        {
            var result = new DataResult<int>();

            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ModifyAnnounceVM, EditContentDbReq>());
                var mapper = config.CreateMapper();
                EditContentDbReq req = mapper.Map<EditContentDbReq>(model);

                req.Modifier = account;
                req.AnnounceStatus = 1;
                req.StartDate = Convert.ToDateTime(model.StartDate + " " + model.StartDateTime);
                req.EndDate = DateTime.MaxValue;
                req.AnnounceContent = req.AnnounceContent.IndexOf("#divContent") == -1 ? SetContent(req.AnnounceContent) : req.AnnounceContent;

                if (model.IsTop == 1)
                {
                    req.IsTopStartDate = Convert.ToDateTime(model.IsTopStartDate + " " + model.IsTopStartDateTime);
                    req.IsTopEndDate = Convert.ToDateTime(model.IsTopEndDate + " " + model.IsTopEndDateTime);
                }

                result = _announcementRepository.EditContent(req);

                AddContentLog(account, realIP, proxyIP, req.NID, req);
            }
            catch
            {
                result.SetError();
            }

            return result;
        }

        /// <summary>
        /// 修改MID名單
        /// </summary>
        /// <param name="account"></param>
        /// <param name="deleteMidList"></param>
        /// <returns></returns>
        public BaseResult EditMidList(string account, List<MidFileModel> deleteMidList)
        {
            BaseResult result = new BaseResult();
            result.SetSuccess();

            foreach (var item in deleteMidList)
            {
                EditMidDbReq req = new EditMidDbReq()
                {
                    FileID = item.FileID,
                    Status = item.Status,
                    Modifier = account
                };

                var editResult = _announcementRepository.EditMidList(req);
                if (!editResult.IsSuccess)
                {
                    result = editResult;
                }
            }

            return result;
        }

        /// <summary>
        /// 刪除訊息公告
        /// </summary>
        /// <param name="account"></param>
        /// <param name="realIP"></param>
        /// <param name="proxyIP"></param>
        /// <param name="nID"></param>
        /// <returns></returns>
        public DataResult<int> DeleteAnnounce(string account, long realIP, long proxyIP, int nID)
        {
            EditContentDbReq req = new EditContentDbReq()
            {
                NID = nID,
                AnnounceStatus = 2,
                Modifier = account
            };

            var result = _announcementRepository.EditContent(req);

            AddContentLog(account, realIP, proxyIP, req.NID, req);

            return result;
        }

        /// <summary>
        /// 審核訊息公告
        /// </summary>
        /// <param name="account"></param>
        /// <param name="realIP"></param>
        /// <param name="proxyIP"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<int> AuthAnnounce(string account, long realIP, long proxyIP, ModifyAnnounceVM model)
        {
            EditContentDbReq req = new EditContentDbReq()
            {
                NID = model.NID,
                AuthStatus = model.AuthStatus,
                AuthUser = account
            };

            var result = _announcementRepository.EditContent(req);

            AddContentLog(account, realIP, proxyIP, req.NID, req);

            return result;
        }

        /// <summary>
        /// 新增公告類別
        /// </summary>
        /// <param name="account"></param>
        /// <param name="realIP"></param>
        /// <param name="proxyIP"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult AddCategory(string account, long realIP, long proxyIP, ModifyCategoryVM model)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ModifyCategoryVM, AddCategoryDbReq>());
            var mapper = config.CreateMapper();
            AddCategoryDbReq req = mapper.Map<AddCategoryDbReq>(model);
            req.Modifier = account;

            var result = _announcementRepository.AddCategory(req);

            AddCategoryLog(account, realIP, proxyIP, result.RtnData, req);

            return result;
        }

        /// <summary>
        /// 取得公告類別
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public DataResult<ModifyCategoryVM> GetCategory(int categoryID)
        {
            var result = new DataResult<ModifyCategoryVM>();

            try
            {
                ModifyCategoryVM model = new ModifyCategoryVM();
                List<ListCategoryDbRes> list = new List<ListCategoryDbRes>();
                ListCategoryDbReq req = new ListCategoryDbReq()
                {
                    CategoryID = categoryID,
                    PageSize = 100
                };

                list = ListCategory(req);

                var config = new MapperConfiguration(cfg => cfg.CreateMap<ListCategoryDbRes, ModifyCategoryVM>());
                var mapper = config.CreateMapper();
                model = mapper.Map<ModifyCategoryVM>(list.Where(x => x.CategoryID == categoryID).FirstOrDefault());

                result.SetSuccess(model);
            }
            catch
            {
                result.SetError();

            }

            return result;
        }

        /// <summary>
        /// 修改公告類別
        /// </summary>
        /// <param name="account"></param>
        /// <param name="realIP"></param>
        /// <param name="proxyIP"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult EditCategory(string account, long realIP, long proxyIP, ModifyCategoryVM model)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ModifyCategoryVM, EditCategoryDbReq>());
            var mapper = config.CreateMapper();
            EditCategoryDbReq req = mapper.Map<EditCategoryDbReq>(model);
            req.Modifier = account;

            var result = _announcementRepository.EditCategory(req);

            AddCategoryLog(account, realIP, proxyIP, model.CategoryID, req);

            return result;
        }

        /// <summary>
        /// 新增訊息中心
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult AddNotifyMessage(ModifyAnnounceVM model)
        {
            BaseResult result = new BaseResult();
            string[] midList = model.TestMidList.Replace(" ", "").Split(',');
            result.SetSuccess();

            try
            {
                foreach (string mid in midList)
                {
                    long MID = 0;
                    if (mid.Length == 16)
                    {
                        MID = _announcementRepository.GetMID(null, mid).RtnData;
                    }
                    else
                    {
                        MID = long.Parse(mid);
                    }

                    if (MID > 0)
                    {
                        var args = new
                        {
                            MID,
                            Subject = model.Title,
                            Body = model.AnnounceContent,
                            model.CategoryID
                        };

                        var addResult = _announcementRepository.AddNotifyMessage(args);
                        if (!addResult.IsSuccess)
                        {
                            result.RtnCode = addResult.RtnCode;
                            result.RtnMsg = addResult.RtnMsg;
                        }
                    }
                }
            }
            catch
            {
                result.SetError();
            }

            return result;
        }

        #region Add Log
        /// <summary>
        /// add Content Log
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="account"></param>
        /// <param name="realIP"></param>
        /// <param name="proxyIP"></param>
        /// <param name="nID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private BaseResult AddContentLog<T>(string account, long realIP, long proxyIP, int nID, T model)
        {
            var result = new BaseResult();

            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<T, AddContentLogDbReq>());
                var mapper = config.CreateMapper();

                AddContentLogDbReq req = mapper.Map<AddContentLogDbReq>(model);
                req.CreateUser = account;
                req.RealIP = realIP;
                req.ProxyIP = proxyIP;
                req.NID = nID;

                result = _announcementRepository.AddContentLog(req);
            }
            catch
            {
                result.SetError();
            }

            return result;
        }

        /// <summary>
        /// Add Category Log
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="account"></param>
        /// <param name="realIP"></param>
        /// <param name="proxyIP"></param>
        /// <param name="categoryID"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private BaseResult AddCategoryLog<T>(string account, long realIP, long proxyIP, int categoryID, T model)
        {
            var result = new BaseResult();

            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<T, AddCategoryLogDbReq>());
                var mapper = config.CreateMapper();

                AddCategoryLogDbReq req = mapper.Map<AddCategoryLogDbReq>(model);
                req.CategoryID = categoryID;
                req.CreateUser = account;
                req.RealIP = realIP;
                req.ProxyIP = proxyIP;

                result = _announcementRepository.AddCategoryLog(req);
            }
            catch
            {
                result.SetError();
            }

            return result;
        }
        #endregion

        /// <summary>
        /// 設定公告內容
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private string SetContent(string content)
        {
            string str = "<div id=\"divContent\">" + content + "</div><script type=\"text/javascript\">$('#divContent').find('img').css('max-width', '100%')</script>";
            return str;
        }

    }
}

