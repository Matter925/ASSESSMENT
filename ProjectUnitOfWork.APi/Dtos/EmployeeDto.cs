using System.ComponentModel.DataAnnotations;

namespace ProjectUnitOfWork.APi.Dtos;

public class EmployeeDto
{
    public int EmployeeID { get; set; }

    public string Name { get; set; }

    public string Department { get; set; }

    public decimal Salary { get; set; }
}

public class EmployeeCreateDto
{

    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Department is required.")]
    [MaxLength(100, ErrorMessage = "Department cannot exceed 100 characters.")]
    public string Department { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Salary must be a positive number.")]
    public decimal Salary { get; set; }
}

public class EmployeeUpdateDto
{

    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Department is required.")]
    [MaxLength(100, ErrorMessage = "Department cannot exceed 100 characters.")]
    public string Department { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Salary must be a positive number.")]
    public decimal Salary { get; set; }
}