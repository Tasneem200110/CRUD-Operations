using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class Department : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime DateOfCreation { get; set; } = DateTime.Now;
    }
}
