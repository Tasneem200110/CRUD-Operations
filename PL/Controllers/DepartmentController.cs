using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Reprositories;
using Demo.DAL.Context;
using Demo.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PL.Models;

namespace PL.Controllers
{
    public class DepartmentController : Controller
    {
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly ILogger<DepartmentController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentController(
            //IDepartmentRepository departmentRepository, 
            ILogger<DepartmentController> logger,
            IUnitOfWork unitOfWork,
            IMapper mapper)
            //IEmployeeRepository employeeRepository)

        {
            //_departmentRepository = departmentRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();
            var departmentvm = _mapper.Map<IEnumerable<DepartmentVM>>(departments);


            return View(departmentvm);
        }

        public IActionResult Create()
        {
            return View(new DepartmentVM());
        }
        [HttpPost]
        public IActionResult Create(DepartmentVM departmentvm)
        {
            if (ModelState.IsValid)
            {
                #region Manual Mapping

                //var Department = new Department()
                //{
                //    Id = departmentvm.Id,
                //    Name = departmentvm.Name,
                //    Code = departmentvm.Code,
                //    CreationDate = departmentvm.CreationDate,
                //}; 
                #endregion
                var department = _mapper.Map<Department>(departmentvm);

                _unitOfWork.DepartmentRepository.Add(department);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(departmentvm);
            }
        }

        public IActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();

                var department = _unitOfWork.DepartmentRepository.GetById(id);

                if (department == null)
                    return NotFound();

                return View(department);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error","Home");
            }
        }
        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (id == null)
                return NotFound();

            var department = _unitOfWork.DepartmentRepository.GetById(id);
            var departmentvm = _mapper.Map<DepartmentVM>(department);

            if (department == null)
                return NotFound();
            return View(departmentvm);

        }
        [HttpPost]
        public IActionResult Update(int id, DepartmentVM departmentvm)
        {
            if (id != departmentvm.Id)
                return NotFound();
            if (ModelState.IsValid)
            {
                var department = _mapper.Map<Department>(departmentvm);

                _unitOfWork.DepartmentRepository.Update(department);
                return RedirectToAction(nameof(Index));
            }

            return View(departmentvm);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var department = _unitOfWork.DepartmentRepository.GetById(id);

            if (department == null)
                return NotFound();

            _unitOfWork.DepartmentRepository.Delete(department);
            return RedirectToAction(nameof(Index));

        }
    }
}
