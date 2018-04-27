using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeBank.WebApp.Models.BankAccountViewModels
{
    public class CreateAccountModel
    {
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
    }
}
