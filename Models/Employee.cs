using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage="Name length cannot be more than 50")]
        public string Name { get; set; }
        [Required]
        [Display(Name="Office Email")]
        public string Email { get; set; }
        public Dpt Department { get; set; }
        public string PhotoPath { get; set; }
        public string Documents { get; set; }
    }
}
