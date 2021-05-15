using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orders.Protos;

namespace ShopConsole
{
    public class Worker : BackgroundService
    {
        private readonly OrderService.OrderServiceClient _orders;
        private readonly ILogger<Worker> _logger;

        public Worker(OrderService.OrderServiceClient orders, ILogger<Worker> logger)
        {
            _orders = orders;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var call = _orders.Subscribe(new SubscribeRequest(), cancellationToken: stoppingToken);
                
                await foreach (var response in call.ResponseStream.ReadAllAsync(stoppingToken))
                {
                    Console.WriteLine($"Order received: {response.CrustId}");
                    foreach (var toppingId in response.ToppingIds)
                    {
                        Console.WriteLine($"    {toppingId}");
                    }

                    Console.WriteLine($"Due by: {response.Time.ToDateTimeOffset():t}");
                }
            }
        }
    }
}
