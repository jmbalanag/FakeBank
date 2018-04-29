using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FakeBank.WebApp.Models.BankAccountViewModels
{
    public class CreateAccountModel
    {
        [Display(Name = "Account Name")]
        [Required(ErrorMessage ="Account Name is Required")]
        public string AccountName { get; set; }

        [Display(Name = "Account Number")]
        [Required(ErrorMessage = "Account Number is Required")]
        public string AccountNumber { get; set; }
    }
}
