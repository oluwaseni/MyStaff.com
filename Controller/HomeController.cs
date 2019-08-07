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

        [HttpGet]
        public ViewResult Edit(int Id)
        {
            Employee employee = _employeeRepository.GetEmployee(Id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath = employee.PhotoPath
            };
            return View(employeeEditViewModel);
        }


        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {

            if (ModelState.IsValid)
            {
                Employee employee = _employeeRepository.GetEmployee(model.Id);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;
                if(model.Photo != null)
                {
                    if(model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                        //To delete the existing file

                        System.IO.File.Delete(filePath);
                    }
                    //This is a method to check of a file or many files were chosen
                    employee.PhotoPath = ProcessUpload(model);
                }
               


                _employeeRepository.Update(employee);
                return RedirectToAction("Index");
            }
            return View();
        }
        
        
        //This is the definition of a method to check if a file or many files were chosen

        private string ProcessUpload(EmployeeCreateViewModel model)
        {
            string uniqueFilesName = null;

            if (model.Photo != null)
            {
                //foreach (IFormFile Document in model.Photo)
                //{
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFilesName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filesPath = Path.Combine(uploadsFolder, uniqueFilesName);
                    using(var filestream = new FileStream(filesPath, FileMode.Create))
                    {
                        model.Photo.CopyTo(filestream);
                    }
                    //Document.CopyTo(new FileStream(filesPath, FileMode.Create));
                //}

            }

            return uniqueFilesName;
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
                //string uniqueFileName = null;
                string uniqueFilesName = ProcessUpload(model);
               

                Employee anotherEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFilesName,
                    //Documents = uniqueFilesName
                };

                _employeeRepository.Post(anotherEmployee);
                return RedirectToAction("Details", new { id = anotherEmployee.Id });
            }
            return View("Create");
        }

    }
}
