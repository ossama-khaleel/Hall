using Hall_Boking_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hall_Boking_System.Controllers
{
    public class DashBoardController : Controller
    {
        private readonly ModelContext _context;
        public DashBoardController(ModelContext context)
        {
            _context = context;
        }

        public IActionResult Admin()
        {
            //Admin
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            //Count Customers
            ViewBag.NumberOfUser = _context.UserLogins.Where(e => e.RoleId == 2).Count();
            //Hall Booked 
            //var hallbooked = _context.Acceptances.Where(c => c.AcceptlistId == 1).Distinct().Count();
            var hallbooked = _context.Reservations.Select(b => b.HallId).Distinct().Count();
            //All Hall
            var allhall = _context.Halls.Select(w => w.Id).Count();
            //Hall Not Booked
            var hallnotbooked = allhall - hallbooked;
            ViewBag.HallBooked = hallbooked;
            ViewBag.HallNotBooked = hallnotbooked;
            ViewBag.AllHall = allhall;


            var user = _context.Customers.ToList();
            var hall = _context.Halls.ToList();
            var cat = _context.Categories.ToList();

            var categoriesName = _context.Categories.Select(x => x.CategoryName).ToList();
            var modelContext = _context.Acceptances.Where(c => c.AcceptlistId == 1).Include(p => p.Reservation).Include(h => h.Hall).Include(u => u.Customers).ToList();
            var chart = Tuple.Create<IEnumerable<Acceptance>, IEnumerable<Customer>, IEnumerable<Hall>, IEnumerable<Category>>(modelContext, user, hall, cat);

            List<int> count = new List<int>();
            foreach (var item in categoriesName) // item = dessert
            {
                count.Add(hall.Count(x => x.Category.CategoryName == item));
            }
            ViewBag.categoriesName = categoriesName;
            ViewBag.count = count;
            return View(chart);
        }
        public IActionResult Customer()
        {
            ViewBag.CustomersId = HttpContext.Session.GetInt32("CustomersId");
            ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");
            ViewBag.CustomersImage = HttpContext.Session.GetString("CustomersImage");
            return View();
        }

        public IActionResult Detail_Users()
        {
            var users = _context.Customers.ToList();
            var login = _context.UserLogins.ToList();
            var role = _context.Roles.ToList();

            //Join

            var multiTable = from u in users
                             join l in login on u.Id equals l.CustomersId
                             join r in role on l.RoleId equals r.Id
                             select new JoinTable { customer = u, userLogin = l, role = r };

            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(multiTable);
        }

        [HttpGet]
        public IActionResult Search()
        {
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            var modelContext = _context.Acceptances.Where(b => b.AcceptlistId == 1).Include(a => a.Reservation).Include(h => h.Hall).Include(p => p.Customers).ToList();
            return View(modelContext);
        }

        [HttpPost]
        public async Task<IActionResult> Search(DateTime? startDate, DateTime? endDate)
        {
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            var modelContext = _context.Acceptances.Where(b => b.AcceptlistId == 1).Include(p => p.Reservation).Include(h => h.Hall).Include(u => u.Customers);
            if (startDate == null && endDate == null)
            {
                return View(modelContext);
            }
            else if (startDate == null && endDate != null)
            {
                var result = await modelContext.Where(r => r.Reservation.DateOut.Value.Date <= endDate).ToListAsync();
                return View(result);
            }
            else if (startDate != null && endDate == null)
            {
                var result = await modelContext.Where(r => r.Reservation.DateIn.Value.Date >= startDate).ToListAsync();
                return View(result);
            }
            else
            {
                var result = await modelContext.Where(r => r.Reservation.DateIn.Value.Date >= startDate && r.Reservation.DateOut.Value.Date <= endDate).ToListAsync();
                return View(result);
            }
        }



        [HttpGet]
        public IActionResult Report_yaear()
        {
            ViewBag.home = _context.Homes.FirstOrDefault();
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminUserName = HttpContext.Session.GetString("AdminUserName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");

            ViewBag.NumberOfUsersWhoHaveBooked = _context.Acceptances.Where(r => r.AcceptlistId == 1).Select(x => x.CustomersId).Distinct().Count();
            ViewBag.ReportDate = DateTime.Now;
            var ac = _context.Acceptances.Where(u => u.AcceptlistId == 1).ToList();
            var payment = _context.Payments.ToList();
            var reservations = _context.Reservations.ToList();
            var join = from a in ac
                       join r in reservations on a.ReservationId equals r.Id
                       join p in payment on r.PaymentId equals p.Id
                       select new JoinPayAcc { payment = p, acceptance = a, reservation = r };
            ViewBag.sum = join.Sum(u => u.payment.PaymentAmount);



            var Reports_Annual = _context.Acceptances.Where(c => c.AcceptlistId == 1).Include(p => p.Reservation).Include(h => h.Hall).Include(u => u.Customers).ToList();
            return View(Reports_Annual);
        }

        [HttpPost]
        public async Task<IActionResult> Report_yaear(DateTime? AcceptDate)
        {

            var ac = _context.Acceptances.Where(u => u.AcceptlistId == 2).ToList();
            var payment = _context.Payments.ToList();
            var reservations = _context.Reservations.ToList();

            var join = from a in ac
                       join r in reservations on a.ReservationId equals r.Id
                       join p in payment on r.PaymentId equals p.Id
                       select new JoinPayAcc { payment = p, acceptance = a, reservation = r };
            ViewBag.sum = join.Sum(u => u.payment.PaymentAmount);

            ViewBag.home = _context.Homes.FirstOrDefault();
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminUserName = HttpContext.Session.GetString("AdminUserName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            var Reports_Annual = _context.Acceptances.Where(c => c.AcceptlistId == 1).Include(p => p.Reservation).Include(h => h.Hall).Include(u => u.Customers);

            if (AcceptDate == null)
            {
                var join1 = from a in ac
                            join r in reservations on a.ReservationId equals r.Id
                            join p in payment on r.PaymentId equals p.Id
                            select new JoinPayAcc { payment = p, acceptance = a, reservation = r };
                ViewBag.sum = join1.Sum(u => u.payment.PaymentAmount);

                ViewBag.NumberOfUsersWhoHaveBooked = _context.Acceptances.Where(r => r.AcceptlistId == 1).Select(x => x.CustomersId).Distinct().Count();
                ViewBag.ReportDate = DateTime.Now;
                return View(Reports_Annual);
            }
            else /*if (checkedDate != null)*/
            {
                ViewBag.NumberOfUsersWhoHaveBooked = _context.Acceptances.Where(r => r.AcceptlistId == 1 && r.AcceptDate.Value.Year == AcceptDate.Value.Year).Select(x => x.CustomersId).Distinct().Count();
                ViewBag.ReportDate = DateTime.Now;


                var join1 = from a in ac
                            join r in reservations on a.ReservationId equals r.Id
                            join p in payment on r.PaymentId equals p.Id
                            where a.AcceptDate.Value.Year == AcceptDate.Value.Year
                            select new JoinPayAcc { payment = p, acceptance = a, reservation = r };
                ViewBag.sum = join1.Sum(u => u.payment.PaymentAmount);

                var result = await Reports_Annual.Where(c => c.AcceptDate.Value.Year == AcceptDate.Value.Year).ToListAsync();
                return View(result);
            }
        }

















        [HttpGet]
        public IActionResult Report()
        {
            ViewBag.home = _context.Homes.FirstOrDefault();
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            ViewBag.NumberOfUsersWhoHaveBooked = _context.Acceptances.Where(r => r.AcceptlistId == 1).Select(x => x.CustomersId).Distinct().Count();
            ViewBag.ReportDate = DateTime.Now;
            var ac = _context.Acceptances.Where(u => u.AcceptlistId == 1).ToList();
            var payment = _context.Payments.ToList();
            var reservations = _context.Reservations.ToList();

            var join = from a in ac
                       join r in reservations on a.ReservationId equals r.Id
                       join p in payment on r.PaymentId equals p.Id
                       select new JoinPayAcc { payment = p, acceptance = a, reservation = r };
            ViewBag.sum = join.Sum(u => u.payment.PaymentAmount);

            var modelContext = _context.Acceptances.Where(c => c.AcceptlistId == 1).Include(p => p.Reservation).Include(h => h.Hall).Include(u => u.Customers).ToList();
            return View(modelContext);
        }

        [HttpPost]
        public async Task<IActionResult> Report(DateTime? AcceptDate)
        {
            var ac = _context.Acceptances.Where(u => u.AcceptlistId == 1).ToList();
            var payment = _context.Payments.ToList();
            var reservations = _context.Reservations.ToList();

            var join = from a in ac
                       join r in reservations on a.ReservationId equals r.Id
                       join p in payment on r.PaymentId equals p.Id
                       select new JoinPayAcc { payment = p, acceptance = a, reservation = r };
            ViewBag.sum = join.Sum(u => u.payment.PaymentAmount);


            ViewBag.home = _context.Homes.FirstOrDefault();
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            var modelContext = _context.Acceptances.Where(c => c.AcceptlistId == 1).Include(p => p.Reservation).Include(h => h.Hall).Include(u => u.Customers);

            if (AcceptDate == null)
            {


                var join1 = from a in ac
                            join r in reservations on a.ReservationId equals r.Id
                            join p in payment on r.PaymentId equals p.Id
                            select new JoinPayAcc { payment = p, acceptance = a, reservation = r };
                ViewBag.sum = join1.Sum(u => u.payment.PaymentAmount);

                ViewBag.NumberOfUsersWhoHaveBooked = _context.Acceptances.Where(r => r.AcceptlistId == 1).Select(x => x.CustomersId).Distinct().Count();
                ViewBag.ReportDate = DateTime.Now;
                return View(modelContext);
            }
            else
            {
                var join1 = from a in ac
                            join r in reservations on a.ReservationId equals r.Id
                            join p in payment on r.PaymentId equals p.Id
                            where a.AcceptDate.Value.Month == AcceptDate.Value.Month
                            select new JoinPayAcc { payment = p, acceptance = a, reservation = r };
                ViewBag.sum = join1.Sum(u => u.payment.PaymentAmount);

                ViewBag.NumberOfUsersWhoHaveBooked = _context.Acceptances.Where(r => r.AcceptlistId == 1 && r.AcceptDate.Value.Month == AcceptDate.Value.Month).Select(x => x.CustomersId).Distinct().Count();
                ViewBag.ReportDate = DateTime.Now;

                var result = await modelContext.Where(c => c.AcceptDate.Value.Month == AcceptDate.Value.Month).ToListAsync();
                return View(result);
            }
        }
    }
}
