using LiquorCabinet.Models;
using LiquorCabinet.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace LiquorCabinet.UnitTests.Repositories
{
    internal class InMemoryIngredientRepository : BaseInMemoryRepository<int, Ingredient>
    {
        internal Ingredient Vodka = new Ingredient {Id = 1, Name = "Vodka", Description = "Clear Spirit"};
        internal Ingredient Whiskey = new Ingredient {Id = 2, Name = "Whiskey", Description = "Bourbon is good."};

        public InMemoryIngredientRepository() : this(false) { }

        public InMemoryIngredientRepository(bool throws) : base(throws)
        {
            Ingredients.Add(Vodka.Id, Vodka);
            Ingredients.Add(Whiskey.Id, Whiskey);
        }

        public IDictionary<int, Ingredient> Ingredients { get; } = new Dictionary<int, Ingredient>();

        protected override Task DoInsertAsync(Ingredient entityToCreate)
        {
            Ingredients.Add(Ingredients.Max(kvp => kvp.Key) + 1, entityToCreate);
            return Task.CompletedTask;
        }

        protected override Task DoInsertListAsync(IEnumerable<Ingredient> entitesToCreate) => throw new NotImplementedException();

        protected override Task<Ingredient> DoGetAsync(int id)
        {
            var queriedIngredients = Ingredients.Where(kvp => kvp.Key == id).ToArray();
            if (!queriedIngredients.Any())
            {
                throw new EntityNotFoundException("Ingredient", id);
            }
            return Task.FromResult(queriedIngredients.Select(kvp => kvp.Value).First());
        }

        protected override Task<IEnumerable<Ingredient>> DoGetListAsync() => Task.FromResult(Ingredients.Select(kvp => kvp.Value));

        protected override Task DoUpdateAsync(Ingredient entityToUpdate)
        {
            var ingredient = Ingredients.FirstOrDefault(kvp => kvp.Key == entityToUpdate.Id).Value;
            if (ingredient == null)
            {
                throw new EntityNotFoundException("Ingredient", entityToUpdate.Id);
            }

            ingredient.Name = entityToUpdate.Name;
            ingredient.Description = entityToUpdate.Description;
            return Task.CompletedTask;
        }

        protected override Task DoDeleteAsync(int id)
        {
            if (Ingredients.Remove(Ingredients.SingleOrDefault(kvp => kvp.Key == id)))
            {
                return Task.CompletedTask;
            }
            throw new EntityNotFoundException("Ingredient", id);
        }
    }
}