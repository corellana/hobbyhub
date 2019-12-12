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
        private MyContext dbContext;

        // here we can "inject" our context service into the constructor
        public UsersController(MyContext context)
        {
            dbContext = context;
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
                return Redirect("/weddings");
            }
            return View();
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(User theUser)
        {
            // TODO revisar por qué no está funcionando.
            int? userId = HttpContext.Session.GetInt32("UserId");
            var currentUser = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (currentUser == null)
            {
                return Redirect("/weddings");
            }

            if (ModelState.IsValid)
            {
                if (dbContext.Users.Any(user => user.Email == theUser.Email))
                {
                    // Manually add a ModelState error to the Email field, with provided
                    // error message
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("register", theUser);
                }
                else
                {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    theUser.Password = Hasher.HashPassword(theUser, theUser.Password);
                    dbContext.Users.Add(theUser);
                    dbContext.SaveChanges();
                    HttpContext.Session.SetInt32("UserId", theUser.UserId);
                    return Redirect("/weddings");
                }
            }
            else 
            {
                return View("register", theUser);
            }
        }


        [HttpGet]
        [Route("success")]
        public IActionResult Success()
        {
            // TODO deslogeada me tira a un error.
            int? userId = HttpContext.Session.GetInt32("UserId");
            var currentUser = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            ViewBag.CurrentUser = currentUser;

            return View();
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            // List<User> AllUsers = dbContext.Users.ToList();

            return View();
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginUser userSubmission)
        {
            if (ModelState.IsValid)
            {
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

                return Redirect("/weddings");
            }
            ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Login");
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/register");
        }

        // [HttpGet]
        // [Route("dishes/{id}")]
        // public IActionResult Show(int id)
        // {
        //     Dish theDish = dbContext.Dishes.FirstOrDefault(dish => dish.id == id);

        //     if (theDish == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(theDish);
        // }

        // [HttpGet]
        // [Route("users/{id}/edit")]
        // public IActionResult Edit(int id)
        // {
        //     User theUser = dbContext.Users.FirstOrDefault(user => user.id == id);

        //     if (theUser == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(theUser);
        // }

        
        // [HttpPost]
        // [Route("users/edit/{id}")]
        // public IActionResult Update(int id, User theUser)
        // {
        //     User theDish = dbContext.Users.FirstOrDefault(user => user.id == id);
        //     if (ModelState.IsValid)
        //     {
        //         theUser.id = id;
        //         dbContext.Update(theUser);
                
                
        //         theDish.UpdatedAt = DateTime.Now;
        //         dbContext.SaveChanges();
        //         return Redirect($"/dishes/{dish.id}");
        //     }
        //     else
        //     {
        //         return View("Edit");
        //     }
        // }


        // // El form está en el Index
        // [HttpDelete]
        // [Route("dishes/{id}")]
        // public IActionResult Destroy(int id)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         return View("", dish);
        //     }
        //     else
        //     {
        //         return View("Index");
        //     }
        // }
    }
}






