using ICP.Infrastructure.Core.Helpers;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.MemberModels;
using ICP.Library.Models.OpenWalletApi.WebUIApi;
using ICP.Library.Repositories.MemberRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Web.Helpers;

namespace ICP.Modules.Api.Member.Services
{
    public class OPWebUIService
    {
        ILogger<OPWebUIService> _logger;
        MemberInfoRepository _memberInfoRepository;
        MemberTeenagersRepository _memberTeenagersRepository;
        SaveFileHelper _saveFileHelper;

        public OPWebUIService(
            ILogger<OPWebUIService> logger,
            MemberInfoRepository memberInfoRepository,
            MemberTeenagersRepository memberTeenagersRepository
            )
        {
            _logger = logger;
            _memberInfoRepository = memberInfoRepository;
            _memberTeenagersRepository = memberTeenagersRepository;
            _saveFileHelper = new SaveFileHelper();
        }

        public List<GetUserDataWebUIResult.userData> TeenagersLegalDetail_To_UserData(List<MemberTeenagersLegalDetail> list)
        {
            return list.Select(t => 
            {
                var memberData = _memberInfoRepository.GetMemberData(t.TeenagersMID);

                var basic = memberData.basic;

                var detail = memberData.detail;

                var teenager = _memberTeenagersRepository.GetTeenager(t.TeenagersMID);

                return new GetUserDataWebUIResult.userData
                {
                    MID = basic.ICPMID,
                    UserName = basic.CName,
                    UserID = detail.IDNO,
                    UserPhone = detail.CellPhone,
                    ValidDate = teenager.ExpireDate.ToString("yyyy/MM/dd HH:mm:ss"),
                    Adult = detail.Birthday <= DateTime.Today.AddYears(-20) ? "1" : "0",
                    ValidType = t.LegalType.ToString()
                };
            }).ToList();
        }

        public BaseResult SaveAgreeRegisterImages(ref MemberTeenagersLegalDetail detail, AgreeRegisterWebUIRequest request, string urlDir, string saveDir)
        {
            var result = new BaseResult();
            result.SetError();
         
            try
            {
                if (request.IdentityCard != null)
                {
                    var images = request.IdentityCard;
                    _saveFileHelper.SaveImgToModel(urlDir, saveDir, $"{Guid.NewGuid()}.jpg", images.ImageFile1, detail, t => t.IDNOFile1);
                    _saveFileHelper.SaveImgToModel(urlDir, saveDir, $"{Guid.NewGuid()}.jpg", images.ImageFile2, detail, t => t.IDNOFile2);
                }

                if (request.HouseholdRegistration != null)
                {
                    var images = request.HouseholdRegistration;
                    _saveFileHelper.SaveImgToModel(urlDir, saveDir, $"{Guid.NewGuid()}.jpg", images.ImageFile1, detail, t => t.FilePath1);
                    _saveFileHelper.SaveImgToModel(urlDir, saveDir, $"{Guid.NewGuid()}.jpg", images.ImageFile2, detail, t => t.FilePath2);
                    _saveFileHelper.SaveImgToModel(urlDir, saveDir, $"{Guid.NewGuid()}.jpg", images.ImageFile3, detail, t => t.FilePath3);
                    _saveFileHelper.SaveImgToModel(urlDir, saveDir, $"{Guid.NewGuid()}.jpg", images.ImageFile4, detail, t => t.FilePath4);
                    _saveFileHelper.SaveImgToModel(urlDir, saveDir, $"{Guid.NewGuid()}.jpg", images.ImageFile5, detail, t => t.FilePath5);
                    _saveFileHelper.SaveImgToModel(urlDir, saveDir, $"{Guid.NewGuid()}.jpg", images.ImageFile6, detail, t => t.FilePath6);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "儲存 [同意未成年註冊] 上傳圖片");
                return result;
            }

            result.SetSuccess();
            return result;
        }

        public DataResult<MemberTeenagersLegalDetail> CheckTeenagersLegalDetail(long TeenagersMID, List<MemberTeenagersLegalDetail> list)
        {
            var result = new DataResult<MemberTeenagersLegalDetail>();
            result.SetError();

            var data = list.FirstOrDefault(t => t.TeenagersMID == TeenagersMID);
            if (data == null)
            {
                return result;
            }

            result.SetSuccess(data);
            return result;
        }

        /// <summary>
        /// 同意未成年註冊
        /// </summary>
        /// <param name="model">同意資料</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <returns></returns>
        public BaseResult UpdateTeenagersLegalAgree(MemberTeenagersLegalDetail model, long RealIP = 0, long ProxyIP = 0)
        {
            return _memberTeenagersRepository.UpdateTeenagersLegalAgree(model, RealIP, ProxyIP);
        }
    }
}
