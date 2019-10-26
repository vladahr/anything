using System.Text.Json;
using System.Threading.Tasks;
using Anything.Api.Infrastructure.Dto.Entity;
using Anything.Api.Infrastructure.Services.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Anything.Api.Controllers
{
    public class EntityController : ApiControllerBase
    {
        private readonly IEntityService entityService;

        public EntityController(IEntityService entityService)
        {
            this.entityService = entityService;
        }

        [HttpPost("{entityTypeName}")]
        public async Task<IActionResult> CreateEntity(EntityAdd model)
        {
            await entityService.AddEntityAsync(model);
            return Ok();
        }

        [HttpGet("{entityTypeName}")]
        public async Task<IActionResult> Get(EntityGet model)
        {
            var result = await entityService.GetEntitiesAsync(model);
            return Ok(result);
        }
    }
}
