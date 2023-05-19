using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Commands
{
    using ICP.Infrastructure.Abstractions.EmailSender;
    using ICP.Infrastructure.Core.Extensions;
    using ICP.Infrastructure.Core.Models;
    using Library.Models.MailLibrary;
    using Models.MailLibrary;
    using Services;

    public class MailLibraryManageCommand
    {
        MailLibraryManageService _mailLibraryManageService;
        MailManageService _mailManageService;
        NotifyManageService _notifyManageService;
        IEmailSender _emailSender;

        public MailLibraryManageCommand(
            MailLibraryManageService mailLibraryManageService,
            MailManageService mailManageService,
            NotifyManageService notifyManageService,
            IEmailSender emailSender
            )
        {
            _mailLibraryManageService = mailLibraryManageService;
            _mailManageService = mailManageService;
            _notifyManageService = notifyManageService;
            _emailSender = emailSender;
        }

        public List<ContentQueryResult> QueryContent(ContentQueryModel query)
        {
            return _mailLibraryManageService.ListContent(query);
        }

        #region MailContent
        public DataResult<MailContentModel> GetMailContent(long MailID)
        {
            var result = new DataResult<MailContentModel>();
            var getResult = _mailManageService.GetMailContent(MailID);

            if (!getResult.IsSuccess)
            {
                result.SetError(getResult);
                return result;
            }

            var model = AutoMapper.Mapper.Map<MailContentModel>(getResult.RtnData);
            result.SetSuccess(model);
            return result;
        }

        public DataResult<MailContentModel> GetAddMailContent(long NotifyID)
        {
            var result = new DataResult<MailContentModel>();
            result.SetError();

            if (NotifyID <= 0) return result;

            //跟訊息使用同一個Key、Description
            var getContenResult = _notifyManageService.GetNotifyContent(NotifyID);
            if (!getContenResult.IsSuccess)
            {
                return result;
            }
            var content = getContenResult.RtnData;

            var model = new MailContentModel();
            model.MailKey = content.NotifyKey;
            model.Description = content.Description;

            result.SetSuccess(model);
            return result;
        }

        public DataResult<long> AddMailContent(MailContentModel model, string creator)
        {
            if (!model.IsValid())
            {
                var result = new DataResult<long>();
                result.SetError();
                result.RtnMsg = model.GetFirstErrorMessage();
                return result;
            }

            model.Creator = creator;

            return _mailManageService.AddMailContent(model);
        }

        public BaseResult UpdateMailContent(long MailID, MailContentModel model, string modifier)
        {
            var result = new DataResult<long>();
            result.SetError();

            if (!model.IsValid())
            {
                result.RtnMsg = model.GetFirstErrorMessage();
                return result;
            }

            model.MailID = MailID;
            model.Modifier = modifier;

            return _mailManageService.UpdateMailContent(model);
        }

        public BaseResult DeleteMailContent(long MailID)
        {
            return _mailManageService.DeleteMailContent(MailID);
        }
        #endregion

        #region MailTag
        public List<MailTag> ListMailTag(long MailID)
        {
            return _mailManageService.ListMailTag(MailID);
        }

        public DataResult<long> AddMailTag(long MailID, MailTag model)
        {
            if (!model.IsValid())
            {
                var result = new DataResult<long>();
                result.SetError();
                result.RtnMsg = model.GetFirstErrorMessage();
                return result;
            }

            model.MailID = MailID;

            return _mailManageService.AddMailTag(model);
        }
        
        public BaseResult UpdateMailTag(long TagID, MailTag model)
        {
            if (!model.IsValid())
            {
                var result = new DataResult<long>();
                result.SetError();
                result.RtnMsg = model.GetFirstErrorMessage();
                return result;
            }

            model.TagID = TagID;

            return _mailManageService.UpdateMailTag(model);
        }

        public BaseResult DeleteMailTag(long TagID)
        {
            return _mailManageService.DeleteMailTag(TagID);
        }
        #endregion

        #region NotifyContent
        public DataResult<NotifyContentModel> GetNotifyContent(long NotifyID)
        {
            var result = new DataResult<NotifyContentModel>();
            var getResult = _notifyManageService.GetNotifyContent(NotifyID);

            if (!getResult.IsSuccess)
            {
                result.SetError(getResult);
                return result;
            }

            var model = AutoMapper.Mapper.Map<NotifyContentModel>(getResult.RtnData);
            result.SetSuccess(model);
            return result;
        }

        public DataResult<NotifyContentModel> GetAddNotifyContent(long MailID)
        {
            var result = new DataResult<NotifyContentModel>();
            result.SetError();

            if (MailID <= 0) return result;

            //跟訊息使用同一個Key、Description
            var getContenResult = _mailManageService.GetMailContent(MailID);
            if (!getContenResult.IsSuccess)
            {
                return result;
            }
            var content = getContenResult.RtnData;

            var model = new NotifyContentModel();
            model.NotifyKey = content.MailKey;
            model.Description = content.Description;

            result.SetSuccess(model);
            return result;
        }

        public DataResult<long> AddNotifyContent(NotifyContentModel model, string creator)
        {
            if (!model.IsValid())
            {
                var result = new DataResult<long>();
                result.SetError();
                result.RtnMsg = model.GetFirstErrorMessage();
                return result;
            }

            model.Creator = creator;

            return _notifyManageService.AddNotifyContent(model);
        }

        public BaseResult UpdateNotifyContent(long NotifyID, NotifyContentModel model, string modifier)
        {
            var result = new DataResult<long>();
            result.SetError();

            if (!model.IsValid())
            {
                result.RtnMsg = model.GetFirstErrorMessage();
                return result;
            }

            model.NotifyID = NotifyID;
            model.Modifier = modifier;

            return _notifyManageService.UpdateNotifyContent(model);
        }

        public BaseResult DeleteNotifyContent(long NotifyID)
        {
            return _notifyManageService.DeleteNotifyContent(NotifyID);
        }
        #endregion

        #region NotifyTag
        public List<NotifyTag> ListNotifyTag(long NotifyID)
        {
            return _notifyManageService.ListNotifyTag(NotifyID);
        }

        public DataResult<long> AddNotifyTag(long NotifyID, NotifyTag model)
        {
            if (!model.IsValid())
            {
                var result = new DataResult<long>();
                result.SetError();
                result.RtnMsg = model.GetFirstErrorMessage();
                return result;
            }

            model.NotifyID = NotifyID;

            return _notifyManageService.AddNotifyTag(model);
        }

        public BaseResult UpdateNotifyTag(long TagID, NotifyTag model)
        {
            if (!model.IsValid())
            {
                var result = new DataResult<long>();
                result.SetError();
                result.RtnMsg = model.GetFirstErrorMessage();
                return result;
            }

            model.TagID = TagID;

            return _notifyManageService.UpdateNotifyTag(model);
        }

        public BaseResult DeleteNotifyTag(long TagID)
        {
            return _notifyManageService.DeleteNotifyTag(TagID);
        }
        #endregion

        #region Test
        public BaseResult TestMail(string to, long MailID, Dictionary<string, string> dict)
        {
            var result = new BaseResult();
            result.SetError();

            var content = _mailManageService.Generate(MailID, dict);
            var IsSendSuccess = _emailSender.SendMail(to, content.Title, content.Body);
            if (!IsSendSuccess)
            {
                return result;
            }

            result.SetSuccess();
            return result;
        }
        #endregion

        #region PreView
        public DataResult<MailContent> PreViewMail(long MailID, Dictionary<string, string> dict)
        {
            var result = new DataResult<MailContent>();
            result.SetError();
            var content = _mailManageService.Generate(MailID, dict);
            result.SetSuccess(content);
            return result;
        }
        #endregion
    }
}