using Microsoft.AspNetCore.Mvc;
using Access.Application.UseCases.Areas.Queries.GetAreasList;
using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Results;

namespace Access.Api.Controllers
{
    [Route("api/[controller]")]
    public class AreasController : ApiController
    {
        private readonly IMediator _mediator;

        public AreasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            Result<List<AreaDTO>> result = await _mediator.Send(new GetAreasListQuery());

            return ToActionResult(result);
        }
    }
}
