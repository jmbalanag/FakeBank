using System;
using System.Collections.Generic;
using System.Text;

namespace FakeBank.Domain.Entities.Exceptions
{
    public class InactiveAccountFundTransferException : Exception
    {
        // Account with Inactive Status
        public Guid AccountId { get; set; }
        public string AccountNumberReciever { get; set; }

        public InactiveAccountFundTransferException()
        {
            AccountId = Guid.Empty;
        }

        public InactiveAccountFundTransferException(string message)
            : base(message)
        {
        }

        public InactiveAccountFundTransferException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public InactiveAccountFundTransferException(string message, Guid accountId, string reciever) : base(message)
        {
            AccountId = accountId;
            AccountNumberReciever = reciever;
        }
    }
}
