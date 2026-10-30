using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Access.Api.DTOs.AccessPoints;
using Access.Application.UseCases.AccessPoints.Commands.CreateAccessPoint;
using Access.Application.UseCases.AccessPoints.Queries.GetAccessPointById;
using Access.Application.UseCases.AccessPoints.Queries.GetAccessPointsList;
using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Pagination;
using Access.Application.Utilities.Results;

namespace Access.Api.Controllers
{
    [Route("api/access-points")]
    public class AccessPointsController : ApiController
    {
        private readonly IMediator _mediator;

        public AccessPointsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] int pageNumber = 1,
                                                 [FromQuery] int pageSize = PaginationRequest.DEFAULT_PAGE_SIZE,
                                                 [FromQuery] bool? isEnabled = null)
        {
            GetAccessPointsListQuery query = new()
            {
                Pagination = new PaginationRequest(pageNumber, pageSize),
                IsEnabled = isEnabled
            };

            Result<PaginationResponse<AccessPointListItemDTO>> result = await _mediator.Send(query);

            return ToActionResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            GetAccessPointByIdQuery query = new()
            {
                Id = id
            };

            Result<AccessPointDetailsDTO> result = await _mediator.Send(query);

            return ToActionResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAccessPointDTO request)
        {
            CreateAccessPointCommand command = new()
            {
                Name = request.Name,
                Type = request.Type,
                IsEnabled = request.IsEnabled
            };

            Result<Guid> result = await _mediator.Send(command);

            return ToActionResult(result, StatusCodes.Status201Created);
        }
    }
}
