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

namespace FakeBank.WebApp.Controllers
{
     
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

        // GET: BankAccount/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .SingleOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
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
        public async Task<IActionResult> Deposit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.SingleOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: BankAccount/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deposit(Guid id, [Bind("Id,AccountNumber,AccountName")] Account account)
        {
            if (id != account.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: BankAccount/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .SingleOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: BankAccount/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(m => m.Id == id);
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(Guid id)
        {
            return _context.Accounts.Any(e => e.Id == id);
        }
    }
}
