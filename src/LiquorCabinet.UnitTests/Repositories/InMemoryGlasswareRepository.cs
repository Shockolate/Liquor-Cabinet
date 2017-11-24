using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiquorCabinet.Repositories.Entities;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.UnitTests.Repositories
{
    internal class InMemoryGlasswareRepository : BaseInMemoryRepository<Glass, int>
    {
        public InMemoryGlasswareRepository() : this(false) { }

        public InMemoryGlasswareRepository(bool throws) : base(throws)
        {
            Glasses = new List<Glass> {CocktailGlass, WhiskeyTumbler};
        }

        public Glass WhiskeyTumbler { get; } =
            new Glass {Id = 2, Name = "Whiskey Tumbler", Description = "A Whiskey Tumbler", TypicalSize = "Enough for some whiskey."};

        public Glass CocktailGlass { get; } =
            new Glass {Id = 1, Name = "Cocktail Glass", Description = "A Cocktail Glass", TypicalSize = "Enough for a Martini"};

        public IList<Glass> Glasses { get; }

        protected override Task DoInsertAsync(Glass entityToCreate, ILogger logger)
        {
            Glasses.Add(entityToCreate);
            return Task.CompletedTask;
        }

        protected override Task<Glass> DoGetAsync(int id, ILogger logger) => throw new NotImplementedException();

        protected override Task<IEnumerable<Glass>> DoGetListAsync(ILogger logger) => Task.FromResult(Glasses.AsEnumerable());

        protected override Task DoUpdateAsync(Glass entityToUpdate, ILogger logger) => throw new NotImplementedException();

        protected override Task DoDeleteAsync(int id, ILogger logger) => throw new NotImplementedException();
    }
}