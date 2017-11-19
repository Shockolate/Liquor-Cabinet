using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiquorCabinet.Repositories;
using LiquorCabinet.Repositories.Entities;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.UnitTests.Repositories
{
    internal class InMemoryGlasswareRepository : ICrudRepository<Glass, int>
    {
        private readonly bool _throws;

        public InMemoryGlasswareRepository() : this(false) { }

        public InMemoryGlasswareRepository(bool throws)
        {
            _throws = throws;
            Glasses = new List<Glass> {CocktailGlass, WhiskeyTumbler};
        }

        public Glass WhiskeyTumbler { get; } =
            new Glass {Id = 2, Name = "Whiskey Tumbler", Description = "A Whiskey Tumbler", TypicalSize = "Enough for some whiskey."};

        public Glass CocktailGlass { get; } =
            new Glass {Id = 1, Name = "Cocktail Glass", Description = "A Cocktail Glass", TypicalSize = "Enough for a Martini"};

        public IList<Glass> Glasses { get; }

        public Task InsertAsync(Glass entityToCreate, ILogger logger)
        {
            if (_throws)
            {
                throw new Exception("SQL Error");
            }

            Glasses.Add(entityToCreate);
            return Task.CompletedTask;
        }

        public Task<Glass> GetAsync(int id, ILogger logger) => throw new NotImplementedException();

        public Task<IEnumerable<Glass>> GetListAsync(ILogger logger)
        {
            if (_throws)
            {
                throw new Exception("SQL Error");
            }
            return Task.FromResult(Glasses.AsEnumerable());
        }

        public Task UpdateAsync(Glass entityToUpdate, ILogger logger) => throw new NotImplementedException();

        public Task DeleteAsync(int id, ILogger logger) => throw new NotImplementedException();
    }
}