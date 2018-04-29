using System;
using System.Collections.Generic;
using System.Text;

namespace FakeBank.Domain.Entities.Accounts
{
    public enum TransactionType
    {
        Deposit = 1,
        Withdraw = 2,
        Transfer = 3
    }
}
