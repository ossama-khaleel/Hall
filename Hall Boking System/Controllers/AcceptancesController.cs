using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hall_Boking_System.Models;
using Microsoft.AspNetCore.Http;
using EmailServices;

namespace Hall_Boking_System.Controllers
{
    public class AcceptancesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IEmailSender _emailSender;
        public AcceptancesController(IEmailSender emailSender,ModelContext modelContext)
        {
            _emailSender = emailSender;
            _context = modelContext;
        }

        // GET: Acceptances
        public async Task<IActionResult> Index()
        {
           
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            var modelContext = _context.Acceptances.Include(a => a.Acceptlist).Include(a => a.Customers).Include(a => a.Hall).Include(a => a.Reservation);
            return View(await modelContext.ToListAsync());
        }

        // GET: Acceptances/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var acceptance = await _context.Acceptances
                .Include(a => a.Acceptlist)
                .Include(a => a.Customers)
                .Include(a => a.Hall)
                .Include(a => a.Reservation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (acceptance == null)
            {
                return NotFound();
            }

            return View(acceptance);
        }

        // GET: Acceptances/Create
        public IActionResult Create()
        {
            ViewData["AcceptlistId"] = new SelectList(_context.Acceptedlists, "Id", "AcceptStatus");
            ViewData["CustomersId"] = new SelectList(_context.Customers, "Id", "Email");
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "HallName");
            ViewData["ReservationId"] = new SelectList(_context.Reservations, "Id", "Id");
           
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View();
        }

        // POST: Acceptances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AcceptDate,AcceptlistId,CustomersId,HallId,ReservationId")] Acceptance acceptance)
        {
            if (ModelState.IsValid)
            {
                acceptance.AcceptlistId = 1;
                _context.Add(acceptance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AcceptlistId"] = new SelectList(_context.Acceptedlists, "Id", "AcceptStatus", acceptance.AcceptlistId);
            ViewData["CustomersId"] = new SelectList(_context.Customers, "Id", "Email", acceptance.CustomersId);
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "HallName", acceptance.HallId);
            ViewData["ReservationId"] = new SelectList(_context.Reservations, "Id", "Id", acceptance.ReservationId);
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(acceptance);
        }

        // GET: Acceptances/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var acceptance = await _context.Acceptances.Include(r => r.Reservation).Where(r => r.Id == id).FirstOrDefaultAsync();
            if (acceptance.AcceptlistId == 1)
            {
                acceptance.AcceptDate = DateTime.Now;
            }
            else if (acceptance.AcceptlistId == 2)
            {
                acceptance.AcceptDate = DateTime.Now;
            }
            else
            {
                acceptance.AcceptDate = null;
            }


            ViewData["AcceptlistId"] = new SelectList(_context.Acceptedlists, "Id", "AcceptStatus", acceptance.AcceptlistId);
            ViewData["CustomersId"] = new SelectList(_context.Customers, "Id", "Email", acceptance.CustomersId);
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "HallName", acceptance.HallId);
            ViewData["ReservationId"] = new SelectList(_context.Reservations, "Id", "Id", acceptance.ReservationId);
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(acceptance);
        }

        // POST: Acceptances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,AcceptDate,AcceptlistId,CustomersId,HallId,ReservationId")] Acceptance acceptance,int AcceptlistId)
        {
            //var acc = _context.Acceptances.Where(i => i.Id == id).FirstOrDefault();
            //var res = await _context.Reservations.Where(z => z.Id == acceptance.ReservationId).FirstOrDefaultAsync();
            //var visa = _context.Visas.Where(q => q.Id == res.VisaId).FirstOrDefault();
            //var hall = _context.Halls.Where(w => w.Id == res.HallId).FirstOrDefault();
            //var user = _context.Customers.Where(x => x.Id == acc.CustomersId).FirstOrDefault();
            if (id != acceptance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var accept = await _context.Acceptances.Include(u => u.Reservation).Where(u => u.Id == id).FirstOrDefaultAsync();
                    var res = await _context.Reservations.Where(u => u.Id == accept.ReservationId).FirstOrDefaultAsync();
                    var pay = _context.Payments.Where(u => u.Id == res.PaymentId).Include(u => u.Visa).FirstOrDefault();
                    var visa = _context.Visas.Where(u => u.Id == pay.VisaId).FirstOrDefault();
                    var user = _context.Customers.Where(u => u.Id == res.CustomersId).FirstOrDefault();
                    var userLog = _context.UserLogins.Where(u => u.CustomersId == user.Id).FirstOrDefault();
                    var hall = _context.Halls.Include(u => u.Address).Where(u => u.Id == accept.HallId).FirstOrDefault();

                    if (AcceptlistId == 2)
                    {
                        accept.AcceptlistId = AcceptlistId;
                        accept.AcceptDate = DateTime.Now;
                        _context.Update(accept);
                        await _context.SaveChangesAsync();
                        visa.VisaAmount = visa.VisaAmount +hall.HallPrice;
                        _context.Update(visa);
                        _context.SaveChanges();


                        //sending email

                        var message = new Message(new string[] { $"{user.Email}" }, "your Reservation was Rejact", $"Hi \t {userLog.UserName} \n your Reservation was Accpted \n Hall Name : {hall.HallName} \n Hall Address : {hall.Address.City} \n Reservation Date : {res.DateIn}");

                        _emailSender.SendEmail(message);


                    }
                    else if (AcceptlistId == 3)
                    {
                        accept.AcceptlistId = AcceptlistId;
                        accept.AcceptDate = null;
                        _context.Update(accept);
                        _context.SaveChanges();
                    }
                    else if (AcceptlistId == 1)
                    {
                        var message = new Message(new string[] { $"{user.Email}" }, "your Reservation was Accepted", $"Hi \t {userLog.UserName}  \n Hall Name : {hall.HallName} \n Hall Address : {hall.Address.City} \n Reservation Date : {res.DateIn} \n ${hall.HallPrice}Has Been Discount from \t Visa Number:{visa.VisaNumber}");
                        _emailSender.SendEmail(message);

                        accept.AcceptlistId = AcceptlistId;
                        accept.AcceptDate = DateTime.Now;
                        _context.Update(accept);
                    }
                    _context.Update(accept);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AcceptanceExists(acceptance.Id))
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
            ViewData["AcceptlistId"] = new SelectList(_context.Acceptedlists, "Id", "AcceptStatus", acceptance.AcceptlistId);
            ViewData["CustomersId"] = new SelectList(_context.Customers, "Id", "Email", acceptance.CustomersId);
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "HallName", acceptance.HallId);
            ViewData["ReservationId"] = new SelectList(_context.Reservations, "Id", "Id", acceptance.ReservationId);
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(acceptance);
        }

        // GET: Acceptances/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var acceptance = await _context.Acceptances
                .Include(a => a.Acceptlist)
                .Include(a => a.Customers)
                .Include(a => a.Hall)
                .Include(a => a.Reservation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (acceptance == null)
            {
                return NotFound();
            }
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(acceptance);
        }

        // POST: Acceptances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var acceptance = await _context.Acceptances.FindAsync(id);
            _context.Acceptances.Remove(acceptance);
            await _context.SaveChangesAsync();
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return RedirectToAction(nameof(Index));
        }

        private bool AcceptanceExists(decimal id)
        {
            return _context.Acceptances.Any(e => e.Id == id);
        }
    }
}
