using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Access.Api.DTOs.AccessLogs;
using Access.Application.UseCases.AccessLogs.Commands.RegisterAccess;
using Access.Application.UseCases.AccessLogs.Queries.GetAccessHistory;
using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Pagination;
using Access.Application.Utilities.Results;
using Access.Domain.Entities.AccessLogs.ValueObjects;

namespace Access.Api.Controllers
{
    [Route("api/access")]
    public class AccessController : ApiController
    {
        private readonly IMediator _mediator;

        public AccessController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // RF-03: el resultado (autorizado o rechazado) viene en data.result; ambos quedan en el historial.
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterAccessDTO request)
        {
            RegisterAccessCommand command = new()
            {
                CredentialCode = request.CredentialCode,
                AccessPointId = request.AccessPointId,
                MovementType = request.MovementType
            };

            Result<RegisterAccessResultDTO> result = await _mediator.Send(command);

            return ToActionResult(result, StatusCodes.Status201Created);
        }

        // RF-04: las fechas se interpretan en UTC.
        [HttpGet("history")]
        public async Task<IActionResult> GetHistory([FromQuery] int pageNumber = 1,
                                                    [FromQuery] int pageSize = PaginationRequest.DEFAULT_PAGE_SIZE,
                                                    [FromQuery] Guid? employeeId = null,
                                                    [FromQuery] DateTime? from = null,
                                                    [FromQuery] DateTime? to = null,
                                                    [FromQuery] Guid? accessPointId = null,
                                                    [FromQuery] AccessResult? result = null)
        {
            GetAccessHistoryQuery query = new()
            {
                Pagination = new PaginationRequest(pageNumber, pageSize),
                EmployeeId = employeeId,
                From = from,
                To = to,
                AccessPointId = accessPointId,
                Result = result
            };

            Result<PaginationResponse<AccessLogListItemDTO>> response = await _mediator.Send(query);

            return ToActionResult(response);
        }
    }
}
