using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ICP.Host.Middleware.JCIC.Services
{
    using Infrastructure.Core.Models.Consts;

    public class RegexService
    {
        public static bool VerifyIDNO(string IDNO)
        {
            return Regex.IsMatch(IDNO, RegexConst.IDNO);
        }

        public static bool VerifyUniformID(string UniformID)
        {
            return Regex.IsMatch(UniformID, RegexConst.UniformID);
        }

        public static bool VerifyYYYMMDD(string YYYMMDD)
        {
            return Regex.IsMatch(YYYMMDD, RegexConst.ChineseYYYMMDD);
        }

        public static bool VerifyIssueLoc(string issueLoc)
        {
            return Regex.IsMatch(issueLoc, "^[0-9]{5,8}$");
        }
    }
}