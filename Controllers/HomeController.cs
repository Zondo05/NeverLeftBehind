using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NeverLeftBehind.Models;

namespace NeverLeftBehind.Controllers
{
    public class HomeController : Controller
    {
        private NeverLeftBehindContext db;

                private int? uid
        {
            get
            {
                return HttpContext.Session.GetInt32("UserId");
            }
        }

        private bool isLoggedIn
        {
            get
            {
                return uid != null;
            }
        }

        public HomeController(NeverLeftBehindContext context)
        {
            db = context;
        }

        [HttpGet("/register")]
        public IActionResult Registration()
        {
            return View("Register");
        }

        [HttpPost("/register")]
        public IActionResult Register (User newUser)
        {
            if (ModelState.IsValid)
            {
                // if Any already user exists that matches the email
                if (db.Users.Any(u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "is taken");
                }
            }

            if (ModelState.IsValid == false)
            {
                // To display the custom error message above IF it was added, OR to display the other validation errors
                return View("Register");
            }

            // hash pw
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            newUser.Password = hasher.HashPassword(newUser, newUser.Password);

            db.Users.Add(newUser);
            db.SaveChanges();

            HttpContext.Session.SetInt32("UserId", newUser.UserId);
            HttpContext.Session.SetString("UserName", newUser.FirstName);
            return RedirectToAction("Index");
        }

    [HttpPost("/login")]
        public IActionResult Login(LoginUser loginUser)
        {

            if (ModelState.IsValid == false)
            {
                // display validation errors
                return View("Index");
            }

            User dbUser = db.Users.FirstOrDefault(user => user.Email == loginUser.LoginEmail);

            if (dbUser == null)
            {
                // ModelState.AddModelError("LoginEmail", genericErrMsg);
                ModelState.AddModelError("LoginEmail", "Email not found");
                return View("Index");
            }

            // user was found b/c above return did not happen
            PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
            // right click the PasswordVerificationResult and go to definition for more info
            PasswordVerificationResult pwCompareResult = hasher.VerifyHashedPassword(loginUser, dbUser.Password, loginUser.LoginPassword);

            if (pwCompareResult == 0)
            {
                // ModelState.AddModelError("LoginEmail", genericErrMsg);
                ModelState.AddModelError("LoginEmail", "wrong password");
                return View("Index");
            }

            // no returns happened, everything is good
            HttpContext.Session.SetInt32("UserId", dbUser.UserId);
            HttpContext.Session.SetString("UserName", dbUser.FirstName);
            return RedirectToAction("Dashboard", "VetResources");
        }

        [HttpPost("/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index,Home");
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
