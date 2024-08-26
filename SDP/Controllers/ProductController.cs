using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SDP.Data;
using SDP.Models;
using SDP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace SDP.Controllers
{
    public class ProductController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork unitOfWork;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment;

        public ProductController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.hostingEnvironment = hostingEnvironment;
        }

        [Authorize(Roles ="Admin")]
        public IActionResult AddProduct()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AddProduct(ProductViewModel model)
        {
            HttpContext.Session.Remove("ProductName");
            ViewBag.PName = null;
            if (ModelState.IsValid) {
                string uniqueFileName = null;
                if (model.Photo != null){
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "img");
                    Console.WriteLine("The upload folder is {0}", uploadsFolder);
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string FilePath = Path.Combine(uploadsFolder, uniqueFileName);
                    model.Photo.CopyTo(new FileStream(FilePath, FileMode.Create));
                }

                product newProduct = new product {

                    Name = model.Name,
                    Category = model.Category,
                    originalPrice = model.originalPrice,
                    MRP = model.MRP,
                    Quantity = model.Quantity,
                    Photopath = uniqueFileName
                };
                
                unitOfWork.ProductRepository.Add(newProduct);
                unitOfWork.Save();
                HttpContext.Session.SetString("ProductName", model.Name);
                ViewBag.PName = model.Name;
                return RedirectToAction("ViewProducts", "Product");
            }
           
            return View();
        }
        static string photoPath = "";
        [HttpGet]
        public async Task<IActionResult>  UpdateProduct(int ?id)
        {
            product pd = unitOfWork.ProductRepository.Get(u=>u.productId == id);
            photoPath = ""+pd.Photopath;
            if (pd == null)
            {
                return NotFound();
            }

            return View(pd);
        }
        [HttpPost]

        public async Task<IActionResult> UpdateProduct(product pd, IFormFile? productImage)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (productImage != null) {
                        
                        string uniqueFileName = null;
                        string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "img");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + productImage.FileName;
                        string FilePath = Path.Combine(uploadsFolder, uniqueFileName);
                        productImage.CopyTo(new FileStream(FilePath, FileMode.Create));
                        pd.Photopath = uniqueFileName;
                    }
                    else {
                        pd.Photopath = photoPath;
                    }
                    unitOfWork.ProductRepository.Update(pd);
                    unitOfWork.Save();
                }
                catch (Exception)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(SearchProduct));
            }
            return View(pd);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProduct(int ?id)
        {
            if (id == null)
            {
                return NotFound();
            }
            product pd = await _context.products.FindAsync(id);
            if (pd == null)
            {
                return NotFound();
            }

            return View(pd);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var pd = await _context.products.FindAsync(id);
            _context.products.Remove(pd);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(SearchProduct));
        }
        
        public IActionResult ViewProducts()
        {
            ViewBag.Email = null;
            ViewBag.Email = (HttpContext.Session.GetString("Email"));
            var productsList = _context.products.ToList();
            ViewBag.ProductName = (HttpContext.Session.GetString("ProductName"));
            return View(productsList);
        }

        
        public IActionResult SearchProduct()
        {

            return View(unitOfWork.ProductRepository.GetAll());
        }


    }
}
