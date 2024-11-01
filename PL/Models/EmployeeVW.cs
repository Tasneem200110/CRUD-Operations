using Demo.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace PL.Models
{
    public class EmployeeVM
    {
        public int Id { get; set; }
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
        public IFormFile Image { get; set; }
        public string ImageURL { get; set; }
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
    }
}
