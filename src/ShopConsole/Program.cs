using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orders.Protos;

namespace ShopConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddGrpcClient<OrderService.OrderServiceClient>(options =>
                    {
                        options.Address = new Uri("https://localhost:5005");
                    });
                    services.AddHostedService<Worker>();
                });
    }
}
