using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace JaegerTracing
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJaegerTracing(this IServiceCollection services,
            IConfiguration config,
            params string[] sources)
        {
            services.AddOpenTelemetryTracing(builder =>
            {
                var name = config.GetValue<string>("Jaeger:ServiceName");
                var host = config.GetValue<string>("Jaeger:Host");
                var port = config.GetValue<int>("Jaeger:Port");

                if (name is {Length: > 0} && host is {Length: > 0} && port > 0)
                {
                    builder.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(name))
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation();

                    if (sources.Length > 0)
                    {
                        builder.AddSource(sources);
                    }

                    builder.AddJaegerExporter(options =>
                    {
                        options.AgentHost = host;
                        options.AgentPort = port;
                    });
                }
            });

            return services;
        } 
    }
}