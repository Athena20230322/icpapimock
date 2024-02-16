using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Models.Consts
{
    public class RegexConst
    {
        public const string Only_Number = @"^[0-9]+$";

        public const string AdminUserPwd = @"^(?!.*[^0-9a-zA-Z])(?=.*\d)(?=.*[a-zA-Z]).{6,20}$";

        public const string UserCode = @"^(?!.*[^0-9a-zA-Z])(?=.*\d)(?=.*[a-zA-Z]).{6,12}$";

        public const string UserPwd = @"^(?!.*[^0-9a-zA-Z])(?=.*\d)(?=.*[a-zA-Z]).{6,12}$";

        public const string CellPhone = @"^09[0-9]{8}$";

        public const string Email = @"^((([A-Za-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([A-Za-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([A-Za-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([A-Za-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([A-Za-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([A-Za-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([A-Za-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([A-Za-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([A-Za-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([A-Za-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";

        public const string NickName = @"^[\u4e00-\u9fa5\uf900-\ufa2d@@ \-&a-zA-Z0-9]+$";
        public const string ChineseCName = @"^['•\u4E00-\u9FA5\uF900-\uFA2D]+$";
		/// <summary>
        /// 身分證字號
        /// </summary>
        public const string IDNO = @"^[A-Z]{1}[0-9]{9}$";

        /// <summary>
        /// 居留證字號
        /// </summary>
        public const string UniformID = @"^(?=.*[A-Z]{1}[A-Z]{1}[0-9]{8}).{10}$";

        /// <summary>
        /// 民國日期(YYYMMDD)
        /// </summary>
        public const string ChineseYYYMMDD = @"^[0-9]{7}$";

		

		public const string ICPMID = @"[0-9]{16}$";

		public const string UnifiedBusinessNo = @"[0-9]{8}$";

        /// <summary>
        /// 發票期號
        /// </summary>
        public const string InvPeriod = @"^[0-9]{4}[02468]{1}$";
        
        /// <summary>
        /// 發票號碼
        /// </summary>
        public const string InvNum = @"^[a-zA-Z]{2}[0-9]{8}$";

        /// 中英數字
        /// </summary>
        public const string Ch_En_Number = @"^[\u4e00-\u9fa5_a-zA-Z0-9_]+$";

        /// <summary>
        /// HH:mm
        /// </summary>
        public const string HHmm = @"^(20|21|22|23|[0-1]?\d):[0-5]?\d$";

        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public const string yyyyMMdd = @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$";

        public const string IDNOOrUniformID = @"^[A-Z]{1}[A-D1-2]{1}[0-9]{8}$";
        /// <summary>
        /// 英文或數字
        /// </summary>
        public const string En_Number = @"^[a-zA-Z0-9]+$";

        /// <summary>
        /// 市話
        /// </summary>
        public const string Tel = @"^\(?\d{2}\)?[\s\-]?\d{4}\-?\d{4}$";

    }
}
