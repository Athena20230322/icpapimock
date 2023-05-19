using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Smtp;

namespace ICP.Library.Services.MailLibrary
{
    using Repositories;
    using Infrastructure.Core.Extensions;
    using Infrastructure.Core.Models;
    using Library.Models.MailLibrary;
    using Library.Repositories.MailLibrary;
    
    public class MailManageService
    {
        MailManageRepository _mailManageRepository;
        
        public MailManageService(
            MailManageRepository mailManageRepository
            )
        {
            _mailManageRepository = mailManageRepository;

            dictKeys = new Dictionary<string, MailContent>();
            dictIDs = new Dictionary<long, MailContent>();
        }

        #region Query
        public DataResult<MailContent> GetMailContent(long MailID)
        {
            var result = new DataResult<MailContent>();
            result.SetError();

            if (dictIDs.ContainsKey(MailID))
            {
                result.SetSuccess(dictIDs[MailID]);
                return result;
            }

            var rtnData = _mailManageRepository.GetMailContent(MailID);
            if (rtnData == null)
            {
                return result;
            }

            dictIDs.Add(rtnData.MailID, rtnData);
            dictKeys.Add(rtnData.MailKey, rtnData);

            result.SetSuccess(rtnData);
            return result;
        }

        public DataResult<MailContent> GetMailContentByKey(string MailKey)
        {
            var result = new DataResult<MailContent>();
            result.SetError();

            if (dictKeys.ContainsKey(MailKey))
            {
                result.SetSuccess(dictKeys[MailKey]);
                return result;
            }

            var rtnData = _mailManageRepository.GetMailContentByKey(MailKey);
            if (rtnData == null)
            {
                return result;
            }

            dictKeys.Add(rtnData.MailKey, rtnData);
            dictIDs.Add(rtnData.MailID, rtnData);

            result.SetSuccess(rtnData);
            return result;
        }
        #endregion

        #region Generate
        private Dictionary<string, MailContent> dictKeys { get; set; }
        private Dictionary<long, MailContent> dictIDs { get; set; }

        public MailContent Generate(long MailID, object bodyArgs = null, object titleArgs = null)
        {
            var getResult = GetMailContent(MailID);
            if (!getResult.IsSuccess) throw new Exception("Mail不存在");
            var d = getResult.RtnData;

            Dictionary<string, string> dictTitle = null;
            Dictionary<string, string> dictBody = null;
            GetDictArgs(d, ref dictBody, ref dictTitle, bodyArgs, titleArgs);

            return Generate(d, dictBody, dictTitle);
        }

        public MailContent Generate(string Mailkey, object bodyArgs = null, object titleArgs = null)
        {
            var getResult = GetMailContentByKey(Mailkey);
            if (!getResult.IsSuccess) throw new Exception("Mail不存在");
            var d = getResult.RtnData;

            Dictionary<string, string> dictTitle = null;
            Dictionary<string, string> dictBody = null;
            GetDictArgs(d, ref dictBody, ref dictTitle, bodyArgs, titleArgs);

            return Generate(d, dictBody, dictTitle);
        }

        public MailContent Generate(long MailID, Dictionary<string, string> dictBody = null, Dictionary<string, string> dictTitle = null)
        {
            var getResult = GetMailContent(MailID);
            if (!getResult.IsSuccess) throw new Exception("Mail不存在");
            var d = getResult.RtnData;

            return Generate(d, dictBody, dictTitle);
        }

        public MailContent Generate(string Mailkey, Dictionary<string, string> dictBody = null, Dictionary<string, string> dictTitle = null)
        {
            var getResult = GetMailContentByKey(Mailkey);
            if (!getResult.IsSuccess) throw new Exception("Mail不存在");
            var d = getResult.RtnData;

            return Generate(d, dictBody, dictTitle);
        }

        private void GetDictArgs(
            MailContent model,
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

        private void GetLayouts(ref MailContent d)
        {
            if (d.Layout != null || d.LayoutID <= 0) return;

            var getResult = GetMailContent(d.LayoutID);

            if (!getResult.IsSuccess) return;

            var layout = getResult.RtnData;

            if (layout.LayoutID > 0)
            {
                GetLayouts(ref layout);
            }

            d.Layout = layout;
        }

        private MailContent Generate(MailContent template, Dictionary<string, string> dictBody, Dictionary<string, string> dictTitle)
        {
            GetLayouts(ref template);

            var d = new MailContent
            {
                Title = template.Title,
                Body = template.Body
            };

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

            if (d.Layout != null)
            {
                var L = d.Layout;
                while (L != null)
                {
                    d.Body = L.Body.Replace("{{content}}", d.Body);

                    L = L.Layout;
                }
            }

            return d;
        }
        #endregion
    }
}
