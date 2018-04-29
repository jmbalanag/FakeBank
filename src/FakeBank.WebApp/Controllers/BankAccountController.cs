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
                Account record = Account.CreateAccount(UserId, account.AccountName, account.AccountNumber);

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

            var account = await _context.Accounts.SingleOrDefaultAsync(m => m.Id == id);
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
                    var record = _context.Accounts.Find(id);

                    if (account.TransactionTypeId == TransactionType.Deposit)
                    {
                        record.Deposit(account.Amount, "");
                    }
                    else if (account.TransactionTypeId == TransactionType.Withdraw)
                    {
                        record.Withdraw(account.Amount);
                    }
                    else if (account.TransactionTypeId == TransactionType.Transfer)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    _context.Update(record);

                    _context.AddRange(record.AccountTransactions);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                    throw;

                }

                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

   
    }
}
