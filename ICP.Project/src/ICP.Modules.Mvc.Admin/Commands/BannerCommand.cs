using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Models.Banner;
using ICP.Modules.Mvc.Admin.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Commands
{
    public class BannerCommand
    {
        BannerService _bannerService;

        public BannerCommand(BannerService bannerService)
        {
            _bannerService = bannerService;
        }

        #region 設定下拉選單
        /// <summary>
        /// 上架狀態選單
        /// </summary>
        /// <returns></returns>
        /// <remarks>1待審核、2審核通過、3審核不通過、4審核通過上架中、5審核通過已下架</remarks>
        public IEnumerable<SelectListItem> ListBannerStatusItem()
        {
            var selectList = new List<SelectListItem>();

            selectList.Add(new SelectListItem { Value = "1", Text = "待審核" });
            selectList.Add(new SelectListItem { Value = "2", Text = "審核通過" });
            selectList.Add(new SelectListItem { Value = "3", Text = "審核不通過" });
            selectList.Add(new SelectListItem { Value = "4", Text = "審核通過上架中" });
            selectList.Add(new SelectListItem { Value = "5", Text = "審核通過已下架" });

            return selectList;
        }

        /// <summary>
        /// 廣告位置選單
        /// </summary>
        /// <returns></returns>
        public List<BannerSiteModel> ListBannerSite()
        {
            return _bannerService.ListBannerSiteData();
        }
        #endregion

        #region 廣告管理清單
        /// <summary>
        /// QryBannerVM MappingTo ListBannerReq
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<ListBannerReq> MappingToListBannerReq(QryBannerVM model)
        {
            return _bannerService.MappingToListBannerReq(model);
        }

        /// <summary>
        /// 廣告清單
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public DataResult<List<ListBannerVM>> ListBanner(QryBannerVM model)
        {
            var result = new DataResult<List<ListBannerVM>>();

            var mapResult = _bannerService.MappingToListBannerReq(model);
            if (!mapResult.IsSuccess)
            {
                result.RtnCode = mapResult.RtnCode;
                result.RtnMsg = mapResult.RtnMsg;
                return result;
            }
            ListBannerReq req = mapResult.RtnData;

            return _bannerService.ListBanner(req);
        }
        #endregion

        /// <summary>
        /// 檔案上傳
        /// </summary>
        /// <param name="fileType"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public DataResult<object> UploadFile(string fileType, HttpPostedFileBase file)
        {
            var result = new DataResult<object>();

            try
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string path = string.Format("~/Upload/Banner/{0}/{1}/", DateTime.UtcNow.AddHours(8).ToString("yyyyMM"), fileType);
                string serverPath = HttpContext.Current.Server.MapPath(path);

                if (!Directory.Exists(serverPath))
                {
                    Directory.CreateDirectory(serverPath);
                }

                file.SaveAs(Path.Combine(serverPath, fileName).ToLower());

                result.SetSuccess(_bannerService.SetFileUrl(path + fileName));
            }
            catch
            {
                result.SetError();
            }

            return result;
        }

        /// <summary>
        /// 新增廣告
        /// </summary>
        /// <param name="account"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult AddBanner(string account, ModifyBannerVM model)
        {
            BaseResult result = new BaseResult();
            result.SetError();

            #region 新增廣告
            var addBannerResult = _bannerService.AddBanner(account, model);
            if (!addBannerResult.IsSuccess)
            {
                result.RtnCode = addBannerResult.RtnCode;
                result.RtnMsg = addBannerResult.RtnMsg;
                return result;
            }
            model.BannerID = addBannerResult.RtnData;
            #endregion

            #region 新增廣告位置
            if (model.BannerSiteList != null)
            {
                var addSiteResult = _bannerService.SetBannerSite(model);
                if (!addSiteResult.IsSuccess)
                {
                    return addSiteResult;
                }
            }
            #endregion

            #region 新增廣告內頁圖片
            if (model.ImagePathList != null)
            {
                var addImageResult = _bannerService.AddImage(model);
                if (!addImageResult.IsSuccess)
                {
                    return addImageResult;
                }
            }
            #endregion

            result.SetSuccess();

            return result;
        }

        /// <summary>
        /// 取得廣告
        /// </summary>
        /// <param name="bannerID"></param>
        /// <returns></returns>
        public DataResult<ModifyBannerVM> GetBanner(int bannerID)
        {
            GetBannerRes content = new GetBannerRes();
            List<BannerSiteModel> bannerSiteList = new List<BannerSiteModel>();
            var result = new DataResult<ModifyBannerVM>();
            result.SetError();

            #region 取得廣告
            var getResult = _bannerService.GetBanner(bannerID);
            if (!getResult.IsSuccess)
            {
                result.RtnMsg = getResult.RtnMsg;
                return result;
            }
            content = getResult.RtnData;
            #endregion

            #region 取得廣告位置
            bannerSiteList = _bannerService.ListBannerSiteData();
            var selectedList = _bannerService.GetBannerSite(bannerID);
            foreach (var item in selectedList)
            {
                bannerSiteList.ForEach(x =>
                {
                    if (x.SiteID == item.SiteID)
                    {
                        x.IsSelected = true;
                    }
                });
            }
            #endregion

            var mappingResult = _bannerService.MappingBannerVM(content, bannerSiteList);
            if (!mappingResult.IsSuccess)
            {
                result.RtnMsg = mappingResult.RtnMsg;
                return result;
            }

            result.SetSuccess(mappingResult.RtnData);

            return result;
        }

        /// <summary>
        /// 修改廣告
        /// </summary>
        /// <param name="account"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult EditBanner(string account, ModifyBannerVM model)
        {
            BaseResult result = new BaseResult();
            result.SetError();

            #region 修改廣告
            var editContentResult = _bannerService.EditBanner(account, model);
            if (!editContentResult.IsSuccess)
            {
                return editContentResult;
            }
            #endregion

            #region 設定廣告位置
            if (model.BannerSiteList != null)
            {
                var addSiteResult = _bannerService.SetBannerSite(model);
                if (!addSiteResult.IsSuccess)
                {
                    return addSiteResult;
                }
            }
            #endregion

            #region 新增廣告內頁圖片
            if (model.ImagePathList != null)
            {
                var addImageResult = _bannerService.AddImage(model);
                if (!addImageResult.IsSuccess)
                {
                    return addImageResult;
                }
            }
            #endregion

            result.SetSuccess();

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
            return _bannerService.DeleteBanner(account, bannerID);
        }

        /// <summary>
        /// 審核廣告
        /// </summary>
        /// <param name="account"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult AuthBanner(string account, ModifyBannerVM model)
        {
            return _bannerService.AuthBanner(account, model);
        }

        /// <summary>
        /// 檢查廣告排序
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult CheckBannerOrderID(ModifyBannerVM model)
        {
            return _bannerService.CheckBannerOrderID(model);
        }
    }
}
