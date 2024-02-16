namespace ICP.Batch.AppRssPush.Models
{
    public enum AppRssPushEnum
    {
        /// <summary>
        /// 啟用且未推
        /// </summary>
        EnableAndNoPush=0,
        /// <summary>
        /// 啟用且已推
        /// </summary>
        EnableAndPush=1,
        /// <summary>
        /// 停用
        /// </summary>
        Disable=2,
        /// <summary>
        /// 發送中
        /// </summary>
        Send=3,
        /// <summary>
        /// 發送失敗
        /// </summary>
        Fail=4
    }
}