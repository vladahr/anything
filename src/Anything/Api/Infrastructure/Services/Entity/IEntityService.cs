using System.Collections.Generic;
using System.Threading.Tasks;
using Anything.Api.Infrastructure.Dto.Entity;

namespace Anything.Api.Infrastructure.Services.Entity
{
    public interface IEntityService
    {
        Task AddEntityAsync(EntityAdd model);
        Task<IEnumerable<string>> GetEntitiesAsync(EntityGet model);
    }
}
