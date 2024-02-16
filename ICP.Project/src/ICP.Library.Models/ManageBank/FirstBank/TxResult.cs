using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ICP.Library.Models.ManageBank.FirstBank
{
    public class TxResult
    {
        public TxHeaderModel TxHeader { get; set; }

        [XmlIgnore]
        public B2BResult TxRs { get; set; }
    }

    public class TxResult<T>: TxResult where T: B2BResult
    {
        public new T TxRs
        {
            get
            {
                return (T)base.TxRs;
            }
            set
            {
                base.TxRs = value;
            }
        }
    }
}
