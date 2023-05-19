using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    /// <summary>
    /// 取得會員等級更新結果
    /// </summary>
    public class GetMemberLevelResult: BaseResult
    {
        /// <summary>
        /// 會員等級
        /// </summary>
        public byte LevelID { get; set; }

        /// <summary>
        /// 會員類別
        /// </summary>
        public short MemberType { get; set; }

        /// <summary>
        /// 驗證類別
        /// </summary>
        public byte AuthType { get; set; }

        /// <summary>
        /// 手機驗證
        /// </summary>
        public bool checkPhone { get; set; }

        /// <summary>
        /// 身份證驗證
        /// </summary>
        public bool checkIDNO { get; set; }

        /// <summary>
        /// 金融工具驗證
        /// </summary>
        public bool checkFinancial { get; set; }

        /// <summary>
        /// 是否能夠[收/付款] 原因:(個人:11、商務:21，可能存在驗證尚未完成的會員，故不能進行收/附款)
        /// 0:不能進行交易
        /// 1:只允許付款
        /// 2:收/付款皆可
        /// </summary>
        public byte IsTradeStatus { get; set; }

        /// <summary>
        /// 原來的會員等級
        /// </summary>
        public byte OldLevelID { get; set; }
    }
}