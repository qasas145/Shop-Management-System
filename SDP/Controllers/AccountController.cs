using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SDP.ViewModels;
using SDP.Models;
using SDP.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Data.Repository;
namespace SDP.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private IUnitOfWork unitOfWork;

        public AccountController(UserManager<IdentityUser> userManager,
                                SignInManager<IdentityUser> signInManager, 
                                ApplicationDbContext context, 
                                IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _context = context;
            this.unitOfWork = unitOfWork;

        }
        // GET: AccountController
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {   
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email, EmailConfirmed= true };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                
                    await signInManager.SignInAsync(user, isPersistent: false);
                    await userManager.AddToRoleAsync(user, model.Role);

                    if (model.Role == "Admin"){
                        return RedirectToAction("index", "home");
                    }
                    else if (model.Role == "Customer") {
                        customer new_customer = new customer(){
                            Name = model.Name,
                            email = user.Email,
                            contact = model.Contact,
                            address = model.Address
                        };
                        unitOfWork.CustomerRepository.Add(new_customer);
                        unitOfWork.Save();
                        // adding the customer to the list  
                        return RedirectToAction("index", "customer");

                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            ViewBag.Email = null;
            if (ModelState.IsValid)
            {
                if (model.Role == "Admin")
                {
                    var user = await userManager.FindByEmailAsync(model.Email);
                    if (user != null && !user.EmailConfirmed)
                    {
                        ModelState.AddModelError("message", "Email not confirmed yet");
                        return View();

                    }
                    var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                    if (result.Succeeded)
                    {
                        // HttpContext.Session.SetString("Role", model.Role);
                        return RedirectToAction("index", "home");
                    }
                    else {
                        ViewBag.Message = "Admin not Exist!!!";
                        ModelState.AddModelError("", "Invalid Login Attempt");
                        return View();
                    }

                    
                }
                else {
                    var user = unitOfWork.UserRepository.Get(u=>u.Email == model.Email);
                    if ( user!= null)
                    {
                        HttpContext.Session.SetString("Email", model.Email);
                        return RedirectToAction("index", "customer");

                    }
                    else
                    {
                        HttpContext.Session.Remove("Email");
                        ViewBag.Message = "Customer not Exist!!!";
                        return View();

                    }
                }
                

            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {

            await signInManager.SignOutAsync();
            HttpContext.Session.Remove("Email");
            return RedirectToAction("index", "customer");
        }

    }
}
