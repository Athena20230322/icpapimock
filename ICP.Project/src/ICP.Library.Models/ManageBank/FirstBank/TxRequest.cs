using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.ManageBank.FirstBank
{
    public class TxRequest<T>
    {
        public TxHeaderModel TxHeader { get; set; }

        public T TxRq { get; set; }
    }
}
