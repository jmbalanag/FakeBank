using System;
using System.Collections.Generic;
using System.Text;

namespace FakeBank.Domain.Entities.Exceptions
{

    public class ZeroBalanceException : Exception
    {

        public Guid AccountId { get; set; }

        public ZeroBalanceException()
        {
            AccountId = Guid.Empty;
        }

        public ZeroBalanceException(string message)
            : base(message)
        {
        }

        public ZeroBalanceException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public ZeroBalanceException(string message, Guid accountId) : base(message)
        {
            AccountId = accountId;
        }
    }
}
