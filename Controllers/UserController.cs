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

        // checks if the user is signed in by checking the value in session
        private bool isLoggedIn { get{return uid != null;} }





        [HttpPost("/login")]
        public IActionResult Login(LoginUser loginUser)
        {
            // returns back if the values passed in do not meet the model requirements
            if (!ModelState.IsValid) { return View("/Views/Home/Index.cshtml"); }

            // find the user in the db with the same email
            User dbUser = _db.Users.FirstOrDefault(user => user.Email == loginUser.LoginEmail);

            // if the user isnt found return back to login page
            if (dbUser == null) 
            {
                ModelState.AddModelError("LoginEmail", "Invalid Credentials");
                return View("/Views/Home/Index.cshtml");
            }

            // instantiate password hasher and compare the password passed in and the password in the db
            PasswordHasher<LoginUser> passwordHasher = new PasswordHasher<LoginUser>();
            PasswordVerificationResult isPasswordMatch = passwordHasher.VerifyHashedPassword(loginUser, dbUser.Password, loginUser.LoginPassword);

            // return back to login if the passwords dont match
            if (isPasswordMatch == 0)
            {
                ModelState.AddModelError("LoginEmail", "Invalid Credentials");
                return View("/Views/Home/Index.cshtml");
            }

            // store the users unique id into session
            HttpContext.Session.SetInt32("UserId", dbUser.UserId);
            return RedirectToAction("Dashboard", "Home");
        }


        [HttpPost("/logout")]
        public IActionResult Logout()
        {
            // clear session and return to login page
            HttpContext.Session.Remove("UserId");
            return RedirectToAction ("Index", "Home");
        }


        [HttpGet("/register")]
        public IActionResult Register()
        {
            // if an id is in session login in the user
            if (isLoggedIn) { return RedirectToAction("Index", "Home"); }
            return View("Register");
        }


        [HttpPost("/create/user")]
        public IActionResult CreateUser(User newUser)
        {
            // checks if the passed in info meets the model requirements
            if (!ModelState.IsValid) { return View("Register"); }

            // replace the password with a hashed version
            PasswordHasher<User> hash = new PasswordHasher<User>();
            newUser.Password = hash.HashPassword(newUser, newUser.Password);

            // add the usser and save the changes to db
            _db.Users.Add(newUser);
            _db.SaveChanges();
            
            // add the users id to session
            HttpContext.Session.SetInt32("UserId", newUser.UserId);

            return RedirectToAction("Dashboard", "Home");
        }




    }
}