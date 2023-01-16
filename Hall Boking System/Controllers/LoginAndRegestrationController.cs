using Hall_Boking_System.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Hall_Boking_System.Controllers
{
    public class LoginAndRegestrationController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public LoginAndRegestrationController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Register([Bind("FirstName,LastName,UserName,Password,UserImage,Email,PhoneNumber,ImageFile")] Customer customer, string username, string password)
        {
            if (ModelState.IsValid)
            {
                string wwwrootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + customer.ImageFile.FileName;
                string extension = Path.GetExtension(customer.ImageFile.FileName);
                customer.UserImage = fileName;
                string path = Path.Combine(wwwrootPath + "/Images/" + fileName);
                using (var filestream = new FileStream(path, FileMode.Create))
                {
                    await customer.ImageFile.CopyToAsync(filestream);
                }
                _context.Add(customer);
                await _context.SaveChangesAsync();
                var ListId = _context.Customers.OrderByDescending(p => p.Id).FirstOrDefault().Id;
                UserLogin userlogin = new UserLogin();
                userlogin.RoleId = 2;
                userlogin.UserName = username;
                userlogin.Password = password;
                userlogin.CustomersId = ListId;
                _context.Add(userlogin);
                await _context.SaveChangesAsync();
                ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");
                return RedirectToAction("Login", "LoginAndRegestration");
            }
            return View(customer);
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([Bind("UserName,Password")] UserLogin userlogin)
        {
            var auth = _context.UserLogins.Include(q=>q.Customers).Where(x => x.UserName == userlogin.UserName && x.Password == userlogin.Password).SingleOrDefault();
            //var userImage = _context.Customers.Where(x => x.Id == auth.CustomersId).SingleOrDefault();
            if (auth != null)
            {
                switch (auth.RoleId)
                {
                    case 1:
                        HttpContext.Session.SetInt32("AdminId",(int)auth.CustomersId);
                        HttpContext.Session.SetString("AdminName", auth.UserName);
                        HttpContext.Session.SetString("AdminImage",auth.Customers.UserImage);
                        return RedirectToAction("Admin", "DashBoard");

                    case 2:
                        HttpContext.Session.SetInt32("CustomersId", (int)auth.CustomersId);
                        HttpContext.Session.SetString("CustomersName", auth.UserName);
                        HttpContext.Session.SetString("CustomersImage", auth.Customers.UserImage);
                        ViewBag.CustomersName = HttpContext.Session.GetString("CustomersName");
                        return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "LoginAndRegestration");
        }

    }
}
