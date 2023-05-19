using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Net.Http;

namespace ICP.Library.Repositories.ManageBank
{
    using Infrastructure.Abstractions.Logging;
    using Infrastructure.Core.Extensions;
    using Infrastructure.Core.Models;
    using Infrastructure.Core.Helpers;
    using Infrastructure.Abstractions.DbUtil;
    using Library.Models.ManageBank.FirstBank;
    using Library.Repositories.SystemRepositories;
    using Library.Repositories.MemberRepositories;
    using ICP.Infrastructure.Core.Utils;

    public class FirstBankRepository
    {
        XMLHelper _xmlHelper;
        RsaCryptoHelper _rsaCryptoHelper;
        CertificateHelper _certificateHelper;
        ILogger _logger;
        IDbConnectionPool _dbConnectionPool;

        const string rootName = "XML";

        public string fxml_url { get; set; }
        public string fxml_publicKey { get; set; }
        public BankTransferSettingModel setting { get; set; }

        string CertificateSubject = GlobalConfigUtil.GetAppSetting("CertificateSubject");
        string StoreName = GlobalConfigUtil.GetAppSetting("StoreName");
        string StoreLocation = GlobalConfigUtil.GetAppSetting("StoreLocation");
        string SerialNumber = GlobalConfigUtil.GetAppSetting("SerialNumber");

        public FirstBankRepository(
            ILogger<FirstBankRepository> logger,
            IDbConnectionPool dbConnectionPool
            )
        {
            _xmlHelper = new XMLHelper();
            _rsaCryptoHelper = new RsaCryptoHelper();
            _certificateHelper = new CertificateHelper();
            _logger = logger;
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 取得請求序號
        /// </summary>
        /// <returns></returns>
        private DataResult<string> GetSvcRqSerial()
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);

            string sql = "EXEC ausp_Log_Payment_FirstBank_FXML_GetSvcRqSerial_SIU";

            return db.QuerySingleOrDefault<DataResult<string>>(sql);
        }

        /// <summary>
        /// 取得請求標頭
        /// </summary>
        /// <returns></returns>
        private DataResult<TxHeaderModel> GetReqHeader<T>()
        {
            var result = new DataResult<TxHeaderModel>();
            result.SetError();

            var getSvcRqSerialResult = GetSvcRqSerial();
            if (!getSvcRqSerialResult.IsSuccess)
            {
                result.SetError(getSvcRqSerialResult);
                return result;
            }
            string SvcRqSerial = getSvcRqSerialResult.RtnData;

            var txHeader = new TxHeaderModel();
            txHeader.TxID = typeof(T).Name;
            txHeader.SvcRqId = $"{setting.CustId}-{DateTime.Today.ToString("YYYYMMDD")}-{SvcRqSerial}";
            txHeader.CustId = setting.CustId;
            txHeader.MsgDirection = "RQ";

            result.SetSuccess(txHeader);
            return result;
        }

        /// <summary>
        /// FXML Log
        /// </summary>
        /// <param name="TransferID"></param>
        /// <param name="TxHeader"></param>
        /// <param name="XML"></param>
        /// <param name="result"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public BaseResult AddFXMLLog(long TransferID, TxHeaderModel TxHeader, string XML, B2BResult result = null, StatusModel status = null, string RQSvcRqID = null)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);

            string sql = "EXEC ausp_Log_Payment_FirstBank_AddFXMLLog_I";

            var args = new
            {
                TransferID,
                TxHeader.TxID,
                TxHeader.SvcRqId,
                TxHeader.CustId,
                TxHeader.MsgDirection,
                XML,
                result?.StatusCode,
                result?.StatusDesc,
                RQSvcRqID,
                status?.TxnDate,
                status?.SettleAmt,
                status?.SettleDate,
                Pay_StatusCode = status?.StatusCode,
                Pay_StatusDesc = status?.StatusDesc,
                status?.FcbErrCode,
                status?.FcbErrDesc
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<DataResult<string>>(sql, args);
        }

        /// <summary>
        /// 加簽
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private DataResult<string> AddSign2XML(string xml)
        {
            var result = new DataResult<string>();
            result.SetError();

            // Local Sign Or POST XML TO HSM
            var xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.LoadXml(xml);

            var cert = _certificateHelper.GetLocalCert(StoreName, StoreLocation, CertificateSubject, SerialNumber);
            var signedXml = _xmlHelper.SignDocument(xmlDoc, cert);
            xml = signedXml.OuterXml;

            result.SetSuccess(xml);
            return result;
        }

