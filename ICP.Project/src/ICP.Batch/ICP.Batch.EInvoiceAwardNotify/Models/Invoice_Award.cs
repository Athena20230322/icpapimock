using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICP.Batch.EInvoiceAwardNotify.Models
{
    public class Invoice_Award
    {
        //資料類別，X:社福團體,A:會員廠商,B:非會員廠商,Y:無載具
        public string Type { get; set; }

        //廠商總公司統一編號
        public string TMer_Identifier { get; set; }

        //發票所屬年月ex:103012,10406
        public string YearMonth { get; set; }

        //檔案流水號
        public int Sys_ID { get; set; }

        //檔案名稱
        public string FileName { get; set; }

        //主檔代號
        public string Main { get; set; }

        //總記錄數
        public int Total_Count { get; set; }

        //總中獎金額
        public string TotalPrize_Amount { get; set; }

        //領獎時間起始
        public DateTime AwardBegin_Time { get; set; }

        //檔案下載時間
        public DateTime AwardEnd_Time { get; set; }

        //發票金額總計 (12)
        public DateTime Download_Time { get; set; }
    }
}
