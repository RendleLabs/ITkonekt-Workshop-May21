using System;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Ingredients.Protos;
using Orders.Protos;
using Orders.PubSub;

namespace Orders.Services
{
    public class OrdersImpl : OrderService.OrderServiceBase
    {
        private readonly IngredientsService.IngredientsServiceClient _ingredients;
        private readonly IOrderPublisher _orderPublisher;
        private readonly IOrderMessages _orderMessages;

        public OrdersImpl(IngredientsService.IngredientsServiceClient ingredients,
            IOrderPublisher orderPublisher,
            IOrderMessages orderMessages)
        {
            _ingredients = ingredients;
            _orderPublisher = orderPublisher;
            _orderMessages = orderMessages;
        }

        public override async Task<PlaceOrderResponse> PlaceOrder(PlaceOrderRequest request, ServerCallContext context)
        {
            var time = DateTimeOffset.UtcNow.AddHours(0.5d);

            await _orderPublisher.PublishOrder(request.CrustId, request.ToppingIds, time);
            
            var decrementToppingsRequest = new DecrementToppingsRequest
            {
                ToppingIds = {request.ToppingIds}
            };
            await _ingredients.DecrementToppingsAsync(decrementToppingsRequest);

            var decrementCrustsRequest = new DecrementCrustsRequest
            {
                CrustId = request.CrustId
            };
            await _ingredients.DecrementCrustsAsync(decrementCrustsRequest);

            return new PlaceOrderResponse
            {
                Time = time.ToTimestamp()
            };
        }

        public override Task Subscribe(SubscribeRequest request, IServerStreamWriter<SubscribeResponse> responseStream, ServerCallContext context)
        {
            return base.Subscribe(request, responseStream, context);
        }
    }
}