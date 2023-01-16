using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hall_Boking_System.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Hall_Boking_System.Controllers
{
    public class HomesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment webHostEnviroment;
        public HomesController(ModelContext context , IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            this.webHostEnviroment = webHostEnviroment;
        }

        // GET: Homes
        public async Task<IActionResult> Index()
        {
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(await _context.Homes.ToListAsync());
        }


            // GET: Homes/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var home = await _context.Homes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (home == null)
            {
                return NotFound();
            }
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(home);
        }

        // GET: Homes/Create
        public IActionResult Create()
        {

            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View();
        }

        // POST: Homes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,HomeImage1,HomeImage2,HomeImage3")] Home home)
        {
            if (ModelState.IsValid)
            {
                _context.Add(home);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(home);
        }

        // GET: Homes/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var home = await _context.Homes.FindAsync(id);
            if (home == null)
            {
                return NotFound();
            }
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(home);
        }

        // POST: Homes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,HomeImage1,HomeImage2,HomeImage3,ImageFile1,ImageFile2,ImageFile3")] Home home)
        {
            if (id != home.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (home.ImageFile1 != null)
                    {
                        string wwwRootPath = webHostEnviroment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + home.ImageFile1.FileName;
                        string path = Path.Combine(wwwRootPath + "/Images/" + fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await home.ImageFile1.CopyToAsync(fileStream);
                        }
                        home.HomeImage1 = fileName;
                        home.HomeImage2 = _context.Homes.Select(u => u.HomeImage2).FirstOrDefault();
                        home.HomeImage3 = _context.Homes.Select(u => u.HomeImage3).FirstOrDefault();
                        _context.Update(home);
                        await _context.SaveChangesAsync();

                    }
                    if (home.ImageFile2 != null)
                    {                        
                        string wwwRootPath = webHostEnviroment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + home.ImageFile2.FileName;
                        string path = Path.Combine(wwwRootPath + "/Images/" + fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await home.ImageFile2.CopyToAsync(fileStream);
                        }
                        home.HomeImage2 = fileName;
                        home.HomeImage1 = _context.Homes.Select(u => u.HomeImage1).FirstOrDefault();
                        home.HomeImage3 = _context.Homes.Select(u => u.HomeImage3).FirstOrDefault();
                        _context.Update(home);
                        await _context.SaveChangesAsync();

                    }
                    if (home.ImageFile3 != null)
                    {
                        string wwwRootPath = webHostEnviroment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + home.ImageFile3.FileName;
                        string path = Path.Combine(wwwRootPath + "/Images/" + fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await home.ImageFile3.CopyToAsync(fileStream);
                        }
                        home.HomeImage3 = fileName;
                        home.HomeImage2 = _context.Homes.Select(u => u.HomeImage2).FirstOrDefault();
                        home.HomeImage1 = _context.Homes.Select(u => u.HomeImage1).FirstOrDefault();
                        _context.Update(home);
                        await _context.SaveChangesAsync();
                    }

                    if (home.ImageFile3 == null && home.ImageFile2 == null && home.ImageFile1 == null)
                    {
                        home.HomeImage1 = _context.Homes.Select(u => u.HomeImage1).FirstOrDefault();
                        home.HomeImage2 = _context.Homes.Select(u => u.HomeImage2).FirstOrDefault();
                        home.HomeImage3 = _context.Homes.Select(u => u.HomeImage3).FirstOrDefault();
                    }
                    _context.Update(home);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomeExists(home.Id))
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
            return View(home);
        }

        // GET: Homes/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var home = await _context.Homes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (home == null)
            {
                return NotFound();
            }
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return View(home);
        }

        // POST: Homes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var home = await _context.Homes.FindAsync(id);
            _context.Homes.Remove(home);
            await _context.SaveChangesAsync();
            ViewBag.AdminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminImage = HttpContext.Session.GetString("AdminImage");
            return RedirectToAction(nameof(Index));
        }

        private bool HomeExists(decimal id)
        {
            return _context.Homes.Any(e => e.Id == id);
        }
    }
}
