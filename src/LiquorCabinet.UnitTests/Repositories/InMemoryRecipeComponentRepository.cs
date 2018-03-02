using LiquorCabinet.Models;
using LiquorCabinet.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace LiquorCabinet.UnitTests.Repositories
{
    internal class InMemoryRecipeComponentRepository : BaseInMemoryRepository<int, RecipeComponent>
    {
        private readonly RecipeComponent _campari = new RecipeComponent
        {
            Id = 295,
            ComponentId = 13,
            ComponentName = "Campari",
            RecipeId = 2,
            QuantityPart = "1 Part",
            QuantityMetric = 3.0,
            QuantityImperial = 1.5
        };

        private readonly RecipeComponent _soda = new RecipeComponent
        {
            Id = 297,
            ComponentId = 70,
            ComponentName = "Soda",
            RecipeId = 2,
            QuantityPart = "Splash",
            QuantityMetric = null,
            QuantityImperial = null
        };

        private readonly RecipeComponent _sweetVermouth = new RecipeComponent
        {
            Id = 296,
            ComponentId = 76,
            ComponentName = "Sweet Vermouth",
            QuantityPart = "1 Part",
            RecipeId = 2,
            QuantityMetric = 3.0,
            QuantityImperial = 1.5
        };

        public IDictionary<int, RecipeComponent> RecipeComponents;

        public InMemoryRecipeComponentRepository() : this(false) { }

        public InMemoryRecipeComponentRepository(bool throws) : base(throws)
        {
            RecipeComponents = new Dictionary<int, RecipeComponent> {{_campari.Id, _campari}, {_sweetVermouth.Id, _sweetVermouth}, {_soda.Id, _soda}};
        }

        protected override Task DoDeleteAsync(int id)
        {
            if (RecipeComponents.ContainsKey(id))
            {
                RecipeComponents.Remove(id);
                return Task.CompletedTask;
                ;
            }
            throw new EntityNotFoundException("RecipeComponent", id);
        }

        protected override Task DoInsertListAsync(IEnumerable<RecipeComponent> entitesToCreate) => throw new NotImplementedException();

        protected override Task<RecipeComponent> DoGetAsync(int id)
        {
            if (RecipeComponents.ContainsKey(id))
            {
                return Task.FromResult(RecipeComponents[id]);
            }
            throw new EntityNotFoundException("RecipeComponent", id);
        }

        protected override Task<IEnumerable<RecipeComponent>> DoGetListAsync() => throw new NotImplementedException();

        protected override Task DoInsertAsync(RecipeComponent entityToCreate) => throw new NotImplementedException();

        protected override Task DoUpdateAsync(RecipeComponent entityToUpdate)
        {
            if (RecipeComponents.ContainsKey(entityToUpdate.Id))
            {
                RecipeComponents[entityToUpdate.Id] = entityToUpdate;
                return Task.CompletedTask;
            }

            throw new EntityNotFoundException("RecipeComponent", entityToUpdate.Id);
        }
    }
}