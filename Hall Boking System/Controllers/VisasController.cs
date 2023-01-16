using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hall_Boking_System.Models;
using Microsoft.AspNetCore.Http;

namespace Hall_Boking_System.Controllers
{
    public class VisasController : Controller
    {
        private readonly ModelContext _context;

        public VisasController(ModelContext context)
        {
            _context = context;
        }

        // GET: Visas
        public IActionResult Index()
        {
            var customerId = HttpContext.Session.GetInt32("CustomersId");

            ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");
            ViewBag.CustomersImage = HttpContext.Session.GetString("CustomersImage");
            var visa = _context.Visas.Where(p => p.CustomersId == HttpContext.Session.GetInt32("CustomersId"));
                return View(visa);
        }

        // GET: Visas/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visa = await _context.Visas
                .Include(v => v.Customers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (visa == null)
            {
                return NotFound();
            }
            ViewBag.CustomersId = HttpContext.Session.GetInt32("CustomersId");
            ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");
            return View(visa);
        }

        // GET: Visas/Create
        public IActionResult Create()
        {
            ViewData["CustomersId"] = new SelectList(_context.Customers, "Id", "Id");
            ViewBag.CustomersId = HttpContext.Session.GetInt32("CustomersId");
            ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");
            return View();
        }

        // POST: Visas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VisaName,VisaNumber,VisaAmount,CustomersId")] Visa visa)
        {
            if (ModelState.IsValid)
            {
                visa.VisaName = " ";
                visa.CustomersId = HttpContext.Session.GetInt32("CustomersId");
                _context.Add(visa);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "Reservations");
            }
            ViewData["CustomersId"] = new SelectList(_context.Customers, "Id", "Id", visa.CustomersId);
            ViewBag.CustomersId = HttpContext.Session.GetInt32("CustomersId");
            ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");
            return View(visa);
        }

        // GET: Visas/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visa = await _context.Visas.FindAsync(id);
            if (visa == null)
            {
                return NotFound();
            }
            ViewData["CustomersId"] = new SelectList(_context.Customers, "Id", "Id", visa.CustomersId);
            ViewBag.CustomersId = HttpContext.Session.GetInt32("CustomersId");
            ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");

            return View(visa);
        }

        // POST: Visas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,VisaName,VisaNumber,VisaAmount,CustomersId")] Visa visa)
        {
            if (id != visa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(visa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VisaExists(visa.Id))
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
            ViewData["CustomersId"] = new SelectList(_context.Customers, "Id", "Id", visa.CustomersId);
            ViewBag.CustomersId = HttpContext.Session.GetInt32("CustomersId");
            ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");
            return View(visa);
        }

        // GET: Visas/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visa = await _context.Visas
                .Include(v => v.Customers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (visa == null)
            {
                return NotFound();
            }
            ViewBag.CustomersId = HttpContext.Session.GetInt32("CustomersId");
            ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");

            return View(visa);
        }

        // POST: Visas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var visa = await _context.Visas.FindAsync(id);
            _context.Visas.Remove(visa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VisaExists(decimal id)
        {
            return _context.Visas.Any(e => e.Id == id);
        }
    }
}
