using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.EInvoiceAwardNotify.Services
{
    public class ProgramService
    {
        #region 取得初始年月
        /// <summary>
        /// 取得初始年月
        /// </summary>
        /// <returns>回傳年月，以陣列格式，[0]為年[1]為月</returns>
        public string[] GetInitYearMonth(DateTime today)
        {
            string strDefaultMonths;

            //是否單月
            if (today.Month % 2 > 0)
            {
                //因為1月要撈去年的11、12月，所以另外判斷
                if (today.Month == 1)
                {
                    strDefaultMonths = "1112";
                }
                else
                {
                    //是否已開獎
                    if (today.Day >= 28)
                    {
                        //抓前2個月
                        strDefaultMonths = (today.Month - 2).ToString("00") + (today.Month - 1).ToString("00");
                    }
                    else
                    {
                        //抓前3-4個月
                        strDefaultMonths = (today.Month - 4).ToString("00") + (today.Month - 3).ToString("00");
                    }
                }
            }
            else
            {
                if (today.Month == 2)
                {
                    strDefaultMonths = "1112";
                }
                else
                {
                    //抓前2-3個月
                    strDefaultMonths = (today.Month - 3).ToString("00") + (today.Month - 2).ToString("00");
                }
            }

            //民國年
            int intTwYear = intTwYear = today.Year - 1911;

            //若對獎月為1112要抓去年
            if (strDefaultMonths == "1112")
                intTwYear--;

            //int intDefaultIndex = arrMonthParas.ToList().IndexOf(strDefaultMonths);

            return new string[]
            {
                //預設下載年份
                intTwYear.ToString(),

                //預設下載月份 "0102", "0304", "0506", "0708", "0910", "1112"
                strDefaultMonths
            };
        }

        public string GetInitMonth(DateTime today)
        {
            string strDefaultMonths;

            if (today.Month % 2 > 0)
            {
                //因為1月要撈去年的11、12月，所以另外判斷
                if (today.Month == 1)
                {
                    strDefaultMonths = "12";
                }
                else
                {
                    //是否已開獎
                    if (today.Day >= 28)
                    {
                        //抓前2個月
                        strDefaultMonths = (today.Month - 1).ToString("00");
                    }
                    else
                    {
                        //抓前3-4個月
                        strDefaultMonths = (today.Month - 3).ToString("00");
                    }
                }
            }
            else
            {
                if (today.Month == 2)
                {
                    strDefaultMonths = "12";
                }
                else
                {
                    //抓前2-3個月
                    strDefaultMonths = (today.Month - 2).ToString("00");
                }
            }

            return strDefaultMonths;
        }
        #endregion
    }
}
