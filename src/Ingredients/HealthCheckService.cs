using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Health.V1;
using Grpc.HealthCheck;
using Microsoft.Extensions.Hosting;
using Pizza.Data;

namespace Ingredients
{
    public class HealthCheckService : BackgroundService
    {
        private readonly IToppingData _topping;
        private readonly HealthServiceImpl _service;

        public HealthCheckService(IToppingData topping, HealthServiceImpl service)
        {
            _topping = topping;
            _service = service;
            service.SetStatus("IngredientsService", HealthCheckResponse.Types.ServingStatus.NotServing);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var _ = await _topping.GetAsync(stoppingToken);
                    _service.SetStatus("IngredientsService", HealthCheckResponse.Types.ServingStatus.Serving);
                }
                catch (Exception ex)
                {
                    _service.SetStatus("IngredientsService", HealthCheckResponse.Types.ServingStatus.NotServing);
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}