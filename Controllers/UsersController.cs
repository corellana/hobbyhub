using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;


namespace Project.Controllers
{
    public class UsersController : Controller
    {
        private static string SUCCESS_URL = "/weddings";

        private MyContext dbContext;

        // here we can "inject" our context service into the constructor
        public UsersController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var currentUser = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (currentUser != null)
            {
                return Redirect(SUCCESS_URL);
            }

            // Cambiar returns para separar logins
            // return Redirect("/login");
            return View();
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            // List<User> AllUsers = dbContext.Users.ToList();
            // TODO revisar por qué no está funcionando.
            int? userId = HttpContext.Session.GetInt32("UserId");
            var currentUser = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (currentUser != null)
            {
                return Redirect(SUCCESS_URL);
            }
            return View();
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(User theUser)
        {
            if (!ModelState.IsValid)
            {
                // The model is invalid render the register form again
                return View("register", theUser);
            }

            if (dbContext.Users.Any(user => user.Email == theUser.Email))
            {
                // Manually add a ModelState error to the Email field, with provided
                // error message
                ModelState.AddModelError("Email", "Email already in use!");
                return View("register", theUser);
            }

            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            theUser.Password = Hasher.HashPassword(theUser, theUser.Password);
            theUser.CreatedAt = theUser.UpdatedAt = DateTime.Now;
            
            dbContext.Users.Add(theUser);
            dbContext.SaveChanges();

            HttpContext.Session.SetInt32("UserId", theUser.UserId);
            return Redirect(SUCCESS_URL);
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginUser userSubmission)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Email", "Invalid Email/Password");
                return View("Login");
            }

            // If inital ModelState is valid, query for a user with provided email
            var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == userSubmission.Email);
            // If no user exists with provided email
            if (userInDb == null)
            {
                // Add an error to ModelState and return to View!
                ModelState.AddModelError("Email", "Invalid Email/Password");
                return View("Login");
            }
            // Initialize hasher object
            var hasher = new PasswordHasher<LoginUser>();

            // verify provided password against hash stored in db
            var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.Password);

            // result can be compared to 0 for failure
            if (result == 0)
            {
                // handle failure (this should be similar to how "existing email" is handled)
                ModelState.AddModelError("Email", "Invalid Email/Password");
                return View("Login");
            }

            HttpContext.Session.SetInt32("UserId", userInDb.UserId);
            return Redirect(SUCCESS_URL);
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }
    }
}






