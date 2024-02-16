using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.PICChargeFeeOpen.Commands
{
    using ICP.Batch.PICChargeFeeOpen.Models;
    using ICP.Batch.PICChargeFeeOpen.Models.Enums;
    using ICP.Batch.PICChargeFeeOpen.PICWebService;
    using ICP.Batch.PICChargeFeeOpen.Repositories;
    using ICP.Batch.PICChargeFeeOpen.Services;
    using Infrastructure.Abstractions.Logging;

    public class PICChargeFeeOpenCommand
    {
        private readonly ChargeFeeRepository _chargeFeeRepository = null;
        private readonly ILogger<PICChargeFeeOpenCommand> _logger;
        private readonly EMailNotifyService _eMailNotifyService;
        private readonly SellerJobReceiverServiceImplService _picWS;

        public PICChargeFeeOpenCommand(
            ILogger<PICChargeFeeOpenCommand> logger,
             EMailNotifyService eMailNotifyService,
             ChargeFeeRepository chargeFeeRepository) 
        {
            _logger = logger;
            _eMailNotifyService = eMailNotifyService;
            _chargeFeeRepository = chargeFeeRepository;

            _picWS = new SellerJobReceiverServiceImplService();
            _picWS.Url = ConfigService.PicWSUrl;
        }

        /// <summary>
        /// 執行排程
        /// </summary>
        /// <returns></returns>
        public void exec()
        {
            execWithTryCatch(ChargeFeeOpen);
        }

        /// <summary>
        /// 使用 try_catch 執行
        /// </summary>
        /// <param name="action"></param>
        private void execWithTryCatch(Action action)
        {
            try
            {
                _logger.Info($"開始 - 待開立手續費發票排程");

                action();

                _logger.Info($"結束 - 待開立手續費發票排程");
            }
            catch (Exception ex)
            {
                _logger.Warning(ex.ToString());
                _eMailNotifyService.SendErrorEmail(ex);
            }
        }

        /// <summary>
        /// PIC  手續費開立(主流程)
        /// </summary>
        private void ChargeFeeOpen()
        {

            //取得待開立手續費發票資料
            List<CF_InvoiceIssueModel> IssueList = GetIssueAndProductList();

            if (IssueList.Count == 0)
            {
                _logger.Info($"無待開立手續費發票");
                return;
            }

            // 取得 PIC需要的使用者資訊
            wsUser user = GetPicUserModel();

            // 取得 PIC 需要的發票資訊( DB格式 轉換為 PIC 格式)
            List<iuo008OrdersVO> orderInfoList = GetPicOrderInfoList(IssueList);

            //呼叫 PIC Web Service 取得回傳值
            resultWebVo PicResult = _picWS.SIUO008(user, orderInfoList.ToArray());

            //紀錄回傳Log
            _logger.Info($"回傳代碼 : {PicResult.Code}, 回傳錯誤訊息 : {PicResult.Description} , 總筆數 : {orderInfoList.Count}, 錯誤比數 : {PicResult.ErrorInfos.Count()}");

            //回寫狀態到資料庫
            UpdatePicReturnInfo(PicResult, IssueList);

            //判斷是否成功
            if (PicResult.Code == "0")
            {
                _logger.Info($"開立成功");

            }
            else
            {
                //寄送錯誤訊息 & Log
                StringBuilder sb = new StringBuilder();
                PicResult.ErrorInfos.ToList().ForEach(errorInfo =>
                    sb.AppendLine($"錯誤單號 : {errorInfo.OrderNo}, 錯誤訊息 : {errorInfo.ErrorMsg}")
                );
                _eMailNotifyService.SendErrorEmail(sb.ToString(), true);
                _logger.Warning(sb.ToString());
            }

        }

        /// <summary>
        /// 取得 PIC需要的使用者資訊
        /// </summary>
        /// <returns></returns>
        private wsUser GetPicUserModel() {

            //需事先與統一資訊電子發票平台申請
            wsUser user = new wsUser();
            user.unifiedNo = ConfigService.Identifier;   //登入統編
            user.usrAcc = ConfigService.Account;   //登入帳號
            user.usrMima = ConfigService.Password;    //登入密碼

            return user;
        }

        /// <summary>
        /// 取得 PIC 需要的發票資訊( DB格式 轉換為 PIC 格式)
        /// </summary>
        /// <returns></returns>
        private List<iuo008OrdersVO> GetPicOrderInfoList(List<CF_InvoiceIssueModel> issueList)
        {
            //建立 Pic 發票格式 陣列
            List<iuo008OrdersVO> orderInfoList = new List<iuo008OrdersVO>();

            //逐筆進行格式轉換
            issueList.ForEach(issue =>
            {
                //建立 Pic 發票格式
                iuo008OrdersVO orderInfo = new iuo008OrdersVO();

                //建立固定值
                orderInfo.type = "ORD";

                //轉換 發票主檔 資料
                orderInfo.M = new iuo008MainVO()
                {
                    TransactionNo = issue.InvoiceNo,
                    TransactionDate = DateTime.Now.ToString("yyyyMMdd"),
                    //如果是預設值則帶入空白
                    InvoiceDate = issue.InvoiceDate.ToString("yyyyMMdd") == "19000101" ? string.Empty : issue.InvoiceDate.ToString("yyyyMMdd"),
                    CategoryNo = issue.CategoryNo,
                    BuyerIdentifier = issue.BuyerIdentifier,
                    BuyerName = issue.BuyerName,
                    BuyerAddress = issue.BuyerAddress,
                    BuyerCustomerNo = issue.BuyerCustomerNo,
                    BuyerEmail = issue.BuyerEmail,
                    SellerIdentifier = issue.SellerIdentifier,
                    SellerName = issue.SellerName,
                    MainRemark = issue.MainRemark,
                    SalesAmount = issue.SalesAmount.ToString(),
                    FreeTaxSalesAmount = issue.FreeTaxSalesAmount.ToString(),
                    ZeroTaxSalesAmount = issue.ZeroTaxSalesAmount.ToString(),
                    TaxRate = issue.TaxRate.ToString(),
                    TaxAmount = issue.TaxAmount.ToString(),
                    TaxType = issue.TaxType,
                    TotalAmount = issue.TotalAmount.ToString(),
                    InvoiceType = issue.InvoiceType,
                    GroupNumber = issue.GroupNumber,
                    BuyerDonateMark = issue.BuyerDonateMark,
                    BuyerCarrierType = issue.BuyerCarrierType,
                    BuyerCarrierid1 = issue.BuyerCarrierid1,
                    BuyerCarrierid2 = issue.BuyerCarrierid2,
                    BuyerNpoban = issue.BuyerNpoban
                };

                //轉換 商品清單 資料
                orderInfo.D = issue.ProductItem.Select(x => new iuo008DetailVO()
                {
                    TransactionNo = issue.InvoiceNo,
                    SellerIdentifier = issue.SellerIdentifier,
                    ProductNo = x.RelateNumber,
                    ProductName = x.Description,
                    Quantity = x.Quantity.ToString(),
                    UnitPrice = x.UnitPrice.ToString(),
                    Unit = x.Unit,
                    Amount = x.Amount.ToString(),
                    TaxType = issue.TaxType,
                    SequenceNumber = x.SequenceNumber.ToString("000"),
                    Remark = x.Remark
                }).ToArray();

                //加入陣列
                orderInfoList.Add(orderInfo);

            });

            return orderInfoList;
        }

        /// <summary>
        /// 取得發票清單主表 和 商品清單
        /// </summary>
        /// <returns></returns>
        private List<CF_InvoiceIssueModel> GetIssueAndProductList()
        {
            //取得發票清單主表
            List<CF_InvoiceIssueModel> issueList = _chargeFeeRepository.ListChargeFeeInvoiceIssue<CF_InvoiceIssueModel>();

            //主表 PK 組字串
            string Invoices = string.Join(",", issueList.Select(x => x.InvoiceID));

            //取的商品清單(依據主表字串)
            List<CF_InvoiceIssue_ProductItemModel> productItemList = _chargeFeeRepository.ListChargeFeeInvoiceIssue_ProductItem<CF_InvoiceIssue_ProductItemModel>(Invoices);

            //將發票清單 和 商品清單 整合
            issueList.ForEach(issue =>

                issue.ProductItem = productItemList.Where(x => x.InvoiceID == issue.InvoiceID).ToList()
            );

            return issueList;
        }

        /// <summary>
        /// 回寫狀態到資料庫
        /// </summary>
        /// <param name="RtnCode"></param>
        /// <param name="RtnMsg"></param>
        /// <param name="errorInfoList"></param>
        private void UpdatePicReturnInfo(resultWebVo PicResult, List<CF_InvoiceIssueModel> IssueList)
        {
            //逐筆回寫到資料庫
            IssueList.ForEach(issueModel =>
            {
                //判斷錯誤訊息中是否有此筆資料(有資料代表錯誤)
                var errorData = PicResult.ErrorInfos.Where(x => x.OrderNo == issueModel.InvoiceNo).FirstOrDefault();

                CF_InvoiceIssue_UpdateStatusModel updateModel = new CF_InvoiceIssue_UpdateStatusModel();
                updateModel.InvoiceID = issueModel.InvoiceID;
                updateModel.InvoiceNo = issueModel.InvoiceNo;
                updateModel.RtnCode = errorData == null ? "0" : PicResult.Code;
                updateModel.RtnMsg = errorData == null ? string.Empty : errorData.ErrorMsg;
                updateModel.Issue_Status = errorData == null ? Issue_StatusEnum.Default : Issue_StatusEnum.Fail;
                updateModel.State = errorData == null ? StateEnums.Opening : StateEnums.Done;
                updateModel.Modifier = "PICChargeFeeOpen";

                _chargeFeeRepository.UpdateInvoiceIssueByPicResult(updateModel);

            });
        }


    }
}
