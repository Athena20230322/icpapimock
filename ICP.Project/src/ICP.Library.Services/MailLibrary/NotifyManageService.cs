using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Services.MailLibrary
{
    using Infrastructure.Core.Extensions;
    using Infrastructure.Core.Models;
    using Library.Models.MailLibrary;
    using Library.Repositories.MailLibrary;

    public class NotifyManageService
    {
        NotifyManageRepository _notifyManageRepository;

        public NotifyManageService(NotifyManageRepository notifyManageRepository)
        {
            _notifyManageRepository = notifyManageRepository;
        }

        #region Query
        public DataResult<NotifyContent> GetNotifyContent(long NotifyID)
        {
            var result = new DataResult<NotifyContent>();
            result.SetError();

            var rtnData = _notifyManageRepository.GetNotifyContent(NotifyID);
            if (rtnData == null)
            {
                return result;
            }

            result.SetSuccess(rtnData);
            return result;
        }

        public DataResult<NotifyContent> GetNotifyContentByKey(string NotifyKey)
        {
            var result = new DataResult<NotifyContent>();
            result.SetError();

            var rtnData = _notifyManageRepository.GetNotifyContentByKey(NotifyKey);
            if (rtnData == null)
            {
                return result;
            }

            result.SetSuccess(rtnData);
            return result;
        }
        #endregion

        #region Generate
        public NotifyContent Generate(long NotifyID, object bodyArgs = null, object titleArgs = null)
        {
            var d = _notifyManageRepository.GetNotifyContent(NotifyID);
            if (d == null) throw new Exception("Notify不存在");

            var layouts = GetLayouts(d.LayoutID);
            Dictionary<string, string> dictTitle = null;
            Dictionary<string, string> dictBody = null;
            GetDictArgs(d, ref dictBody, ref dictTitle, bodyArgs, titleArgs);
            return Generate(d, layouts, dictBody, dictTitle);
        }

        public NotifyContent Generate(string Notifykey, object bodyArgs = null, object titleArgs = null)
        {
            var d = _notifyManageRepository.GetNotifyContentByKey(Notifykey);
            if (d == null) throw new Exception("Notify不存在");

            var layouts = GetLayouts(d.LayoutID);
            Dictionary<string, string> dictTitle = null;
            Dictionary<string, string> dictBody = null;
            GetDictArgs(d, ref dictBody, ref dictTitle, bodyArgs, titleArgs);
            return Generate(d, layouts, dictBody, dictTitle);
        }

        public NotifyContent Generate(long NotifyID, Dictionary<string, string> dictBody = null, Dictionary<string, string> dictTitle = null)
        {
            var d = _notifyManageRepository.GetNotifyContent(NotifyID);
            if (d == null) throw new Exception("Notify不存在");

            var layouts = GetLayouts(d.LayoutID);
            return Generate(d, layouts, dictBody, dictTitle);
        }

        public NotifyContent Generate(string Notifykey, Dictionary<string, string> dictBody = null, Dictionary<string, string> dictTitle = null)
        {
            var d = _notifyManageRepository.GetNotifyContentByKey(Notifykey);
            if (d == null) throw new Exception("Notify不存在");

            var layouts = GetLayouts(d.LayoutID);
            return Generate(d, layouts, dictBody, dictTitle);
        }

        public void GetDictArgs(
            NotifyContent model,
            ref Dictionary<string, string> dictBody,
            ref Dictionary<string, string> dictTitle,
            object bodyArgs = null,
            object titleArgs = null
            )
        {
            if (bodyArgs == null && titleArgs == null) return;

            if (bodyArgs != null)
            {
                dictBody = new Dictionary<string, string>();

                foreach (System.Reflection.PropertyInfo property in bodyArgs.GetType().GetProperties())
                {
                    object value = property.GetValue(bodyArgs, null);

                    string strVaule = value == null ? string.Empty : value.ToString();

                    dictBody.Add(property.Name, strVaule);
                }
            }

            if (titleArgs != null)
            {
                dictTitle = new Dictionary<string, string>();

                foreach (System.Reflection.PropertyInfo property in titleArgs.GetType().GetProperties())
                {
                    object value = property.GetValue(titleArgs, null);

                    string strVaule = value == null ? string.Empty : value.ToString();

                    dictTitle.Add(property.Name, strVaule);
                }
            }
            else if (model.Title.Contains("{{"))
            {
                dictTitle = dictBody;
            }
        }

        private List<NotifyContent> GetLayouts(long LayoutID)
        {
            if (LayoutID <= 0) return null;

            long CurrentLayoutID = LayoutID;

            var layouts = new List<NotifyContent>();

            while (CurrentLayoutID > 0)
            {
                var layout = _notifyManageRepository.GetNotifyContent(CurrentLayoutID);

                if (layout == null) break;

                layouts.Add(layout);

                CurrentLayoutID = layout.LayoutID;
            }

            return layouts;
        }

        private NotifyContent Generate(NotifyContent d, List<NotifyContent> layouts, Dictionary<string, string> dictBody, Dictionary<string, string> dictTitle)
        {
            if (dictTitle != null)
            {
                foreach (string key in dictTitle.Keys)
                {
                    string TagKey = "{{" + key + "}}";

                    string strVaule = dictTitle[key];

                    d.Title = d.Title.Replace(TagKey, strVaule);
                }
            }

            if (dictBody != null)
            {
                foreach (string key in dictBody.Keys)
                {
                    string TagKey = "{{" + key + "}}";

                    string strVaule = dictBody[key];

                    d.Body = d.Body.Replace(TagKey, strVaule);
                }
            }

            d.BodyWithoutLayout = d.Body;

            if (layouts != null && layouts.Count > 0)
            {
                layouts.ForEach(layout =>
                {
                    d.Body = layout.Body.Replace("{{content}}", d.Body);
                });
            }

            return d;
        }
        #endregion
    }
}
