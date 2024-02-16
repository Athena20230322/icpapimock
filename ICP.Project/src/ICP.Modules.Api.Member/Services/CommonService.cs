using ICP.Infrastructure.Core.Helpers;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Utils;
using ICP.Modules.Api.Member.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Services
{
    public class CommonService
    {
        /// <summary>
        /// 檢查密碼規則
        /// １、接受6-20碼 (英數字/英文加符號/數字加符號/英+數+符號)
        /// ２、密碼不得與會員帳號相同
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="password"></param>
        /// <param name="confirmPassword"></param>
        /// <returns></returns>
        public BaseResult VerifyPassword(string userCode, string password, long mid = -1, bool isPCAdmin = false)
        {
            BaseResult baseModel = VerifyPasswordFormat(password);

            // 密碼不得與帳號相同
            if (userCode == password)
            {
                baseModel.RtnCode = 200023;
                //baseModel.RtnMsg = "密碼不可與帳號相同";
                return baseModel;
            }

            if (baseModel.RtnCode != 1)
            {
                baseModel.RtnCode = 200002;
                //baseModel.RtnMsg = "請輸入6-20個半形英數混合之密碼";
                return baseModel;
            }

            return baseModel;
        }

        public BaseResult VerifyPasswordFormat(string password)
        {
            BaseResult baseModel = new BaseResult
            {
                RtnCode = 0,
                RtnMsg = "失敗"
            };

            // 符號僅允許 ~ ! $ % ^ & * ( ) [ ] { } : ; " ' , . / ? < > + - = | _ @ # \
            string regexNum = "^[0-9]+$";
            string regexEng = "^[A-Za-z]+$";
            string regexMark = "^[~!$%^&*()\\[\\]{}:;\"',.\\/\\\\?\\<\\>\\+\\-=|_@#]+$";
            string regexAllowFormat = "^[A-Za-z0-9]{6,20}$";

            // 檢查規則
            if (string.IsNullOrWhiteSpace(password))
            {
                baseModel.RtnMsg = "請輸入密碼。";
            }
            // 只允許英文+數字+符號 
            else if (!Regex.IsMatch(password, regexAllowFormat))
            {
                baseModel.RtnMsg = "密碼格式不符。";
            }
            // 檢查密碼強度
            else
            {
                bool hasEng = password.Any(x => Regex.IsMatch(x.ToString(), regexEng));
                bool hasNum = password.Any(x => Regex.IsMatch(x.ToString(), regexNum));
                bool hasMark = password.Any(x => Regex.IsMatch(x.ToString(), regexMark));

                // 英文+數字
                bool isPass_1 = (hasEng && hasNum);
                // 英文+符號
                bool isPass_2 = (hasEng && hasMark);
                // 數字+符號
                bool isPass_3 = (hasNum && hasMark);
                // 英文+數字+符號
                bool isPass_4 = (hasEng && hasNum && hasMark);

                if (!isPass_1 && !isPass_2 && !isPass_3 && !isPass_4)
                {
                    baseModel.RtnMsg = "密碼強度不符。";
                }
                else
                {
                    baseModel.RtnCode = 1;
                    baseModel.RtnMsg = "成功";
                }
            }

            // 統一錯誤訊息
            if (baseModel.RtnCode != 1)
            {
                baseModel.RtnCode = 200002;
                baseModel.RtnMsg = "請輸入6-20個半形英數混合之密碼";
            }

            return baseModel;
        }

        #region 比較版本

        public bool CompareToAppVersion(string CurrentVersion, string StandardVersion)
        {

            bool reslut = false;

            if (string.IsNullOrWhiteSpace(CurrentVersion)) { return false; }

            var currentVersion = new Version(CurrentVersion);

            int Major = currentVersion.Major == -1 ? 0 : currentVersion.Major;
            int Minor = currentVersion.Minor == -1 ? 0 : currentVersion.Minor;
            int Build = currentVersion.Build == -1 ? 0 : currentVersion.Build;
            int MinorRevision = currentVersion.Revision == -1 ? 0 : currentVersion.MinorRevision;

            var standardVersion = new Version(StandardVersion);
            var new_Version = new Version(Major, Minor, Build, MinorRevision);

            if (new_Version.CompareTo(standardVersion) >= 0)
            {
                reslut = true;
            }

            return reslut;
        }
        #endregion

    }
}
