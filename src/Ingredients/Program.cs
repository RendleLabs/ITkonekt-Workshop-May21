using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AuthHelp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Hosting;

namespace Ingredients
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    if (OperatingSystem.IsMacOS())
                    {
                        webBuilder.ConfigureKestrel(options =>
                        {
                            options.ListenLocalhost(5003, o => o.Protocols = HttpProtocols.Http2);
                        });
                    }

                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.ConfigureHttpsDefaults(defaults =>
                        {
                            var serverCert = ServerCert.Get();
                            defaults.ServerCertificate = serverCert;
                            defaults.ClientCertificateMode = ClientCertificateMode.RequireCertificate;
                            defaults.ClientCertificateValidation = (clientCert, _, _) =>
                                clientCert.Issuer == serverCert.Issuer;
                        });
                    });
                    
                    webBuilder.UseStartup<Startup>();
                });
    }
}
