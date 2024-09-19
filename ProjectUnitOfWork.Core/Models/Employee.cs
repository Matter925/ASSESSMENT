using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectUnitOfWork.Core.Models;
public class Employee
{
    [Key]
    public int EmployeeID { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Department is required.")]
    [MaxLength(100, ErrorMessage = "Department cannot exceed 100 characters.")]
    public string Department { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Salary must be a positive number.")]
    public decimal Salary { get; set; }
}
