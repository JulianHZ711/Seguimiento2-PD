using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Access.Api.DTOs.Credentials;
using Access.Application.UseCases.Credentials.Commands.AssignCredential;
using Access.Application.UseCases.Credentials.Commands.BlockCredential;
using Access.Application.UseCases.Credentials.Commands.RevokeCredential;
using Access.Application.UseCases.Credentials.Queries.GetCredentialByCode;
using Access.Application.UseCases.Credentials.Queries.GetCredentialsByEmployee;
using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Results;

namespace Access.Api.Controllers
{
    [Route("api/[controller]")]
    public class CredentialsController : ApiController
    {
        private readonly IMediator _mediator;

        public CredentialsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetByCode([FromRoute] string code)
        {
            GetCredentialByCodeQuery query = new()
            {
                Code = code
            };

            Result<CredentialDetailsDTO> result = await _mediator.Send(query);

            return ToActionResult(result);
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetByEmployee([FromRoute] Guid employeeId)
        {
            GetCredentialsByEmployeeQuery query = new()
            {
                EmployeeId = employeeId
            };

            Result<List<CredentialListItemDTO>> result = await _mediator.Send(query);

            return ToActionResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Assign([FromBody] AssignCredentialDTO request)
        {
            AssignCredentialCommand command = new()
            {
                Code = request.Code,
                EmployeeId = request.EmployeeId
            };

            Result<Guid> result = await _mediator.Send(command);

            return ToActionResult(result, StatusCodes.Status201Created);
        }

        [HttpPatch("{code}/block")]
        public async Task<IActionResult> Block([FromRoute] string code)
        {
            Result<Guid> result = await _mediator.Send(new BlockCredentialCommand { Code = code });

            return ToActionResult(result);
        }

        [HttpPatch("{code}/revoke")]
        public async Task<IActionResult> Revoke([FromRoute] string code)
        {
            Result<Guid> result = await _mediator.Send(new RevokeCredentialCommand { Code = code });

            return ToActionResult(result);
        }
    }
}
