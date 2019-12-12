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
    public class WeddingsController : Controller
    {
        private MyContext dbContext;

        // here we can "inject" our context service into the constructor
        public WeddingsController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet]
        [Route("weddings")]
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var currentUser = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (currentUser == null)
            {
                return Redirect("/register");
            }
            ViewBag.CurrentUser = currentUser;

            List<Wedding> AllWeddings = dbContext.Weddings
                .Include(w => w.Creator)
                .Include(w => w.Guests)
                .ThenInclude(a => a.User)
                .ToList();
            ViewBag.AllWeddings = AllWeddings;
            return View(AllWeddings);
        }

        [HttpGet]
        [Route("weddings/new")]
        public IActionResult New()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var currentUser = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (currentUser == null)
            {
                return Redirect("/register");
            }
            return View();
        }

        [HttpPost]
        [Route("weddings")]
        public IActionResult Create(Wedding wedding)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var currentUser = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (currentUser == null)
            {
                return Redirect("/register");
            }
            if (ModelState.IsValid)
            {
                wedding.Creator = currentUser;
                dbContext.Weddings.Add(wedding);
                dbContext.SaveChanges();
                return Redirect("weddings");
            }
            else
            {
                return View("New", wedding);
            }
        }

        [HttpGet("weddings/{weddingId}")]
        public IActionResult Show(int WeddingId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var currentUser = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (currentUser == null)
            {
                return Redirect("/register");
            }
            Wedding theWedding = dbContext.Weddings.FirstOrDefault(w => w.WeddingId == WeddingId);
            if (theWedding == null)
            {
                return NotFound();
            }
            return View(theWedding);
        }

        [HttpPost]
        [Route("weddings/{weddingId}/destroy")]
        public IActionResult Destroy(int weddingId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var currentUser = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (currentUser == null)
            {
                return Redirect("/register");
            }
            //Obtengo el wedding
            Wedding theWedding = dbContext.Weddings.FirstOrDefault(w => w.WeddingId == weddingId);
            if (theWedding == null)
            {
                return NotFound();
            }
            if (theWedding.Creator.UserId == currentUser.UserId)
            {
                dbContext.Weddings.Remove(theWedding);
                dbContext.SaveChanges();
            }
            return Redirect("/weddings");
        }

        [HttpPost]
        [Route("weddings/{weddingId}/rsvp")]
        public IActionResult RSVP(int weddingId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var currentUser = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (currentUser == null)
            {
                return Redirect("/register");
            }
            //Obtengo el wedding
            Wedding theWedding = dbContext.Weddings.FirstOrDefault(w => w.WeddingId == weddingId);
            if (theWedding == null)
            {
                return NotFound();
            }
            // Found if the user already confirm asistance
            Association existingRSVP = dbContext.Association
                .Where(a => a.User == currentUser && a.Wedding == theWedding)
                .FirstOrDefault();
            if (existingRSVP != null)
            {
                return BadRequest(); //TODO Notices.
            }

            // Crear la asociacion y guardar ese RSVP como una nueva linea en la tabla association
            Association rsvp = new Association();
            rsvp.Wedding = theWedding;
            // rsvp.WeddingId = theWedding.WeddingId;
            // rsvp.WeddingId = weddingId;

            rsvp.User = currentUser;
            dbContext.Association.Add(rsvp);
            dbContext.SaveChanges();

            return Redirect("/weddings");
        }

        [HttpPost]
        [Route("weddings/{weddingId}/unrsvp")]
        public IActionResult UnRSVP(int weddingId)
        {
            // estas en sesion?
            int? userId = HttpContext.Session.GetInt32("UserId");
            var currentUser = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (currentUser == null)
            {
                return Redirect("/register");
            }
            //Obtengo el wedding
            Wedding theWedding = dbContext.Weddings.FirstOrDefault(w => w.WeddingId == weddingId);
            if (theWedding == null)
            {
                return NotFound();
            }
            //Remover el rsvp
            //Encontrar la asociacion RSVP
            Association unrsvp = dbContext.Association
                .Where(a => a.User == currentUser && a.Wedding == theWedding)
                .FirstOrDefault();

            if (unrsvp != null) {
                dbContext.Association.Remove(unrsvp);
                dbContext.SaveChanges();
            }

            return Redirect("/weddings");
        }

        // No poder confirmar dos veces
        // trató de remover un rsvp desde la consola en en a href al link de rsvp
        // no poder borrar la boda de otro creator.
        // revisar si algo existe con ese especifico Id
        // validar que no pueda ingresar un valor diferente a una fecha en date
        // cuando trate de ir a rutas a la mala, tirarlo a una pagina de error y enlace a volver al home.
        // hash the email
        // aplication//// storage... cookies
        // No mostrar "Logout" si ya estoy deslogeado
        //Weddings expire when the scheduled date passes, and are removed from the database.







    }


}
