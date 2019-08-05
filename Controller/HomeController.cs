using EmployeeManagement.Models;
using EmployeeManagement.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHostingEnvironment hostingEnvironment;

        public HomeController(IEmployeeRepository employeeRepository, 
                                IHostingEnvironment hostingEnvironment) 
        {
            _employeeRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
        }


        [Route("")]
        [Route("Home")]
        [Route("Home/Index")]
        public ViewResult index()
        {
            var model = _employeeRepository.GetAllEmployee();
            return View(model);
        }

        //This could work for API creations
        //public JsonResult Details()
        //{
        //    Employee model = _employeeRepository.GetEmployee(1);
        //    return Json(model);
        //}

        [Route("Home/Details/{Id?}")]
        public ViewResult Details(int? Id )
        {
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = _employeeRepository.GetEmployee(Id??1),
                PageTitle = "Employee Details"
            };

            return View(homeDetailsViewModel);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        //public RedirectToActionResult Create(Employee employee)
        //{
        //    Employee newEmployee = _employeeRepository.Add(employee);
        //    return RedirectToAction("Details", new { id = newEmployee.Id });
        //}
        [HttpPost]
        public IActionResult CreateNew(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                string uniqueFilesName = null;
                if (model.Photo != null)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                if (model.Documents != null && model.Documents.Count > 0)
                {
                    foreach(IFormFile Document in model.Documents)
                    {
                        string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "Documents");
                        uniqueFilesName = Guid.NewGuid().ToString() + "_" + Document.FileName; 
                        string filesPath = Path.Combine(uploadsFolder, uniqueFileName);
                        Document.CopyTo(new FileStream(filesPath, FileMode.Create));
                    }
                   
                }

                Employee anotherEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFileName,
                    Documents = uniqueFilesName
                };

                _employeeRepository.Post(anotherEmployee);
                return RedirectToAction("Details", new { id = anotherEmployee.Id });
            }
            return View("Create");
        }

    }
}
