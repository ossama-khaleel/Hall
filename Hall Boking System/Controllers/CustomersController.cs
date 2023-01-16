using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hall_Boking_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Hall_Boking_System.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment webHostEnviroment;

        public CustomersController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            this.webHostEnviroment = webHostEnviroment;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            var customerId = HttpContext.Session.GetInt32("CustomersId");
            if (HttpContext.Session.GetInt32("AdminId") != null)
            {
                ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
                ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
                ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");

                var users = await _context.Customers.Include(u => u.UserLogins).Where(r => r.Id == adminId).FirstOrDefaultAsync();
                return View(users);
            }
            else if (HttpContext.Session.GetInt32("CustomersId") != null)
            {
                ViewBag.CustomersId = HttpContext.Session.GetInt32("CustomersId");
                ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");
                ViewBag.CustomersImage = HttpContext.Session.GetString("CustomersImage");
                var users = await _context.Customers.Include(l => l.UserLogins).Where(i => i.Id == customerId).FirstOrDefaultAsync();
                //var users = await _context.UserLogins.Include(u => u.Customers).Where(r => r.CustomersId == customerId).FirstOrDefaultAsync();
                return View(users);
            }
            return View();

        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,UserName,Password,UserImage,Email,PhoneNumber,ImageFile")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                if (customer.ImageFile != null)
                {
                    string wwwrootPath = webHostEnviroment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + customer.ImageFile.FileName;
                    //1523f14f-5535-40c6-82bb-7d3b9edf2e75_piza2.jpg
                    string path = Path.Combine(wwwrootPath + "/Images/" + fileName);
                    using (var filestream = new FileStream(path, FileMode.Create))
                    {
                        await customer.ImageFile.CopyToAsync(filestream);
                    }
                    customer.UserImage = fileName;
                }
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            ViewBag.CustomersId = HttpContext.Session.GetInt32("CustomersId");
            ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");
            ViewBag.CustomersImage = HttpContext.Session.GetString("CustomersImage");
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,FirstName,LastName,UserName,Password,UserImage,Email,PhoneNumber,ImageFile")] Customer customer,string UserName, string Password)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (customer.ImageFile != null)
                    {
                        string wwwrootPath = webHostEnviroment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + customer.ImageFile.FileName;
                        //1523f14f-5535-40c6-82bb-7d3b9edf2e75_piza2.jpg
                        string path = Path.Combine(wwwrootPath + "/Images/" + fileName);
                        using (var filestream = new FileStream(path, FileMode.Create))
                        {
                            await customer.ImageFile.CopyToAsync(filestream);
                        }

                        customer.UserImage = fileName;
                        _context.Update(customer);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {

                        //var login = _context.Customers.Where(u => u.Id == id).FirstOrDefault();
                        //var loginx = _context.UserLogins.Where(u => u.CustomersId == id).FirstOrDefault();
                        if (customer.Id == HttpContext.Session.GetInt32("AdminId"))
                        {
                            customer.UserImage = HttpContext.Session.GetString("AdminImage");
                            _context.Update(customer);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            customer.UserImage = HttpContext.Session.GetString("CustomersImage");
                            _context.Update(customer);
                            await _context.SaveChangesAsync();
                        }

                    }
                    var login = _context.UserLogins.Where(u => u.CustomersId == id).FirstOrDefault();
                    if(Password != null)
                    {
                        login.Password= Password;
                    }

                    if(UserName != null)
                    {
                        login.UserName = UserName;
                    }
                    _context.Update(login);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");
            ViewBag.CustomersImage = HttpContext.Session.GetString("CustomersImage");
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(decimal id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}