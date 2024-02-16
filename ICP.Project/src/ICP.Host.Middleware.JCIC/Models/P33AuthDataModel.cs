using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICP.Host.Middleware.JCIC.Models
{
    public class P33AuthDataModel
    {
        /// <summary>
        /// 通報種類
        /// </summary>
        public string TYPE { get; set; }

        /// <summary>
        /// 通報單位
        /// </summary>
        public string CRI_PLACE { get; set; }

        /// <summary>
        /// 發生日期 民國年(YYYMMDD)
        /// </summary>
        public string CRI_DATE { get; set; }

        /// <summary>
        /// 通報日期 民國年(YYYMMDD)
        /// </summary>
        public string DOC_DATE { get; set; }

        /// <summary>
        /// 中文戶名
        /// </summary>
        public string CNAME { get; set; }

        /// <summary>
        /// 單據號碼 / 人頭帳戶
        /// </summary>
        public string INVOICE_NO { get; set; }

        /// <summary>
        /// 說明 1
        /// </summary>
        public string REMARK_1 { get; set; }

        /// <summary>
        /// 說明 2
        /// </summary>
        public string REMARK_2 { get; set; }

        /// <summary>
        /// 說明 3
        /// </summary>
        public string REMARK_3 { get; set; }

        /// <summary>
        /// 說明 4
        /// </summary>
        public string REMARK_4 { get; set; }

        /// <summary>
        /// 通報單位中文名稱
        /// </summary>
        public string DOC_NAME { get; set; }

        /// <summary>
        /// 警示帳戶解除代碼
        /// </summary>
        public string REL_CODE { get; set; }

        /// <summary>
        /// 警示帳戶解除原因
        /// </summary>
        public string REL_REASON { get; set; }

        /// <summary>
        /// 警示帳戶解除日期 民國年(YYYMMDD)
        /// </summary>
        public string REL_DATE { get; set; }

        /// <summary>
        /// 保留欄位
        /// </summary>
        public string FILLER { get; set; }
    }
}