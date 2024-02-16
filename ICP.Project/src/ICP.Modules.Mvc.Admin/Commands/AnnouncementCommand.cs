using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Models.Announcement;
using ICP.Modules.Mvc.Admin.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Commands
{
    public class AnnouncementCommand
    {
        AnnouncementService _announcementService;

        public AnnouncementCommand(AnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        /// <summary>
        /// 訊息公告清單
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<ListAnnounceDbRes> ListContent(ListAnnounceDbReq req)
        {
            return _announcementService.ListContent(req);
        }

        /// <summary>
        /// 公告類別選單
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> ListCategoryItem()
        {
            var selectList = new List<SelectListItem>();

            ListCategoryDbReq req = new ListCategoryDbReq()
            {
                CategoryID = 0,
                PageSize = 100
            };

            List<ListCategoryDbRes> list = _announcementService.ListCategory(req);
            list.Where(x => x.Status == 1).ToList().ForEach(x => selectList.Add(
                    new SelectListItem
                    {
                        Value = x.CategoryID.ToString(),
                        Text = x.CategoryName
                    }
                ));

            return selectList;
        }

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
                string path = string.Format("~/Upload/Announce/{0}/{1}/", DateTime.UtcNow.AddHours(8).ToString("yyyyMM"), fileType);
                string serverPath = HttpContext.Current.Server.MapPath(path);

                if (!Directory.Exists(serverPath))
                {
                    Directory.CreateDirectory(serverPath);
                }

                file.SaveAs(Path.Combine(serverPath, fileName).ToLower());

                result.SetSuccess(_announcementService.SetFileUrl(path + fileName));
            }
            catch
            {
                result.SetError();
            }

            return result;
        }

        /// <summary>
        /// 讀取CSV文件
        /// </summary>
        /// <param name="filePath">CSV文件路徑</param>
        public DataResult<List<long>> OpenCSV(string filePath)
        {
            return _announcementService.OpenCSV(filePath);
        }

        /// <summary>
        /// 新增訊息公告
        /// </summary>
        /// <param name="account"></param>
        /// <param name="realIP"></param>
        /// <param name="proxyIP"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult AddAnnounce(string account, long realIP, long proxyIP, ModifyAnnounceVM model)
        {
            List<MidFileModel> addMidList = new List<MidFileModel>();
            BaseResult result = new BaseResult();
            result.SetError();

            #region 新增公告
            var addContentResult = _announcementService.AddContent(account, realIP, proxyIP, model);
            if (!addContentResult.IsSuccess)
            {
                result.RtnCode = addContentResult.RtnCode;
                result.RtnMsg = addContentResult.RtnMsg;
                return result;
            }
            model.NID = addContentResult.RtnData;
            #endregion

            #region 新增圖片
            if (model.ImagePathList != null)
            {
                var addImageResult = _announcementService.AddImage(model);
                if (!addImageResult.IsSuccess)
                {
                    return addImageResult;
                }
            }
            #endregion

            #region 新增MID名單
            if (model.CsvPathList != null)
            {
                addMidList = model.CsvPathList.Where(x => x.Status == 1).ToList();
                var addMidListResult = _announcementService.AddMidList(account, model.NID, addMidList);
                if (!addMidListResult.IsSuccess)
                {
                    return addMidListResult;
                }
            }
            #endregion

            result.SetSuccess();

            return result;
        }

        /// <summary>
        /// 取得訊息公告
        /// </summary>
        /// <param name="nID"></param>
        /// <returns></returns>
        public DataResult<ModifyAnnounceVM> GetAnnounce(int nID)
        {
            GetContentDbRes content = new GetContentDbRes();
            List<GetMidFileDbRes> midFileList = new List<GetMidFileDbRes>();
            var result = new DataResult<ModifyAnnounceVM>();
            result.SetError();

            #region 取得公告
            var getContentResult = _announcementService.GetContent(nID);
            if (!getContentResult.IsSuccess)
            {
                result.RtnMsg = getContentResult.RtnMsg;
                return result;
            }
            content = getContentResult.RtnData;
            #endregion

            #region 取得MID名單檔案
            var getMidFileResult = _announcementService.GetMidFile(nID);
            if (!getMidFileResult.IsSuccess)
            {
                result.RtnMsg = getMidFileResult.RtnMsg;
                return result;
            }
            midFileList = getMidFileResult.RtnData;
            #endregion

            var mappingResult = _announcementService.MappingAnnounceVM(content, midFileList);
            if (!mappingResult.IsSuccess)
            {
                result.RtnMsg = mappingResult.RtnMsg;
                return result;
            }

            result.SetSuccess(mappingResult.RtnData);

            return result;
        }

        /// <summary>
        /// 修改訊息公告
        /// </summary>
        /// <param name="account"></param>
        /// <param name="realIP"></param>
        /// <param name="proxyIP"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult EditAnnounce(string account, long realIP, long proxyIP, ModifyAnnounceVM model)
        {
            List<MidFileModel> addMidList = new List<MidFileModel>();
            List<MidFileModel> editMidList = new List<MidFileModel>();
            BaseResult result = new BaseResult();
            result.SetError();

            #region 修改公告
            var editContentResult = _announcementService.EditContent(account, realIP, proxyIP, model);
            if (!editContentResult.IsSuccess)
            {
                result.RtnCode = editContentResult.RtnCode;
                result.RtnMsg = editContentResult.RtnMsg;
                return result;
            }
            #endregion

            #region 新增圖片
            if (model.ImagePathList != null)
            {
                var addImageResult = _announcementService.AddImage(model);
                if (!addImageResult.IsSuccess)
                {
                    return addImageResult;
                }
            }
            #endregion

            #region 新增&修改MID名單
            if (model.CsvPathList != null)
            {
                addMidList = model.CsvPathList.Where(x => x.Status == 1 && x.FileID == 0).ToList();
                editMidList = model.CsvPathList.Where(x => x.Status == 0 && x.FileID > 0).ToList();

                #region 新增MID名單
                if (addMidList.Count() > 0)
                {
                    var addMidListResult = _announcementService.AddMidList(account, model.NID, addMidList);
                    if (!addMidListResult.IsSuccess)
                    {
                        return addMidListResult;
                    }
                }
                #endregion

                #region 修改MID名單
                if (editMidList.Count() > 0)
                {
                    var editMidListResult = _announcementService.EditMidList(account, editMidList);
                    if (!editMidListResult.IsSuccess)
                    {
                        return editMidListResult;
                    }
                }
                #endregion
            }
            #endregion

            result.SetSuccess();

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
        public BaseResult DeleteAnnounce(string account, long realIP, long proxyIP, int nID)
        {
            return _announcementService.DeleteAnnounce(account, realIP, proxyIP, nID);
        }

        /// <summary>
        /// 審核訊息公告
        /// </summary>
        /// <param name="account"></param>
        /// <param name="realIP"></param>
        /// <param name="proxyIP"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult AuthAnnounce(string account, long realIP, long proxyIP, ModifyAnnounceVM model)
        {
            return _announcementService.AuthAnnounce(account, realIP, proxyIP, model);
        }

        /// <summary>
        /// 公告類別清單
        /// </summary>
        /// <returns></returns>
        public List<ListCategoryDbRes> ListCategory(ListCategoryDbReq model)
        {
            return _announcementService.ListCategory(model).Where(x => x.Status < 2).ToList();
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
            return _announcementService.AddCategory(account, realIP, proxyIP, model);
        }

        /// <summary>
        /// 取得公告類別
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public DataResult<ModifyCategoryVM> GetCategory(int categoryID)
        {
            return _announcementService.GetCategory(categoryID);
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
            return _announcementService.EditCategory(account, realIP, proxyIP, model);
        }

        /// <summary>
        /// 新增訊息中心
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult AddNotifyMessage(ModifyAnnounceVM model)
        {
            return _announcementService.AddNotifyMessage(model);
        }
    }
}
