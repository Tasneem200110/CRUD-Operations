using System.ComponentModel.DataAnnotations;

namespace PL.Models
{
    public class DepartmentVM 
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime DateOfCreation { get; set; } = DateTime.Now;
    }
}
