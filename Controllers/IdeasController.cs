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
    public class IdeasController : Controller
    {
        private MyContext dbContext;

        public IdeasController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet]
        [Route("ideas")]
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var currentUser = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (currentUser == null)
            {
                return Redirect("/login");
            }
            ViewBag.CurrentUser = currentUser;
            List<Idea> AllIdeas = dbContext.Ideas 
                .OrderBy(a => a.Detail)
                .Include(w => w.Creator)
                .Include(w => w.Liking)
                .ThenInclude(a => a.User)
                .ToList();
            ViewBag.AllIdeas = AllIdeas;
            
            // Lo que sea que le pase a la vista, se convierte en el model
            return View();
        }

        [HttpGet]
        [Route("ideas/new")]
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
        [Route("ideas")]
        public IActionResult Create(Idea idea)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var currentUser = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (currentUser == null)
            {
                return Redirect("/login");
            }
            if (ModelState.IsValid)
            {
                idea.Creator = currentUser;
                dbContext.Ideas.Add(idea);
                dbContext.SaveChanges();
                return Redirect("ideas");
            }
            else
            {
                return View("New", idea);
            }
        }

        [HttpGet("ideas/{ideaId}")]
        public IActionResult Show(int IdeaId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var currentUser = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (currentUser == null)
            {
                return Redirect("/login");
            }
            ViewBag.CurrentUser = currentUser;

            Idea theIdea = dbContext.Ideas
                .Include(u => u.Creator)
                .Include(u => u.Liking)
                .ThenInclude(u => u.User)
                .FirstOrDefault(w => w.IdeaId == IdeaId);

            if (theIdea == null)
            {
                return NotFound();
            }
            return View(theIdea);
        }

        [HttpPost]
        [Route("ideas/{ideaId}/destroy")]
        public IActionResult Destroy(int ideaId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var currentUser = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (currentUser == null)
            {
                return Redirect("/login");
            }
            Idea theIdea = dbContext.Ideas.FirstOrDefault(w => w.IdeaId == ideaId);
            if (theIdea == null)
            {
                return NotFound();
            }
            if (theIdea.Creator.UserId == currentUser.UserId)
            {
                dbContext.Ideas.Remove(theIdea);
                dbContext.SaveChanges();
            }
            return Redirect("/ideas");
        }

        [HttpPost]
        [Route("ideas/{ideaId}/rsvp")]
        public IActionResult RSVP(int ideaId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var currentUser = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (currentUser == null)
            {
                return Redirect("/login");
            }
            //Obtengo la idea
            Idea theIdea = dbContext.Ideas.FirstOrDefault(w => w.IdeaId == ideaId);
            if (theIdea == null)
            {
                return NotFound();
            }
            // Found if the user already did like to the idea
            Association existingRSVP = dbContext.Associations
                .Where(a => a.User == currentUser && a.Idea == theIdea)
                .FirstOrDefault();
            if (existingRSVP != null)
            {
                return BadRequest(); 
            }
            // Search all User Association, including the idea and compare
            List<Association> allRSVP = dbContext.Associations
                .Where(a => a.User == currentUser)
                .Include(a => a.Idea)
                .ToList();
                // foreach(Association anrsvp in allRSVP)
                // {
                //     if(anrsvp.Idea.Overlaps(theIdea) )
                //     {
                //         return Redirect("/cdactivities");
                //     }
                // }


            // Create assoctiation and save that like as a new line in the association table.
            Association rsvp = new Association();
            rsvp.Idea = theIdea;
            
            rsvp.User = currentUser;
            dbContext.Associations.Add(rsvp);
            dbContext.SaveChanges();

            return Redirect("/ideas");
        }

        [HttpPost]
        [Route("ideas/{ideaId}/unrsvp")]
        public IActionResult UnRSVP(int ideaId)
        {
            // estas en sesion?
            int? userId = HttpContext.Session.GetInt32("UserId");
            var currentUser = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (currentUser == null)
            {
                return Redirect("/login");
            }
            //Obtengo la idea
            Idea theIdea = dbContext.Ideas.FirstOrDefault(w => w.IdeaId == ideaId);
            if (theIdea == null)
            {
                return NotFound();
            }
            //Remove Like
            //Encontrar la asociacion LIKE
            Association unrsvp = dbContext.Associations
                .Where(a => a.User == currentUser && a.Idea == theIdea)
                .FirstOrDefault();

            if (unrsvp != null) {
                dbContext.Associations.Remove(unrsvp);
                dbContext.SaveChanges();
            }

            return Redirect("/ideas");
        }

    }


}
