using FakeBank.Domain.Entities.Accounts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FakeBank.WebApp.Models.BankAccountViewModels
{
    public class AccountTransactionViewModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "Please an amount greater than {1}")]
        public decimal Amount { get; set; }
        public Guid Id { get; set; }

        [Required]
        public TransactionType TransactionTypeId { get; set; }
    }
}
