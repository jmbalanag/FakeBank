using System;
using System.Collections.Generic;
using System.Text;

namespace FakeBank.Domain.Entities.Accounts
{
    public enum TransactionType
    {
        Withdraw = 1,
        Deposit = 2,
        Transfer = 3
    }
}
