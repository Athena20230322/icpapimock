using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.PICChargeFeeOpen.Models
{
    /// <summary>
    /// 手續費電子發票主檔
    /// ChargeFee_InvoiceIssueModel
    /// </summary>
    public class CF_InvoiceIssueModel
    {
        /// <summary>
        /// 發票編號
        /// </summary>
        public long InvoiceID { get; set; }

        /// <summary>
        /// 發票單號 YYMMDDHHMMSSTTTT 唯一值
        /// </summary>
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 發票號碼 開立成功時回壓
        /// </summary>
        public string InvoiceNumber { get; set; }

        /// <summary>
        /// 發票開立日期
        /// </summary>
        public DateTime InvoiceDate { get; set; }
        
        /// <summary>
        /// 發票類別 07：一般稅額計算之電子發票 08：特種稅額計算之電子發票
        /// </summary>
        public string InvoiceType { get; set; }
        
        /// <summary>
        /// 發票類別 0001 : 提領手續費 0002: 金流手續費
        /// </summary>
        public string CategoryNo { get; set; }
        
        /// <summary>
        /// 買方統編 8碼數字或無買方統編帶入10個零
        /// </summary>
        public string BuyerIdentifier { get; set; }
        
        /// <summary>
        /// 買方名稱
        /// </summary>
        public string BuyerName { get; set; }

        /// <summary>
        /// 買方地址
        /// </summary>
        public string BuyerAddress { get; set; }
        
        /// <summary>
        /// 買方會員編號  (電支帳號)
        /// </summary>
        public string BuyerCustomerNo { get; set; }

        /// <summary>
        /// 買方Email
        /// </summary>
        public string BuyerEmail { get; set; }

        /// <summary>
        /// 賣方統編
        /// </summary>
        public string SellerIdentifier { get; set; }

        /// <summary>
        /// 賣方名稱
        /// </summary>
        public string SellerName { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string MainRemark { get; set; }

        /// <summary>
        /// 銷售金額(未稅)
        /// </summary>
        public int SalesAmount { get; set; }

        /// <summary>
        /// 免稅銷售額合計
        /// </summary>
        public int FreeTaxSalesAmount { get; set; }

        /// <summary>
        /// 零稅銷售額合計
        /// </summary>
        public int ZeroTaxSalesAmount { get; set; }

        /// <summary>
        /// 稅率 5%即帶0.05
        /// </summary>
        public decimal TaxRate { get; set; }
        
        /// <summary>
        /// 營業稅額
        /// </summary>
        public int TaxAmount { get; set; }

        /// <summary>
        /// 課稅別 1：應稅 2：零稅率 3：免稅 4：應稅(特種稅率) 9：混合應稅與免稅或零稅率(限訊息C0401使用)
        /// </summary>
        public string TaxType { get; set; }

        /// <summary>
        /// 總計,不應為負數
        /// </summary>
        public int TotalAmount { get; set; }

        /// <summary>
        /// 發票需彙開，此欄位必填 4碼數字，由營業人自行編流水號
        /// </summary>
        public string GroupNumber { get; set; }

        /// <summary>
        /// 捐贈註記若捐贈註記為1，BuyerNpoban欄位須有值1：是(捐贈) 0：否(不捐贈)
        /// </summary>
        public string BuyerDonateMark { get; set; }
        
        /// <summary>
        /// 載具類別
        /// 1.=>無(預設)
        /// 2.=>3J0002:手機條碼；載具號碼:須為8碼且第一碼必為/
        /// 3.=>CQ0001:自然人憑證；載具號碼:須為2碼英文大寫+14碼數字序號
        /// 4.=>1K0001:悠遊卡
        /// 5.=>2G0001:iCash
        /// 6.=>EK0002:信用卡
        /// 7.=>6碼之會員載具類別
        /// 若消費者使用手機條碼索取買方統編發票，則該不論是否已列印紙本，該欄位皆必填
        /// </summary>
        public string BuyerCarrierType { get; set; }

        /// <summary>
        /// "載具號碼1
        /// 1.當<BuyerCarrierType> 有值時，此欄位必填
        /// 2.若<PrintMark> 為Y，則<BuyerCarrierid1> 為空白
        /// 3.若<BuyerCarrierid1> 有值時，則<PrintMark> 為N
        /// 4.若<BuyerCarrierType>=""3J0002”&& <BuyerIdentifier> 為10個0時，此欄位必填
        /// 5.若<BuyerCarrierType>=""EK0002""(信用卡) && <BuyerIdentifier>為10個0時，此欄位為 =民國年月日(7碼)+總計金額(10碼，不足額往左補0)
        /// 6.若消費者使用手機條碼索取買方統編發票，則該不論是否已列印紙本，該欄位皆必填"
        /// </summary>
        public string BuyerCarrierid1 { get; set; }

        /// <summary>
        /// "載具號碼2
        /// 1.當<BuyerCarrierType> 有值時，此欄位必填
        /// 2.若<PrintMark> 為Y，則<BuyerCarrierid2> 為空白
        /// 3.若<BuyerCarrierid2> 有值時，則<PrintMark> 為N
        /// 4.若<BuyerCarrierType>=” 3J0002”&&<BuyerIdentifier> 有值時，該欄位必填
        /// 5.若消費者使用手機條碼索取買方統編發票，則該不論是否已列印紙本，該欄位皆必填"
        /// </summary>
        public string BuyerCarrierid2 { get; set; }

        /// <summary>
        /// 捐贈對象 (捐贈碼)
        /// </summary>
        public string BuyerNpoban { get; set; }

        /// <summary>
        /// 發票狀態 0: 預設 1:開立成功 2: 開立失敗
        /// </summary>
        public int Issue_Status { get; set; }

        /// <summary>
        /// 發票階段 0: 預設 1: 完成  2: 開立中
        /// </summary>
        public int State { get; set; }
        
        /// <summary>
        /// 發票作廢狀態 : 0:否 1:是
        /// </summary>
        public int Invalid_Status { get; set; }
        
        /// <summary>
        /// 開立回傳代碼
        /// </summary>
        public string RtnCode { get; set; }
        
        /// <summary>
        /// 開立回傳說明
        /// </summary>
        public string RtnMsg { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 異動日期
        /// </summary>
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// 異動人員
        /// </summary>
        public string Modifier { get; set; }

        /// <summary>
        /// 商品清單
        /// </summary>
        public List<CF_InvoiceIssue_ProductItemModel> ProductItem { get; set; }
    }

}
