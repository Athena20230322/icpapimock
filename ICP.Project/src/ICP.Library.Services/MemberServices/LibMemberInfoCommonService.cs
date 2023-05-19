namespace ICP.Library.Services.MemberServices
{
    //本service請勿使用任何相依注入，要相依注入請洽LibMemberInfoService
    public class LibMemberInfoCommonService
    {
        /// <summary>
        /// 姓名隱碼
        /// </summary>
        /// <param name="cName"></param>
        /// <returns></returns>
        public string ConcealPartialCName(string cName, char shadow = '*')
        {
            int index = cName.Length;

            char[] chars = new char[index - (index == 2 ? 1 : 2)];
            for (int i = 0; i < chars.Length; i++) { chars[i] = shadow; }
            string LastWord = index == 2 ? "" : cName.Substring(index - 1, 1);
            
            return $"{cName.Substring(0, 1)}{new string(chars)}{LastWord}";
        }
    }
}