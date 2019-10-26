using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Anything.Api.Infrastructure.Database;
using Anything.Api.Infrastructure.Dto.Entity;
using Microsoft.EntityFrameworkCore;

namespace Anything.Api.Infrastructure.Services.Entity
{
    public class EntityService : IEntityService
    {
        private readonly EntitiesDbContext dbContext;

        public EntityService(EntitiesDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddEntityAsync(EntityAdd model)
        {
            if (string.IsNullOrWhiteSpace(model.EntityTypeName))
                throw new ArgumentException("Entity type is not specified");

            var entityTypeName = model.EntityTypeName.Trim().ToLowerInvariant();
            var json = model.Data;

            var jsonValueKind = json.ValueKind;
            if (jsonValueKind != JsonValueKind.Object &&
                jsonValueKind != JsonValueKind.Array)
                throw new ArgumentException("Invalid json");

            await ProcessEntitiesInternalAsync(entityTypeName, json);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<string>> GetEntitiesAsync(EntityGet model)
        {
            if (string.IsNullOrWhiteSpace(model.EntityTypeName))
                throw new ArgumentException("Entity type is not specified");

            var entityTypeName = model.EntityTypeName.Trim().ToLowerInvariant();
            var result = await (from et in dbContext.EntityTypes
                                join e in dbContext.Entities on et.Id equals e.EntityTypeId
                                where et.Name == entityTypeName
                                select e.Data).ToListAsync();
            return result;
        }

        private async Task ProcessEntitiesInternalAsync(
            string entityTypeName,
            JsonElement jsonData)
        {
            if (jsonData.ValueKind == JsonValueKind.Object)
            {
                foreach (var objectPair in jsonData.EnumerateObject())
                    await ProcessEntitiesInternalAsync(objectPair.Name, objectPair.Value);
            }
            else if (jsonData.ValueKind == JsonValueKind.Array)
            {
                foreach (var arrayValue in jsonData.EnumerateArray())
                    await ProcessEntitiesInternalAsync(null, arrayValue);
            }
            else return;

            if (entityTypeName != null)
                await AddEntityInternalAsync(entityTypeName, JsonSerializer.Serialize(jsonData));
        }

        private async Task<Database.Entities.Entity> AddEntityInternalAsync(string entityTypeName, string data)
        {
            var entityType = await GetOrAddEntityTypeInternalAsync(entityTypeName);
            var entity = new Database.Entities.Entity
            {
                Data = data,
                EntityTypeId = entityType.Id
            };

            dbContext.Add(entity);
            return entity;
        }

        private async Task<Database.Entities.EntityType> GetOrAddEntityTypeInternalAsync(string entityTypeName)
        {
            var entityType = await dbContext.EntityTypes.FirstOrDefaultAsync(x => x.Name == entityTypeName);
            if (entityType == null)
            {
                entityType = new Database.Entities.EntityType
                {
                    Name = entityTypeName
                };
                dbContext.Add(entityType);
                await dbContext.SaveChangesAsync();
            }

            return entityType;
        }
    }
}
