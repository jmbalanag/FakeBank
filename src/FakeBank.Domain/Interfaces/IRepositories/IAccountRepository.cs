using FakeBank.Domain.Entities.Accounts;
using System;
using System.Collections.Generic;
using System.Text;

namespace FakeBank.Domain.Interfaces.IRepositories
{
    public interface IAccountRepository
    {
        /// <summary>
        /// checks whether acccount number exists
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        bool AccountNumberExists(string accountNumber);

        void Create(Account account);


    }
}
