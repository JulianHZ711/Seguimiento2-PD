using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Access.Api.DTOs.Employees;
using Access.Application.UseCases.Employees.Commands.CreateEmployee;
using Access.Application.UseCases.Employees.Commands.DeactivateEmployee;
using Access.Application.UseCases.Employees.Commands.UpdateEmployee;
using Access.Application.UseCases.Employees.Queries.GetEmployeeById;
using Access.Application.UseCases.Employees.Queries.GetEmployeesList;
using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Pagination;
using Access.Application.Utilities.Results;
using Access.Domain.Entities.Employees.ValueObjects;

namespace Access.Api.Controllers
{
    [Route("api/[controller]")]
    public class EmployeesController : ApiController
    {
        private readonly IMediator _mediator;

        public EmployeesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] int pageNumber = 1,
                                                 [FromQuery] int pageSize = PaginationRequest.DEFAULT_PAGE_SIZE,
                                                 [FromQuery] Guid? areaId = null,
                                                 [FromQuery] EmploymentStatus? status = null)
        {
            GetEmployeesListQuery query = new()
            {
                Pagination = new PaginationRequest(pageNumber, pageSize),
                AreaId = areaId,
                Status = status
            };

            Result<PaginationResponse<EmployeeListItemDTO>> result = await _mediator.Send(query);

            return ToActionResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            GetEmployeeByIdQuery query = new()
            {
                Id = id
            };

            Result<EmployeeDetailsDTO> result = await _mediator.Send(query);

            return ToActionResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeDTO request)
        {
            CreateEmployeeCommand command = new()
            {
                DocumentNumber = request.DocumentNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Position = request.Position,
                AreaId = request.AreaId
            };

            Result<Guid> result = await _mediator.Send(command);

            return ToActionResult(result, StatusCodes.Status201Created);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateEmployeeDTO request)
        {
            UpdateEmployeeCommand command = new()
            {
                Id = request.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Position = request.Position,
                AreaId = request.AreaId
            };

            Result<Guid> result = await _mediator.Send(command);

            return ToActionResult(result);
        }

        [HttpPatch("{id}/deactivate")]
        public async Task<IActionResult> Deactivate([FromRoute] Guid id)
        {
            DeactivateEmployeeCommand command = new()
            {
                Id = id
            };

            Result<Guid> result = await _mediator.Send(command);

            return ToActionResult(result);
        }
    }
}
