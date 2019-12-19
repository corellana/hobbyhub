using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Project.Controllers
{
    public class HobbiesController : Controller
    {
        private MyContext dbContext;

        public HobbiesController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet]
        [Route("hobbies")]
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var currentUser = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (currentUser == null)
            {
                return Redirect("/login");
            }
            ViewBag.CurrentUser = currentUser;

            List<Hobby> AllHobbies = dbContext.Hobbies 
                .OrderBy(i => i.Description)
                .Include(i => i.Creator)
                .Include(i => i.Liking)
                .ThenInclude(i => i.User)
                .OrderByDescending(i => i.Liking.Count())
                .ToList();
            ViewBag.AllHobbies = AllHobbies;
            
            // Lo que sea que le pase a la vista, se convierte en el model
            return View();
        }

        [HttpGet]
        [Route("hobbies/new")]
        public IActionResult New()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var currentUser = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (currentUser == null)
            {
                return Redirect("/login");
            }
            return View();
        }

        [HttpPost]
        [Route("hobbies")]
        public IActionResult Create(Hobby hobby)
        {
            // Login User
            int? userId = HttpContext.Session.GetInt32("UserId");
            var currentUser = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (currentUser == null)
            {
                return Redirect("/login");
            }

            // No duplicates Hobbies
            if (dbContext.Hobbies.Any(h => h.Name == hobby.Name))
            {
                ModelState.AddModelError("Name", "Hobby Name already in use!");
                return View("edit", hobby);
            }

            if (ModelState.IsValid)
            {
                hobby.Creator = currentUser;
                dbContext.Hobbies.Add(hobby);
                dbContext.SaveChanges();
                return Redirect("hobbies");
            }
            else
            {
                return View("New", hobby);
            }
        }

        [HttpGet("hobbies/{hobbyId}")]
        public IActionResult Show(int HobbyId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var currentUser = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (currentUser == null)
            {
                return Redirect("/login");
            }
            ViewBag.CurrentUser = currentUser;

            Hobby theHobby = dbContext.Hobbies
                .Include(u => u.Creator)
                .Include(u => u.Liking)
                .ThenInclude(u => u.User)
                .FirstOrDefault(w => w.HobbyId == HobbyId);

            if (theHobby == null)
            {
                return NotFound();
            }
            return View(theHobby);
        }

        [HttpGet]
        [Route("hobbies/{hobbyId}/edit")]
        public IActionResult Edit(int hobbyId)
        {
            Hobby theHobby = dbContext.Hobbies.FirstOrDefault(h => h.HobbyId == hobbyId);

            if (theHobby == null)
            {
                return NotFound();
            }

            return View(theHobby);
        }


        [HttpPost]
        [Route("hobbies/{hobbyId}")]
        public IActionResult Update(int hobbyId, Hobby formHobby)
        {

            int? userId = HttpContext.Session.GetInt32("UserId");
            var currentUser = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (currentUser == null)
            {
                return Redirect("/login");
            }

            Hobby theHobby = dbContext.Hobbies.FirstOrDefault(h => h.HobbyId == hobbyId);
            if (ModelState.IsValid)
            {
                theHobby.Name = formHobby.Name;
                theHobby.Description = formHobby.Description;
                theHobby.UpdatedAt = DateTime.Now;
                dbContext.SaveChanges();
                return Redirect($"/hobbies");
            }
            else
            {
                return View("Edit", formHobby);
            }
        }

        [HttpPost]
        [Route("hobbies/{hobbyId}/destroy")]
        public IActionResult Destroy(int hobbyId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var currentUser = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (currentUser == null)
            {
                return Redirect("/login");
            }
            Hobby theHobby = dbContext.Hobbies.FirstOrDefault(w => w.HobbyId == hobbyId);
            if (theHobby == null)
            {
                return NotFound();
            }
            if (theHobby.Creator.UserId == currentUser.UserId)
            {
                dbContext.Hobbies.Remove(theHobby);
                dbContext.SaveChanges();
            }
            return Redirect("/hobbies");
        }

        [HttpPost]
        [Route("hobbies/{hobbyId}/rsvp")]
        public IActionResult RSVP(int hobbyId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var currentUser = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (currentUser == null)
            {
                return Redirect("/login");
            }
            //Obtengo el hobby
            Hobby theHobby = dbContext.Hobbies.FirstOrDefault(w => w.HobbyId == hobbyId);
            if (theHobby == null)
            {
                return NotFound();
            }
            // Found if the user already did like this hobby
            Association existingRSVP = dbContext.Associations
                .Where(a => a.User == currentUser && a.Hobby == theHobby)
                .FirstOrDefault();
            if (existingRSVP != null)
            {
                return Redirect("/hobbies"); 
            }
            // Search all User Association, including the hobby and compare
            List<Association> allRSVP = dbContext.Associations
                .Where(a => a.User == currentUser)
                .Include(a => a.Hobby)
                .ToList();
        

            // Create assoctiation and save that like as a new line in the association table.
            Association rsvp = new Association();
            rsvp.Hobby = theHobby;
            
            rsvp.User = currentUser;
            dbContext.Associations.Add(rsvp);
            dbContext.SaveChanges();

            return Redirect("/hobbies");
            
        }

        [HttpPost]
        [Route("hobbies/{hobbyId}/unrsvp")]
        public IActionResult UnRSVP(int hobbyId)
        {
            // estas en sesion?
            int? userId = HttpContext.Session.GetInt32("UserId");
            var currentUser = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (currentUser == null)
            {
                return Redirect("/login");
            }
            //Obtengo el hobby
            Hobby theHobby = dbContext.Hobbies.FirstOrDefault(w => w.HobbyId == hobbyId);
            if (theHobby == null)
            {
                return NotFound();
            }
            //Remove Like
            //Encontrar la asociacion LIKE entre user y Hobbie
            Association unrsvp = dbContext.Associations
                .Where(a => a.User == currentUser && a.Hobby == theHobby)
                .FirstOrDefault();

            if (unrsvp != null) {
                dbContext.Associations.Remove(unrsvp);
                dbContext.SaveChanges();
            }

            return Redirect("/hobbies");
        }

    }


}
