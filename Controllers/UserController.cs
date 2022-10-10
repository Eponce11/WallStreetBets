using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WallStreetBets.Models;

namespace WallStreetBets.Controllers
{
    public class UserController : Controller
    {
        public MyContext _db;
        public UserController(MyContext db) { _db = db; }

        private int? uid
        { 
            get
            { 
                return HttpContext.Session.GetInt32("UserId");
            } 
        }
        private bool isLoggedIn { get{return uid != null;} }





        [HttpPost("/login")]
        public IActionResult Login(LoginUser loginUser)
        {
            if (!ModelState.IsValid) { return View("/Views/Home/Index.cshtml"); }

            User dbUser = _db.Users.FirstOrDefault(user => user.Email == loginUser.LoginEmail);

            if (dbUser == null) 
            {
                ModelState.AddModelError("LoginEmail", "Invalid Credentials");
                return View("/Views/Home/Index.cshtml");
            }

            PasswordHasher<LoginUser> passwordHasher = new PasswordHasher<LoginUser>();
            PasswordVerificationResult isPasswordMatch = passwordHasher.VerifyHashedPassword(loginUser, dbUser.Password, loginUser.LoginPassword);

            if (isPasswordMatch == 0)
            {
                ModelState.AddModelError("LoginEmail", "Invalid Credentials");
                return View("/Views/Home/Index.cshtml");
            }

            HttpContext.Session.SetInt32("UserId", dbUser.UserId);
            return RedirectToAction("Dashboard", "Home");
        }


        [HttpPost("/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            return RedirectToAction ("Index", "Home");
        }


        [HttpGet("/register")]
        public IActionResult Register()
        {
            if (isLoggedIn) { return RedirectToAction("Index", "Home"); }
            return View("Register");
        }


        [HttpPost("/create/user")]
        public IActionResult CreateUser(User newUser)
        {
            if (!ModelState.IsValid) { return View("Register"); }

            PasswordHasher<User> hash = new PasswordHasher<User>();
            newUser.Password = hash.HashPassword(newUser, newUser.Password);

            _db.Users.Add(newUser);
            _db.SaveChanges();
            
            HttpContext.Session.SetInt32("UserId", newUser.UserId);

            return RedirectToAction("Dashboard", "Home");
        }




    }
}