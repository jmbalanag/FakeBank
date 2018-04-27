using FakeBank.Domain.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace FakeBank.Domain.Services
{
    public class AccountNumberGeneratorService : IAccountNumberGeneratorService
    {
        private IAccountRepository _accountRepository;
        public AccountNumberGeneratorService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public string GenerateAccountNumber()
        {
            throw new NotImplementedException();
        }
    }
}
