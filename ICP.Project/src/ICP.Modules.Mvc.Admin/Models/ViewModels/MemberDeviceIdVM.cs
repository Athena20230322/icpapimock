using System;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{
    public class MemberDeviceIdVM
    {
        
        [StringLength(50, MinimumLength = 1, ErrorMessage = "請輸入1-50裝置ID")]
        [Display(Name = "裝置 ID")]
        [Required(ErrorMessage = "請輸入裝置ID")]
        public string DeviceID { get; set; }

        public int Status { get; set; }

        
        [StringLength(150, MinimumLength = 1, ErrorMessage = "請輸入1-150備註")]
        [Display(Name = "備註")]
        [Required(ErrorMessage = "請輸入備註")]
        public string Memo { get; set; }

        public string User { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
    }
}