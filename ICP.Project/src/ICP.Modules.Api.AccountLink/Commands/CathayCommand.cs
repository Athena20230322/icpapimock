using AutoMapper;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Helpers;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.AccountLinkApi.Enums;
using ICP.Library.Models.MemberModels;
using ICP.Library.Services.MemberServices;
using ICP.Modules.Api.AccountLink.Enums;
using ICP.Modules.Api.AccountLink.Models;
using ICP.Modules.Api.AccountLink.Models.Cathay;
using ICP.Modules.Api.AccountLink.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;

namespace ICP.Modules.Api.AccountLink.Commands
{
    /// <summary>
    /// 國泰世華 AccountLink
    /// </summary>
    class CathayCommand : BaseACLinkCommand
    {
        private readonly CathayService _cathayService = null;
        private readonly LibMemberBankService _libMemberBankService = null;
        private readonly ILogger _logger = null;

        private readonly BankType _bankType = BankType.Cathay;

        public CathayCommand(
            CathayService cathayService,
            LibMemberBankService libMemberBankService,
            ILogger<CathayCommand> logger
            )
        {
            _cathayService = cathayService;
            _logger = logger;
            _libMemberBankService = libMemberBankService;
        }

        public override string Test(string json)
        {
            #region  測試 綁定結果通知 中

            //BankBindReplyReq model = new BankBindReplyReq
            //{
            //    header = new BankHeaderModel
            //    {
            //        msgid = "ALSN001BINDING",
            //        sourcechannel = "",
            //        returncode = "0000",
            //        returndesc = "交易成功",
            //        txnseq = "P0012018080812345678",
            //        fuseID = "65001386338922647828"
            //    },
            //    sendMsgTime = "20180808080808",
            //    cooPerAtor = "P001",
            //    mbrActNo = "987654321",
            //    mbrIdno = "8D07C71DFA170AD70107FDE0A93F1766",
            //    bnkActNo = "465FE9D50881AA47559F41CC67A42126",
            //    digestHash = "8A52DD9D979A3BA2722792230835C2272E80353154F9544AEAE481DC9143675D"
            //};

            //var result = _cathayService.Post(@"http://localhost:50938/AccountLink/CathayBindResult", model);

            #endregion

            #region  測試 簽章 中

            string ss = "1234567890";

            var result = _cathayService.Sign(ss);

            #endregion

            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// 綁定
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public override string ACLinkBind(string json)
        {
            ApiType apiType = ApiType.ACLinkBind; //api類型

            ACLinkBindModel sourceModel = new ACLinkBindModel();//api執行參數
            BankBindReq postReqModel = new BankBindReq();//post請求檔
            BankBindRes postResModel = new BankBindRes();//post回應檔
            DataResult rtnResult = new DataResult();//最後回傳結果

            try
            {
                #region 來源資料 轉Model

                var parseSourceResult = _cathayService.ParseToModel<ACLinkBindModel>(json);

                if (!parseSourceResult.IsSuccess)
                {
                    rtnResult.SetCode(parseSourceResult.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                sourceModel = parseSourceResult.RtnData;

                #endregion

                #region 檢查 api執行是否逾時

                var chkTimeoutResult = _cathayService.CheckTimeout(sourceModel.Timestamp);

                if (!chkTimeoutResult.IsSuccess)
                {
                    rtnResult.SetCode(chkTimeoutResult.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                #endregion

                #region 建立 請求Model 並記log(db)

                var createResult = _cathayService.CreateBindModel(apiType, sourceModel);

                if (!createResult.IsSuccess)
                {
                    rtnResult.SetCode(createResult.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                postReqModel = createResult.RtnData;

                //組完 傳送請求資料 先記log(db)
                _cathayService.AddSendLog(apiType, postReqModel, sourceModel.MID);

                #endregion

                #region 驗證 請求Model

                var validateFieldResult = _cathayService.ValidateField(postReqModel);

                if (!validateFieldResult.IsSuccess)
                {
                    rtnResult.SetCode(validateFieldResult.RtnCode);
                    rtnResult.RtnMsg = validateFieldResult.RtnMsg;
                    return JsonConvert.SerializeObject(rtnResult);
                }

                #endregion


                if (_cathayService.isMockBank())
                {
                    #region Mock

                    postResModel = new BankBindRes
                    {
                        Header = new BankHeaderModel
                        {
                            msgid = "ALSM001BINDING",
                            sourcechannel = "",
                            returncode = "0000",
                            returndesc = "交易成功(test)",
                            txnseq = postReqModel.header.txnseq,
                            fuseID = DateTime.Now.ToString("yyyyMMddHHmmss") + new System.Random().Next(999999).ToString().PadLeft(6, '0')
                        },
                        ReturnMsgTime = DateTime.Now.ToString("yyyyMMddHHmmss"),
                        CooPerAtor = postReqModel.cooPerAtor,
                        MbrActNo = postReqModel.mbrActNo,
                        DigestHash = "xxx",
                        CubWebPage = $"{ _cathayService.BindResultApiUrl}Mock?" +
                            $"txnseq={postReqModel.header.txnseq}" +
                            $"&fuseID=R{postReqModel.header.txnseq.Substring(1)}" +
                            $"&mbrActNo={postReqModel.mbrActNo}" +
                            $"&mbrIdno={postReqModel.mbrIdno}"
                    };

                    #endregion
                }
                else
                {
                    #region 傳送 至銀行 並取得回應

                    Dictionary<string, string> headers = new Dictionary<string, string>
                    {
                        { "x-ibm-client-id", _cathayService.ACLinkClientID }
                    };

                    var postResult = _cathayService.PostToBankWithJson(apiType, postReqModel, headers);

                    if (!postResult.IsSuccess)
                    {
                        rtnResult.SetCode(postResult.RtnCode);
                        return JsonConvert.SerializeObject(rtnResult);
                    }

                    #endregion

                    #region 回應資料 轉Model 並記log(db)

                    var parseResResult = _cathayService.ParseToModel<BankBindRes>(postResult.RtnData);

                    if (!parseResResult.IsSuccess)
                    {
                        rtnResult.SetCode(parseResResult.RtnCode);
                        return JsonConvert.SerializeObject(rtnResult);
                    }

                    postResModel = parseResResult.RtnData;

                    postResModel.CooPerAtor = postResModel.CooPerAtor ?? postReqModel.cooPerAtor;
                    postResModel.Header.msgid = postResModel.Header.msgid ?? postReqModel.header.msgid;
                    postResModel.Header.txnseq = postResModel.Header.txnseq ?? postReqModel.header.txnseq;
                    postResModel.Header.returndesc = HttpUtility.UrlDecode(postResModel.Header.returndesc);

                    //組完 傳送回應資料 先記log(db)
                    //_cathayService.AddReceiveLog(apiType, postResModel, sourceModel.MID);

                    #endregion
                }

                _cathayService.AddReceiveLog(apiType, postResModel, sourceModel.MID);


                #region 驗證 銀行回應資料

                if ((postResModel.Header.returncode == "0000" || postResModel.Header.returncode == "AL11" || !string.IsNullOrWhiteSpace(postResModel.DigestHash)) && !_cathayService.isMockBank())
                {
                    string sCheckData = $"{postResModel.Header.txnseq}{postResModel.CooPerAtor}{postResModel.MbrActNo}{postResModel.ReturnMsgTime}";

                    var validateHashResult = _cathayService.ValidateHash(postResModel.DigestHash, sCheckData);

                    if (!validateHashResult.IsSuccess)
                    {
                        rtnResult.SetCode(validateHashResult.RtnCode);
                        return JsonConvert.SerializeObject(rtnResult);
                    }
                }

                #endregion

                #region 判斷 銀行回應狀態 (成功:returncode=0000)

                string rtncode = postResModel.Header.returncode;

                if (rtncode == "0000")
                {
                    rtnResult.SetSuccess();
                }
                else if (rtncode == "AL11")
                {
                    rtnResult.SetCode(7441);//連結帳戶已綁定
                }
                else if (rtncode == "AL67")
                {
                    rtnResult.SetCode(7497);//銀行系統維護中
                }
                else if (!rtncode.StartsWith("AL"))
                {
                    rtnResult.SetCode(7498);//銀行端非預期錯誤
                }
                else
                {
                    rtnResult.SetCode(7440, _cathayService.GetApiName(apiType));//xx失敗
                }

                if (rtncode != "0000" && rtncode != "AL11")
                {
                    _logger.Trace(rtnResult.RtnMsg, rtnResult);
                }

                #endregion
            }
            catch (Exception ex)
            {
                rtnResult.SetCode(7499);//系統非預期錯誤
                _logger.Trace(ex, rtnResult.RtnMsg, rtnResult);
            }

            if (rtnResult.IsSuccess)
            {
                rtnResult.RtnData = new { URL = postResModel.CubWebPage };
            }

            return JsonConvert.SerializeObject(rtnResult);
        }

        /// <summary>
        /// 綁定結果通知(背景)(銀行->業者)
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public override string ACLinkBindResult(string json)
        {
            ApiType apiType = ApiType.BindApiResult; //api類型

            BankBindReplyReq sourceModel = new BankBindReplyReq();//post請求檔
            BankBindReplyRes postResModel = new BankBindReplyRes();//post回應檔
            DataResult rtnResult = new DataResult();//最後回傳結果

            long mid = 0;

            try
            {
                #region 來源資料 轉Model 並記log(db)

                var parseSourceResult = _cathayService.ParseToModel<BankBindReplyReq>(json);

                if (!parseSourceResult.IsSuccess)
                {
                    rtnResult.SetCode(parseSourceResult.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                if (_cathayService.isMockBank())
                {
                    parseSourceResult.RtnData.bnkActNo = _cathayService.TripleDESEncrypt(parseSourceResult.RtnData.bnkActNo);
                }

                sourceModel = parseSourceResult.RtnData;

                //查詢 申請綁定的記錄
                var getBindLog = _cathayService.GetBindLog(new ACLinkBindLogQryModel
                {
                    ApiType = ApiType.ACLinkBind.ToString(),
                    Txnseq = sourceModel.header.txnseq,
                    MbrActNo = sourceModel.mbrActNo
                });

                if (!getBindLog.IsSuccess)
                {
                    rtnResult.SetCode(getBindLog.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                mid = getBindLog.RtnData.MID;

                ////組完 來源資料 先記log(db)
                _cathayService.AddReceiveLog(apiType, sourceModel, mid);

                #endregion

                #region 驗證 Model

                var validateFieldResult = _cathayService.ValidateField(sourceModel);

                if (!validateFieldResult.IsSuccess)
                {
                    rtnResult.SetCode(validateFieldResult.RtnCode);
                    rtnResult.RtnMsg = validateFieldResult.RtnMsg;
                    return JsonConvert.SerializeObject(rtnResult);
                }

                #endregion

                #region 驗證 銀行通知資料

                if (sourceModel.header.returncode == "0000" || !string.IsNullOrWhiteSpace(sourceModel.digestHash))
                {
                    string bnkActNo = _cathayService.TripleDESDecrypt(sourceModel.bnkActNo);
                    string sCheckData = $"{sourceModel.header.txnseq}{sourceModel.mbrActNo}{bnkActNo}{sourceModel.sendMsgTime}";

                    var validateHashResult = _cathayService.ValidateHash(sourceModel.digestHash, sCheckData);

                    if (!validateHashResult.IsSuccess)
                    {
                        rtnResult.SetCode(validateHashResult.RtnCode);
                        return JsonConvert.SerializeObject(rtnResult);
                    }
                }

                #endregion

                #region 判斷 銀行通知狀態 (成功:returncode=0000)

                string rtncode = sourceModel.header.returncode;

                postResModel.Header = AutoMapper.Mapper.Map<BankHeaderModel>(sourceModel.header);
                rtnResult.RtnData = postResModel;

                if (rtncode == "0000")
                {
                    rtnResult.SetSuccess();
                }
                else if (rtncode == "AL11")
                {
                    rtnResult.SetCode(7441);//連結帳戶已綁定
                }
                else if (rtncode == "AL67")
                {
                    rtnResult.SetCode(7497);//銀行系統維護中
                }
                else if (!rtncode.StartsWith("AL"))
                {
                    rtnResult.SetCode(7498);//銀行端非預期錯誤
                }
                else
                {
                    rtnResult.SetCode(7440, _cathayService.GetApiName(apiType));//xx失敗
                }

                if (rtncode != "0000")
                {
                    _logger.Trace(rtnResult.RtnMsg, rtnResult);
                }

                #endregion

                #region 新增 綁定  

                string INDTAccount = _cathayService.GetINDTAccount(_bankType);

                var addResult = _cathayService.AddACLinkInfo(new ACLinkAddModel
                {
                    MID = mid,
                    INDTAccount = INDTAccount,
                    BankCode = $"{(int)_bankType:000}",
                    BankAccount = _cathayService.TripleDESDecrypt(sourceModel.bnkActNo),
                    MsgNo = _cathayService.GetMsgNo(_bankType),
                    Status = (sourceModel.header.returncode == "0000" || sourceModel.header.returncode == "AL11") ? 1 : 3,//1:綁定 2:解綁 3:驗證失敗 //AL11:銀行端為已綁定狀態
                    IsDefault = false
                });

                if (!addResult.IsSuccess)
                {
                    rtnResult.SetCode(7440, _cathayService.GetApiName(apiType));//xx失敗
                    _logger.Trace(rtnResult.RtnMsg, rtnResult);
                }
                else
                {
                    rtnResult = _cathayService.CreateBindResultModel(apiType, sourceModel);
                }

                #endregion

                #region 更新 會員銀行帳號驗證狀態  

                var updateResult = _libMemberBankService.UpdateMemberBankAccountStatus(new UpdateBankAccountStatusModel
                {
                    MID = mid,
                    Category = 1,
                    BankCode = $"{(int)_bankType:000}",
                    BankAccount = sourceModel.bnkActNo,
                    AccountStatus = (byte)(addResult.IsSuccess ? 1 : 2),
                    INDTAccount = INDTAccount
                });

                if (!updateResult.IsSuccess)
                {
                    _logger.Trace(updateResult.RtnMsg, updateResult);
                }

                #endregion

            }
            catch (Exception ex)
            {
                rtnResult.SetCode(7499);//系統非預期錯誤
                _logger.Trace(ex, rtnResult.RtnMsg);
            }

            return JsonConvert.SerializeObject(rtnResult.RtnData);
        }

        /// <summary>
        /// 解綁
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public override string ACLinkCancel(string json)
        {
            ApiType apiType = ApiType.ACLinkCancel; //api類型

            ACLinkCancelModel sourceModel = new ACLinkCancelModel();//api執行參數
            BankUnbindReq postReqModel = new BankUnbindReq();//post請求檔
            BankUnbindRes postResModel = new BankUnbindRes();//post回應檔
            DataResult rtnResult = new DataResult();//最後回傳結果

            try
            {
                #region 來源資料 轉Model

                var parseSourceResult = _cathayService.ParseToModel<ACLinkCancelModel>(json);

                if (!parseSourceResult.IsSuccess)
                {
                    rtnResult.SetCode(parseSourceResult.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                sourceModel = parseSourceResult.RtnData;
                sourceModel.BankType = _bankType;

                #endregion

                #region 檢查 api執行是否逾時

                var chkTimeoutResult = _cathayService.CheckTimeout(sourceModel.Timestamp);

                if (!chkTimeoutResult.IsSuccess)
                {
                    rtnResult.SetCode(chkTimeoutResult.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                #endregion

                #region 撈取 綁定資料

                var infoQryModel = new ACLinkInfoQryModel
                {
                    MID = sourceModel.MID,
                    INDTAccount = sourceModel.INDTAccount,
                    BankType = sourceModel.BankType
                };

                var acInfo = _cathayService.GetACLinkInfo(infoQryModel);

                if (!acInfo.IsSuccess)
                {
                    rtnResult.SetCode(acInfo.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                sourceModel.BankAccount = acInfo.RtnData.BankAccount;

                #endregion

                #region 建立 請求Model 並記log(db)

                var createResult = _cathayService.CreateUnbindModel(apiType, sourceModel);

                if (!createResult.IsSuccess)
                {
                    rtnResult.SetCode(createResult.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                postReqModel = createResult.RtnData;

                //組完 傳送請求資料 先記log(db)
                _cathayService.AddSendLog(apiType, postReqModel, sourceModel.MID);

                #endregion

                #region 驗證 請求Model

                var validateFieldResult = _cathayService.ValidateField(postReqModel);

                if (!validateFieldResult.IsSuccess)
                {
                    rtnResult.SetCode(validateFieldResult.RtnCode);
                    rtnResult.RtnMsg = validateFieldResult.RtnMsg;
                    return JsonConvert.SerializeObject(rtnResult);
                }

                #endregion


                if (_cathayService.isMockBank())
                {
                    #region Mock

                    postResModel = new BankUnbindRes
                    {
                        Header = new BankHeaderModel
                        {
                            msgid = "ALSM002UNBIND",
                            sourcechannel = "",
                            returncode = "0000",
                            returndesc = "交易成功(test)",
                            txnseq = postReqModel.header.txnseq,
                            fuseID = DateTime.Now.ToString("yyyyMMddHHmmss") + new System.Random().Next(999999).ToString().PadLeft(6, '0')
                        },
                        ReturnMsgTime = DateTime.Now.ToString("yyyyMMddHHmmss"),
                        CooPerAtor = postReqModel.cooPerAtor,
                        MbrActNo = postReqModel.mbrActNo,
                        DigestHash = "xxxxx"
                    };

                    #endregion
                }
                else
                {
                    #region 傳送 至銀行 並取得回應

                    Dictionary<string, string> headers = new Dictionary<string, string>
                    {
                        { "x-ibm-client-id", _cathayService.ACLinkClientID }
                    };

                    var postResult = _cathayService.PostToBankWithJson(apiType, postReqModel, headers);

                    if (!postResult.IsSuccess)
                    {
                        rtnResult.SetCode(postResult.RtnCode);
                        return JsonConvert.SerializeObject(rtnResult);
                    }

                    #endregion

                    #region 回應資料 轉Model 並記log(db)

                    var parseResResult = _cathayService.ParseToModel<BankUnbindRes>(postResult.RtnData);

                    if (!parseResResult.IsSuccess)
                    {
                        rtnResult.SetCode(parseResResult.RtnCode);
                        return JsonConvert.SerializeObject(rtnResult);
                    }

                    postResModel = parseResResult.RtnData;

                    postResModel.CooPerAtor = postResModel.CooPerAtor ?? postReqModel.cooPerAtor;
                    postResModel.Header.msgid = postResModel.Header.msgid ?? postReqModel.header.msgid;
                    postResModel.Header.txnseq = postResModel.Header.txnseq ?? postReqModel.header.txnseq;
                    postResModel.Header.returndesc = HttpUtility.UrlDecode(postResModel.Header.returndesc);

                    //組完 傳送回應資料 先記log(db)
                    //_cathayService.AddReceiveLog(apiType, postResModel, sourceModel.MID);

                    #endregion
                }

                _cathayService.AddReceiveLog(apiType, postResModel, sourceModel.MID);


                #region 驗證 銀行回應資料

                if ((postResModel.Header.returncode == "0000" || !string.IsNullOrWhiteSpace(postResModel.DigestHash)) && !_cathayService.isMockBank())
                {
                    string sCheckData = $"{postResModel.Header.txnseq}{postResModel.CooPerAtor}{postResModel.MbrActNo}{postResModel.ReturnMsgTime}";

                    var validateHashResult = _cathayService.ValidateHash(postResModel.DigestHash, sCheckData);

                    if (!validateHashResult.IsSuccess)
                    {
                        rtnResult.SetCode(validateHashResult.RtnCode);
                        return JsonConvert.SerializeObject(rtnResult);
                    }
                }

                #endregion

                #region 判斷 銀行回應狀態 (成功:returncode=0000)

                string rtncode = postResModel.Header.returncode;

                if (rtncode == "0000")
                {
                    rtnResult.SetSuccess();
                }
                else if (rtncode == "AL13")
                {
                    rtnResult.SetCode(7442);//查無此綁定記錄
                }
                else if (rtncode == "AL67")
                {
                    rtnResult.SetCode(7497);//銀行系統維護中
                }
                else if (!rtncode.StartsWith("AL"))
                {
                    rtnResult.SetCode(7498);//銀行端非預期錯誤
                }
                else
                {
                    rtnResult.SetCode(7440, _cathayService.GetApiName(apiType));//xx失敗
                }

                if (rtncode != "0000")
                {
                    _logger.Trace(rtnResult.RtnMsg, rtnResult);
                }

                #endregion
            }
            catch (Exception ex)
            {
                rtnResult.SetCode(7499);//系統非預期錯誤
                _logger.Trace(ex, rtnResult.RtnMsg, rtnResult);
            }

            return JsonConvert.SerializeObject(rtnResult);
        }

        /// <summary>
        /// 付款
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public override string ACLinkPay(string json)
        {
            ApiType apiType = ApiType.ACLinkPay; //api類型

            ACLinkPayModel sourceModel = new ACLinkPayModel();//api執行參數
            BankPayReq postReqModel = new BankPayReq();//post請求檔
            BankPayRes postResModel = new BankPayRes();//post回應檔
            DataResult<ACLinkResultModel> rtnResult = new DataResult<ACLinkResultModel>();//最後回傳結果

            try
            {
                #region 來源資料 轉Model

                var parseSourceResult = _cathayService.ParseToModel<ACLinkPayModel>(json);

                if (!parseSourceResult.IsSuccess)
                {
                    rtnResult.SetCode(parseSourceResult.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                sourceModel = parseSourceResult.RtnData;
                sourceModel.BankType = _bankType;

                #endregion

                #region 檢查 api執行是否逾時

                var chkTimeoutResult = _cathayService.CheckTimeout(sourceModel.Timestamp);

                if (!chkTimeoutResult.IsSuccess)
                {
                    rtnResult.SetCode(chkTimeoutResult.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                #endregion

                #region 撈取 綁定資料

                var infoQryModel = new ACLinkInfoQryModel
                {
                    MID = sourceModel.MID,
                    INDTAccount = sourceModel.INDTAccount,
                    BankType = sourceModel.BankType
                };

                var acInfo = _cathayService.GetACLinkInfo(infoQryModel);

                if (!acInfo.IsSuccess)
                {
                    rtnResult.SetCode(acInfo.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                sourceModel.BankAccount = acInfo.RtnData.BankAccount;

                #endregion

                #region 建立 請求Model 並記log(db)

                var createResult = _cathayService.CreatePayModel(apiType, sourceModel);

                if (!createResult.IsSuccess)
                {
                    rtnResult.SetCode(createResult.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                postReqModel = createResult.RtnData;

                //組完 傳送請求資料 先記log(db)
                _cathayService.AddSendLog(apiType, postReqModel, sourceModel.MID);

                #endregion

                #region 驗證 請求Model

                var validateFieldResult = _cathayService.ValidateField(postReqModel);

                if (!validateFieldResult.IsSuccess)
                {
                    rtnResult.SetCode(validateFieldResult.RtnCode);
                    rtnResult.RtnMsg = validateFieldResult.RtnMsg;
                    return JsonConvert.SerializeObject(rtnResult);
                }

                #endregion


                if (_cathayService.isMockBank())
                {
                    #region Mock

                    postResModel = new BankPayRes
                    {
                        Header = new BankHeaderModel
                        {
                            msgid = "ALSD001PAYMENT",
                            sourcechannel = "",
                            returncode = "0000",
                            returndesc = "交易成功(test)",
                            txnseq = postReqModel.header.txnseq,
                            fuseID = DateTime.Now.ToString("yyyyMMddHHmmss") + new System.Random().Next(999999).ToString().PadLeft(6, '0')
                        },
                        ReturnMsgTime = DateTime.Now.ToString("yyyyMMddHHmmss"),
                        CooPerAtor = postReqModel.cooPerAtor,
                        MbrActNo = postReqModel.mbrActNo,
                        OrderNo = postReqModel.orderNo,
                        TxnAmt = postReqModel.txnAmt,
                        DigestHash = "xxxxx"
                    };

                    #endregion
                }
                else
                { 
                    #region 傳送 至銀行 並取得回應

                    Dictionary<string, string> headers = new Dictionary<string, string>
                    {
                        { "x-ibm-client-id", _cathayService.ACLinkClientID }
                    };

                    var postResult = _cathayService.PostToBankWithJson(apiType, postReqModel, headers);

                    if (!postResult.IsSuccess)
                    {
                        rtnResult.SetCode(postResult.RtnCode);
                        return JsonConvert.SerializeObject(rtnResult);
                    }

                    #endregion

                    #region 回應資料 轉Model 並記log(db)

                    var parseResResult = _cathayService.ParseToModel<BankPayRes>(postResult.RtnData);

                    if (!parseResResult.IsSuccess)
                    {
                        rtnResult.SetCode(parseResResult.RtnCode);
                        return JsonConvert.SerializeObject(rtnResult);
                    }

                    postResModel = parseResResult.RtnData;

                    postResModel.CooPerAtor = postResModel.CooPerAtor ?? postReqModel.cooPerAtor;
                    postResModel.Header.msgid = postResModel.Header.msgid ?? postReqModel.header.msgid;
                    postResModel.Header.txnseq = postResModel.Header.txnseq ?? postReqModel.header.txnseq;
                    postResModel.Header.returndesc = HttpUtility.UrlDecode(postResModel.Header.returndesc);

                    //組完 傳送回應資料 先記log(db)
                    //_cathayService.AddReceiveLog(apiType, postResModel, sourceModel.MID);

                    #endregion
                }

                _cathayService.AddReceiveLog(apiType, postResModel, sourceModel.MID);


                #region 驗證 銀行回應資料

                if ((postResModel.Header.returncode == "0000" || !string.IsNullOrWhiteSpace(postResModel.DigestHash)) && !_cathayService.isMockBank())
                {
                    string sCheckData = $"{postResModel.Header.txnseq}{postResModel.CooPerAtor}{postResModel.MbrActNo}{postResModel.TxnAmt}{postResModel.ReturnMsgTime}";

                    var validateHashResult = _cathayService.ValidateHash(postResModel.DigestHash, sCheckData);

                    if (!validateHashResult.IsSuccess)
                    {
                        rtnResult.SetCode(validateHashResult.RtnCode);
                        return JsonConvert.SerializeObject(rtnResult);
                    }
                }

                #endregion

                #region 判斷 銀行回應狀態 (成功:returncode=0000)

                string rtncode = postResModel.Header.returncode;

                rtnResult.RtnData = new ACLinkResultModel { BankTradeNo = postResModel.Header.txnseq };

                if (rtncode == "0000")
                {
                    rtnResult.SetSuccess();
                }
                else if (rtncode == "AL13")
                {
                    rtnResult.SetCode(7442);//查無此綁定記錄
                }
                else if (rtncode == "AL67")
                {
                    rtnResult.SetCode(7497);//銀行系統維護中
                }
                else if (!rtncode.StartsWith("AL"))
                {
                    rtnResult.SetCode(7498);//銀行端非預期錯誤
                }
                else
                {
                    rtnResult.SetCode(7440, _cathayService.GetApiName(apiType));//xx失敗
                }

                if (rtncode != "0000")
                {
                    _logger.Trace(rtnResult.RtnMsg, rtnResult);
                }

                #endregion
            }
            catch (Exception ex)
            {
                rtnResult.SetCode(7499);//系統非預期錯誤
                _logger.Trace(ex, rtnResult.RtnMsg, rtnResult);
            }

            return JsonConvert.SerializeObject(rtnResult);
        }

        /// <summary>
        /// 儲值
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public override string ACLinkDeposit(string json)
        {
            ApiType apiType = ApiType.ACLinkDeposit; //api類型

            ACLinkDepositModel sourceModel = new ACLinkDepositModel();//api執行參數
            BankDepositReq postReqModel = new BankDepositReq();//post請求檔
            BankDepositRes postResModel = new BankDepositRes();//post回應檔
            DataResult<ACLinkResultModel> rtnResult = new DataResult<ACLinkResultModel>();//最後回傳結果

            try
            {
                #region 來源資料 轉Model

                var parseSourceResult = _cathayService.ParseToModel<ACLinkDepositModel>(json);

                if (!parseSourceResult.IsSuccess)
                {
                    rtnResult.SetCode(parseSourceResult.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                sourceModel = parseSourceResult.RtnData;
                sourceModel.BankType = _bankType;

                #endregion

                #region 檢查 api執行是否逾時

                var chkTimeoutResult = _cathayService.CheckTimeout(sourceModel.Timestamp);

                if (!chkTimeoutResult.IsSuccess)
                {
                    rtnResult.SetCode(chkTimeoutResult.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                #endregion

                #region 撈取 綁定資料

                var infoQryModel = new ACLinkInfoQryModel
                {
                    MID = sourceModel.MID,
                    INDTAccount = sourceModel.INDTAccount,
                    BankType = sourceModel.BankType
                };

                var acInfo = _cathayService.GetACLinkInfo(infoQryModel);

                if (!acInfo.IsSuccess)
                {
                    rtnResult.SetCode(acInfo.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                sourceModel.BankAccount = acInfo.RtnData.BankAccount;

                #endregion

                #region 建立 請求Model 並記log(db)

                var createResult = _cathayService.CreateDepositModel(apiType, sourceModel);

                if (!createResult.IsSuccess)
                {
                    rtnResult.SetCode(createResult.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                postReqModel = createResult.RtnData;

                //組完 傳送請求資料 先記log(db)
                _cathayService.AddSendLog(apiType, postReqModel, sourceModel.MID);

                #endregion

                #region 驗證 請求Model

                var validateFieldResult = _cathayService.ValidateField(postReqModel);

                if (!validateFieldResult.IsSuccess)
                {
                    rtnResult.SetCode(validateFieldResult.RtnCode);
                    rtnResult.RtnMsg = validateFieldResult.RtnMsg;
                    return JsonConvert.SerializeObject(rtnResult);
                }

                #endregion


                if (_cathayService.isMockBank())
                {
                    #region Mock

                    postResModel = new BankDepositRes
                    {
                        Header = new BankHeaderModel
                        {
                            msgid = "ALSD002DEPOSIT",
                            sourcechannel = "",
                            returncode = "0000",
                            returndesc = "交易成功(test)",
                            txnseq = postReqModel.header.txnseq,
                            fuseID = DateTime.Now.ToString("yyyyMMddHHmmss") + new System.Random().Next(999999).ToString().PadLeft(6, '0')
                        },
                        ReturnMsgTime = DateTime.Now.ToString("yyyyMMddHHmmss"),
                        CooPerAtor = postReqModel.cooPerAtor,
                        MbrActNo = postReqModel.mbrActNo,
                        OrderNo = postReqModel.orderNo,
                        TxnAmt = postReqModel.txnAmt,
                        DigestHash = "xxxxx"
                    };

                    #endregion
                }
                else
                {
                    #region 傳送 至銀行 並取得回應

                    Dictionary<string, string> headers = new Dictionary<string, string>
                    {
                        { "x-ibm-client-id", _cathayService.ACLinkClientID }
                    };

                    var postResult = _cathayService.PostToBankWithJson(apiType, postReqModel, headers);

                    if (!postResult.IsSuccess)
                    {
                        rtnResult.SetCode(postResult.RtnCode);
                        return JsonConvert.SerializeObject(rtnResult);
                    }

                    #endregion

                    #region 回應資料 轉Model 並記log(db)

                    var parseResResult = _cathayService.ParseToModel<BankDepositRes>(postResult.RtnData);

                    if (!parseResResult.IsSuccess)
                    {
                        rtnResult.SetCode(parseResResult.RtnCode);
                        return JsonConvert.SerializeObject(rtnResult);
                    }

                    postResModel = parseResResult.RtnData;

                    postResModel.CooPerAtor = postResModel.CooPerAtor ?? postReqModel.cooPerAtor;
                    postResModel.Header.msgid = postResModel.Header.msgid ?? postReqModel.header.msgid;
                    postResModel.Header.txnseq = postResModel.Header.txnseq ?? postReqModel.header.txnseq;
                    postResModel.Header.returndesc = HttpUtility.UrlDecode(postResModel.Header.returndesc);

                    //組完 傳送回應資料 先記log(db)
                    //_cathayService.AddReceiveLog(apiType, postResModel, sourceModel.MID);

                    #endregion
                }

                _cathayService.AddReceiveLog(apiType, postResModel, sourceModel.MID);


                #region 驗證 銀行回應資料

                if ((postResModel.Header.returncode == "0000" || !string.IsNullOrWhiteSpace(postResModel.DigestHash)) && !_cathayService.isMockBank())
                {
                    string sCheckData = $"{postResModel.Header.txnseq}{postResModel.CooPerAtor}{postResModel.MbrActNo}{postResModel.TxnAmt}{postResModel.ReturnMsgTime}";

                    var validateHashResult = _cathayService.ValidateHash(postResModel.DigestHash, sCheckData);

                    if (!validateHashResult.IsSuccess)
                    {
                        rtnResult.SetCode(validateHashResult.RtnCode);
                        return JsonConvert.SerializeObject(rtnResult);
                    }
                }

                #endregion

                #region 判斷 銀行回應狀態 (成功:returncode=0000)

                string rtncode = postResModel.Header.returncode;

                rtnResult.RtnData = new ACLinkResultModel { BankTradeNo = postResModel.Header.txnseq };

                if (rtncode == "0000")
                {
                    rtnResult.SetSuccess();
                }
                else if (rtncode == "AL13")
                {
                    rtnResult.SetCode(7442);//查無此綁定記錄
                }
                else if (rtncode == "AL67")
                {
                    rtnResult.SetCode(7497);//銀行系統維護中
                }
                else if (!rtncode.StartsWith("AL"))
                {
                    rtnResult.SetCode(7498);//銀行端非預期錯誤
                }
                else
                {
                    rtnResult.SetCode(7440, _cathayService.GetApiName(apiType));//xx失敗
                }

                if (rtncode != "0000")
                {
                    _logger.Trace(rtnResult.RtnMsg, rtnResult);
                }

                #endregion
            }
            catch (Exception ex)
            {
                rtnResult.SetCode(7499);//系統非預期錯誤
                _logger.Trace(ex, rtnResult.RtnMsg, rtnResult);
            }

            return JsonConvert.SerializeObject(rtnResult);
        }

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public override string ACLinkRefund(string json)
        {
            ApiType apiType = ApiType.ACLinkRefund; //api類型

            ACLinkRefundModel sourceModel = new ACLinkRefundModel();//api執行參數
            BankRefundReq postReqModel = new BankRefundReq();//post請求檔
            BankRefundRes postResModel = new BankRefundRes();//post回應檔
            DataResult<ACLinkResultModel> rtnResult = new DataResult<ACLinkResultModel>();//最後回傳結果

            try
            {
                #region 來源資料 轉Model

                var parseSourceResult = _cathayService.ParseToModel<ACLinkRefundModel>(json);

                if (!parseSourceResult.IsSuccess)
                {
                    rtnResult.SetCode(parseSourceResult.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                sourceModel = parseSourceResult.RtnData;
                sourceModel.BankType = _bankType;

                #endregion

                #region 檢查 api執行是否逾時

                var chkTimeoutResult = _cathayService.CheckTimeout(sourceModel.Timestamp);

                if (!chkTimeoutResult.IsSuccess)
                {
                    rtnResult.SetCode(chkTimeoutResult.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                #endregion

                #region 撈取 綁定資料

                var infoQryModel = new ACLinkInfoQryModel
                {
                    MID = sourceModel.MID,
                    INDTAccount = sourceModel.INDTAccount,
                    BankType = sourceModel.BankType
                };

                var acInfo = _cathayService.GetACLinkInfo(infoQryModel);

                if (!acInfo.IsSuccess)
                {
                    rtnResult.SetCode(acInfo.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                sourceModel.BankAccount = acInfo.RtnData.BankAccount;

                #endregion

                #region 建立 請求Model 並記log(db)

                var createResult = _cathayService.CreateRefundModel(apiType, sourceModel);

                if (!createResult.IsSuccess)
                {
                    rtnResult.SetCode(createResult.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                postReqModel = createResult.RtnData;

                //組完 傳送請求資料 先記log(db)
                _cathayService.AddSendLog(apiType, postReqModel, sourceModel.MID);

                #endregion

                #region 驗證 請求Model

                var validateFieldResult = _cathayService.ValidateField(postReqModel);

                if (!validateFieldResult.IsSuccess)
                {
                    rtnResult.SetCode(validateFieldResult.RtnCode);
                    rtnResult.RtnMsg = validateFieldResult.RtnMsg;
                    return JsonConvert.SerializeObject(rtnResult);
                }

                #endregion


                if (_cathayService.isMockBank())
                {
                    #region Mock

                    postResModel = new BankRefundRes
                    {
                        Header = new BankHeaderModel
                        {
                            msgid = "ALSC001REFUND",
                            sourcechannel = "",
                            returncode = "0000",
                            returndesc = "交易成功(test)",
                            txnseq = postReqModel.header.txnseq,
                            fuseID = DateTime.Now.ToString("yyyyMMddHHmmss") + new System.Random().Next(999999).ToString().PadLeft(6, '0')
                        },
                        ReturnMsgTime = DateTime.Now.ToString("yyyyMMddHHmmss"),
                        CooPerAtor = postReqModel.cooPerAtor,
                        MbrActNo = postReqModel.mbrActNo,
                        OrderNo = postReqModel.orderNo,
                        Org_OrderNo = postReqModel.org_OrderNo,
                        TxnAmt = postReqModel.txnAmt,
                        DigestHash = "xxxxx"
                    };

                    #endregion
                }
                else
                {
                    #region 傳送 至銀行 並取得回應

                    Dictionary<string, string> headers = new Dictionary<string, string>
                    {
                        { "x-ibm-client-id", _cathayService.ACLinkClientID }
                    };

                    var postResult = _cathayService.PostToBankWithJson(apiType, postReqModel, headers);

                    if (!postResult.IsSuccess)
                    {
                        rtnResult.SetCode(postResult.RtnCode);
                        return JsonConvert.SerializeObject(rtnResult);
                    }

                    #endregion

                    #region 回應資料 轉Model 並記log(db)

                    var parseResResult = _cathayService.ParseToModel<BankRefundRes>(postResult.RtnData);

                    if (!parseResResult.IsSuccess)
                    {
                        rtnResult.SetCode(parseResResult.RtnCode);
                        return JsonConvert.SerializeObject(rtnResult);
                    }

                    postResModel = parseResResult.RtnData;

                    postResModel.CooPerAtor = postResModel.CooPerAtor ?? postReqModel.cooPerAtor;
                    postResModel.Header.msgid = postResModel.Header.msgid ?? postReqModel.header.msgid;
                    postResModel.Header.txnseq = postResModel.Header.txnseq ?? postReqModel.header.txnseq;
                    postResModel.Header.returndesc = HttpUtility.UrlDecode(postResModel.Header.returndesc);

                    //組完 傳送回應資料 先記log(db)
                    //_cathayService.AddReceiveLog(apiType, postResModel, sourceModel.MID);

                    #endregion
                }

                _cathayService.AddReceiveLog(apiType, postResModel, sourceModel.MID);


                #region 驗證 銀行回應資料

                if ((postResModel.Header.returncode == "0000" || !string.IsNullOrWhiteSpace(postResModel.DigestHash)) && !_cathayService.isMockBank())
                {
                    string sCheckData = $"{postResModel.Header.txnseq}{postResModel.CooPerAtor}{postResModel.MbrActNo}{postResModel.TxnAmt}{postResModel.ReturnMsgTime}";

                    var validateHashResult = _cathayService.ValidateHash(postResModel.DigestHash, sCheckData);

                    if (!validateHashResult.IsSuccess)
                    {
                        rtnResult.SetCode(validateHashResult.RtnCode);
                        return JsonConvert.SerializeObject(rtnResult);
                    }
                }

                #endregion

                #region 判斷 銀行回應狀態 (成功:returncode=0000)

                string rtncode = postResModel.Header.returncode;

                rtnResult.RtnData = new ACLinkResultModel { BankTradeNo = postResModel.Header.txnseq };

                if (rtncode == "0000")
                {
                    rtnResult.SetSuccess();
                }
                else if (rtncode == "AL13")
                {
                    rtnResult.SetCode(7442);//查無此綁定記錄
                }
                else if (rtncode == "AL67")
                {
                    rtnResult.SetCode(7497);//銀行系統維護中
                }
                else if (!rtncode.StartsWith("AL"))
                {
                    rtnResult.SetCode(7498);//銀行端非預期錯誤
                }
                else
                {
                    rtnResult.SetCode(7440, _cathayService.GetApiName(apiType));//xx失敗
                }

                if (rtncode != "0000")
                {
                    _logger.Trace(rtnResult.RtnMsg, rtnResult);
                }

                #endregion
            }
            catch (Exception ex)
            {
                rtnResult.SetCode(7499);//系統非預期錯誤
                _logger.Trace(ex, rtnResult.RtnMsg, rtnResult);
            }

            return JsonConvert.SerializeObject(rtnResult);
        }

        /// <summary>
        /// 提領
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public override string ACLinkWithdrawal(string json)
        {
            ApiType apiType = ApiType.ACLinkWithdrawal; //api類型

            ACLinkWithdrawalModel sourceModel = new ACLinkWithdrawalModel();//api執行參數
            BankRestoreReq postReqModel = new BankRestoreReq();//post請求檔
            BankRestoreRes postResModel = new BankRestoreRes();//post回應檔
            DataResult<ACLinkResultModel> rtnResult = new DataResult<ACLinkResultModel>();//最後回傳結果

            try
            {
                #region 來源資料 轉Model

                var parseSourceResult = _cathayService.ParseToModel<ACLinkWithdrawalModel>(json);

                if (!parseSourceResult.IsSuccess)
                {
                    rtnResult.SetCode(parseSourceResult.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                sourceModel = parseSourceResult.RtnData;
                sourceModel.BankType = _bankType;

                #endregion

                #region 檢查 api執行是否逾時

                var chkTimeoutResult = _cathayService.CheckTimeout(sourceModel.Timestamp);

                if (!chkTimeoutResult.IsSuccess)
                {
                    rtnResult.SetCode(chkTimeoutResult.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                #endregion

                #region 撈取 綁定資料

                var infoQryModel = new ACLinkInfoQryModel
                {
                    MID = sourceModel.MID,
                    INDTAccount = sourceModel.INDTAccount,
                    BankType = sourceModel.BankType
                };

                var acInfo = _cathayService.GetACLinkInfo(infoQryModel);

                if (!acInfo.IsSuccess)
                {
                    rtnResult.SetCode(acInfo.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                sourceModel.BankAccount = acInfo.RtnData.BankAccount;

                #endregion

                #region 建立 請求Model 並記log(db)

                var createResult = _cathayService.CreateRestoreModel(apiType, sourceModel);

                if (!createResult.IsSuccess)
                {
                    rtnResult.SetCode(createResult.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                postReqModel = createResult.RtnData;

                //組完 傳送請求資料 先記log(db)
                _cathayService.AddSendLog(apiType, postReqModel, sourceModel.MID);

                #endregion

                #region 驗證 請求Model

                var validateFieldResult = _cathayService.ValidateField(postReqModel);

                if (!validateFieldResult.IsSuccess)
                {
                    rtnResult.SetCode(validateFieldResult.RtnCode);
                    rtnResult.RtnMsg = validateFieldResult.RtnMsg;
                    return JsonConvert.SerializeObject(rtnResult);
                }

                #endregion


                if (_cathayService.isMockBank())
                {
                    #region Mock

                    postResModel = new BankRestoreRes
                    {
                        Header = new BankHeaderModel
                        {
                            msgid = "ALSC002RESTORE",
                            sourcechannel = "",
                            returncode = "0000",
                            returndesc = "交易成功(test)",
                            txnseq = postReqModel.header.txnseq,
                            fuseID = DateTime.Now.ToString("yyyyMMddHHmmss") + new System.Random().Next(999999).ToString().PadLeft(6, '0')
                        },
                        ReturnMsgTime = DateTime.Now.ToString("yyyyMMddHHmmss"),
                        CooPerAtor = postReqModel.cooPerAtor,
                        MbrActNo = postReqModel.mbrActNo,
                        TxnAmt = postReqModel.txnAmt,
                        OrderNo = postReqModel.orderNo,
                        DigestHash = "xxxxx"
                    };

                    #endregion
                }
                else
                {
                    #region 傳送 至銀行 並取得回應

                    Dictionary<string, string> headers = new Dictionary<string, string>
                    {
                        { "x-ibm-client-id", _cathayService.ACLinkClientID }
                    };

                    var postResult = _cathayService.PostToBankWithJson(apiType, postReqModel, headers);

                    if (!postResult.IsSuccess)
                    {
                        //post失敗,改查提領狀態有沒有成功
                        ACLinkPayQryModel queryModel = Mapper.Map<ACLinkPayQryModel>(postReqModel);
                        queryModel.SerMsgNo = postReqModel.header.txnseq;//用提領的交易序號查詢

                        string qryJson = JsonConvert.SerializeObject(queryModel);
                        string qryResult = ACLinkPayQuery(qryJson);

                        return JsonConvert.SerializeObject(qryResult);
                    }

                    #endregion

                    #region 回應資料 轉Model 並記log(db)

                    var parseResResult = _cathayService.ParseToModel<BankRestoreRes>(postResult.RtnData);

                    if (!parseResResult.IsSuccess)
                    {
                        rtnResult.SetCode(parseResResult.RtnCode);
                        return JsonConvert.SerializeObject(rtnResult);
                    }

                    postResModel = parseResResult.RtnData;

                    postResModel.CooPerAtor = postResModel.CooPerAtor ?? postReqModel.cooPerAtor;
                    postResModel.Header.msgid = postResModel.Header.msgid ?? postReqModel.header.msgid;
                    postResModel.Header.txnseq = postResModel.Header.txnseq ?? postReqModel.header.txnseq;
                    postResModel.Header.returndesc = HttpUtility.UrlDecode(postResModel.Header.returndesc);

                    //組完 傳送回應資料 先記log(db)
                    //_cathayService.AddReceiveLog(apiType, postResModel, sourceModel.MID);

                    #endregion
                }

                _cathayService.AddReceiveLog(apiType, postResModel, sourceModel.MID);


                #region 驗證 銀行回應資料

                if ((postResModel.Header.returncode == "0000" || !string.IsNullOrWhiteSpace(postResModel.DigestHash)) && !_cathayService.isMockBank())
                {
                    string sCheckData = $"{postResModel.Header.txnseq}{postResModel.CooPerAtor}{postResModel.MbrActNo}{postResModel.TxnAmt}{postResModel.ReturnMsgTime}";

                    var validateHashResult = _cathayService.ValidateHash(postResModel.DigestHash, sCheckData);

                    if (!validateHashResult.IsSuccess)
                    {
                        rtnResult.SetCode(validateHashResult.RtnCode);
                        return JsonConvert.SerializeObject(rtnResult);
                    }
                }

                #endregion

                #region 判斷 銀行回應狀態 (成功:returncode=0000)

                string rtncode = postResModel.Header.returncode;

                rtnResult.RtnData = new ACLinkResultModel { BankTradeNo = postResModel.Header.txnseq };

                if (rtncode == "0000")
                {
                    rtnResult.SetSuccess();
                }
                else if (rtncode == "AL13")
                {
                    rtnResult.SetCode(7442);//查無此綁定記錄
                }
                else if (rtncode == "AL67")
                {
                    rtnResult.SetCode(7497);//銀行系統維護中
                }
                else if (!rtncode.StartsWith("AL"))
                {
                    rtnResult.SetCode(7498);//銀行端非預期錯誤
                }
                else
                {
                    rtnResult.SetCode(7440, _cathayService.GetApiName(apiType));//xx失敗
                }

                if (rtncode != "0000")
                {
                    _logger.Trace(rtnResult.RtnMsg, rtnResult);
                }

                #endregion
            }
            catch (Exception ex)
            {
                rtnResult.SetCode(7499);//系統非預期錯誤
                _logger.Trace(ex, rtnResult.RtnMsg, rtnResult);
            }

            return JsonConvert.SerializeObject(rtnResult);
        }

        /// <summary>
        /// 綁定查詢
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public override string ACLinkBindQuery(string json)
        {
            ApiType apiType = ApiType.ACLinkBindQuery; //api類型

            ACLinkBindQryModel sourceModel = new ACLinkBindQryModel();//api執行參數
            BankBindQryReq postReqModel = new BankBindQryReq();//post請求檔
            BankBindQryRes postResModel = new BankBindQryRes();//post回應檔
            DataResult rtnResult = new DataResult();//最後回傳結果

            try
            {
                #region 來源資料 轉Model

                var parseSourceResult = _cathayService.ParseToModel<ACLinkBindQryModel>(json);

                if (!parseSourceResult.IsSuccess)
                {
                    rtnResult.SetCode(parseSourceResult.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                sourceModel = parseSourceResult.RtnData;
                sourceModel.BankType = _bankType;

                #endregion

                #region 檢查 api執行是否逾時

                var chkTimeoutResult = _cathayService.CheckTimeout(sourceModel.Timestamp);

                if (!chkTimeoutResult.IsSuccess)
                {
                    rtnResult.SetCode(chkTimeoutResult.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                #endregion

                #region 撈取 綁定資料

                var infoQryModel = new ACLinkInfoQryModel
                {
                    MID = sourceModel.MID,
                    INDTAccount = sourceModel.INDTAccount,
                    BankType = sourceModel.BankType
                };

                var acInfo = _cathayService.GetACLinkInfo(infoQryModel);

                if (!acInfo.IsSuccess)
                {
                    rtnResult.SetCode(acInfo.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                string bankAccount = acInfo.RtnData.BankAccount;

                #endregion

                #region 建立 請求Model 並記log(db)

                var createResult = _cathayService.CreateBindQryModel(apiType, sourceModel);

                if (!createResult.IsSuccess)
                {
                    rtnResult.SetCode(createResult.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                postReqModel = createResult.RtnData;

                //組完 傳送請求資料 先記log(db)
                _cathayService.AddSendLog(apiType, postReqModel, sourceModel.MID);

                #endregion

                #region 驗證 請求Model

                var validateFieldResult = _cathayService.ValidateField(postReqModel);

                if (!validateFieldResult.IsSuccess)
                {
                    rtnResult.SetCode(validateFieldResult.RtnCode);
                    rtnResult.RtnMsg = validateFieldResult.RtnMsg;
                    return JsonConvert.SerializeObject(rtnResult);
                }

                #endregion


                if (_cathayService.isMockBank())
                {
                    #region Mock

                    postResModel = new BankBindQryRes
                    {
                        Header = new BankHeaderModel
                        {
                            msgid = "ALSQ001BINDING",
                            sourcechannel = "",
                            returncode = "0000",
                            returndesc = "交易成功(test)",
                            txnseq = postReqModel.header.txnseq,
                            fuseID = DateTime.Now.ToString("yyyyMMddHHmmss") + new System.Random().Next(999999).ToString().PadLeft(6, '0')
                        },
                        ReturnMsgTime = DateTime.Now.ToString("yyyyMMddHHmmss"),
                        CooPerAtor = postReqModel.cooPerAtor,
                        MbrActNo = postReqModel.mbrActNo,
                        MbrIdno = postReqModel.mbrIdno,
                        DigestHash = "xxxxx",
                        RecordInfo = new List<BankRecordInfoModel>{
                            new BankRecordInfoModel {
                                 BnkActNo = _cathayService.TripleDESEncrypt(bankAccount.PadLeft(16,'0')),
                                 BindTime = DateTime.Now.ToString("yyyyMMddHHmmss"),
                                 ActStstus = "02"
                            }
                        }
                    };

                    #endregion
                }
                else
                {
                    #region 傳送 至銀行 並取得回應

                    Dictionary<string, string> headers = new Dictionary<string, string>
                    {
                        { "x-ibm-client-id", _cathayService.ACLinkClientID }
                    };

                    var postResult = _cathayService.PostToBankWithJson(apiType, postReqModel, headers);

                    if (!postResult.IsSuccess)
                    {
                        rtnResult.SetCode(postResult.RtnCode);
                        return JsonConvert.SerializeObject(rtnResult);
                    }

                    #endregion

                    #region 回應資料 轉Model 並記log(db)

                    var parseResResult = _cathayService.ParseToModel<BankBindQryRes>(postResult.RtnData);

                    if (!parseResResult.IsSuccess)
                    {
                        rtnResult.SetCode(parseResResult.RtnCode);
                        return JsonConvert.SerializeObject(rtnResult);
                    }

                    postResModel = parseResResult.RtnData;

                    postResModel.CooPerAtor = postResModel.CooPerAtor ?? postReqModel.cooPerAtor;
                    postResModel.Header.msgid = postResModel.Header.msgid ?? postReqModel.header.msgid;
                    postResModel.Header.txnseq = postResModel.Header.txnseq ?? postReqModel.header.txnseq;
                    postResModel.Header.returndesc = HttpUtility.UrlDecode(postResModel.Header.returndesc);

                    //組完 傳送回應資料 先記log(db)
                    //_cathayService.AddReceiveLog(apiType, postResModel, sourceModel.MID);

                    #endregion
                }

                _cathayService.AddReceiveLog(apiType, postResModel, sourceModel.MID);


                #region 驗證 銀行回應資料

                if ((postResModel.Header.returncode == "0000" || !string.IsNullOrWhiteSpace(postResModel.DigestHash)) && !_cathayService.isMockBank())
                {
                    string sCheckData = $"{postResModel.Header.txnseq}{postResModel.CooPerAtor}{postResModel.MbrActNo}{postResModel.ReturnMsgTime}";

                    var validateHashResult = _cathayService.ValidateHash(postResModel.DigestHash, sCheckData);

                    if (!validateHashResult.IsSuccess)
                    {
                        rtnResult.SetCode(validateHashResult.RtnCode);
                        return JsonConvert.SerializeObject(rtnResult);
                    }
                }

                #endregion

                #region 判斷 銀行回應狀態 (成功:returncode=0000)

                string rtncode = postResModel.Header.returncode;

                if (rtncode == "0000")
                {
                    rtnResult.SetCode(7442);//查無此綁定記錄

                    //比對綁定帳號
                    foreach (var item in postResModel.RecordInfo)
                    {
                        string _bnkActNo = _cathayService.TripleDESDecrypt(item.BnkActNo);
                        if (_bnkActNo == bankAccount.PadLeft(16,'0'))
                        {
                            switch (item.ActStstus)
                            {
                                case "01":
                                    rtnResult.SetError();
                                    rtnResult.RtnMsg = "申請中";
                                    break;
                                case "02":
                                    rtnResult.SetSuccess();
                                    break;
                                case "03":
                                    rtnResult.SetError();
                                    rtnResult.RtnMsg = "取消綁定";
                                    break;
                                default:
                                    break;
                            }
                            break;
                        }
                    }
                }
                else if (rtncode == "AL13")
                {
                    rtnResult.SetCode(7442);//查無此綁定記錄
                }
                else if (rtncode == "AL67")
                {
                    rtnResult.SetCode(7497);//銀行系統維護中
                }
                else if (!rtncode.StartsWith("AL"))
                {
                    rtnResult.SetCode(7498);//銀行端非預期錯誤
                }
                else
                {
                    rtnResult.SetCode(7440, _cathayService.GetApiName(apiType));//xx失敗
                }

                if (rtncode != "0000")
                {
                    _logger.Trace(rtnResult.RtnMsg, rtnResult);
                }

                #endregion
            }
            catch (Exception ex)
            {
                rtnResult.SetCode(7499);//系統非預期錯誤
                _logger.Trace(ex, rtnResult.RtnMsg, rtnResult);
            }

            return JsonConvert.SerializeObject(rtnResult);
        }

        /// <summary>
        /// 交易查詢
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public override string ACLinkPayQuery(string json)
        {
            ApiType apiType = ApiType.ACLinkPayQuery; //api類型

            ACLinkPayQryModel sourceModel = new ACLinkPayQryModel();//api執行參數
            BankPayQryReq postReqModel = new BankPayQryReq();//post請求檔
            BankPayQryRes postResModel = new BankPayQryRes();//post回應檔
            DataResult<ACLinkResultModel> rtnResult = new DataResult<ACLinkResultModel>();//最後回傳結果

            try
            {
                #region 來源資料 轉Model

                var parseSourceResult = _cathayService.ParseToModel<ACLinkPayQryModel>(json);

                if (!parseSourceResult.IsSuccess)
                {
                    rtnResult.SetCode(parseSourceResult.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                sourceModel = parseSourceResult.RtnData;
                sourceModel.BankType = _bankType;

                #endregion

                #region 檢查 api執行是否逾時

                var chkTimeoutResult = _cathayService.CheckTimeout(sourceModel.Timestamp);

                if (!chkTimeoutResult.IsSuccess)
                {
                    rtnResult.SetCode(chkTimeoutResult.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                #endregion

                #region 撈取 綁定資料

                var infoQryModel = new ACLinkInfoQryModel
                {
                    MID = sourceModel.MID,
                    INDTAccount = sourceModel.INDTAccount,
                    BankType = sourceModel.BankType
                };

                var acInfo = _cathayService.GetACLinkInfo(infoQryModel);

                if (!acInfo.IsSuccess)
                {
                    rtnResult.SetCode(acInfo.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                sourceModel.BankAccount = acInfo.RtnData.BankAccount;

                #endregion

                #region 建立 請求Model 並記log(db)

                var createResult = _cathayService.CreatePayQryModel(apiType, sourceModel);

                if (!createResult.IsSuccess)
                {
                    rtnResult.SetCode(createResult.RtnCode);
                    return JsonConvert.SerializeObject(rtnResult);
                }

                postReqModel = createResult.RtnData;

                //組完 傳送請求資料 先記log(db)
                _cathayService.AddSendLog(apiType, postReqModel, sourceModel.MID);

                #endregion

                #region 驗證 請求Model

                var validateFieldResult = _cathayService.ValidateField(postReqModel);

                if (!validateFieldResult.IsSuccess)
                {
                    rtnResult.SetCode(validateFieldResult.RtnCode);
                    rtnResult.RtnMsg = validateFieldResult.RtnMsg;
                    return JsonConvert.SerializeObject(rtnResult);
                }

                #endregion


                if (_cathayService.isMockBank())
                {
                    #region Mock

                    postResModel = new BankPayQryRes
                    {
                        Header = new BankHeaderModel
                        {
                            msgid = "ALSQ002TRANSACTION",
                            sourcechannel = "",
                            returncode = "0000",
                            returndesc = "交易成功(test)",
                            txnseq = postReqModel.header.txnseq,
                            fuseID = DateTime.Now.ToString("yyyyMMddHHmmss") + new System.Random().Next(999999).ToString().PadLeft(6, '0')
                        },
                        ReturnMsgTime = DateTime.Now.ToString("yyyyMMddHHmmss"),
                        CooPerAtor = postReqModel.cooPerAtor,
                        MbrActNo = postReqModel.mbrActNo,
                        MbrIdno = postReqModel.mbrIdno,
                        TransType = "TRANSTYPE",
                        TxnDateTime = DateTime.Now.ToString("yyyyMMddHHmmss"),
                        TxnAmt = 100,
                        RtnCode = "0000",
                        RtnDesc = "交易成功(test)",
                        OrderNo = "(單號)",
                        DigestHash = "xxxxx"
                    };

                    #endregion
                }
                else
                {
                    #region 傳送 至銀行 並取得回應

                    Dictionary<string, string> headers = new Dictionary<string, string>
                    {
                        { "x-ibm-client-id", _cathayService.ACLinkClientID }
                    };

                    var postResult = _cathayService.PostToBankWithJson(apiType, postReqModel, headers);

                    if (!postResult.IsSuccess)
                    {
                        rtnResult.SetCode(postResult.RtnCode);
                        return JsonConvert.SerializeObject(rtnResult);
                    }

                    #endregion

                    #region 回應資料 轉Model 並記log(db)

                    var parseResResult = _cathayService.ParseToModel<BankPayQryRes>(postResult.RtnData);

                    if (!parseResResult.IsSuccess)
                    {
                        rtnResult.SetCode(parseResResult.RtnCode);
                        return JsonConvert.SerializeObject(rtnResult);
                    }

                    postResModel = parseResResult.RtnData;

                    postResModel.CooPerAtor = postResModel.CooPerAtor ?? postReqModel.cooPerAtor;
                    postResModel.Header.msgid = postResModel.Header.msgid ?? postReqModel.header.msgid;
                    postResModel.Header.txnseq = postResModel.Header.txnseq ?? postReqModel.header.txnseq;
                    postResModel.Header.returndesc = HttpUtility.UrlDecode(postResModel.Header.returndesc);

                    //組完 傳送回應資料 先記log(db)
                    //_cathayService.AddReceiveLog(apiType, postResModel, sourceModel.MID);

                    #endregion
                }

                _cathayService.AddReceiveLog(apiType, postResModel, sourceModel.MID);


                #region 驗證 銀行回應資料

                if ((postResModel.Header.returncode == "0000" || !string.IsNullOrWhiteSpace(postResModel.DigestHash)) && !_cathayService.isMockBank())
                {
                    string sCheckData = $"{postResModel.Header.txnseq}{postResModel.CooPerAtor}{postResModel.MbrActNo}{postResModel.ReturnMsgTime}";

                    var validateHashResult = _cathayService.ValidateHash(postResModel.DigestHash, sCheckData);

                    if (!validateHashResult.IsSuccess)
                    {
                        rtnResult.SetCode(validateHashResult.RtnCode);
                        return JsonConvert.SerializeObject(rtnResult);
                    }
                }

                #endregion

                #region 判斷 銀行回應狀態 (成功:returncode=0000)

                string rtncode = postResModel.Header.returncode;

                rtnResult.RtnData = new ACLinkResultModel { BankTradeNo = postResModel.Header.txnseq };

                if (rtncode == "0000")
                {
                    rtnResult.SetSuccess();
                }
                else if (rtncode == "AL13")
                {
                    rtnResult.SetCode(7442);//查無此綁定記錄
                }
                else if (rtncode == "AL67")
                {
                    rtnResult.SetCode(7497);//銀行系統維護中
                }
                else if (!rtncode.StartsWith("AL"))
                {
                    rtnResult.SetCode(7498);//銀行端非預期錯誤
                }
                else
                {
                    rtnResult.SetCode(7440, _cathayService.GetApiName(apiType));//xx失敗
                }

                if (rtncode != "0000")
                {
                    _logger.Trace(rtnResult.RtnMsg, rtnResult);
                }

                #endregion
            }
            catch (Exception ex)
            {
                rtnResult.SetCode(7499);//系統非預期錯誤
                _logger.Trace(ex, rtnResult.RtnMsg, rtnResult);
            }

            return JsonConvert.SerializeObject(rtnResult);
        }
    }
}
