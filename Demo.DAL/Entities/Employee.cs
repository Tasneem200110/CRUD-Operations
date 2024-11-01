using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class Employee : BaseEntity
    {

        [Required]
        [MaxLength(50)]
        [MinLength(10)]
        public string Name { get; set; }

        public string Address { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public DateTime HireDate { get; set; }
        public string ImageURL { get; set; }
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
    }
}
