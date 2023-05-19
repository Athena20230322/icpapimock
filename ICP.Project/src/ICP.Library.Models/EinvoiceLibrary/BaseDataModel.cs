using System.Collections.Generic;
using ICP.Infrastructure.Core.Models;

namespace ICP.Library.Models.EinvoiceLibrary
{
    public class BaseDataModel:BaseResult
    {
        
        public Dictionary<string, object> RtnData { get; set; }
    }
    
}