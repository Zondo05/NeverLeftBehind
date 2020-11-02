using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeverLeftBehind.Models;

namespace NeverLeftBehind.Controllers
{
    [Route("resource")]
    public class VetResourcesController : Controller
    {
        private int? UserSession
        {
            get { return HttpContext.Session.GetInt32("UserId"); }
            set { HttpContext.Session.SetInt32("UserId", (int)value); }
        }
        private NeverLeftBehindContext dbContext;
        public VetResourcesController(NeverLeftBehindContext context)
        {
            dbContext = context;
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            // Redirect to register/login page if no user in session
            if (UserSession == null)
                return RedirectToAction("Index", "Home");

            // Get all resources with included resources ordered by Name
            var AllResources = dbContext.Resources
                .Include(w => w.Responses)
                .OrderByDescending(w => w.ResourceName)
                .ToList();

            ViewBag.UserId = UserSession;
            return View(AllResources);
        }

        [HttpGet("{resourceId}")]
        public IActionResult Details(int resourceId)
        {
            var thisResource = dbContext.Resources
            // .Include(w => w.ResourceName)
            // .ThenInclude(r => r.Desc)
            .FirstOrDefault(w => w.ResourceId == resourceId);
            return View(thisResource);
        }

        [HttpGet("new")]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost("create")]
        public IActionResult Create(Resource newResource)
        {
            if (UserSession == null)
                    return RedirectToAction("Index", "Home");
            if (ModelState.IsValid)
            {
                // Crete new resource with UserId set to current session user's id
                newResource.UserId = (int)UserSession;
                dbContext.Resources.Add(newResource);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return View("New");
        }

        [HttpGet("delete")]
        public IActionResult Delete(int resourceId)
        {
            if (UserSession == null)
                return RedirectToAction("Index", "Home");

            Resource toDelete = dbContext.Resources.FirstOrDefault(w => w.ResourceId == resourceId);

            if (toDelete == null)
                return RedirectToAction("Dashboard");
            // Redirect to dashboard if user trying to delete isn't the resource creator
            if (toDelete.UserId != UserSession)
                return RedirectToAction("Dashboard");

            dbContext.Resources.Remove(toDelete);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

    }
}