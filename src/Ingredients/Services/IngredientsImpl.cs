using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Ingredients.Protos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Pizza.Data;

namespace Ingredients
{
    public class IngredientsImpl : IngredientsService.IngredientsServiceBase
    {
        private readonly IToppingData _toppingData;
        private readonly ILogger<IngredientsImpl> _logger;

        public IngredientsImpl(IToppingData toppingData, ILogger<IngredientsImpl> logger)
        {
            _toppingData = toppingData;
            _logger = logger;
        }

        public override async Task<GetToppingsResponse> GetToppings(GetToppingsRequest request, ServerCallContext context)
        {
            try
            {
                var toppings = await _toppingData.GetAsync(context.CancellationToken);
                var response = new GetToppingsResponse
                {
                    Toppings =
                    {
                        toppings.Select(t => new Topping
                        {
                            Id = t.Id,
                            Name = t.Name,
                            Price = t.Price
                        })
                    }
                };
                return response;
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Operation was cancelled.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error: {ex.Message}");
                throw;
            }
        }
    }
}