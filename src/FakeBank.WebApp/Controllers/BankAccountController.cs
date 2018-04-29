using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FakeBank.Domain.Entities.Accounts;
using FakeBank.WebApp.Data;
using FakeBank.WebApp.Models.BankAccountViewModels;
using Microsoft.AspNetCore.Authorization;

namespace FakeBank.WebApp.Controllers
{

    [Authorize]
    public class BankAccountController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public BankAccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BankAccount
        public async Task<IActionResult> Index()
        {
            ViewBag.UserName = UserName;

            return View(await _context.Accounts.Where(x => x.UserId == UserId).ToListAsync());
        }

        public async Task<IActionResult> Transaction(Guid Id)
        {
            var model = from s in _context.Accounts
                        join p in _context.Transactions on s.Id equals p.AccountId
                        where s.UserId == UserId && s.Id == Id
                        select p;

            return View(model.OrderByDescending(x => x.Date));
        }


        // GET: BankAccount/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BankAccount/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountNumber,AccountName")] CreateAccountModel account)
        {
            if (ModelState.IsValid)
            {
                Account record = Account.CreateAccount(UserId, account.AccountNumber, account.AccountName);

                _context.Add(record);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: BankAccount/Edit/5
        public async Task<IActionResult> Transact(Guid? id, TransactionType transactionTypeId)
        {
            ViewBag.TransactionType = transactionTypeId.ToString();


            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.SingleOrDefaultAsync(m => m.Id == id && m.UserId == UserId);
            if (account == null)
            {
                return NotFound();
            }

            AccountTransactionViewModel model = new AccountTransactionViewModel()
            {
                Id = account.Id,
                TransactionTypeId = transactionTypeId
            };

            return View(model);
        }


        // POST: BankAccount/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Transact(Guid id, [Bind("Id,Amount, TransactionTypeId")] AccountTransactionViewModel account)
        {

            if (ModelState.IsValid)
            {
                try
                {

                    var record = _context.Accounts.FirstOrDefault(x => x.Id == id && x.UserId == UserId);

                    if (record == null)
                    {
                        ModelState.AddModelError("", "Depositor account not found.");
                        return View(account);
                    }

                    if (account.TransactionTypeId == TransactionType.Deposit)
                    {
                        record.Deposit(account.Amount, "");
                    }
                    else if (account.TransactionTypeId == TransactionType.Withdraw)
                    {
                        if(record.Balance < account.Amount)
                        {
                            ModelState.AddModelError("", "Amount cannot exceed balance.");
                            return View(account);
                        }
                        record.Withdraw(account.Amount);
                    }


                    _context.Update(record);

                    _context.AddRange(record.AccountTransactions);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    //this is how you get entries
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is Account)
                        {
                            if (account.TransactionTypeId == TransactionType.Withdraw)
                            {
                                if (_context.Accounts.Find(id).Balance < account.Amount)
                                {
                                    ModelState.AddModelError("Amount", "Another transaction completed while trying to complete current transsaction.");
                                    return View(account);
                                }
                                else
                                {
                                    var retry = _context.Accounts.FirstOrDefault(x => x.Id == id && x.UserId == UserId);
                                    retry.Withdraw(account.Amount);
                                    _context.Update(retry);
                                    _context.AddRange(retry.AccountTransactions);
                                    _context.SaveChanges();
                                }
                                //var proposedValues = entry.CurrentValues;
                                //var databaseValues = entry.GetDatabaseValues();

                                //foreach (var property in proposedValues.Properties)
                                //{
                                //    var proposedValue = proposedValues[property];
                                //    var databaseValue = databaseValues[property];

                                //    // TODO: decide which value should be written to database
                                //    // proposedValues[property] = <value to be saved>;
                                //}

                                //// Refresh original values to bypass next concurrency check
                                //entry.OriginalValues.SetValues(databaseValues);


                            }
                            else if (account.TransactionTypeId == TransactionType.Deposit)
                            {
                                var retry = _context.Accounts.FirstOrDefault(x => x.Id == id && x.UserId == UserId);
                                retry.Deposit(account.Amount, "");
                                _context.Update(retry);
                                _context.AddRange(retry.AccountTransactions);
                                _context.SaveChanges();

                            }

                        }


                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        public async Task<IActionResult> Transfer(Guid? id)
        {
            ViewBag.TransactionType = "Account Transfer";


            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.SingleOrDefaultAsync(m => m.Id == id && m.UserId == UserId);
            if (account == null)
            {
                return NotFound();
            }

            AccountTransferViewModel model = new AccountTransferViewModel()
            {
                Id = account.Id,

            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Transfer(Guid id, [Bind("Id,Amount, RecieverAccountNumber, Remarks")] AccountTransferViewModel account)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var record = _context.Accounts.FirstOrDefault(x => x.Id == id && x.UserId == UserId);

                    if (record == null)
                    {
                        ModelState.AddModelError("", "Depositor account not found.");
                        return View(account);
                    }

                    if (record.Balance < account.Amount)
                    {
                        ModelState.AddModelError("Amount", "Amount cannot exceed Balance.");
                        return View(account);
                    }

                    var reciever = _context.Accounts.FirstOrDefault(x => x.AccountNumber == account.RecieverAccountNumber);

                    if (reciever == null)
                    {
                        ModelState.AddModelError("RecieverAccountNumber", "Account Number does not exists. Please check the number and try again.");
                        return View(account);
                    }
                    if (reciever.AccountNumber == record.AccountNumber)
                    {
                        ModelState.AddModelError("RecieverAccountNumber", "Cant Transfer to same account. Please check the number and try again.");
                        return View(account);
                    }
                    else
                    {


                        record.Transfer(account.RecieverAccountNumber, account.Amount, account.Remarks);
                        reciever.ReceiveTransfer(record.AccountNumber, account.Amount, account.Remarks);
                        _context.Update(record);
                        _context.AddRange(record.AccountTransactions);

                        _context.Update(reciever);
                        _context.AddRange(reciever.AccountTransactions);


                        await _context.SaveChangesAsync();

                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_context.Accounts.Find(id).Balance < account.Amount)
                    {
                        ModelState.AddModelError("Amount", "Another transaction completed while trying to complete current transsaction.");
                        return View(account);
                    }
                    else
                    {
                        var record = _context.Accounts.FirstOrDefault(x => x.Id == id && x.UserId == UserId);
                        var reciever = _context.Accounts.FirstOrDefault(x => x.AccountNumber == account.RecieverAccountNumber);

                        record.Transfer(account.RecieverAccountNumber, account.Amount, account.Remarks);
                        reciever.ReceiveTransfer(record.AccountNumber, account.Amount, account.Remarks);
                        _context.Update(record);
                        _context.AddRange(record.AccountTransactions);

                        _context.Update(reciever);
                        _context.AddRange(reciever.AccountTransactions);
                        await _context.SaveChangesAsync();
                    }

                }

                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

    }
}
