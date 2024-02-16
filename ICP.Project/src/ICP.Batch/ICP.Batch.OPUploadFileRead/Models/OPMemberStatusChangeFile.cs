using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.OPUploadFileRead.Models
{
    public class OPMemberStatusChangeFile
    {
        public class Header
        {
            public string yyyyMMdd { get; set; }

            public int count { get; set; }
        }

        public class Item
        {
            public string OPMID { get; set; }

            public string Status { get; set; }
        }

        public OPMemberStatusChangeFile()
        {
            header = new Header();
            items = new List<Item>();
        }

        public Header header { get; set; }

        public List<Item> items { get; set; }
    }
}
