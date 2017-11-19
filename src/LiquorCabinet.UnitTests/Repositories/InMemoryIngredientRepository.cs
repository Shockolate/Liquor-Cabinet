using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiquorCabinet.Repositories;
using LiquorCabinet.Repositories.Entities;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.UnitTests.Repositories
{
    internal class InMemoryIngredientRepository : ICrudRepository<Ingredient, int>
    {
        private readonly bool _throws;
        internal Ingredient Vodka = new Ingredient {Id = 1, Name = "Vodka", Description = "Clear Spirit"};
        internal Ingredient Whiskey = new Ingredient {Id = 2, Name = "Whiskey", Description = "Bourbon is good."};

        public InMemoryIngredientRepository() : this(false) { }

        public InMemoryIngredientRepository(bool throws)
        {
            _throws = throws;
            Ingredients.Add(Vodka);
            Ingredients.Add(Whiskey);
        }

        public IList<Ingredient> Ingredients { get; } = new List<Ingredient>();

        public Task InsertAsync(Ingredient entityToCreate, ILogger logger)
        {
            if (_throws)
            {
                throw new Exception("Repository Error");
            }
            Ingredients.Add(entityToCreate);
            return Task.CompletedTask;
        }

        public Task<Ingredient> GetAsync(int id, ILogger logger)
        {
            if (_throws)
            {
                throw new Exception("Repository Error");
            }
            var queriedIngredients = Ingredients.Where(i => i.Id == id).ToArray();
            if (!queriedIngredients.Any())
            {
                throw new EntityNotFoundException($"Ingredient: {id} Not Found");
            }
            return Task.FromResult(queriedIngredients.First());
        }

        public Task<IEnumerable<Ingredient>> GetListAsync(ILogger logger)
        {
            if (_throws)
            {
                throw new Exception("Repository Error");
            }
            return Task.FromResult(Ingredients.AsEnumerable());
        }

        public Task UpdateAsync(Ingredient entityToUpdate, ILogger logger)
        {
            if (_throws)
            {
                throw new Exception("Repository Error");
            }

            var ingredient = Ingredients.FirstOrDefault(i => i.Id == entityToUpdate.Id);
            if (ingredient == null)
            {
                throw new EntityNotFoundException($"Ingredient: {entityToUpdate.Id} Not Found");
            }

            ingredient.Name = entityToUpdate.Name;
            ingredient.Description = entityToUpdate.Description;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id, ILogger logger)
        {
            if (_throws)
            {
                throw new Exception("Repository Error");
            }

            if (Ingredients.Remove(Ingredients.SingleOrDefault(i => i.Id == id)))
            {
                return Task.CompletedTask;
            }
            throw new EntityNotFoundException($"Ingredient: {id} Not Found");
        }
    }
}