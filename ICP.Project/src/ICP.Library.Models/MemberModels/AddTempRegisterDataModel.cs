using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    using Infrastructure.Core.Models;

    /// <summary>
    /// 新增暫存會員
    /// </summary>
    public class AddTempRegisterDataModel: BaseIPModel
    {
        /// <summary>
        /// OPEN WALLET MID
        /// </summary>
        public string OPMID { get; set; }

        /// <summary>
        /// 帳號(已加密)
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密碼(已加密)
        /// </summary>
        public string Pwd { get; set; }

        /// <summary>
        /// 手機號碼
        /// </summary>
        public string CellPhone { get; set; }

        /// <summary>
        /// 會員註冊來源 0: App注冊 1:後台批次匯入
        /// </summary>
        public byte Source { get; set; }

        /// <summary>
        /// 推薦人
        /// </summary>
        public long ReferrerMID { get; set; }
    }
}
