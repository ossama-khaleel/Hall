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
    public class AcceptedlistsController : Controller
    {
        private readonly ModelContext _context;

        public AcceptedlistsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Acceptedlists
        public async Task<IActionResult> Index()
        {
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(await _context.Acceptedlists.ToListAsync());
        }

        // GET: Acceptedlists/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var acceptedlist = await _context.Acceptedlists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (acceptedlist == null)
            {
                return NotFound();
            }
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(acceptedlist);
        }

        // GET: Acceptedlists/Create
        public IActionResult Create()
        {
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View();
        }

        // POST: Acceptedlists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AcceptStatus")] Acceptedlist acceptedlist)
        {
            if (ModelState.IsValid)
            {
                _context.Add(acceptedlist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(acceptedlist);
        }

        // GET: Acceptedlists/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var acceptedlist = await _context.Acceptedlists.FindAsync(id);
            if (acceptedlist == null)
            {
                return NotFound();
            }
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(acceptedlist);
        }

        // POST: Acceptedlists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,AcceptStatus")] Acceptedlist acceptedlist)
        {
            if (id != acceptedlist.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(acceptedlist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AcceptedlistExists(acceptedlist.Id))
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
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(acceptedlist);
        }

        // GET: Acceptedlists/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var acceptedlist = await _context.Acceptedlists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (acceptedlist == null)
            {
                return NotFound();
            }
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(acceptedlist);
        }

        // POST: Acceptedlists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var acceptedlist = await _context.Acceptedlists.FindAsync(id);
            _context.Acceptedlists.Remove(acceptedlist);
            await _context.SaveChangesAsync();
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return RedirectToAction(nameof(Index));
        }

        private bool AcceptedlistExists(decimal id)
        {
            return _context.Acceptedlists.Any(e => e.Id == id);
        }
    }
}
