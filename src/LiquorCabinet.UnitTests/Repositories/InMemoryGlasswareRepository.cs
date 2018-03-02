using LiquorCabinet.Models;
using LiquorCabinet.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiquorCabinet.UnitTests.Repositories
{
    internal class InMemoryGlasswareRepository : BaseInMemoryRepository<int, Glass>
    {
        public readonly IDictionary<int, Glass> Glasses = new Dictionary<int, Glass>();
        public InMemoryGlasswareRepository() : this(false) { }

        public InMemoryGlasswareRepository(bool throws) : base(throws)
        {
            Glasses.Add(1, CocktailGlass);
            Glasses.Add(2, WhiskeyTumbler);
        }

        public Glass WhiskeyTumbler { get; } =
            new Glass {Id = 2, Name = "Whiskey Tumbler", Description = "A Whiskey Tumbler", TypicalSize = "Enough for some whiskey."};

        public Glass CocktailGlass { get; } =
            new Glass {Id = 1, Name = "Cocktail Glass", Description = "A Cocktail Glass", TypicalSize = "Enough for a Martini"};


        protected override Task DoInsertAsync(Glass entityToCreate)
        {
            Glasses.Add(entityToCreate.Id, entityToCreate);
            return Task.CompletedTask;
        }

        protected override Task DoInsertListAsync(IEnumerable<Glass> entitesToCreate) => throw new NotImplementedException();

        protected override Task<Glass> DoGetAsync(int id)
        {
            if (Glasses.ContainsKey(id))
            {
                return Task.FromResult(Glasses[id]);
            }
            throw new EntityNotFoundException("Glass", id);
        }

        protected override Task<IEnumerable<Glass>> DoGetListAsync() => Task.FromResult(Glasses.Select(kvp => kvp.Value));

        protected override Task DoUpdateAsync(Glass entityToUpdate) => throw new NotImplementedException();

        protected override Task DoDeleteAsync(int id) => throw new NotImplementedException();
    }
}