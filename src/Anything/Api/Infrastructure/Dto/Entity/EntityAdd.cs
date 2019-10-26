using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Anything.Api.Infrastructure.Dto.Entity
{
    public class EntityAdd
    {
        [FromRoute(Name = "entityTypeName")]
        public string EntityTypeName { get; set; }

        [FromBody]
        public JsonElement Data { get; set; }
    }
}
