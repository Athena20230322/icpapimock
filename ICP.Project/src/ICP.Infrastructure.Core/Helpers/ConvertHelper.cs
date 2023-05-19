using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Helpers
{
    public class ConvertHelper
    {
        /// <summary>
        /// 西元年轉民國年YYYMMDD
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSimpleTaiwanDate(DateTime? input)
        {
            if (input == null)
            {
                return string.Empty;
            }

            return (input.Value.Year - 1911).ToString().PadLeft(3, '0') + input.Value.Month.ToString().PadLeft(2, '0') + input.Value.Day.ToString().PadLeft(2, '0');
        }

        public long Date2TimeSpan(DateTime d)
        {
            DateTime dtTimeSpanBase = new DateTime(1970, 1, 1).AddHours(8);
            return (long)d.Subtract(dtTimeSpanBase).TotalMilliseconds;
        }

        public DateTime? TimeSpan2Date(long t)
        {
            if (t <= 0) return null;

            long tt = long.Parse(t.ToString().PadRight(13, '0'));

            DateTime dtTimeSpanBase = new DateTime(1970, 1, 1).AddHours(8);
            return dtTimeSpanBase.AddMilliseconds(tt);
        }
    }
}
