using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.OpenWalletApi.Enums
{
    /// <summary>
    /// OP API 方法類型 (對應 ICP_Logging.dbo.Log_Member_OP_CustomAPI.Method)
    /// </summary>
    public enum CustomApiMethodType
    {
        None = 0,
        BindicashAccount = 1,
        UnBindicashAccount = 2,
        NoticeMemberDelete = 3,
        UpdateCarrierNum=4,
        NoticeMobileBarcode=5
    }
}
