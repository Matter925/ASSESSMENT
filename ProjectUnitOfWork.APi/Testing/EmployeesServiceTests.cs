using AutoMapper;
using Microsoft.AspNetCore.Mvc;

using Moq;
using ProjectUnitOfWork.APi.Controllers;
using ProjectUnitOfWork.APi.Dtos;
using ProjectUnitOfWork.Core;
using ProjectUnitOfWork.Core.Models;

using Xunit;

namespace ProjectUnitOfWork.APi.Testing;

public class EmployeesServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly EmployeesController _controller;

    public EmployeesServiceTests()
    {
        // Create mocks for IUnitOfWork and IMapper
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();

        // Instantiate EmployeesController with the mocked dependencies
        _controller = new EmployeesController(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnAllEmployees()
    {
        // Arrange
        var employees = new List<Employee>
    {
        new Employee { EmployeeID = 1, Name = "John" },
        new Employee { EmployeeID = 2, Name = "Jane" }
    };

        _unitOfWorkMock.Setup(u => u.Employees.GetAllAsync()).ReturnsAsync(employees);
        var employeeDtos = new List<EmployeeDto>
    {
        new EmployeeDto { EmployeeID = 1, Name = "John",Department="It",Salary= 5000 },
        new EmployeeDto { EmployeeID = 2, Name = "Jane",Department="It",Salary= 5000 }
    };

        _mapperMock.Setup(m => m.Map<IEnumerable<EmployeeDto>>(employees)).Returns(employeeDtos);

        // Act
        var result = await _controller.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnEmployees = Assert.IsAssignableFrom<IEnumerable<EmployeeDto>>(okResult.Value);
        Assert.Equal(2, returnEmployees.Count());
    }

}
