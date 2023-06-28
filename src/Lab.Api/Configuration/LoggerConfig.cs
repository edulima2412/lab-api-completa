using Elmah.Io.Extensions.Logging;
using HealthChecks.UI.Client;
using Lab.Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Lab.Api.Configuration
{
    public static class LoggerConfig
    {
        public static IServiceCollection AddLoggingConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddElmahIo(o =>
            {
                o.ApiKey = "0b42d3c76ca54ac9b962632f946b675e";
                o.LogId = new Guid("d46c7dc1-6469-4f7e-bd68-cf49e4d12ced");
            });

            //services.AddLogging(builder =>
            //{
            //    builder.AddElmahIo(o =>
            //    {
            //        o.ApiKey = "0b42d3c76ca54ac9b962632f946b675e";
            //        o.LogId = new Guid("d46c7dc1-6469-4f7e-bd68-cf49e4d12ced");
            //    });
            //    builder.AddFilter<ElmahIoLoggerProvider>(null, LogLevel.Warning);
            //});

            services.AddHealthChecks()
                .AddElmahIoPublisher(opt =>
                {
                    opt.ApiKey = "0b42d3c76ca54ac9b962632f946b675e";
                    opt.LogId = new Guid("d46c7dc1-6469-4f7e-bd68-cf49e4d12ced");
                    opt.Application = "API Fornecedores";
                    opt.HeartbeatId = "2429ed07897b4d6fb55ec0623a29d7f4";
                })
                .AddCheck("Produtos", new SqlServerHealthCheck(configuration.GetConnectionString("DefaultConnection")))
                .AddSqlServer(configuration.GetConnectionString("DefaultConnection"), name: "BancoSQL");

            services
                .AddHealthChecksUI()
                .AddInMemoryStorage();

            return services;
        }

        public static IApplicationBuilder UseLoggingConfig(this IApplicationBuilder app)
        {
            app.UseElmahIo();

            app.UseHealthChecks("/api/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI(options =>
            {
                options.UIPath = "/api/hc-ui";
                options.ResourcesPath = "/api/hc-ui-resources";

                options.UseRelativeApiPath = false;
                options.UseRelativeResourcesPath = false;
                options.UseRelativeWebhookPath = false;
            });

            return app;
        }
    }
}
