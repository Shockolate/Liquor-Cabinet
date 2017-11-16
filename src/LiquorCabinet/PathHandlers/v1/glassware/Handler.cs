using System;
using System.Threading.Tasks;
using LiquorCabinet.Repositories;
using LiquorCabinet.Repositories.Entities;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.PathHandlers.v1.glassware
{
    internal sealed class Handler : AbstractPathHandler
    {
        private readonly ICrudRepository<Glass, int> _glasswareCrudRepository;

        public Handler(RestResponseFactory restResponseFactory, IPayloadSerializer payloadSerializer, ICrudRepository<Glass, int> glasswareCrudRepository) :
            base(restResponseFactory, payloadSerializer)
        {
            _glasswareCrudRepository = glasswareCrudRepository;
            VerbHandlers.Add(HttpVerb.Get, GetAsync);
            VerbHandlers.Add(HttpVerb.Post, PostAsync);
        }

        public async Task<RestResponse> GetAsync(RestRequest request, ILogger logger)
        {
            var glasswareList = await _glasswareCrudRepository.GetListAsync(logger);
            var response = RestResponseFactory.CreateCorsRestResponse(200);
            response.Body = glasswareList;
            return response;
        }

        public async Task<RestResponse> PostAsync(RestRequest request, ILogger logger)
        {
            Glass newGlass;
            try
            {
                newGlass = PayloadSerializer.DeserializePayload<Glass>(request.Body);
            }
            catch (Exception e)
            {
                logger.LogError(() => $"Error deserializing Glass: #{e.Message}");
                return RestResponseFactory.CreateErrorMessageRestResponse("Malformed Body.", 400);
            }

            try
            {
                await _glasswareCrudRepository.InsertAsync(newGlass, logger);
            }
            catch (Exception e)
            {
                logger.LogDebug(() => $"Error inserting into repository: ${e.Message}");
                return RestResponseFactory.CreateErrorMessageRestResponse("Internal Server Error: Persistence error.", 500);
            }

            return RestResponseFactory.CreateCorsRestResponse(201);
        }
    }
}