        /// <summary>
        /// 驗簽
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private BaseResult VerifySign(XmlDocument doc)
        {
            var result = new BaseResult();
            result.SetError();

            var key = _rsaCryptoHelper.DecodePublicKey(fxml_publicKey);

            var verifySignResult = _xmlHelper.VerifySign(doc, key, signTagName: "Signature");
            if (!verifySignResult.IsSuccess)
            {
                result.SetError(verifySignResult);
                return result;
            }

            result.SetSuccess();
            return result;
        }

        /// <summary>
        /// Call API
        /// </summary>
        /// <typeparam name="T">回傳型別</typeparam>
        /// <typeparam name="TxRq">請求內容型別</typeparam>
        /// <param name="obj">請求內容</param>
        /// <param name="TransferID">轉帳ID</param>
        /// <param name="RQSvcRqID"></param>
        /// <returns></returns>
        public DataResult<T> CallApi<T, TxRq>(TxRq obj, long TransferID = 0, string RQSvcRqID = null) where T: XML, new()
        {
            var result = new DataResult<T>();
            result.SetError();

            var getReqHeaderResult = GetReqHeader<TxRq>();
            if (!getReqHeaderResult.IsSuccess)
            {
                result.SetError(getReqHeaderResult);
                return result;
            }
            var txHeader = getReqHeaderResult.RtnData;

            var tx = new TxRequest<TxRq>();
            tx.TxHeader = txHeader;
            tx.TxRq = obj;

            var xmlObj = new XML<TxRequest<TxRq>>();
            xmlObj.Tx = tx;

            // 轉 xml
            string xml = _xmlHelper.OB2XML(xmlObj, rootName);

            // 加簽
            var addSign2XMLResult = AddSign2XML(xml);
            if (!addSign2XMLResult.IsSuccess)
            {
                result.SetError(addSign2XMLResult);
                _logger.Error(result.RtnMsg);
                return result;
            }
            xml = addSign2XMLResult.RtnData;

            string rsXML = null;

            try
            {
                // log send xml
                AddFXMLLog(TransferID, txHeader, xml, RQSvcRqID: RQSvcRqID);

                // POST xml
                using (var _httpClient = new HttpClient())
                {
                    var postResult = _httpClient.PostAsync(new Uri(fxml_url), new StringContent(xml)).Result;
                    rsXML = postResult.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                result.RtnMsg = "post fxml";
                _logger.Error(ex, result.RtnMsg);
                return result;
            }

            if (string.IsNullOrWhiteSpace(rsXML))
            {
                result.RtnMsg = "response is empty";
                _logger.Error(result.RtnMsg);
                return result;
            }

            var doc = new XmlDocument();
            doc.LoadXml(rsXML);

            // Verify sign
            var verifySignResult = VerifySign(doc);
            if (!verifySignResult.IsSuccess)
            {
                result.SetError(result);
                _logger.Error(result.RtnMsg);
                return result;
            }

            T rtnData = default(T);

            bool logging = false;

            try
            {
                rtnData = _xmlHelper.XMLStr2OB<T>(rsXML, rootName);
                if (rtnData == null)
                {
                    _logger.Error("XMLStr2OB is null");
                    return result;
                }

                var rtnTx = (TxResult)rtnData.Tx;

                var listStatus = new List<StatusModel>();

                #region 取得狀態結果回覆
                const string statusTagName = "Status";

                var nodes = doc.SelectNodes("//" + statusTagName);
                if (nodes.Count > 0)
                {
                    foreach (XmlNode node in nodes)
                    {
                        var status = _xmlHelper.XMLStr2OB<StatusModel>(node.OuterXml, statusTagName);

                        listStatus.Add(status);
                    }
                }
                #endregion

                // log receive xml
                logging = true;

                //XML, 結果, 狀態 Log
                if (listStatus.Count > 0)
                {
                    foreach (var status in listStatus)
                    {
                        AddFXMLLog(TransferID, rtnTx.TxHeader, rsXML, rtnTx.TxRs, status, RQSvcRqID: RQSvcRqID);
                    }
                }
                //XML, 結果 Log
                else
                {
                    AddFXMLLog(TransferID, rtnTx.TxHeader, rsXML, rtnTx.TxRs, RQSvcRqID: RQSvcRqID);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "XMLStr2OB");

                if (!logging)
                {
                    //XML Log
                    txHeader.MsgDirection = "RQ";
                    AddFXMLLog(TransferID, txHeader, rsXML, RQSvcRqID: RQSvcRqID);
                }

                return result;
            }

            result.SetSuccess(rtnData);
            return result;
        }
    }
}
