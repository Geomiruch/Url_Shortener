using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using URLShortener.Data;
using URLShortener.Models;

namespace URLShortener.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext context;
        private UserManager<IdentityUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var urls = await context.URLs.ToListAsync();
            return View(urls);
        }
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(URLModel url)
        {
            var user = await _userManager.GetUserAsync(User);

            url.Date = DateTime.Now;
            url.Owner = user.Email;
            bool exist = true;
            
            while (exist)
            {
                url.URL = url.RandomString(5);
                if (await context.URLs.FirstOrDefaultAsync(p => p.URL == url.URL) == null)
                    exist = false;
            }

            if (await context.URLs.FirstOrDefaultAsync(p => p.OriginalURL == url.OriginalURL) != null)
            {
                return View("This URL already exist!");
            }

            /*TryValidateModel(url);
            if (ModelState.IsValid)
            {*/
                context.URLs.Add(url);
                await context.SaveChangesAsync();

                return RedirectToAction(actionName: nameof(List), routeValues: new { id = url.Id });
            //}

            //return View(url);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                URLModel file = await context.URLs.FirstOrDefaultAsync(p => p.Id == id);
                if (file != null)
                    return View(file);
            }
            return NotFound();
        }
        [Authorize]
        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                var user = await _userManager.GetUserAsync(User);
                URLModel url = await context.URLs.FirstOrDefaultAsync(p => p.Id == id);
                if (url.Owner == user.Email || User.IsInRole("admin"))
                {
                    if (url != null)
                        return View(url);
                }
                return RedirectToAction("OwnerError");
            }
            return NotFound();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                URLModel url = await context.URLs.FirstOrDefaultAsync(p => p.Id == id);

                context.URLs.Remove(url);
                await context.SaveChangesAsync();
                return RedirectToAction("List");

            }
            return NotFound();
        }

        public IActionResult OwnerError()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("/Home/RedirectTo/{path:required}", Name = "ShortUrls_RedirectTo")]
        public async Task<IActionResult> RedirectTo(string path)
        {
            if (path == null)
            {
                return NotFound();
            }

            URLModel shortUrl = await context.URLs.FirstOrDefaultAsync(p => p.URL == path);
            if (shortUrl == null)
            {
                return NotFound();
            }

            return Redirect(shortUrl.OriginalURL);
        }
    }
}
