using BancoApi.Data.Database;
using BancoApi.Data.Repository;
using BancoApi.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace BancoApi.Data
{
    public static class Startup
    {
        public static IServiceCollection AddInfraDataIoC(this IServiceCollection services)
        {
            services.AddDbContext<BancoDbContext>(options =>
               options.UseSqlServer(
                   DatabaseConnection.ConnectionConfiguration
                   .GetConnectionString("DbConnection"),
                   //Problemas de erro transitórios podem ser causados devido a problemas de conectividade de rede ou
                   //à indisponibilidade temporária de um serviço, ou problemas de tempo limite, etc.
                   //Um aplicativo que usa recursos compartilhados em um ambiente compartilhado
                   //especialmente no ambiente de nuvem é mais sensível a falhas transitórias.
                   //https://www.thecodebuzz.com/enabling-transient-error-resiliency-enableretryonfailure/
                   sqlServerOptionsAction: sqlOptions =>
                   {
                       sqlOptions.EnableRetryOnFailure(

                           maxRetryCount: 10,
                           maxRetryDelay: TimeSpan.FromSeconds(130),
                           errorNumbersToAdd: null);
                   }
                   ));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IBancoRepository, BancoRepository>();

            return services;
        }
    }

    internal class DatabaseConnection
    {
        public static IConfiguration ConnectionConfiguration
        {
            get
            {
                IConfigurationRoot Configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile("appsettings.Development.json")
                    .Build();
                return Configuration;
            }
        }
    }
}
