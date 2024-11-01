using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Reprositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly MVCAppDbContext _context;
        public EmployeeRepository(MVCAppDbContext context) : base(context) 
        {
            _context = context;
        }

        public Employee GetByName(string name)
            =>_context.Employees.Find(name);

        public IEnumerable<Employee> Search(string name)
            => _context.Employees.Where(emp => emp.Name.Trim().ToLower().Contains(name.ToLower().Trim()));

        public IEnumerable<Employee> GetAllByDepId(int? depId)
            => _context.Employees.Where(emp => emp.DepartmentId==depId);


        //public int Add(Employee employee)
        //{
        //    _context.Employees.Add(employee);
        //    return _context.SaveChanges();
        //}

        //public int Delete(Employee employee)
        //{
        //    _context.Employees.Remove(employee);
        //    return _context.SaveChanges();
        //}

        //public IEnumerable<Employee> GetAll()
        //    => _context.Employees.ToList();

        //public Employee GetById(int? id)
        //    => _context.Employees.Find(id);

        //public int Update(Employee employee)
        //{
        //    _context.Employees.Update(employee);
        //    return _context.SaveChanges();
        //}
    }
}
