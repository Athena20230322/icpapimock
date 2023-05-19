using AutoMapper;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Utils;
using ICP.Modules.Mvc.Admin.Models.Banner;
using ICP.Modules.Mvc.Admin.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ICP.Modules.Mvc.Admin.Services
{
    public class BannerService
    {
        BannerRepository _bannerRepository;

        public BannerService(BannerRepository bannerRepository)
        {
            _bannerRepository = bannerRepository;
        }

        /// <summary>
        /// QryBannerVM MappingTo ListBannerReq
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<ListBannerReq> MappingToListBannerReq(QryBannerVM model)
        {
            ListBannerReq req = new ListBannerReq();
            var result = new DataResult<ListBannerReq>();

            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<QryBannerVM, ListBannerReq>());
                var mapper = config.CreateMapper();
                req = mapper.Map<ListBannerReq>(model);

                //0全部、1待審核、2審核通過、3審核不通過、4審核通過上架中、5審核通過已下架
                switch (model.BannerStatus)
                {
                    case 0:
                        req.Status = 1;
                        req.AuthStatus = null;
                        break;
                    case 1:
                        req.Status = 1;
                        req.AuthStatus = 0;
                        break;
                    case 2:
                        req.Status = null;
                        req.AuthStatus = 1;
                        break;
                    case 3:
                        req.Status = 1;
                        req.AuthStatus = 2;
                        break;
                    case 4:
                        req.Status = 1;
                        req.AuthStatus = 1;
                        req.StartDate = req.StartDate == null ? DateTime.Now : req.StartDate;
                        req.EndDate = req.EndDate == null ? DateTime.Now : req.EndDate;
                        break;
                    case 5:
                        req.Status = 0;
                        req.AuthStatus = 1;
                        break;
                }

                result.SetSuccess(req);
            }
            catch
            {
                result.SetError();
                result.RtnMsg = "資料轉換錯誤";
            }

            return result;
        }

        /// <summary>
        /// 廣告清單
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public DataResult<List<ListBannerVM>> ListBanner(ListBannerReq req)
        {
            List<ListBannerVM> rtnList = new List<ListBannerVM>();
            List<ListBannerRes> bannerList = _bannerRepository.ListBanner(req);
            var result = new DataResult<List<ListBannerVM>>();

            try
            {
                foreach (var item in bannerList)
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<ListBannerRes, ListBannerVM>());
                    var mapper = config.CreateMapper();
                    ListBannerVM model = mapper.Map<ListBannerVM>(item);
                    model.BannerSiteList = _bannerRepository.GetBannerSite(item.BannerID);
                    rtnList.Add(model);
                }

                result.SetSuccess(rtnList);
            }
            catch
            {
                result.SetError();
            }

            return result;
        }

        /// <summary>
        /// 取得廣告位置清單
        /// </summary>
        /// <returns></returns>
        public List<BannerSiteModel> ListBannerSiteData()
        {
            List<BannerSiteDbRes> list = _bannerRepository.ListBannerSiteData();
            List<BannerSiteModel> returnList = new List<BannerSiteModel>();

            foreach (var item in list)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<BannerSiteDbRes, BannerSiteModel>());
                var mapper = config.CreateMapper();
                BannerSiteModel model = mapper.Map<BannerSiteModel>(item);
                returnList.Add(model);
            }

            return returnList;
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
        /// 新增廣告
        /// </summary>
        /// <param name="account"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<int> AddBanner(string account, ModifyBannerVM model)
        {
            var result = new DataResult<int>();

            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ModifyBannerVM, AddBannerReq>());
                var mapper = config.CreateMapper();
                AddBannerReq req = mapper.Map<AddBannerReq>(model);

                req.StartDate = Convert.ToDateTime(model.StartDate + " " + model.StartDateTime);
                req.EndDate = Convert.ToDateTime(model.EndDate + " " + model.EndDateTime);
                req.Modifier = account;
                req.Status = 1;

                if (req.IsUseContent == 0)
                {
                    #region 無內頁
                    req.Title = null;
                    req.BannerContent = null;
                    req.UrlLink2 = null;
                    req.OpenNewWindow2 = null;
                    req.ImagePath2 = null;
                    #endregion
                }
                else
                {
                    #region 有內頁
                    req.UrlLink1 = null;
                    if (model.ImagePathList != null)
                    {
                        req.ImagePath2 = model.ImagePathList[0];
                    }

                    req.BannerContent= SetContent(req.BannerContent);
                    #endregion
                }

                result = _bannerRepository.AddBanner(req);
            }
            catch
            {
                result.SetError();
            }

            return result;
        }

        /// <summary>
        /// 設定廣告位置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult SetBannerSite(ModifyBannerVM model)
        {
            string siteIDList = string.Empty;
            BaseResult result = new BaseResult();
            result.SetSuccess();

            foreach (var item in model.BannerSiteList)
            {
                if (item.IsSelected)
                {
                    siteIDList += item.SiteID + ",";
                }
            }

            AddBannerSiteReq req = new AddBannerSiteReq()
            {
                BannerID = model.BannerID,
                SiteIDList = siteIDList.Substring(0, siteIDList.Length - 1)
            };

            var addResult = _bannerRepository.SetBannerSite(req);
            if (!addResult.IsSuccess)
            {
                result = addResult;
            }

            return result;
        }

        /// <summary>
        /// 新增廣告內頁圖片
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult AddImage(ModifyBannerVM model)
        {
            BaseResult result = new BaseResult();
            result.SetSuccess();

            foreach (string filePath in model.ImagePathList)
            {
                AddImageReq req = new AddImageReq()
                {
                    BannerID = model.BannerID,
                    Path = filePath
                };

                var addResult = _bannerRepository.AddImage(req);
                if (!addResult.IsSuccess)
                {
                    result = addResult;
                }
            }

            return result;
        }

        /// <summary>
        /// 取得廣告
        /// </summary>
        /// <param name="bannerID"></param>
        /// <returns></returns>
        public DataResult<GetBannerRes> GetBanner(int bannerID)
        {
            var result = new DataResult<GetBannerRes>();

            try
            {
                GetBannerRes res = _bannerRepository.GetBanner(bannerID);
                result.SetSuccess(res);
            }
            catch
            {
                result.SetError();
            }

            return result;
        }

        /// <summary>
        /// 取得廣告位置(依BannerID)
        /// </summary>
        /// <param name="bannerID"></param>
        /// <returns></returns>
        public List<BannerSiteDbRes> GetBannerSite(int bannerID)
        {
            return _bannerRepository.GetBannerSite(bannerID);
        }

        /// <summary>
        /// 轉換ModifyBannerVM
        /// </summary>
        /// <param name="res"></param>
        /// <param name="bannerSiteList"></param>
        /// <returns></returns>
        public DataResult<ModifyBannerVM> MappingBannerVM(GetBannerRes res, List<BannerSiteModel> bannerSiteList)
        {
            ModifyBannerVM model = new ModifyBannerVM();
            var result = new DataResult<ModifyBannerVM>();
            result.SetError();

            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<GetBannerRes, ModifyBannerVM>());
                var mapper = config.CreateMapper();
                model = mapper.Map<ModifyBannerVM>(res);
                model.BannerSiteList = bannerSiteList;

                //內層BANNER圖片清單
                if (!string.IsNullOrEmpty(res.ImagePath2))
                {
                    model.ImagePathList = new List<string>();
                    model.ImagePathList.Add(res.ImagePath2);
                }

                result.SetSuccess(model);
            }
            catch
            {
                result.RtnMsg = "資料轉換錯誤";
            }

            return result;
        }

        /// <summary>
        /// 修改廣告
        /// </summary>
        /// <param name="account"></param>>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult EditBanner(string account, ModifyBannerVM model)
        {
            var result = new BaseResult();

            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ModifyBannerVM, EditBannerReq>());
                var mapper = config.CreateMapper();
                EditBannerReq req = mapper.Map<EditBannerReq>(model);

                req.StartDate = Convert.ToDateTime(model.StartDate + " " + model.StartDateTime);
                req.EndDate = Convert.ToDateTime(model.EndDate + " " + model.EndDateTime);
                req.Modifier = account;
                req.Status = 1;

                if (req.IsUseContent == 0)
                {
                    #region 無內頁
                    req.Title = null;
                    req.BannerContent = null;
                    req.UrlLink2 = null;
                    req.OpenNewWindow2 = null;
                    req.ImagePath2 = null;
                    #endregion
                }
                else
                {
                    #region 有內頁
                    req.UrlLink1 = null;
                    if (model.ImagePathList != null)
                    {
                        req.ImagePath2 = model.ImagePathList.Where(x => !string.IsNullOrEmpty(x)).FirstOrDefault();
                    }

                    req.BannerContent = req.BannerContent.IndexOf("#divContent") == -1 ? SetContent(req.BannerContent) : req.BannerContent;
                    #endregion
                }

                result = _bannerRepository.EditBanner(req);
            }
            catch
            {
                result.SetError();
            }

            return result;
        }

        /// <summary>
        /// 刪除廣告
        /// </summary>
        /// <param name="account"></param>
        /// <param name="bannerID"></param>
        /// <returns></returns>
        public BaseResult DeleteBanner(string account, int bannerID)
        {
            EditBannerReq req = new EditBannerReq()
            {
                BannerID = bannerID,
                Status = 2,
                Modifier = account
            };

            return _bannerRepository.EditBanner(req);
        }

        /// <summary>
        /// 審核廣告
        /// </summary>
        /// <param name="account"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult AuthBanner(string account, ModifyBannerVM model)
        {
            EditBannerReq req = new EditBannerReq()
            {
                BannerID = model.BannerID,
                AuthStatus = model.AuthStatus,
                AuthUser = account
            };

            return _bannerRepository.EditBanner(req);
        }

        /// <summary>
        /// 檢查廣告排序
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult CheckBannerOrderID(ModifyBannerVM model)
        {
            BaseResult result = new BaseResult();
            result.SetSuccess();

            try
            {
                var args = new
                {
                    StartDate = Convert.ToDateTime(model.StartDate + " " + model.StartDateTime),
                    EndDate = Convert.ToDateTime(model.EndDate + " " + model.EndDateTime),
                    model.OrderID,
                    model.BannerID
                };

                result = _bannerRepository.CheckBannerOrderID(args);
            }
            catch
            {
                result.SetError();
            }

            return result;
        }

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
