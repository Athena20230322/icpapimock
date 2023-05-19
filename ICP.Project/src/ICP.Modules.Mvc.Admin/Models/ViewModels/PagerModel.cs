using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{
    public class PagerModel
    {
        public X.PagedList.IPagedList<object> model { get; set; }

        public Infrastructure.Core.Models.PageModel query { get; set; }

        public string action { get; set; }

        public System.Web.Mvc.Ajax.AjaxOptions ajaxOptions { get; set; }

        public PagerModel(
            string action, 
            X.PagedList.IPagedList<object> model, 
            Infrastructure.Core.Models.PageModel query, 
            System.Web.Mvc.Ajax.AjaxOptions ajaxOptions = null
            )
        {
            this.action = action;
            this.model = model;
            this.query = query;
            this.ajaxOptions = ajaxOptions;
        }
    }
}
