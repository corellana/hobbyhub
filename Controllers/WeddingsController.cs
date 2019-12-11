using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Microsoft.EntityFrameworkCore;


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
            List<Wedding> AllWeddings = dbContext.Weddings.ToList();
            ViewBag.AllWeddings = AllWeddings;
            return View(AllWeddings);
        }

        [HttpGet]
        [Route("weddings/new")]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        [Route("weddings")]
        public IActionResult Create(Wedding wedding)
        {
            if (ModelState.IsValid)
            {
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
            Wedding theWedding = dbContext.Weddings.FirstOrDefault(w => w.weddingId == WeddingId);
            if (theWedding == null)
            {
                return NotFound();
            }
            return View(theWedding);
        }

        //     [HttpPost("products/{ProductId}/categories")]
        //     public IActionResult AddCategory(int ProductId, int CategoryId)
        //     {
        //         Product product = dbContext.Products
        //             .FirstOrDefault(p => p.ProductId == ProductId);

        //         Category category = dbContext.Categories
        //             .FirstOrDefault(c => c.CategoryId == CategoryId);

        //         Association association = new Association();
        //         association.Product = product;
        //         association.Category = category;
        //         dbContext.Association.Add(association);
        //         dbContext.SaveChanges();


        //         return Redirect($"/products/{product.ProductId}");
        //     }

        // [HttpGet]
        // [Route("dishes/{id}/edit")]
        // public IActionResult Edit(int id)
        // {
        //     Dish theDish = dbContext.Dishes.FirstOrDefault(dish => dish.id == id);

        //     if (theDish == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(theDish);
        // }

        // [HttpPost]
        // [Route("dishes/{id}")]
        // public IActionResult Update(int id, Dish formDish)
        // {
        //     Dish theDish = dbContext.Dishes.FirstOrDefault(dish => dish.id == id);
        //     if (ModelState.IsValid)
        //     {
        //         theDish.Name = formDish.Name;
        //         theDish.Chef = formDish.Chef;
        //         theDish.Calories = formDish.Calories;
        //         theDish.Tastiness = formDish.Tastiness;
        //         theDish.Description = formDish.Description;
        //         theDish.UpdatedAt = DateTime.Now;
        //         dbContext.SaveChanges();
        //         return Redirect($"/dishes/{formDish.id}");
        //     }
        //     else
        //     {
        //         return View("Edit");
        //     }


        [HttpDelete]
        [Route("weddings/{weddingId}")]
        public IActionResult Destroy(int id)
        {
            if (ModelState.IsValid)
            {
                return View("");
            }
            else
            {
                return View("Index");
            }
        }








    }


}
