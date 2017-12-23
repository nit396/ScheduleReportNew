using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ScheduleReport.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Tài Khoản")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật Khẩu")]
        public string Password { get; set; }
    }
}