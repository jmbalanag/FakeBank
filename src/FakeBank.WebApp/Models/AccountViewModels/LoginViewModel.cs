using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FakeBank.WebApp.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "ACCOUNT NUMBER")]
        public string AccountNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
