using FakeBank.Domain.Entities.Accounts;
using FakeBank.Domain.Entities.Exceptions;
using System;
using Xunit;

namespace FakeBank.Unit.Tests
{
    public class AccountTest
    {
        private readonly Guid AccountId;
        private readonly string AccountNumber;
        private readonly string AccountName;

        public AccountTest()
        {
            AccountId = Guid.NewGuid();

        }
        [Fact]
        public void DepositPositiveNumberIncrementBalance()
        {
            var account = Account.CreateAccount(AccountId, AccountNumber, AccountName);

            account.Deposit(100, "");

            Assert.Equal(100, account.Balance);
        }

        [Fact]
        public void DepositNegativeNumberThrowException()
        {
            var account = Account.CreateAccount(AccountId, AccountNumber, AccountName);

            Exception ex = Assert.Throws<Exception>(() => account.Deposit(-1, ""));

            Assert.Equal("Amount cannot be less than zero", ex.Message);


        }

        [Fact]
        public void WithdrawBelowBalanceReduceBalance()
        {
            var account = Account.CreateAccount(AccountId, AccountNumber, AccountName);

            account.Deposit(100, "");

            Assert.Equal(100, account.Balance);

            account.Withdraw(100);

            Assert.Equal(0, account.Balance);

            Assert.Equal(2, account.AccountTransactions.Count);
        }



        [Fact]
        public void WithdrawAboveBalanceThrowException()
        {
            var account = Account.CreateAccount(AccountId, AccountNumber, AccountName);

            account.Deposit(100, "");

            Assert.Equal(100, account.Balance);


            NotEnoughBalanceException ex = Assert.Throws<NotEnoughBalanceException>(() => account.Withdraw(10000));


        }

        [Fact]
        public void WithdrawZeroBalanceThrowException()
        {
            var account = Account.CreateAccount(AccountId, AccountNumber, AccountName);

            Assert.Equal(0, account.Balance);


            ZeroBalanceException ex = Assert.Throws<ZeroBalanceException>(() => account.Withdraw(10));
        }

        [Fact]
        public void TransferAboveBalanceThrowZeroBalanceException()
        {
            var account = Account.CreateAccount(AccountId, AccountNumber, AccountName);

            Assert.Equal(0, account.Balance);


            ZeroBalanceException ex = Assert.Throws<ZeroBalanceException>(() => account.Transfer("abcdefg", 1000, "above"));
        }

        [Fact]
        public void TransferBelowBalanceReduceBalance()
        {
            var account = Account.CreateAccount(AccountId, AccountNumber, AccountName);

            Assert.Equal(0, account.Balance);

            account.Deposit(100, "");

            Assert.Equal(100, account.Balance);

            account.Transfer("abcdefg", 99, "above");

            Assert.Equal(1, account.Balance);

        }

        [Fact]
        public void RecieveTransferIncreaseBalance()
        {
            var account = Account.CreateAccount(AccountId, AccountNumber, AccountName);

            Assert.Equal(0, account.Balance);
 

            account.ReceiveTransfer("abcdefg", 99, "above");

            Assert.Equal(99, account.Balance);

        }
    }
}
