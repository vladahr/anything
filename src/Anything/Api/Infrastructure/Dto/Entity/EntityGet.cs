using Microsoft.AspNetCore.Mvc;

namespace Anything.Api.Infrastructure.Dto.Entity
{
    public class EntityGet
    {
        [FromRoute(Name = "entityTypeName")]
        public string EntityTypeName { get; set; }
    }
}
