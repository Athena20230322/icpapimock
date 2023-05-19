using ICP.Host.Middleware.JCIC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace ICP.Host.Middleware.JCIC.Services
{
    using Repositories;
    using Infrastructure.Core.Models;
    using JCICWS;

    public class PersonalAuthService
    {
        PersonalAuthRepository _personalAuthRepository;
        ConfigRepository _configRepository;

        public PersonalAuthService(
            PersonalAuthRepository personalAuthRepository,
            ConfigRepository configRepository
            )
        {
            _personalAuthRepository = personalAuthRepository;
            _configRepository = configRepository;
        }

        #region 呼叫JCIC.wsdl
        /// <summary>
        /// P33驗證
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public P33AuthResponse P33Query(P33Auth model)
        {
            P33AuthResponse response;

            if (_configRepository.Switch == "0")
            {
                response = new P33AuthResponse
                {
                    Return_Code = 1,
                    Return_Msg = "成功",
                    Code = "",
                    Msg = "",
                    SessionID = "",
                    IsPass = 1,
                    DataCount = 1,
                    DataList = ""
                };
                return response;
            }
            else if (_configRepository.Switch == "2")
            {
                response = new P33AuthResponse
                {
                    Return_Code = 0,
                    Return_Msg = "身分證號/統一編號格式錯誤",
                    Code = "",
                    Msg = "",
                    SessionID = "",
                    IsPass = 0,
                    DataCount = 1,
                    DataList = ""
                };
                return response;
            }

            EPaymentImplClient _jcicClient = new EPaymentImplClient();

            RtnOutLoginBean loginResult = _jcicClient.Login(_configRepository.JCICAccount, _configRepository.JCICPassword);

            if (loginResult.code != "0000")
            {
                response = new P33AuthResponse
                {
                    Return_Code = 299999,
                    Return_Msg = "API異常",
                    Code = loginResult.code,
                    Msg = loginResult.msg
                };
                return response;
            }

            RtnOutP33Bean p33Result = _jcicClient.P33Query(loginResult.sessionId, model.IDNO.ToUpper());

            if (p33Result.code != "0000")
            {
                response = new P33AuthResponse
                {
                    Return_Code = 288888,
                    Return_Msg = "API錯誤"
                };
                return response;
            }

            response = new P33AuthResponse
            {
                Return_Code = 1,
                Return_Msg = "成功",
                Code = p33Result.code,
                Msg = p33Result.msg,
                SessionID = loginResult.sessionId,
                IsPass = (short)((this.checkP33IsPass(p33Result.vam033) == true) ? 1 : 0),
                DataCount = (p33Result.vam033 == null || p33Result.vam033.Length == 0) ? 0 : p33Result.vam033.Length,
                DataList = this.vam033toJsonString(p33Result.vam033)
            };

            if (response.IsPass != 1)
            {
                response.Return_Code = 200013;
                response.Return_Msg = "您未通過聯徵中心身分資料驗證，系統已將您的帳號暫時停權，如有任何疑問請與客服人員聯繫0800-233-888";
            }

            return response;
        }

        /// <summary>
        /// P11驗證
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public P11AuthResponse P11Query(P11Auth model)
        {
            P11AuthResponse response;

            if (_configRepository.Switch == "0")
            {
                response = new P11AuthResponse
                {
                    Return_Code = 1,
                    Return_Msg = "成功",
                    Code = "",
                    Msg = "",
                    SessionID = "",
                    IsPass = 1,
                    Answer = ""
                };
                return response;
            }
            else if (_configRepository.Switch == "2")
            {
                response = new P11AuthResponse
                {
                    Return_Code = 0,
                    Return_Msg = "身分證號/統一編號格式錯誤",
                    Code = "",
                    Msg = "",
                    SessionID = "",
                    IsPass = 0,
                    Answer = ""
                };
                return response;
            }

            EPaymentImplClient _jcicClient = new EPaymentImplClient();

            RtnOutLoginBean loginResult = _jcicClient.Login(_configRepository.JCICAccount, _configRepository.JCICPassword);
            if (loginResult.code != "0000")
            {
                response = new P11AuthResponse
                {
                    Return_Code = 199999,
                    Return_Msg = "API異常",
                    Code = loginResult.code,
                    Msg = loginResult.msg
                };
                return response;
            }

            RtnOutP11Bean p11Result = _jcicClient.P11Query(loginResult.sessionId, new ParamInP11Bean
            {
                idn = model.IDNO.ToUpper(),
                issueDate = model.IssueDate,
                issueType = model.IssueType.ToString(),
                birthDate = model.BirthDate,
                picFree = model.PicFree.ToString(),
                issueLoc = model.IssueLoc.PadRight(8, '0')
            });
            if (p11Result.code != "0000")
            {
                response = new P11AuthResponse
                {
                    Return_Code = 188888,
                    Return_Msg = "API錯誤"
                };
                return response;
            }

            response = new P11AuthResponse
            {
                Return_Code = 1,
                Return_Msg = "成功",
                Code = p11Result.code,
                Msg = p11Result.msg,
                SessionID = loginResult.sessionId,
                IsPass = (short)((p11Result.vas011 != null && p11Result.vas011[0].answer == "Y") ? 1 : 0),
                Answer = (p11Result.vas011 != null && p11Result.vas011.Length > 0) ? p11Result.vas011[0].answer : string.Empty,
                Result = (p11Result.vas011 != null && p11Result.vas011.Length > 0) ? p11Result.vas011[0].result : string.Empty
            };

            if (response.IsPass != 1)
            {
                response.Return_Code = 200014;
                response.Return_Msg = "請重新確認您輸入的資料，身分證資訊驗證每日僅限2次，如達錯誤次數上限，需隔日再行驗證";
            }

            return response;
        }
        #endregion

        #region 資料驗證
        public BaseResult VerifyP33Auth(P33Auth model)
        {
            var result = new BaseResult
            {
                RtnCode = 1,
                RtnMsg = "成功"
            };
            if (!RegexService.VerifyIDNO(model.IDNO) && !RegexService.VerifyUniformID(model.IDNO))
            {
                result.RtnCode = 0;
                result.RtnMsg = "身分證/居留證字號格式錯誤";
            }

            return result;
        }

        public BaseResult VerifyP11Auth(P11Auth model)
        {
            var result = new BaseResult
            {
                RtnCode = 1,
                RtnMsg = "成功"
            };
            if (!RegexService.VerifyIDNO(model.IDNO))
            {
                result.RtnCode = 0;
                result.RtnMsg = "身分證/居留證字號格式錯誤";
            }
            else if (!RegexService.VerifyYYYMMDD(model.IssueDate))
            {
                result.RtnCode = 0;
                result.RtnMsg = "領補換日期格式錯誤";
            }
            else if (!RegexService.VerifyYYYMMDD(model.BirthDate))
            {
                result.RtnCode = 0;
                result.RtnMsg = "出生日期格式錯誤";
            }
            else if (!RegexService.VerifyIssueLoc(model.IssueLoc))
            {
                result.RtnCode = 0;
                result.RtnMsg = "領補換地點格式錯誤";
            }

            return result;
        }
        #endregion

        #region DB
        public LogResult AddAuthP33Log(P33Auth model)
        {
            return _personalAuthRepository.AddAuthP33Log(model);
        }

        public LogResult AddAuthP11Log(P11Auth model)
        {
            return _personalAuthRepository.AddAuthP11Log(model);
        }

        public BaseResult UpdateAuthP33Log(P33AuthResponse model)
        {
            return _personalAuthRepository.UpdateAuthP33Log(model);
        }

        public BaseResult UpdateAuthP11Log(P11AuthResponse model)
        {
            return _personalAuthRepository.UpdateAuthP11Log(model);
        }
        #endregion

        #region Private Method
        /// <summary>
        /// 檢查是否驗證通過
        /// </summary>
        /// <param name="vam033"></param>
        /// <returns></returns>
        private bool checkP33IsPass(VAM033Bean[] vam033)
        {
            if (vam033 == null || vam033.Length == 0)
            {
                //沒案件 => 驗證通過
                return true;
            }

            //現在日期轉民國年
            bool isPass = true;
            string nowChineseDate = string.Format("{0}{1}{2}", (DateTime.Now.Year - 1911).ToString("D3"), DateTime.Now.Month.ToString("D2"), DateTime.Now.Day.ToString("D2"));

            //判斷查到的案件是否全都已過警示解除日期
            foreach (var item in vam033)
            {
                //  若警示帳戶解除原因為空的，表示案件尚未解除，直接回驗證失敗
                //  反之警示帳戶解除原因有值，就判斷解除日期是否小於今天  add by Gray 2016.05.04
                if (string.IsNullOrEmpty(item.relCode))
                {
                    isPass = false;
                }
                else if (!string.IsNullOrEmpty(item.relCode) && !string.IsNullOrEmpty(item.relDate) && Convert.ToInt32(nowChineseDate) <= Convert.ToInt32(item.relDate))
                {
                    isPass = false;
                }
            }
            return isPass;
        }

        /// <summary>
        /// 將P33的VAM033 array轉為P33AuthDataModel List後回傳JSON String
        /// </summary>
        /// <param name="arrVam033"></param>
        /// <returns></returns>
        private string vam033toJsonString(VAM033Bean[] arrVam033)
        {
            List<P33AuthDataModel> listP33AuthData = new List<P33AuthDataModel>();
            if (arrVam033 != null && arrVam033.Length > 0)
            {
                foreach (var item in arrVam033)
                {
                    listP33AuthData.Add(new P33AuthDataModel()
                    {
                        TYPE = item.type,
                        CRI_PLACE = item.criPlace,
                        CRI_DATE = item.criDate,
                        DOC_DATE = item.docDate,
                        CNAME = item.cname,
                        INVOICE_NO = item.invoiceNo,
                        REMARK_1 = item.remark,
                        REMARK_2 = "",
                        REMARK_3 = "",
                        REMARK_4 = "",
                        DOC_NAME = item.docName,
                        REL_CODE = item.relCode,
                        REL_REASON = item.relReason,
                        REL_DATE = item.relDate,
                        FILLER = item.filler
                    });
                }
            }
            return new JavaScriptSerializer().Serialize(listP33AuthData);
        }
        #endregion
    }
}