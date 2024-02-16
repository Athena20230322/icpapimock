using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Castle.Core.Internal;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models.Consts;

namespace ICP.Library.Repositories.MailLibrary
{
    using ICP.Library.Models.MailLibrary;
    using Infrastructure.Abstractions.DbUtil;
    using Infrastructure.Core.Models;
    using Models;

    public class MailSendRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;
        public MailSendRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        public class ResultMailModel : BaseResult
        {
            public int MailID { get; set; }
        }

        /// <summary>
        /// 新增Mail內容
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultMailModel AddMail(MailSendContent model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Mail);

            string sql = "EXEC ausp_Mail_AddMail_I";

            var args = new
            {
                model.MID,
                model.MailFrom,
                model.MailTo,
                model.Scc,
                model.SMTPIP,
                model.Source,
                model.ErrorMail,
                model.Subject,
                model.Body
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<ResultMailModel>(sql, args);
        }

        /// <summary>
        /// 更新郵件發送時間與狀態
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult UpdateSendMailDate(int MailID, int Status)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Mail);

            string sql = "EXEC ausp_Mail_UpdateMailSendDate_U";

            var args = new
            {
                MailID,
                Status
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);

        }

        /// <summary>
        /// 新增信件內容至DB
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultMailModel AddBodyMail(MailSendDTO model)
        {
            string MailToLsit = string.Empty;
            string SccList = string.Empty;
            string SbccList = string.Empty;

            //取出寄信地址.密件.副本List傳回分割(;)
            if (model.MailTo.Count > 1)
            {
                model.MailTo.ForEach(t => MailToLsit += t + ";");
            }
            else
            {
                MailToLsit = model.MailTo[0].ToString();
            }

            if (model.Scc != null)
            {
                model.Scc.ForEach(t => SccList += t + ";");
            }
            if (model.Sbcc != null)
            {
                model.Sbcc.ForEach(t => SbccList += t + ";");
            }

            MailSendContent addMail = new MailSendContent
            {
                MID = model.MID,
                Body = model.Body,
                MailTo = MailToLsit,
                Scc = SccList,
                Sbcc = SbccList,
                Subject = model.Subject,
                MailFrom = model.MailFrom,
                SMTPIP = model.SMTPIP,
                Source = model.Source,
                ErrorMail = model.ErrorMail,

            };

            return AddMail(addMail);

        }

        /// <summary>
        /// 檢查信件模型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult CheckMailResult(MailSendDTO model)
        {
            BaseResult result =new BaseResult();
            result.SetError();

            #region 檢查SMTP

            if (model.SMTPIP.IsNullOrEmpty())
            {
                result.RtnMsg = $"{(nameof(model.SMTPIP))}為空";
                return result;
            }

            #endregion

            #region 檢查收信者
            if (!(model.MailTo.IsNullOrEmpty()))
            {
                foreach (var to in model.MailTo)
                {
                    if (!(Regex.IsMatch(to, RegexConst.Email)))
                    {
                        result.RtnMsg = $"{(nameof(model.MailTo))}格式錯誤,{to}";
                        return result;
                    }
                }  
            }
            else
            {
                result.RtnMsg = $"{(nameof(model.MailTo))}為空";
                return result;
            }
            #endregion

            #region 檢查寄信者

            if (model.MailFrom.IsNullOrEmpty())
            {
                result.RtnMsg = $"{(nameof(model.MailFrom))}為空";
                return result; 
            }

            #endregion
            
            #region 檢查副本
            
            if (!(model.Scc.IsNullOrEmpty()))
            {
                foreach (var to in model.Scc)
                {
                    if (!(Regex.IsMatch(to, RegexConst.Email)))
                    {
                        result.RtnMsg = $"{(nameof(model.Scc))}格式錯誤,{to}";
                        return result;
                    }
                }  
            }
            #endregion

            #region 檢查密件
            if (!(model.Sbcc.IsNullOrEmpty()))
            {
                foreach (var to in model.Sbcc)
                {
                    if (!(Regex.IsMatch(to, RegexConst.Email)))
                    {
                        result.RtnMsg = $"{(nameof(model.Sbcc))}格式錯誤,{to}";
                        return result;
                    }
                }  
            }
            #endregion

            #region 檢查主旨

            if (model.Subject.IsNullOrEmpty())
            {
                result.RtnMsg = $"{(nameof(model.Subject))}為空";
                return result;  
            }

            #endregion

            result.SetSuccess();
            return result;
        }
    }
}
