using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICP.Host.ApiTest.Models
{
    public class MockOPCustomSendResult<T>
    {
        public long MID { get; set; }

        public T request { get; set; }
    }
}