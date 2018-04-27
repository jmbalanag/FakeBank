using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeBank.WebApp.Models.BankAccountViewModels
{
    public class DepositViewModel
    {
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
