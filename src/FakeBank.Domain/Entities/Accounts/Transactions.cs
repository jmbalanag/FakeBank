using System;
using System.ComponentModel.DataAnnotations;

namespace FakeBank.Domain.Entities.Accounts
{
    public class Transaction 
    {
        public Transaction() { }

        [Key]
        public Guid Id { get; private set; }
        public DateTime Date { get; private set; }
        public decimal Amount { get; private set; }
        public string AccountNumber { get; private set; }
        public string Remarks { get; private set; }
        public TransactionType TransactionType { get; private set; }
        public Guid AccountId { get; set; }
        public Transaction(Guid accountId, TransactionType transactionType, decimal amount, string reciever, string remarks)
        {
            Id = Guid.NewGuid();
            AccountId = accountId;
            Date = DateTime.Now;
            TransactionType = transactionType;
            Amount = amount;
            AccountNumber = reciever;
            Remarks = remarks;
        }
    }
}
