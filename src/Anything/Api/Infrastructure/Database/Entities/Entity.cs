using System.Collections.Generic;

namespace Anything.Api.Infrastructure.Database.Entities
{
    public class Entity
    {
        public int Id { get; set; }
        public int EntityTypeId { get; set; }
        public string Data { get; set; }

        public EntityType EntityType { get; set; }
    }
}
