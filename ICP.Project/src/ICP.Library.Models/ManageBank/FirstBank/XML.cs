using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ICP.Library.Models.ManageBank.FirstBank
{
    public class XML
    {
        [XmlIgnore]
        public object Tx { get; set; }
    }

    public class XML<T> : XML
    {
        public new T Tx
        {
            get
            {
                return (T)base.Tx;
            }
            set
            {
                base.Tx = value;
            }
        }
    }
}
