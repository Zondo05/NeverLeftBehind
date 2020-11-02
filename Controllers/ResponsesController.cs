
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NeverLeftBehind.Models;

namespace NeverLeftBehind.Controllers
{
    [Route("response")]
    public class ResponsesController : Controller
    {
        private int? UserSession
        {
            get { return HttpContext.Session.GetInt32("UserId"); }
            set { HttpContext.Session.SetInt32("UserId", (int)value); }
        }
        private NeverLeftBehindContext dbContext;
        public ResponsesController(NeverLeftBehindContext context)
        {
            dbContext = context;
        }
        [HttpGet("join/{resourceId}")]
        public IActionResult Join(int resourceId)
        {
            if (UserSession == null)
                return RedirectToAction("Index", "Home");

            // Create a new response with the given weddingId and current userId
            Response newResponse = new Response()
            {
                ResourceId = resourceId,
                UserId = (int)UserSession
            };
            dbContext.Responses.Add(newResponse);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard", "VetResources");
        }

        [HttpGet("leave/{resourceId}")]
        public IActionResult Leave(int resourceId)
        {
            if (UserSession == null)
                return RedirectToAction("Index", "Home");

            // Query to grab the appropriate response to remove
            Response toDelete = dbContext.Responses.FirstOrDefault(r => r.ResourceId == resourceId && r.UserId == UserSession);

            // Redirect to dashboard if no match for response in db
            if (toDelete == null)
                return RedirectToAction("Dashboard", "VetResources");

            dbContext.Responses.Remove(toDelete);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard", "VetResources");
        }
    }
}