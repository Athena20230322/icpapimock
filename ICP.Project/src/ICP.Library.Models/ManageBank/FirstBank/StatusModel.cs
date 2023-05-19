using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ICP.Library.Models.ManageBank.FirstBank
{
    public class StatusModel
    {
        /// <summary>
        /// 網銀批號
        /// </summary>
        public string Pno { get; set; }

        /// <summary>
        /// 主序號
        /// </summary>
        public string Mno { get; set; }

        /// <summary>
        /// 次序號
        /// </summary>
        public string Sno { get; set; }

        /// <summary>
        /// 帳務日
        /// </summary>
        public string TxnDate { get; set; }

        /// <summary>
        /// 清算金額
        /// </summary>
        [XmlIgnore]
        public decimal? SettleAmt
        {
            get
            {
                if (string.IsNullOrEmpty(_SettleAmt))
                    return null;
                else
                    return Convert.ToDecimal(_SettleAmt);
            }
            set
            {
                if (value == null)
                    _SettleAmt = string.Empty;
                else
                    _SettleAmt = value.ToString();
            }
        }

        [XmlElement(ElementName = "SettleAmt")]
        public string _SettleAmt { get; set; }

        /// <summary>
        /// 清算日期
        /// </summary>
        public string SettleDate { get; set; }

        /// <summary>
        /// 清算交易序號
        /// </summary>
        public string SettleSeqNo { get; set; }

        /// <summary>
        /// 匯款序號
        /// </summary>
        public string RemitNo { get; set; }

        /// <summary>
        /// 匯款序號
        /// </summary>
        public string RemitSeqNo { get; set; }

        /// <summary>
        /// 中心處理序號
        /// </summary>
        public string HostSeqNo { get; set; }

        /// <summary>
        /// 餘額正負號
        /// </summary>
        public string PBBalSign { get; set; }

        /// <summary>
        /// 帳面餘額
        /// </summary>
        [XmlIgnore]
        public decimal? PBBalAmt
        {
            get
            {
                if (string.IsNullOrEmpty(_PBBalAmt))
                    return null;
                else
                    return Convert.ToDecimal(_PBBalAmt);
            }
            set
            {
                if (value == null)
                    _PBBalAmt = string.Empty;
                else
                    _PBBalAmt = value.ToString();
            }
        }

        [XmlElement(ElementName = "PBBalAmt")]
        public string _PBBalAmt { get; set; }

        /// <summary>
        /// 可用餘額正負號
        /// </summary>
        public string AvBalSign { get; set; }

        /// <summary>
        /// 可用餘額
        /// </summary>
        [XmlIgnore]
        public decimal? AvBalAmt
        {
            get
            {
                if (string.IsNullOrEmpty(_AvBalAmt))
                    return null;
                else
                    return Convert.ToDecimal(_AvBalAmt);
            }
            set
            {
                if (value == null)
                    _AvBalAmt = string.Empty;
                else
                    _AvBalAmt = value.ToString();
            }
        }

        [XmlElement(ElementName = "AvBalAmt")]
        public string _AvBalAmt { get; set; }

        /// <summary>
        /// 付款狀況代碼
        /// </summary>
        public string StatusCode { get; set; }

        /// <summary>
        /// 付款狀況代碼說明
        /// </summary>
        public string StatusDesc { get; set; }

        /// <summary>
        /// 錯誤代碼
        /// </summary>
        public string FcbErrCode { get; set; }

        /// <summary>
        /// 錯誤代碼說明
        /// </summary>
        public string FcbErrDesc { get; set; }
    }
}
