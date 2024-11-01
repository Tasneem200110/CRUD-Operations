using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Reprositories;
using Demo.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using PL.Helper;
using PL.Models;

namespace PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        

        public IActionResult Index(string SearchValue, int? DepartmentId)
        {
            IEnumerable<Employee> employees;
            IEnumerable<EmployeeVM> mappedEmployees;
            if (string.IsNullOrEmpty(SearchValue) && DepartmentId == null)
            {
                employees = _unitOfWork.EmployeeRepository.GetAll();
                mappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeVM>>(employees);
                return View(mappedEmployees);
            }

            else if(DepartmentId!=null)
            {
                employees = _unitOfWork.EmployeeRepository.GetAllByDepId(DepartmentId);
                mappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeVM>>(employees);
                return View(mappedEmployees);
            }
           
            else
            {
                employees = _unitOfWork.EmployeeRepository.Search(SearchValue);
                mappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeVM>>(employees);
                return View(mappedEmployees);
            }
        }
        public IActionResult Create()
        {
            ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeVM employeeVM)
        {
            //employee.Department = _unitOfWork.DepartmentRepository.GetById(employee.DepartmentId);
            if (ModelState.IsValid)
            {
                try
                {
                    //Manual mapping
                    //Employee employee = new Employee
                    //{
                    //    Name = employeeVM.Name,
                    //    Address = employeeVM.Address,
                    //    DepartmentId = employeeVM.DepartmentId,
                    //    Email = employeeVM.Email,
                    //    HireDate = employeeVM.HireDate,
                    //    IsActive = employeeVM.IsActive,
                    //    Salary = employeeVM.Salary
                    //};    

                    var employee = _mapper.Map<Employee>(employeeVM);
                    employee.ImageURL = DocumentSettings.UploadFile(employeeVM.Image, "Images");
                    _unitOfWork.EmployeeRepository.Add(employee);
                    return RedirectToAction(nameof(Index));
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else 
            {
                ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAll();
                return View(employeeVM); 
            }
        }
        public IActionResult Delete(int? id)
        {
            var employee = _unitOfWork.EmployeeRepository.GetById(id);
            _unitOfWork.EmployeeRepository.Delete(employee);
            var isDeleted = DocumentSettings.DeleteFile(employee.ImageURL, "Images");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (id == null)
                return NotFound();
            ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAll();

            var employee = _unitOfWork.EmployeeRepository.GetById(id);

            var employeeNM = _mapper.Map<EmployeeVM>(employee);



            if (employee == null)
                return NotFound();

            return View(employeeNM);

        }

        [HttpPost]
        public IActionResult Update(int? id, EmployeeVM employeevm)
        {
            if (id != employeevm.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var employee = _mapper.Map<Employee>(employeevm);
                employee.ImageURL = DocumentSettings.UploadFile(employeevm.Image, "Images");

                _unitOfWork.EmployeeRepository.Update(employee);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAll();
            return View(employeevm);
        }

        public IActionResult Details(int id)
        {
            var employee = _unitOfWork.EmployeeRepository.GetById(id);
            return View(employee);

        }

        

    }
}
