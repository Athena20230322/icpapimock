using ICP.Library.Models.AccountLinkApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Services.AccountLinkApi
{
    public class HSMService
    {
        private string account = string.Empty;
        private string CERTSN = string.Empty;
        //private CGPKIAgentATLLib.CGPKIFacadeClass CGPKFacade;
        private string key1 = string.Empty;
        private string keyset = string.Empty;
        private string pwd = string.Empty;

        //Constructor
        public HSMService(string _sURL)
        {
            //CGPKFacade = new CGPKIAgentATLLib.CGPKIFacadeClass();
            //CGPKFacade.RA2_ServerAddURL(_sURL);
        }

        /// <summary>
        /// 取得憑證序號
        /// </summary>
        /// <param name="bankType"></param>
        /// <returns></returns>
        public string GetCERTSN()
        {
            return CERTSN.ToString();
        }

        /// <summary>
        /// 取得簽章前必須做的設定
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string SetHSM(HSMSettingModel model)
        {
            //if (CGPKFacade.RA2_GetReturnCode() != 0)
            //{
            //    return "系統錯誤! " + CGPKFacade.RA2_GetErrorMsg();
            //}

            //key1 = model.HSMKey1;
            //keyset = model.HSMKeyset;
            //account = model.HSMAccount;
            //pwd = model.HSMPwd;
            //CERTSN = model.HSMCERTSN;

            //var isLogin = CGPKFacade.RA2_Login(account, pwd, "登入系統");
            //if (isLogin != 0)
            //{
            //    return "登入失敗! " + CGPKFacade.RA2_GetErrorMsg();
            //}
            return "";
        }

        /// <summary>
        /// 簽章
        /// </summary>
        /// <param name="sData">簽章資料</param>
        /// <returns></returns>
        public string Sign(string sData, string encoding = "big5")
        {
            //var chk = CGPKFacade.SS3_Sign(key1, keyset, sData, encoding, "api", 0, "簽章SS3", "");
            string result = string.Empty;

            //if (chk == 0)
            //{
            //    result = CGPKFacade.SS_GetSignature();
            //}
            //else
            //{
            //    result = CGPKFacade.RA2_GetErrorMsg();
            //}

            return result;
        }
    }
}
