using Access.Application.Contracts.Persistence;
using Access.Application.Contracts.Repositories;
using Access.Application.Exceptions;
using Access.Application.UseCases.Common;
using Access.Application.Utilities.Mediator;
using Access.Application.Utilities.Results;
using Access.Domain.Entities.Credentials;

namespace Access.Application.UseCases.Credentials.Commands.RevokeCredential
{
    public sealed class RevokeCredentialUseCase :
        BaseHandler,
        IRequestHandler<RevokeCredentialCommand, Result<Guid>>
    {
        private readonly ICredentialsRepository _credentialsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RevokeCredentialUseCase(ICredentialsRepository credentialsRepository, IUnitOfWork unitOfWork)
        {
            _credentialsRepository = credentialsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(RevokeCredentialCommand command)
        {
            return await ExecuteAsync(async () =>
            {
                Credential? credential = await _credentialsRepository.GetByCodeAsync(command.Code);

                if (credential is null)
                {
                    throw new NotFoundException($"No existe una credencial con el código {command.Code.Trim().ToUpperInvariant()}.");
                }

                credential.Revoke();

                await _credentialsRepository.UpdateAsync(credential);
                await _unitOfWork.CommitAsync();

                return credential.Id;
            });
        }
    }
}
