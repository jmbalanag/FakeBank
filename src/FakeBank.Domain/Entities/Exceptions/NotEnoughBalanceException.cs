using System;
using System.Collections.Generic;
using System.Text;

namespace FakeBank.Domain.Entities.Exceptions
{

    public class NotEnoughBalanceException : Exception
    {
    
        public Guid AccountId { get; set; }

        public NotEnoughBalanceException()
        {
            AccountId = Guid.Empty;
        }

        public NotEnoughBalanceException(string message)
            : base(message)
        {
        }

        public NotEnoughBalanceException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public NotEnoughBalanceException(string message, Guid accountId) : base(message)
        {
            AccountId = accountId;
        }
    }
}
