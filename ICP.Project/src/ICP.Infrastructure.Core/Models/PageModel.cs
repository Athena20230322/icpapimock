using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Models
{
    public class PageModel : ValidatableObject
    {
        private int _pageNo = 1;
        private int _pageSize = 10;

        public int PageNo
        {
            get

            {
                return _pageNo;
            }
            set
            {
                _pageNo = (value > 0) ? value : 1;
            }
        }

        public int PageSize
        {
            get

            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > 0) ? value : 10;
            }
        }
    }
}
