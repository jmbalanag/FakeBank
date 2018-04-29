using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FakeBank.WebApp.Models.BankAccountViewModels
{
    public class AccountTransferViewModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "Please an amount greater than {1}")]
        public decimal Amount { get; set; }
        public Guid Id { get; set; }

        [Display(Name = "Receiver Account Number")]
        [Required(ErrorMessage ="Reciever Account Number is Required!")]
        public string RecieverAccountNumber { get; set; }

        public string Remarks { get; set; }
    }
}
