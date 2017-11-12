using System.Collections.Generic;
using LiquorCabinet.PathHandlers.v1.glassware;
using Microsoft.Extensions.Configuration;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet
{
    public static class LiquorCabinetComposer
    {
        public static IEnumerable<IHttpPathHandler> CreatePathHandlers(IHttpPathHandlerFactory pathHandlerFactory, IPayloadSerializer payloadSerializer,
            IConfiguration configuration)
        {
            // Use Configuration.
            var restResponseFactory = new RestResponseFactory();

            #region Glassware Handlers

            var glasswareHandler = new Handler(restResponseFactory);

            #endregion

            #region Ingredients Handlers

            var ingredientsHandler = new PathHandlers.v1.ingredients.Handler(restResponseFactory);
            var ingredientsIngredientsIdHandler = new PathHandlers.v1.ingredients.ingredientId.Handler(restResponseFactory);

            #endregion

            #region Recipes Handlers

            var recipesHandler = new PathHandlers.v1.recipes.Handler(restResponseFactory);
            var recipesRecipeIdHandler = new PathHandlers.v1.recipes.recipeId.Handler(restResponseFactory);
            var recipesRecipeIdComponentsHandler = new PathHandlers.v1.recipes.recipeId.components.Handler(restResponseFactory);
            var recipesRecipeIdComponentsComponentIdHandler = new PathHandlers.v1.recipes.recipeId.components.componentId.Handler(restResponseFactory);

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