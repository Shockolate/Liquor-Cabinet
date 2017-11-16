using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiquorCabinet.Repositories;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.UnitTests.Repositories.Glassware
{
    internal class InMemoryGlasswareRepository : ICrudRepository<LiquorCabinet.Repositories.Entities.Glass, int>
    {
        private readonly LiquorCabinet.Repositories.Entities.Glass _cocktailGlass =
            new LiquorCabinet.Repositories.Entities.Glass
            {
                Id = 1,
                Name = "Cocktail Glass",
                Description = "A Cocktail Glass",
                TypicalSize = "Enough for a Martini"
            };

        private readonly LiquorCabinet.Repositories.Entities.Glass _whiskeyTumbler = new LiquorCabinet.Repositories.Entities.Glass
        {
            Id = 2,
            Name = "Whiskey Tumbler",
            Description = "A Whiskey Tumbler",
            TypicalSize = "Enough for some whiskey."
        };

        internal LiquorCabinet.Repositories.Entities.Glass CocktailGlass => _cocktailGlass;

        public Task InsertAsync(LiquorCabinet.Repositories.Entities.Glass entityToCreate, ILogger logger) => throw new NotImplementedException();

        public Task<LiquorCabinet.Repositories.Entities.Glass> GetAsync(int id, ILogger logger) => throw new NotImplementedException();

        public Task<IEnumerable<LiquorCabinet.Repositories.Entities.Glass>> GetListAsync(ILogger logger)
        {
            IEnumerable<LiquorCabinet.Repositories.Entities.Glass> glasses = new List<LiquorCabinet.Repositories.Entities.Glass> {_cocktailGlass, _whiskeyTumbler};
            return Task.FromResult(glasses);

        }

        public Task UpdateAsync(LiquorCabinet.Repositories.Entities.Glass entityToUpdate, ILogger logger) => throw new NotImplementedException();

        public Task DeleteAsync(int id, ILogger logger) => throw new NotImplementedException();
    }
}