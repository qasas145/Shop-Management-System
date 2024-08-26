using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SDP.Data;
using SDP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDP.Controllers
{
    public class StaffController : Controller
    {
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment;
        private readonly IUnitOfWork unitOfWork;
        public StaffController(ApplicationDbContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult AddStaff()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AddStaff(staff model)
        {

            if (ModelState.IsValid)
            {
                staff st = new staff
                {
                    email = model.email,
                    name = model.name,
                    contact = model.contact,
                    salary = model.salary,
                    address = model.address,
                    role = model.role
                };

                unitOfWork.StaffRepository.Add(st);
                unitOfWork.Save();
                return RedirectToAction("ViewStaff","Staff");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> UpdateStaff(int? id)
        {
            staff st = unitOfWork.StaffRepository.Get(u=>u.staffId == id);

            if (st == null)
            {
                return NotFound();
            }

            return View(st);
        }
        [HttpPost]

        public async Task<IActionResult> UpdateStaff(staff st)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.StaffRepository.Update(st);
                    unitOfWork.Save();
                }
                catch (Exception)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(ViewStaff));
            }

            return View(st);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteStaff(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            staff st = unitOfWork.StaffRepository.Get(u=>u.staffId == id);
            if (st == null)
            {
                return NotFound();
            }

            return View(st);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            var st = unitOfWork.StaffRepository.Get(u=>u.staffId == id);
            unitOfWork.StaffRepository.Remove(st);
            unitOfWork.Save();
            return RedirectToAction(nameof(ViewStaff));
        }

        public IActionResult ViewStaff()
        {
            return View(unitOfWork.StaffRepository.GetAll());
        }
    }
}
