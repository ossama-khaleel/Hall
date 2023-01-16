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
    public class TestimonialsController : Controller
    {
        private readonly ModelContext _context;

        public TestimonialsController(ModelContext context)
        {
            _context = context;
        }
        //var modelContext = _context.UserLogins.Include(u => u.Customers).Include(u => u.Role);
        //return View(await modelContext.ToListAsync());


        // GET: Testimonials
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Testimonials.Include(t => t.Acceptlist).Include(t => t.Customers).Include(t => t.Home);
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            ViewBag.CustomersId = HttpContext.Session.GetInt32("CustomersId");
            ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");
            ViewBag.CustomersImage = HttpContext.Session.GetString("CustomersImage");
            return View(await modelContext.ToListAsync());
        }

        // GET: Testimonials/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .Include(t => t.Acceptlist)
                .Include(t => t.Customers)
                .Include(t => t.Home)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testimonial == null)
            {
                return NotFound();
            }
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(testimonial);
        }

        // GET: Testimonials/Create
        public IActionResult Create()
        {
            ViewBag.CustomersId = HttpContext.Session.GetInt32("CustomersId");
            ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");
            ViewBag.CustomersImage = HttpContext.Session.GetString("CustomersImage");

            ViewData["AcceptlistId"] = new SelectList(_context.Acceptedlists, "Id", "AcceptStatus");
            ViewData["CustomersId"] = new SelectList(_context.Customers, "Id", "UserName");
            ViewData["HomeId"] = new SelectList(_context.Homes, "Id", "Id");
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View();
        }

        // POST: Testimonials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TestimonialOpinion,CustomersId,AcceptlistId,HomeId")] Testimonial testimonial)
        {
            if (ModelState.IsValid)
            {
                ViewBag.CustomersId = HttpContext.Session.GetInt32("CustomersId");
                ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");
                ViewBag.CustomersImage = HttpContext.Session.GetString("CustomersImage");
                testimonial.CustomersId = (decimal)HttpContext.Session.GetInt32("CustomersId");
                testimonial.HomeId = 31;
                testimonial.AcceptlistId = 3;
                _context.Add(testimonial);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "Testimonials");
            }
            ViewData["AcceptlistId"] = new SelectList(_context.Acceptedlists, "Id", "AcceptStatus", testimonial.AcceptlistId);
            ViewData["CustomersId"] = new SelectList(_context.Customers, "Id", "UserName", testimonial.CustomersId);
            ViewData["HomeId"] = new SelectList(_context.Homes, "Id", "Id", testimonial.HomeId);
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(testimonial);
        }

        // GET: Testimonials/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial == null)
            {
                return NotFound();
            }
            ViewData["AcceptlistId"] = new SelectList(_context.Acceptedlists, "Id", "AcceptStatus", testimonial.AcceptlistId);
            ViewData["CustomersId"] = new SelectList(_context.Customers, "Id", "UserName", testimonial.CustomersId);
            ViewData["HomeId"] = new SelectList(_context.Homes, "Id", "Id", testimonial.HomeId);
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(testimonial);
        }

        // POST: Testimonials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,TestimonialOpinion,CustomersId,AcceptlistId,HomeId")] Testimonial testimonial)
        {
            if (id != testimonial.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testimonial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestimonialExists(testimonial.Id))
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
            ViewData["AcceptlistId"] = new SelectList(_context.Acceptedlists, "Id", "AcceptStatus", testimonial.AcceptlistId);
            ViewData["CustomersId"] = new SelectList(_context.Customers, "Id", "UserName", testimonial.CustomersId);
            ViewData["HomeId"] = new SelectList(_context.Homes, "Id", "Id", testimonial.HomeId);
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(testimonial);
        }

        // GET: Testimonials/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .Include(t => t.Acceptlist)
                .Include(t => t.Customers)
                .Include(t => t.Home)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testimonial == null)
            {
                return NotFound();
            }
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(testimonial);
        }

        // POST: Testimonials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var testimonial = await _context.Testimonials.FindAsync(id);
            _context.Testimonials.Remove(testimonial);
            await _context.SaveChangesAsync();
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return RedirectToAction(nameof(Index));
        }

        private bool TestimonialExists(decimal id)
        {
            return _context.Testimonials.Any(e => e.Id == id);
        }
    }
}
