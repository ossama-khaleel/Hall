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
    public class ReservationsController : Controller
    {
        private readonly ModelContext _context;

        public ReservationsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Reservations.Include(r => r.Customers).Include(r => r.Hall).Include(r => r.Payment);
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            ViewBag.CustomersId = HttpContext.Session.GetInt32("CustomersId");
            ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");

            return View(await modelContext.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Customers)
                .Include(r => r.Hall)
                .Include(r => r.Payment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            ViewBag.CustomersId = HttpContext.Session.GetInt32("CustomersId");
            ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");

            return View(reservation);
        }

        public IActionResult Hall_Id(int id)
        {
            HttpContext.Session.SetInt32("HallId", id);
            return RedirectToAction("Create", "Reservations");
        }


        // GET: Reservations/Create
        public IActionResult Create()
        {

            ViewBag.Reviews = _context.Reviews.Include(e=>e.Customers).Where(o => o.HallId == HttpContext.Session.GetInt32("HallId")).ToList();
            ViewBag.CustomersId = HttpContext.Session.GetInt32("CustomersId");
            ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            ViewData["CustomersId"] = new SelectList(_context.Customers, "Id", "Id");
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Id");
            ViewData["PaymentId"] = new SelectList(_context.Payments, "Id", "Id");
            ViewData["VisaId"] = new SelectList(_context.Visas.Where(v => v.CustomersId == HttpContext.Session.GetInt32("CustomersId")), "Id", "VisaNumber");
            ViewBag.error = HttpContext.Session.GetString("message");
            ViewBag.accept = HttpContext.Session.GetString("messageAccept");
            ViewBag.CustomersId = HttpContext.Session.GetInt32("CustomersId");
            ViewBag.home = _context.Homes.FirstOrDefault();
            ViewBag.NoMoney = HttpContext.Session.GetString("Money_is_not_enough");
            ViewBag.YouHaveAnOldRes = HttpContext.Session.GetString("YouHaveAnOldRes");
            ViewBag.HallDetails = _context.Halls.Include(a => a.Address).FirstOrDefault(a => a.Id == HttpContext.Session.GetInt32("HallId"));
            HttpContext.Session.Remove("messageAccept");
            HttpContext.Session.Remove("YouHaveAnOldRes");
            HttpContext.Session.Remove("Money_is_not_enough");
            HttpContext.Session.Remove("message");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateIn,DateOut,HallId,CustomersId,PaymentId")] Reservation reservation,DateTime DateIn,decimal VisaId)
        {
            
            ViewBag.Reviews= _context.Reviews.Include(e=>e.Customers).Where(o => o.HallId == HttpContext.Session.GetInt32("HallId")).ToList();
            ViewBag.CustomersId = HttpContext.Session.GetInt32("CustomersId");
            ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");
            if (ModelState.IsValid)
            {
                var getcheckIfHasOldRes = _context.Acceptances.Where(u => u.CustomersId == HttpContext.Session.GetInt32("CustomerId") && u.AcceptlistId == 1).FirstOrDefault();
                if (getcheckIfHasOldRes == null)
                {
                    var res = _context.Reservations.Where(u => u.DateIn == DateIn && u.HallId == HttpContext.Session.GetInt32("HallId")).FirstOrDefault();
                    if (res != null)
                    {

                        ViewBag.CustomersId = HttpContext.Session.GetInt32("CustomersId");
                        ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");

                        HttpContext.Session.SetString("message", "Date Is Already reserve");
                        ViewBag.error = HttpContext.Session.GetString("message");
                        ViewBag.HallDetails = _context.Halls.Include(u => u.Address).FirstOrDefault(u => u.Id == HttpContext.Session.GetInt32("HallId"));
                        ModelState.AddModelError(string.Empty, "The specified date is incorrectdsakjhfskjdlfh lkajshdfkljahs djkfhasjkdfh askjdhf kjashdf lkjhasd lfkjh as ljkdfh akljshdf lkjahsd flkjas.");
                        return RedirectToAction(nameof(Create));
                    }
                    else
                    {

                        ViewBag.CustomersId = HttpContext.Session.GetInt32("CustomersId");
                        ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");

                        HttpContext.Session.SetString("YouHaveAnOldRes", "");
                        var hallprice = _context.Halls.Where(u => u.Id == HttpContext.Session.GetInt32("HallId")).FirstOrDefault();
                        var visaAmount = _context.Visas.Where(u => u.Id == VisaId).FirstOrDefault();
                        if (visaAmount.VisaAmount >= hallprice.HallPrice)
                        {
                            HttpContext.Session.Remove("Money_is_not_enough");
                            HttpContext.Session.Remove("message");
                            HttpContext.Session.SetString("NoMoney", "");
                            var hall = _context.Halls.Where(u => u.Id == HttpContext.Session.GetInt32("HallId")).FirstOrDefault();
                            HttpContext.Session.SetInt32("price", (int)hall.HallPrice);
                            Payment payment = new Payment();
                            payment.PaymentAmount = hall.HallPrice;
                            payment.CustomersId =  HttpContext.Session.GetInt32("CustomersId");
                            payment.VisaId = VisaId;
                            _context.Add(payment);
                            await _context.SaveChangesAsync();

                            HttpContext.Session.SetInt32("ResId", (int)reservation.Id);
                            reservation.HallId = HttpContext.Session.GetInt32("HallId");
                            reservation.CustomersId = HttpContext.Session.GetInt32("CustomersId");
                            reservation.DateOut = DateIn.AddHours(23).AddMinutes(59);
                            reservation.PaymentId = payment.Id;
                            _context.Add(reservation);
                            await _context.SaveChangesAsync();

                            var visa = _context.Visas.Where(u => u.Id == VisaId).SingleOrDefault();
                            visa.VisaAmount = visa.VisaAmount - hall.HallPrice;
                            _context.Update(visa);
                            await _context.SaveChangesAsync();

                            Acceptance acceptance = new Acceptance();
                            acceptance.ReservationId = reservation.Id;
                            acceptance.HallId = hall.Id;
                            acceptance.CustomersId = HttpContext.Session.GetInt32("CustomersId");
                            acceptance.AcceptlistId = 3;
                            _context.Add(acceptance);
                            await _context.SaveChangesAsync();
                            HttpContext.Session.SetString("messageAccept", "Waiting For Accept");
                            ViewBag.acce = HttpContext.Session.GetString("messageAccept");
                            ViewBag.CustomersId = HttpContext.Session.GetInt32("CustomersId");
                            ViewBag.NoMoney = HttpContext.Session.GetString("NoMoney");
                            ViewBag.HallDetails = _context.Halls.Include(u => u.Address).FirstOrDefault(u => u.Id == HttpContext.Session.GetInt32("HallId"));
                            ViewBag.CustomersId = HttpContext.Session.GetInt32("CustomersId");
                            ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");

                            return RedirectToAction(nameof(Create));
                        }
                        else
                        {
                            ViewBag.CustomersId = HttpContext.Session.GetInt32("CustomersId");
                            ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");

                            HttpContext.Session.SetString("Money_is_not_enough", "You Dont Have Enough Money");
                            ViewBag.NoMoney = HttpContext.Session.GetString("Money_is_not_enough");
                            return RedirectToAction(nameof(Create));
                        }
                    }
                }
                else
                {
                    ViewBag.CustomersId = HttpContext.Session.GetInt32("CustomersId");
                    ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");

                    HttpContext.Session.SetString("YouHaveAnOldRes", "You Have An Old Reservation Still In Processing");
                    ViewBag.YouHaveAnOldRes = HttpContext.Session.GetString("YouHaveAnOldRes");
                    return RedirectToAction(nameof(Create));
                }
            }
            ViewBag.CustomersId = HttpContext.Session.GetInt32("CustomersId");
            ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");

            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            ViewData["CustomersId"] = new SelectList(_context.Customers, "Id", "Id", reservation.CustomersId);
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "HallName", reservation.HallId);
            ViewData["PaymentId"] = new SelectList(_context.Payments, "Id", "Id", reservation.PaymentId);
            ViewData["VisaId"] = new SelectList(_context.Visas.Where(v => v.CustomersId == HttpContext.Session.GetInt32("CustomersId")), "Id", "VisaNumber");
            ViewBag.home = _context.Homes.FirstOrDefault();
            return View(reservation);
        }






        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["CustomersId"] = new SelectList(_context.Customers, "Id", "Email", reservation.CustomersId);
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "HallName", reservation.HallId);
            ViewData["PaymentId"] = new SelectList(_context.Payments, "Id", "Id", reservation.PaymentId);
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            ViewBag.CustomersId = HttpContext.Session.GetInt32("CustomersId");
            ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");

            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,DateIn,DateOut,HallId,CustomersId,PaymentId")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
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
            ViewData["CustomersId"] = new SelectList(_context.Customers, "Id", "Email", reservation.CustomersId);
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "HallName", reservation.HallId);
            ViewData["PaymentId"] = new SelectList(_context.Payments, "Id", "Id", reservation.PaymentId);
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            ViewBag.CustomersId = HttpContext.Session.GetInt32("CustomersId");
            ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");

            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Customers)
                .Include(r => r.Hall)
                .Include(r => r.Payment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(decimal id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
