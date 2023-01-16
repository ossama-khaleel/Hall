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
    public class UserLoginsController : Controller
    {
        private readonly ModelContext _context;

        public UserLoginsController(ModelContext context)
        {
            _context = context;
        }

        // GET: UserLogins
        public IActionResult Index()
        {
            var users = _context.Customers.ToList();
            var login = _context.UserLogins.ToList();
            var role = _context.Roles.ToList();

            //Join

            var multiTable = from u in users
                             join l in login on u.Id equals l.CustomersId
                             join r in role on l.RoleId equals r.Id
                             select new JoinTable { customer = u, userLogin = l, role = r };

            var modelContext = _context.UserLogins.Include(u => u.Customers).Include(u => u.Role);
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(multiTable);
        }

        // GET: UserLogins/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userLogin = await _context.UserLogins
                .Include(u => u.Customers)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userLogin == null)
            {
                return NotFound();
            }
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(userLogin);
        }

        // GET: UserLogins/Create
        public IActionResult Create()
        {
            ViewData["CustomersId"] = new SelectList(_context.UserLogins, "Id", "CustomersId");
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "RoleName");
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View();
        }

        // POST: UserLogins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Password,RoleId,CustomersId")] UserLogin userLogin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userLogin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomersId"] = new SelectList(_context.UserLogins, "Id", "CustomersId", userLogin.CustomersId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "RoleName", userLogin.RoleId);
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(userLogin);
        }

        // GET: UserLogins/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userLogin = await _context.UserLogins.FindAsync(id);
            if (userLogin == null)
            {
                return NotFound();
            }
            ViewData["CustomersId"] = new SelectList(_context.UserLogins, "Id", "CustomersId", userLogin.CustomersId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "RoleName", userLogin.RoleId);
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(userLogin);
        }

        // POST: UserLogins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,UserName,Password,RoleId,CustomersId")] UserLogin userLogin , int RoleId)
        {
            if (id != userLogin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var logins = await _context.UserLogins.Where(u => u.Id == id).SingleOrDefaultAsync();
                    HttpContext.Session.SetInt32("LoginId", (int)logins.Id);
                    HttpContext.Session.SetString("UserName", logins.UserName);
                    HttpContext.Session.SetString("Password", logins.Password);

                    logins.CustomersId = HttpContext.Session.GetInt32("LoginId");
                    logins.UserName = HttpContext.Session.GetString("UserName");
                    logins.Password = HttpContext.Session.GetString("Password");

                    if (userLogin.RoleId != null)
                    {
                        logins.RoleId = RoleId;
                    }
                    _context.Update(logins);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserLoginExists(userLogin.Id))
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
            ViewData["CustomersId"] = new SelectList(_context.UserLogins, "Id", "CustomersId", userLogin.CustomersId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "RoleName", userLogin.RoleId);
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(userLogin);
        }

        // GET: UserLogins/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userLogin = await _context.UserLogins
                .Include(u => u.Customers)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userLogin == null)
            {
                return NotFound();
            }
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(userLogin);
        }

        // POST: UserLogins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var userLogin = await _context.UserLogins.FindAsync(id);
            _context.UserLogins.Remove(userLogin);
            await _context.SaveChangesAsync();
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return RedirectToAction(nameof(Index));
        }

        private bool UserLoginExists(decimal id)
        {
            return _context.UserLogins.Any(e => e.Id == id);
        }
    }
}
