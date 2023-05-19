using ICP.Infrastructure.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{
    public class QueryUnregisteredDataVM : PageModel
    {
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public string CName { get; set; }

        public string CellPhone { get; set; }

        [Required]
        public byte Source { get; set; }
    }
}
