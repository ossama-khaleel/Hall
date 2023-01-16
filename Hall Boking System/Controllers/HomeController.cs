using Hall_Boking_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Hall_Boking_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ModelContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ModelContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ViewData["HomeId"] = new SelectList(_context.Homes, "Id", "HomeImage1");
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");

            var home = _context.Homes.ToList();
            var categories = _context.Categories.Distinct().ToList();
            var testion = _context.Testimonials.ToList();
            var custom = _context.Customers.ToList();
            var homePage = Tuple.Create<IEnumerable<Home>, IEnumerable<Category>,IEnumerable<Testimonial>, IEnumerable<Customer>>(home, categories, testion, custom);

            ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");

            return View(homePage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Id,Name,Email,PhoneNumber,Message,HomeId")] Contactu contactu)
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

            var home = _context.Homes.ToList();
            var categories = _context.Categories.Distinct().ToList();
            var testion = _context.Testimonials.ToList();
            var custom = _context.Customers.ToList();
            var homePage = Tuple.Create<IEnumerable<Home>, IEnumerable<Category>, IEnumerable<Testimonial>, IEnumerable<Customer>>(home, categories, testion, custom);
            ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");

            return View(homePage);
        }


        public IActionResult AboutUs()
        {
            var home = _context.Homes.ToList();
            var acbout = _context.Aboutus.ToList();
            var acboutPage = Tuple.Create<IEnumerable<Home>, IEnumerable<Aboutu>>(home, acbout);
            ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");

            return View(acboutPage);
        }

        //[HttpGet]
        //public IActionResult hallcategories(int id)
        //{
        //    ViewBag.CustomersId = HttpContext.Session.GetInt32("CustomersId");
        //    ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");
        //    ViewBag.CustomersImage = HttpContext.Session.GetString("CustomersImage");
        //    var hallcat = _context.Halls.Where(x => x.CategoryId == id).Include(z => z.Address).ToList();
        //    return View(hallcat);
        //}
        //[HttpPost]
        //public async Task<IActionResult> hallcategories(string searchTerm)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(searchTerm))
        //        {
        //            var modelContext = _context.Halls.Include(h => h.Address);
        //            ViewBag.CustomersId = HttpContext.Session.GetInt32("CustomersId");
        //            ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");
        //            ViewBag.CustomersImage = HttpContext.Session.GetString("CustomersImage");
        //            return View(await modelContext.ToListAsync());
        //        }
        //        else
        //        {
        //            var modelContext = _context.Halls.Include(h => h.Address).Where(x => x.HallName.Contains(searchTerm.ToUpper()) || x.Address.City.Contains(searchTerm.ToUpper()));
        //            ViewBag.CustomersId = HttpContext.Session.GetInt32("CustomersId");
        //            ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");
        //            ViewBag.CustomersImage = HttpContext.Session.GetString("CustomersImage");
        //            return View(await modelContext.ToListAsync());
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        return RedirectToAction("Login", "LoginAndRegestration");
        //    }

        //}

        [HttpGet]
        public IActionResult hallcategories(int id)
        {
            ViewBag.CustomersId = HttpContext.Session.GetInt32("CustomersId");
            ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");
            ViewBag.CustomersImage = HttpContext.Session.GetString("CustomersImage");

            Hall hall = new Hall();

            var pro = _context.Halls.Where(x => x.CategoryId == id).Include(z => z.Address).ToList();
            //HttpContext.Session.SetInt32("HallId",(int)hall.HallId);
            return View(pro);
        }

        [HttpPost]
        public async Task<IActionResult> hallcategories(string searchTerm)
        {
            ViewBag.CustomersId = HttpContext.Session.GetInt32("CustomersId");
            ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");
            ViewBag.CustomersImage = HttpContext.Session.GetString("CustomersImage");

            try
            {
                if (string.IsNullOrEmpty(searchTerm))
                {
                    //var modelContext = _context.Halls.Include(h => h.Address).Include(h => h.Category);
                    var modelContext = _context.Halls.Include(h => h.Address);
                    return View(await modelContext.ToListAsync());
                }
                else
                {
                    var modelContext = _context.Halls.Include(h => h.Address).Include(h => h.Category).Where(x => x.HallName.ToUpper().Contains(searchTerm.ToUpper()) || x.Category.CategoryName.ToUpper().Contains(searchTerm.ToUpper()) || x.Address.City.ToUpper().Contains(searchTerm.ToUpper()));
                    return View(await modelContext.ToListAsync());
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "LoginAndRegestration");
            }
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
