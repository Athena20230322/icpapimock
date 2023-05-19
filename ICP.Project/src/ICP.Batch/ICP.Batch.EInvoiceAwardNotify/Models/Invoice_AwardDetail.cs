using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICP.Batch.EInvoiceAwardNotify.Models
{
    public class Invoice_AwardDetail
    {
        //發票流水號
        public long InvoiceID { get; set; }

        //檔案流水號
        public int Sys_ID { get; set; }

        //發票字軌
        public string Number_ID { get; set; }

        //發票號碼
        public string Number { get; set; }

        //賣方名稱
        public string Mer_Name { get; set; }

        //賣方統一編號
        public string Mer_Identifier { get; set; }

        //賣方地址
        public string Mer_Address { get; set; }

        //開立發票時間
        public DateTime Invoice_Time { get; set; }

        //發票金額
        public string Sales_Amount { get; set; }

        //載具類別號碼
        public string Carrier_Type { get; set; }

        //載具類別名稱
        public string Carrier_Name { get; set; }

        //載具明碼
        public string CarrierId_Clear { get; set; }

        //載具隱碼
        public string CarrierId_Hide { get; set; }

        //四位隨機碼
        public string Random_Number { get; set; }

        //獎別，A:特別獎1000萬,B:無實體2000元,C:無實體100萬,0:特獎200萬元,1:頭獎20萬元,2:二獎4萬元,3:三獎1萬元,4:四獎4000元,5:五獎1000元,6:六獎200元
        public string Prize_Type { get; set; }

        //獎金
        public string Prize_Amount { get; set; }

        //買受人統一編號
        public string Customer_Identifier { get; set; }

        //大平台已匯款註記，Y: 會員在大平台註冊且中獎，大平台已辦匯款，會員廠商請自行管制不要重複給獎
        public string Platform_Deposit_Mark { get; set; }

        //例外代碼，10:存證區,20:同店同號,30:不同店同號,40:例外建檔,51:漏上傳
        public string Exception_Code { get; set; }

        //列印格式，P1~P6:傳統發票 發票類別 1~6、T1~T6:電子發票 發票類別 1~6
        public string Print_Type { get; set; }

        //中獎發票唯一識別碼
        public string Unique_Identifier { get; set; }

        //建檔時間
        public DateTime Create_Time { get; set; }

        //通知狀態 0:未發送 1:已發送
        public byte Notify_State { get; set; }

        //通知時間
        public DateTime? Notify_Time { get; set; }
    }
}
