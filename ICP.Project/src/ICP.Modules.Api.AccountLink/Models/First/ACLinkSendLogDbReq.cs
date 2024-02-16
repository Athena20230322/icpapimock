namespace ICP.Modules.Api.AccountLink.Models.First
{
    public class ACLinkSendLogDbReq
    {
        /// <summary>
        /// Api類型
        /// </summary>
        public string ApiType { get; set; }

        /// <summary>
        /// 訊息序號
        /// </summary>
        public string MSG_NO { get; set; }

        /// <summary>
        /// 平台代碼
        /// </summary>
        public string EC_ID { get; set; }

        /// <summary>
        /// 平台會員代號
        /// </summary>
        public string EC_USER { get; set; }

        /// <summary>
        /// 使用者身分證字號
        /// </summary>
        public string CUST_ID { get; set; }

        /// <summary>
        /// 綁定結果回傳URL
        /// </summary>
        public string RSLT_URL { get; set; }

        /// <summary>
        /// 綁定成功導向頁
        /// </summary>
        public string SUCC_URL { get; set; }

        /// <summary>
        /// 綁定失敗導向頁
        /// </summary>
        public string FAIL_URL { get; set; }

        /// <summary>
        /// 帳號識別碼
        /// </summary>
        public string INDT_ACNT { get; set; }

        /// <summary>
        /// 查詢訊息序號
        /// </summary>
        public string SER_MSG_NO { get; set; }

        /// <summary>
        /// 交易時間
        /// </summary>
        public string TRNS_TIME { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        public string TRNS_NO { get; set; }

        /// <summary>
        /// 交易明細
        /// </summary>
        public string TRNS_NOTE { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        public string TRNS_AMT { get; set; }

        /// <summary>
        /// 轉入虛擬帳戶
        /// </summary>
        /// <remarks>由電商平台依據第一銀行的帳號規則進行虛擬帳號編列</remarks>
        public string PAYEE_ACNT { get; set; }

        /// <summary>
        /// 交易類別
        /// </summary>
        /// <remarks>P : 扣 款 D : 儲 值</remarks>
        public string PAY_TYPE { get; set; }

        /// <summary>
        /// 簽章日期時間
        /// </summary>
        public string SIGN_TIME { get; set; }

        /// <summary>
        /// 簽章值
        /// </summary>
        public string SIGN_VALUE { get; set; }

        /// <summary>
        /// 憑證序號
        /// </summary>
        public string CERT_SN { get; set; }
    }
}
