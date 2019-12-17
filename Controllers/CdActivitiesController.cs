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
    public class CdActivitiesController : Controller
    {
        private MyContext dbContext;

        //here we can "inject" our context service into the constructor
        public CdActivitiesController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet]
        [Route("cdactivities")]
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var currentUser = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (currentUser == null)
            {
                return Redirect("/login");
            }
            ViewBag.CurrentUser = currentUser; // REPASAR

            List<CdActivity> AllCdActivities = dbContext.CdActivities 
                .OrderBy(a => a.Date)
                .Where(a => a.Date > DateTime.Now)
                .Include(w => w.Creator) // REPASAR
                .Include(w => w.Guests)
                .ThenInclude(a => a.User)
                .ToList();
            ViewBag.AllCdActivities = AllCdActivities;
            
            return View(AllCdActivities);
        }

        [HttpGet]
        [Route("cdactivities/new")]
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
        [Route("cdactivities")]
        public IActionResult Create(CdActivity cdactivity)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var currentUser = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (currentUser == null)
            {
                return Redirect("/login");
            }
            if (ModelState.IsValid)
            {
                cdactivity.Creator = currentUser;
                dbContext.CdActivities.Add(cdactivity);
                dbContext.SaveChanges();
                return Redirect("cdactivities");
            }
            else
            {
                return View("New", cdactivity);
            }
        }

        [HttpGet("cdactivities/{cdactivityId}")]
        public IActionResult Show(int CdactivityId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var currentUser = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (currentUser == null)
            {
                return Redirect("/login");
            }
            ViewBag.CurrentUser = currentUser;

            CdActivity theCdActivity = dbContext.CdActivities
                .Include(u => u.Creator)
                .Include(u => u.Guests)
                .ThenInclude(u => u.User)
                .FirstOrDefault(w => w.CdActivityId == CdactivityId);

            if (theCdActivity == null)
            {
                return NotFound();
            }
            return View(theCdActivity);
        }

        [HttpPost]
        [Route("cdactivities/{cdactivityId}/destroy")]
        public IActionResult Destroy(int cdactivityId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var currentUser = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (currentUser == null)
            {
                return Redirect("/login");
            }
            //Obtengo el wedding
            CdActivity theCdActivity = dbContext.CdActivities.FirstOrDefault(w => w.CdActivityId == cdactivityId);
            if (theCdActivity == null)
            {
                return NotFound();
            }
            if (theCdActivity.Creator.UserId == currentUser.UserId)
            {
                dbContext.CdActivities.Remove(theCdActivity);
                dbContext.SaveChanges();
            }
            return Redirect("/cdactivities");
        }

        [HttpPost]
        [Route("cdactivities/{cdactivityId}/rsvp")]
        public IActionResult RSVP(int cdactivityId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var currentUser = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (currentUser == null)
            {
                return Redirect("/login");
            }
            //Obtengo el wedding
            CdActivity theCdActivity = dbContext.CdActivities.FirstOrDefault(w => w.CdActivityId == cdactivityId);
            if (theCdActivity == null)
            {
                return NotFound();
            }
            // Found if the user already confirm asistance
            Association existingRSVP = dbContext.Associations
                .Where(a => a.User == currentUser && a.CdActivity == theCdActivity)
                .FirstOrDefault();
            if (existingRSVP != null)
            {
                return BadRequest(); //TODO Notices. REPASAR
            }
            // Search all User Association, including the activity and compare
            List<Association> allRSVP = dbContext.Associations
                .Where(a => a.User == currentUser)
                .Include(a => a.CdActivity)
                .ToList();
                foreach(Association anrsvp in allRSVP)
                {
                    if(anrsvp.CdActivity.Overlaps(theCdActivity) )
                    {
                        return Redirect("/cdactivities");
                    }
                }


            // Crear la asociacion y guardar ese RSVP como una nueva linea en la tabla association
            Association rsvp = new Association();
            rsvp.CdActivity = theCdActivity;
            // rsvp.WeddingId = theWedding.WeddingId;
            // rsvp.WeddingId = weddingId;

            rsvp.User = currentUser;
            dbContext.Associations.Add(rsvp);
            dbContext.SaveChanges();

            return Redirect("/cdactivities");
        }

        [HttpPost]
        [Route("cdactivities/{cdactivityId}/unrsvp")]
        public IActionResult UnRSVP(int cdactivityId)
        {
            // estas en sesion?
            int? userId = HttpContext.Session.GetInt32("UserId");
            var currentUser = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (currentUser == null)
            {
                return Redirect("/login");
            }
            //Obtengo el wedding
            CdActivity theCdActivity = dbContext.CdActivities.FirstOrDefault(w => w.CdActivityId == cdactivityId);
            if (theCdActivity == null)
            {
                return NotFound();
            }
            //Remover el rsvp
            //Encontrar la asociacion RSVP
            Association unrsvp = dbContext.Associations
                .Where(a => a.User == currentUser && a.CdActivity == theCdActivity)
                .FirstOrDefault();

            if (unrsvp != null) {
                dbContext.Associations.Remove(unrsvp);
                dbContext.SaveChanges();
            }

            return Redirect("/cdactivities");
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
        // Weddings expire when the scheduled date passes, and are removed from the database.

    }


}
