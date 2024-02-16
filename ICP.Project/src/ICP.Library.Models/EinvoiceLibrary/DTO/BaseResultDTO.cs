using Newtonsoft.Json;

namespace ICP.Library.Models.EinvoiceLibrary.DTO
{
    public class BaseResultDTO
    {
        public string Code { get; set; }

        public string Msg { get; set; }

        public string V { get; set; }

        public string HashSerial { get; set; }

        /// <summary>
        ///  預設錯誤訊息
        /// </summary>
        public static string DefaultErrorString => JsonConvert.SerializeObject(DefaultErrorModel);

        /// <summary>
        /// 預設錯誤訊息
        /// </summary>
        public static BaseResultDTO DefaultErrorModel =>
            new BaseResultDTO
            {
                Code = "999",
                Msg = "財政部電子發票資訊中心系統異常，請稍候再試或洽客服人員"
            };
    }
}
