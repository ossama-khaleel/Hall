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
    public class ReviewsController : Controller
    {
        private readonly ModelContext _context;

        public ReviewsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Reviews
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Reviews.Include(r => r.Customers).Include(r => r.Hall);
            return View(await modelContext.ToListAsync());
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(r => r.Customers)
                .Include(r => r.Hall)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: Reviews/Create
        public IActionResult Create()
        {
            ViewData["CustomersId"] = new SelectList(_context.Customers, "Id", "Email");
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "HallName");
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Opinion,CustomersId,HallId")] Review review)
        {
            if (ModelState.IsValid)
            {
                review.HallId = HttpContext.Session.GetInt32("HallId");
                review.CustomersId= HttpContext.Session.GetInt32("CustomersId");
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "Reservations");
            }
            ViewData["CustomersId"] = new SelectList(_context.Customers, "Id", "Email", review.CustomersId);
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "HallName", review.HallId);
            return View(review);
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            ViewData["CustomersId"] = new SelectList(_context.Customers, "Id", "Email", review.CustomersId);
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "HallName", review.HallId);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Opinion,CustomersId,HallId")] Review review)
        {
            if (id != review.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.Id))
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
            ViewData["CustomersId"] = new SelectList(_context.Customers, "Id", "Email", review.CustomersId);
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "HallName", review.HallId);
            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(r => r.Customers)
                .Include(r => r.Hall)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var review = await _context.Reviews.FindAsync(id);
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(decimal id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}
