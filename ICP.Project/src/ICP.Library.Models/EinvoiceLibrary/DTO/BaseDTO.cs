

namespace ICP.Library.Models.EinvoiceLibrary.DTO
{
    public class BaseDTO
    {
        private int _timeout = 15;

        /// <summary>
        /// 預設十五秒
        /// </summary>
        public int Timeout
        {
            get
            {
                return this._timeout;
            }
            set
            {
                this._timeout = value;
            }
        }
    }
}
