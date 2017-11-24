using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiquorCabinet.Repositories;
using LiquorCabinet.Repositories.Entities;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.UnitTests.Repositories
{
    internal class InMemoryIngredientRepository : BaseInMemoryRepository<Ingredient, int>
    {
        internal Ingredient Vodka = new Ingredient {Id = 1, Name = "Vodka", Description = "Clear Spirit"};
        internal Ingredient Whiskey = new Ingredient {Id = 2, Name = "Whiskey", Description = "Bourbon is good."};

        public InMemoryIngredientRepository() : this(false) { }

        public InMemoryIngredientRepository(bool throws) : base(throws)
        {
            Ingredients.Add(Vodka);
            Ingredients.Add(Whiskey);
        }

        public IList<Ingredient> Ingredients { get; } = new List<Ingredient>();

        protected override Task DoInsertAsync(Ingredient entityToCreate, ILogger logger)
        {
            Ingredients.Add(entityToCreate);
            return Task.CompletedTask;
        }

        protected override Task<Ingredient> DoGetAsync(int id, ILogger logger)
        {
            var queriedIngredients = Ingredients.Where(i => i.Id == id).ToArray();
            if (!queriedIngredients.Any())
            {
                throw new EntityNotFoundException($"Ingredient: {id} Not Found");
            }
            return Task.FromResult(queriedIngredients.First());
        }

        protected override Task<IEnumerable<Ingredient>> DoGetListAsync(ILogger logger) => Task.FromResult(Ingredients.AsEnumerable());

        protected override Task DoUpdateAsync(Ingredient entityToUpdate, ILogger logger)
        {
            var ingredient = Ingredients.FirstOrDefault(i => i.Id == entityToUpdate.Id);
            if (ingredient == null)
            {
                throw new EntityNotFoundException($"Ingredient: {entityToUpdate.Id} Not Found");
            }

            ingredient.Name = entityToUpdate.Name;
            ingredient.Description = entityToUpdate.Description;
            return Task.CompletedTask;
        }

        protected override Task DoDeleteAsync(int id, ILogger logger)
        {
            if (Ingredients.Remove(Ingredients.SingleOrDefault(i => i.Id == id)))
            {
                return Task.CompletedTask;
            }
            throw new EntityNotFoundException($"Ingredient: {id} Not Found");
        }
    }
}