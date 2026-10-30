using Access.Application.Utilities.Pagination;
using Access.Domain.Entities.AccessPoints;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Application.Contracts.Repositories
{
    public interface IAccessPointsRepository : IRepository<AccessPoint>
    {
        Task<PaginationResponse<AccessPoint>> GetPagedListAsync(PaginationRequest request,
                                                                bool? isEnabled,
                                                                CancellationToken cancellationToken = default);

        Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}
