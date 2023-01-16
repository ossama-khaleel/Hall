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
    public class ContactusController : Controller
    {
        private readonly ModelContext _context;

        public ContactusController(ModelContext context)
        {
            _context = context;
        }

        // GET: Contactus
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Contactus.Include(c => c.Home);
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(await modelContext.ToListAsync());
        }

        // GET: Contactus/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactu = await _context.Contactus
                .Include(c => c.Home)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactu == null)
            {
                return NotFound();
            }
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(contactu);
        }

        // GET: Contactus/Create
        public IActionResult Create()
        {
            ViewData["HomeId"] = new SelectList(_context.Homes, "Id", "HomeImage1");
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View();
        }

        // POST: Contactus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,PhoneNumber,Message,HomeId")] Contactu contactu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contactu);
                await _context.SaveChangesAsync();
            }
            ViewData["HomeId"] = new SelectList(_context.Homes, "Id", "HomeImage1", contactu.HomeId);
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View();
        }

        // GET: Contactus/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactu = await _context.Contactus.FindAsync(id);
            if (contactu == null)
            {
                return NotFound();
            }
            ViewData["HomeId"] = new SelectList(_context.Homes, "Id", "HomeImage1", contactu.HomeId);
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(contactu);
        }

        // POST: Contactus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Name,Email,PhoneNumber,Message,HomeId")] Contactu contactu)
        {
            if (id != contactu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contactu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactuExists(contactu.Id))
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
            ViewData["HomeId"] = new SelectList(_context.Homes, "Id", "HomeImage1", contactu.HomeId);
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(contactu);
        }

        // GET: Contactus/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactu = await _context.Contactus
                .Include(c => c.Home)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactu == null)
            {
                return NotFound();
            }
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(contactu);
        }

        // POST: Contactus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var contactu = await _context.Contactus.FindAsync(id);
            _context.Contactus.Remove(contactu);
            await _context.SaveChangesAsync();
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return RedirectToAction(nameof(Index));
        }

        private bool ContactuExists(decimal id)
        {
            return _context.Contactus.Any(e => e.Id == id);
        }
    }
}
