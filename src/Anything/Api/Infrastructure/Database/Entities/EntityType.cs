using System.Collections.Generic;

namespace Anything.Api.Infrastructure.Database.Entities
{
    public class EntityType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Entity> Entities { get; set; }
    }
}
