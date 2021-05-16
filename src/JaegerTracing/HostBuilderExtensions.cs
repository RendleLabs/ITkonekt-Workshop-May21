using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace JaegerTracing
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder AddJaegerTracing(this IHostBuilder hostBuilder, params string[] sources)
        {
            string name = null;
            string host = null;
            int port = 0;
            ResourceBuilder resourceBuilder = null;

            hostBuilder.ConfigureLogging(((context, builder) =>
            {
                name = context.Configuration.GetValue<string>("Jaeger:ServiceName");
                host = context.Configuration.GetValue<string>("Jaeger:Host");
                port = context.Configuration.GetValue<int>("Jaeger:Port");

                if (name is {Length: >0} && host is {Length: > 0} && port > 0)
                {
                    resourceBuilder = ResourceBuilder.CreateDefault().AddService(name);
                    builder.AddOpenTelemetry(options =>
                    {
                        options.SetResourceBuilder(resourceBuilder);
                    });
                }
            }));

            if (resourceBuilder is not null)
            {
                hostBuilder.ConfigureServices(services =>
                {
                    services.AddOpenTelemetryTracing(builder =>
                    {
                        builder.SetResourceBuilder(resourceBuilder)
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
                    });
                });
            }

            return hostBuilder;
        }
    }
}
