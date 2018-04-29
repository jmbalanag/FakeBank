
using FakeBank.Domain.Entities.Exceptions;
using FakeBank.Domain.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FakeBank.Domain.Entities.Accounts
{
    public class Account
    {
        public Account() { }

        private static string AccountDoesNotExisteption = "Cannot transact with Non Existent Accounts";
        public static string NotEnoughBalanceExceptionMessage = "Not enough balance to perform withdrawal operation";
        public static string ZeroBalanceExceptionMessage = "Zero Balance, invalid withdrawal operation";

        public Guid UserId { get; private set; }

        public List<Transaction> AccountTransactions { get; private set; }

        [Key]
        public Guid Id { get; private set; }
        public string AccountNumber { get; private set; }
        public string AccountName { get; private set; }
        public decimal Balance { get; set; }


        public static Account CreateAccount(Guid userId, string accountNumber, string accountName)
        {
            Account account = new Account()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                AccountName = accountName,
                AccountNumber = accountNumber,
                Balance = 0
            };

            account.AccountTransactions = new List<Transaction>();

            return account;
        }
 


        // (ID, AccountNumber, AccountName, Password, Balance, CreatedDate)

        private void AssertHasEnoughBalance(decimal amount)
        {
            if (Balance == 0)
            {
                throw new ZeroBalanceException(ZeroBalanceExceptionMessage, Id);
            }
            if (amount > Balance)
            {
                throw new NotEnoughBalanceException(NotEnoughBalanceExceptionMessage, Id);
            }

        }

        private void AssertAccountFundTransferAccountExists(IAccountInfoService accountService, string accountNumber)
        {
            if (!accountService.AccountNumberExists(accountNumber))
            {
                throw new InactiveAccountFundTransferException(AccountDoesNotExisteption, Id, accountNumber);
            }
        }

        public decimal GetBalance()
        {
            return Balance;
        }

        public void Withdraw(decimal amount)
        {
       

            AssertHasEnoughBalance(amount);
            Balance -= amount;
            AddTransactionHistory(TransactionType.Withdraw, amount);
        }
        public void Deposit(decimal amount, string remarks)
        {
            if(amount < 0)
            {
                throw new Exception("Amount cannot be less than zero");
            }
     

            Balance += amount;

            AddTransactionHistory(TransactionType.Deposit, amount, remarks);
        }

        public void Transfer(string accountNumberReciever, decimal amount, string remarks)
        {
       

            //  AssertAccountFundTransferAccountExists(accountService, accountNumberReciever);
            AssertHasEnoughBalance(amount);
            Balance -= amount;

            AddTransactionHistory(TransactionType.TransferSent, amount, remarks, accountNumberReciever);
        }


        public void ReceiveTransfer(string accountNumberSender, decimal amount, string remarks)
        {
          


            Balance += amount;
            AddTransactionHistory(TransactionType.TransferReceived, amount, remarks, accountNumberSender);
        }


        private void AddTransactionHistory(TransactionType transactionType, decimal amount, string remarks = "", string accountNumberReceiver = "")
        {
            if (AccountTransactions == null)
            {
                AccountTransactions = new List<Transaction>();
            }


            Transaction trans = new Transaction(Id, transactionType, amount, accountNumberReceiver, remarks);

            AccountTransactions.Add(trans);

        }


        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
