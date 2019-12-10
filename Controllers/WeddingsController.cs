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
            List<Wedding> AllWeddings = dbContext.Weddings
                .ToList();
            ViewBag.AllWeddings = AllWeddings;
            return View();
        }

    //     [HttpPost]
    //     [Route("products")]
    //     public IActionResult Create(Product product)
    //     {
    //         if (ModelState.IsValid)
    //         {
    //             dbContext.Products.Add(product);
    //             dbContext.SaveChanges();
    //             return Redirect("products");
    //         }
    //         else
    //         {
    //             return View("Index", product);
    //         }
    //     }

    //     [HttpGet("products/{ProductId}")]
    //     public IActionResult Show(int ProductId)
    //     {
    //         Product product = dbContext.Products
    //             .Include(p => p.Categories)
    //             .ThenInclude(association => association.Category)
    //             .FirstOrDefault(p => p.ProductId == ProductId);

    //         List<Category> AllCategories = dbContext.Categories
    //             .ToList();
    //         ViewBag.AllCategories = AllCategories;

    //         return View(product);
    //     }

        
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

    }


}
