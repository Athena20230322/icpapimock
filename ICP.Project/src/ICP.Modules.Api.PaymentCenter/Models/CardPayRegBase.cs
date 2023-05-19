using ICP.Infrastructure.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Models
{
    public class CardPayRegBase
    {
        /// <summary>
        /// 傳送序號 → 特店自編需唯一
        /// </summary>
        public string SendSeqNo { get; set; }

        /// <summary>
        /// 特店代碼 → 開辦時,一銀給定
        /// </summary>
        public string MID { get; set; }

        /// <summary>
        /// 申請/取消 → 申請：Y 取消：N
        /// </summary>
        public string Apply { get; set; }

        /// <summary>
        /// 繳款期限 → 西元YYYYMMDD
        /// </summary>
        public string DueDate { get; set; }

        /// <summary>
        /// 銷帳編號 → 虛擬帳號或實體帳號
        /// </summary>
        public string InAccountNo { get; set; }

        /// <summary>
        /// 繳款金額 → 純數字Decimal(11,0)
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// 計算押碼
        /// </summary>
        /// <returns></returns>
        public string ComputeMacValue()
        {
            string sha1MacData = new HashCryptoHelper().HashSha1($"{SendSeqNo}{MID}{DueDate}{InAccountNo}{Amount}");    // 先 SHA1 加密
            sha1MacData = sha1MacData.PadRight(48, '0');    // 限定長度48，不足右補0

            var tripleDesKey = "3347a70B0E31A3E09AAdFeca";
            var tripleDesIv = "00000000";

            return new TripleCryptoHelper().Encrypt(sha1MacData, tripleDesKey, tripleDesIv, CipherMode.CBC, PaddingMode.None);
        }
    }
}
