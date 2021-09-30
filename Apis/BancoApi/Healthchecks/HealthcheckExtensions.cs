using BancoApi.Healthchecks.Custom;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;

namespace BancoApi.Healthchecks
{
    /// <summary>
    /// Extensao para healthchecks customizados
    /// </summary>
    public static class HealthcheckExtensions
    {
        public static IServiceCollection AddCustomHealthchecks(this IServiceCollection services)
        {
            services.AddHealthChecks()
                  .AddCheck<CustomSelfHealthchecks>("Self")
                  .AddCheck("Servidor do Banco de dados", new PingHealthCheck("127.0.0.1", 100))
                  .AddSqlServer(connectionString: "Data Source=.\\sqlexpress;Initial Catalog=Financeiro;Integrated Security=True;", 
                  name: "Instancia do Sql Server",
                  healthQuery: "SELECT 1;",
                  failureStatus: HealthStatus.Degraded);
            
            #region healthcheckUI
            services.AddHealthChecksUI(setupSettings: setup =>
            {
                setup.SetEvaluationTimeInSeconds(60);
                setup.MaximumHistoryEntriesPerEndpoint(50);
            }).AddInMemoryStorage();
            #endregion

            return services;
        }

        public static void UseHealthChecks(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/app-status");
            app.UseHealthChecks("/app-status-json",
                new HealthCheckOptions()
                {
                    ResponseWriter = async (context, report) =>
                    {
                        var result = JsonSerializer.Serialize(

                                 new HealthcheckInformation
                                 {
                                     Name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                     Version = "V1",
                                     Data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                     Status = report.Status.ToString(),
                                    
                                 }
                            );

                        context.Response.ContentType = MediaTypeNames.Application.Json;
                        await context.Response.WriteAsync(result);
                    }
                    
                });
        }

        public static void UserHealthCheckUi(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/healthchecks-data-ui", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,

            });

            // Ativa o dashboard para a visualização da situação de cada Health Check
            app.UseHealthChecksUI(options =>
            {
                options.UIPath = "/monitor";
               
                
            });
        }
    }
}
