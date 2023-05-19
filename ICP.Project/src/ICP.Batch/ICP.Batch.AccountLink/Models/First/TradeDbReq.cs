using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.AccountLink.Models.First
{
    /// <summary>
    /// 交易明細資料 記錄請求
    /// </summary>
    public class TradeDbReq : TradeModel
    {/// <summary>
     /// 檔案名稱
     /// </summary>
        public string FileName { get; set; }
    }
}
