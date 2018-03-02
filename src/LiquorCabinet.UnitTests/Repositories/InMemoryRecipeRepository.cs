using LiquorCabinet.Models;
using LiquorCabinet.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace LiquorCabinet.UnitTests.Repositories
{
    internal class InMemoryRecipeRepository : BaseInMemoryRepository<int, Recipe>, IRecipeRepository
    {
        private readonly Recipe _alexander = new Recipe
        {
            Id = 1,
            Name = "Alexander",
            Instructions = "Make an Alexander",
            Glassware = new Glass {Id = 4, Description = "A Cocktail Glass", Name = "Cocktail Glass", TypicalSize = "4 oz"},
            Components = new List<RecipeComponent>
            {
                new RecipeComponent
                {
                    Id = 292,
                    ComponentId = 20,
                    ComponentName = "Cognac",
                    RecipeId = 1,
                    QuantityPart = "1 Part",
                    QuantityMetric = 3.0,
                    QuantityImperial = 1.0
                },
                new RecipeComponent
                {
                    Id = 293,
                    ComponentId = 16,
                    ComponentName = "Chocolate Liqueur",
                    RecipeId = 1,
                    QuantityPart = "1 Part",
                    QuantityMetric = 3.0,
                    QuantityImperial = 1.0
                },
                new RecipeComponent
                {
                    Id = 294,
                    ComponentId = 23,
                    ComponentName = "Cream",
                    RecipeId = 1,
                    QuantityPart = "1 Part",
                    QuantityMetric = 3.0,
                    QuantityImperial = 1.0
                }
            }
        };

        private readonly Recipe _americano = new Recipe
        {
            Id = 2,
            Name = "Americano",
            Instructions = "Make an Americano",
            Glassware = new Glass {Id = 12, Name = "Old fashioned", Description = "A rocks glass", TypicalSize = "8-10oz"},
            Components = new List<RecipeComponent>
            {
                new RecipeComponent
                {
                    Id = 295,
                    ComponentId = 13,
                    ComponentName = "Campari",
                    RecipeId = 2,
                    QuantityPart = "1 Part",
                    QuantityMetric = 3.0,
                    QuantityImperial = 1.5
                },
                new RecipeComponent
                {
                    Id = 296,
                    ComponentId = 76,
                    ComponentName = "Sweet Vermouth",
                    QuantityPart = "1 Part",
                    RecipeId = 2,
                    QuantityMetric = 3.0,
                    QuantityImperial = 1.5
                },
                new RecipeComponent
                {
                    Id = 297,
                    ComponentId = 70,
                    ComponentName = "Soda",
                    RecipeId = 2,
                    QuantityPart = "Splash",
                    QuantityMetric = null,
                    QuantityImperial = null
                }
            }
        };

        public readonly IDictionary<int, Recipe> Recipes;

        public InMemoryRecipeRepository() : this(false) { }

        public InMemoryRecipeRepository(bool throws) : base(throws)
        {
            Recipes = new Dictionary<int, Recipe> {{_alexander.Id, _alexander}, {_americano.Id, _americano}};
        }

        public Task<IEnumerable<Recipe>> GetRecipeListForUserAsync(int userId) => throw new NotImplementedException();

        public Task AddComponentsToRecipeAsync(int recipeId, IEnumerable<RecipeComponent> newRecipeComponents)
        {
            ThrowCheck();
            if (!Recipes.ContainsKey(recipeId))
            {
                throw new EntityNotFoundException("Recipe", recipeId);
            }
            var newRecipeComponentsArray = newRecipeComponents.ToArray();
            if (newRecipeComponentsArray.Length == 0)
            {
                return Task.CompletedTask;
            }
            var recipeToAddTo = Recipes[recipeId];
            var highestComponentId = recipeToAddTo.Components.Max(rc => rc.Id);

            foreach (var recipeComponent in newRecipeComponentsArray)
            {
                if (recipeComponent.ComponentId == 99)
                {
                    throw new EntityNotFoundException("Component", recipeComponent.ComponentId);
                }
                recipeComponent.Id = ++highestComponentId;
            }

            var existingRecipeComponentsCount = recipeToAddTo.Components.Count;
            var newRecipeComponentCount = newRecipeComponentsArray.Length;
            ISet<int> set = new HashSet<int>(recipeToAddTo.Components.Select(c => c.ComponentId));
            foreach (var recipeComponent in newRecipeComponentsArray)
            {
                set.Add(recipeComponent.ComponentId);
            }

            if (existingRecipeComponentsCount + newRecipeComponentCount != set.Count)
            {
                throw new ArgumentException($"Cannot add a new RecipeComponent when one exists with given ComponentId:");
            }

            foreach (var recipeComponent in newRecipeComponentsArray)
            {
                recipeToAddTo.Components.Add(recipeComponent);
            }

            return Task.CompletedTask;
        }

        protected override Task DoInsertAsync(Recipe entityToCreate)
        {
            Recipes.Add(entityToCreate.Id, entityToCreate);
            return Task.CompletedTask;
        }

        protected override Task DoInsertListAsync(IEnumerable<Recipe> entitesToCreate) => throw new NotImplementedException();

        protected override Task<Recipe> DoGetAsync(int id)
        {
            if (Recipes.ContainsKey(id))
            {
                return Task.FromResult(Recipes[id]);
            }
            throw new EntityNotFoundException("Recipe", id);
        }

        protected override Task<IEnumerable<Recipe>> DoGetListAsync() => Task.FromResult(Recipes.Select(kvp => kvp.Value));

        protected override Task DoUpdateAsync(Recipe entityToUpdate)
        {
            if (Recipes.ContainsKey(entityToUpdate.Id))
            {
                Recipes[entityToUpdate.Id] = entityToUpdate;
                return Task.CompletedTask;
            }
            throw new EntityNotFoundException("Recipe", entityToUpdate.Id);
        }

        protected override Task DoDeleteAsync(int id) => throw new NotImplementedException();
    }
}