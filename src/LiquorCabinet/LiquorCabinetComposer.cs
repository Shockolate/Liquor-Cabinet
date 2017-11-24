using System.Collections.Generic;
using LiquorCabinet.PathHandlers.v1.glassware;
using LiquorCabinet.Repositories.Glasses;
using LiquorCabinet.Repositories.Ingredients;
using LiquorCabinet.Repositories.Recipes;
using Microsoft.Extensions.Configuration;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet
{
    public static class LiquorCabinetComposer
    {
        public static IEnumerable<IHttpPathHandler> CreatePathHandlers(IHttpPathHandlerFactory pathHandlerFactory, IPayloadSerializer payloadSerializer,
            IConfiguration configuration)
        {
            configuration.Bind(Settings.Settings.Instance);

            var restResponseFactory = new RestResponseFactory();

            #region Glassware Handlers

            var glasswareRepository = new GlassRepository();
            var glasswareHandler = new Handler(restResponseFactory, payloadSerializer, glasswareRepository);

            #endregion

            #region Ingredients Handlers

            var ingredientsRepository = new IngredientRepository();
            var ingredientsHandler = new PathHandlers.v1.ingredients.Handler(restResponseFactory, payloadSerializer, ingredientsRepository);
            var ingredientsIngredientsIdHandler = new PathHandlers.v1.ingredients.ingredientId.Handler(restResponseFactory, payloadSerializer, ingredientsRepository);

            #endregion

            #region Recipes Handlers

            var recipeRepository = new RecipeRepository();
            var recipesHandler = new PathHandlers.v1.recipes.Handler(restResponseFactory, payloadSerializer, recipeRepository);
            var recipesRecipeIdHandler = new PathHandlers.v1.recipes.recipeId.Handler(restResponseFactory, payloadSerializer);
            var recipesRecipeIdComponentsHandler = new PathHandlers.v1.recipes.recipeId.components.Handler(restResponseFactory, payloadSerializer);
            var recipesRecipeIdComponentsComponentIdHandler =
                new PathHandlers.v1.recipes.recipeId.components.componentId.Handler(restResponseFactory, payloadSerializer);

            #endregion

            return new List<IHttpPathHandler>
            {
                pathHandlerFactory.CreateHttpPathHandler("/v1/glassware", glasswareHandler.VerbHandlers),
                pathHandlerFactory.CreateHttpPathHandler("/v1/ingredients", ingredientsHandler.VerbHandlers),
                pathHandlerFactory.CreateHttpPathHandler("/v1/ingredients/{ingredientId}", ingredientsIngredientsIdHandler.VerbHandlers),
                pathHandlerFactory.CreateHttpPathHandler("/v1/recipes", recipesHandler.VerbHandlers),
                pathHandlerFactory.CreateHttpPathHandler("/v1/recipes/{recipeId}", recipesRecipeIdHandler.VerbHandlers),
                pathHandlerFactory.CreateHttpPathHandler("/v1/recipes/{recipeId}/components", recipesRecipeIdComponentsHandler.VerbHandlers),
                pathHandlerFactory.CreateHttpPathHandler("/v1/recipes/{recipeId}/components/{componentId}",
                    recipesRecipeIdComponentsComponentIdHandler.VerbHandlers)
            };
        }
    }
}