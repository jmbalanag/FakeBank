using System;
using System.ComponentModel.DataAnnotations;

namespace FakeBank.Domain.Entities.Accounts
{
    public class Transaction 
    {
        [Key]
        public Guid Id { get; private set; }
        public DateTime Date { get; private set; }
        public decimal Amount { get; private set; }
        public string AccountNumber { get; private set; }
        public string Remarks { get; private set; }
        public TransactionType TransactionType { get; private set; }

        public Transaction(TransactionType transactionType, decimal amount, string reciever, string remarks)
        {
            Id = Guid.NewGuid();
            Date = DateTime.Now;
            TransactionType = transactionType;
            Amount = amount;
            AccountNumber = reciever;
            Remarks = remarks;
        }
    }
}
