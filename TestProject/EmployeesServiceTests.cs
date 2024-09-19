using AutoMapper;
using Microsoft.AspNetCore.Mvc;

using Moq;
using ProjectUnitOfWork.APi.Controllers;
using ProjectUnitOfWork.APi.Dtos;
using ProjectUnitOfWork.API.ResponseModule;
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
        new Employee { EmployeeID = 1, Name = "John",Department="It",Salary= 5000 },
        new Employee { EmployeeID = 2, Name = "Jane",Department="It",Salary= 5000 }
    };

        _unitOfWorkMock.Setup(u => u.Employees.GetAllAsync()).ReturnsAsync(employees);
        var employeeDtos = new List<EmployeeDto>
    {
        new EmployeeDto { EmployeeID = 1, Name = "John",Department="It",Salary= 5000 },
        new EmployeeDto { EmployeeID = 2, Name = "Jane",Department="It",Salary= 5000 }
    };


        _unitOfWorkMock.Setup(u => u.Employees.GetAllAsync()).ReturnsAsync(employees);
        _mapperMock.Setup(m => m.Map<IEnumerable<EmployeeDto>>(It.IsAny<IEnumerable<Employee>>())).Returns(employeeDtos);

        // Act
        var result = await _controller.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnEmployees = okResult.Value as IEnumerable<EmployeeDto>;

        // Debugging
        if (returnEmployees == null)
        {
            Console.WriteLine($"okResult.Value is null");
        }
        else
        {
            Console.WriteLine($"okResult.Value is of type {okResult.Value.GetType()}");
        }

        Assert.NotNull(okResult.Value);
        Assert.IsAssignableFrom<IEnumerable<EmployeeDto>>(returnEmployees);
        Assert.Equal(2, returnEmployees.Count());
    }

    [Fact]
    public async Task Get_ShouldReturnNotFound_WhenEmployeeDoesNotExist()
    {
        // Arrange
        var employeeId = 20;
        _unitOfWorkMock.Setup(u => u.Employees.GetById(employeeId)).Returns((Employee)null);

        // Act
        var result = await _controller.Get(employeeId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        var apiResponse = Assert.IsType<ApiResponse>(notFoundResult.Value);
        Assert.Equal(404, apiResponse.StatusCode);
        Assert.Equal($"ItemWithThisIdIsn'tFound", apiResponse.Message);
    }

    [Fact]
    public async Task Create_ShouldReturnOk_WhenEmployeeIsCreated()
    {
        // Arrange
        var dto = new EmployeeCreateDto { Name = "John", Department = "It", Salary = 5000 };
        var employee = new Employee { EmployeeID = 1, Name = "John", Department = "It", Salary = 5000 };
        var id = new Employee { EmployeeID = 1 };

        _mapperMock.Setup(m => m.Map<Employee>(dto)).Returns(employee);
        _unitOfWorkMock.Setup(u => u.Employees.AddAsync(employee)).ReturnsAsync(id);
        _unitOfWorkMock.Setup(u => u.Complete()).Verifiable();

        // Act
        var result = await _controller.Create(dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var apiResponse = Assert.IsType<ApiResponse>(okResult.Value);
        Assert.Equal(200, apiResponse.StatusCode);
        Assert.Equal("ItemIsCreatedSuccessfully", apiResponse.Message);
        Assert.Equal(1, apiResponse.Id);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
    }

    [Fact]
    public async Task Update_ShouldReturnOk_WhenEmployeeIsUpdated()
    {
        // Arrange
        var employeeId = 1;
        var dto = new EmployeeUpdateDto { Name = "John Updated", Department = "HR", Salary = 6000 };
        var existingEmployee = new Employee { EmployeeID = employeeId, Name = "John", Department = "It", Salary = 5000 };
        var updatedEmployee = new Employee { EmployeeID = employeeId, Name = "John Updated", Department = "HR", Salary = 6000 };

        _unitOfWorkMock.Setup(u => u.Employees.GetById(employeeId)).Returns(existingEmployee);
        _mapperMock.Setup(m => m.Map(dto, existingEmployee)).Callback((EmployeeUpdateDto d, Employee e) =>
        {
            e.Name = d.Name;
            e.Department = d.Department;
            e.Salary = d.Salary;
        });
        _unitOfWorkMock.Setup(u => u.Employees.Update(existingEmployee)).Returns(updatedEmployee);
        _unitOfWorkMock.Setup(u => u.Complete()).Verifiable();

        // Act
        var result = await _controller.Update(employeeId, dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var apiResponse = Assert.IsType<ApiResponse>(okResult.Value);
        Assert.Equal(200, apiResponse.StatusCode);
        Assert.Equal("ItemIsUpdatedSuccessfully", apiResponse.Message);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
    }

    [Fact]
    public async Task Update_ShouldReturnNotFound_WhenEmployeeDoesNotExist()
    {
        // Arrange
        var employeeId = 1;
        var dto = new EmployeeUpdateDto { Name = "John Updated", Department = "HR", Salary = 6000 };
        _unitOfWorkMock.Setup(u => u.Employees.GetById(employeeId)).Returns((Employee)null);

        // Act
        var result = await _controller.Update(employeeId, dto);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var apiResponse = Assert.IsType<ApiResponse>(notFoundResult.Value);
        Assert.Equal(404, apiResponse.StatusCode);
        Assert.Equal($"ItemWithThisIdIsn'tFound", apiResponse.Message);
    }
    [Fact]
    public async Task Delete_ShouldReturnOk_WhenEmployeeIsDeleted()
    {
        // Arrange
        var employeeId = 1;
        var employee = new Employee { EmployeeID = employeeId };

        _unitOfWorkMock.Setup(u => u.Employees.GetById(employeeId)).Returns(employee);
        _unitOfWorkMock.Setup(u => u.Employees.Delete(employee)).Verifiable();
        _unitOfWorkMock.Setup(u => u.Complete()).Verifiable();

        // Act
        var result = await _controller.Delete(employeeId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var apiResponse = Assert.IsType<ApiResponse>(okResult.Value);
        Assert.Equal(200, apiResponse.StatusCode);
        Assert.Equal("ItemIsDeletedSuccessfully", apiResponse.Message);
        _unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenEmployeeDoesNotExist()
    {
        // Arrange
        var employeeId = 1;
        _unitOfWorkMock.Setup(u => u.Employees.GetById(employeeId)).Returns((Employee)null);

        // Act
        var result = await _controller.Delete(employeeId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var apiResponse = Assert.IsType<ApiResponse>(notFoundResult.Value);
        Assert.Equal(404, apiResponse.StatusCode);
        Assert.Equal($"ItemWithThisIdIsn'tFound", apiResponse.Message);
    }

}
