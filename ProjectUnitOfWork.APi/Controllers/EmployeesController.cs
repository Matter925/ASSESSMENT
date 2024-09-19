using Microsoft.AspNetCore.Authorization;
using System.Reflection.Metadata;

using Microsoft.AspNetCore.Mvc;

using ProjectUnitOfWork.Core;
using ProjectUnitOfWork.APi.Dtos;
using ProjectUnitOfWork.Core.Models;
using ProjectUnitOfWork.API.ResponseModule;
using AutoMapper;

namespace ProjectUnitOfWork.APi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EmployeesController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

   

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> Get()
    {

        var entities = await _unitOfWork.Employees.GetAllAsync();

        return Ok(entities);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDto>> Get(int id)
    {
        var entity =  _unitOfWork.Employees.GetById(id);
        var mappedEntity = _mapper.Map<EmployeeDto>(entity);
        if (entity == null)
        {
          
            return NotFound(new ApiResponse(404, $"ItemWithThisIdIsn'tFound"));
        }

        return Ok(mappedEntity);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Create(EmployeeCreateDto dto)
    {
        var mappedEntity = _mapper.Map<Employee>(dto);
        var id = await _unitOfWork.Employees.AddAsync(mappedEntity);
        _unitOfWork.Complete();
        return Ok(new ApiResponse(200, $"ItemIsCreatedSuccessfully", (int)id.EmployeeID));
    }



    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, EmployeeUpdateDto dto)
    {
        var entity = _unitOfWork.Employees.GetById(id);
        if (entity == null)
            return NotFound(new ApiResponse(404, $"ItemWithThisIdIsn'tFound"));
        _mapper.Map(dto, entity);
        var isUpdated =  _unitOfWork.Employees.Update(entity);
        if (isUpdated ==null)
            return NotFound(new ApiResponse(404, $"ItemWithThisIdIsn'tFound"));

        _unitOfWork.Complete();
        return Ok(new ApiResponse(200, $"ItemIsUpdatedSuccessfully"));
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var entity = _unitOfWork.Employees.GetById(id);
        _unitOfWork.Employees.Delete(entity);
        _unitOfWork.Complete();
        return Ok(new ApiResponse(200, $"ItemIsDeletedSuccessfully"));
    }
}
