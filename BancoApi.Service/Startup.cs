using BancoApi.Data;
using BancoApi.Domain;
using BancoApi.Service.Command;
using BancoApi.Service.Configurations;
using BancoApi.Service.Notification;
using BancoApi.Service.Query;
using BancoApi.Service.Send;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace BancoApi.Service
{
    public static class Startup
    {
        public static IServiceCollection AddServiceIoC(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddInfraDataIoC();
            services.AddMediatR(typeof(Startup));
            services.AddTransient<IRequestHandler<CreateBancoMessageCommand, Unit>, CreateBancoMessageHandle>();

            services.AddTransient<IRequestHandler<GetBancoByIdQuery, Banco>, GetBancoByIdQueryHandle>();
            services.AddTransient<IRequestHandler<GetBancoAllQuery, List<Banco>>, GetBancoAllQueryHandle>();

            services.AddTransient<IBancoCreateSender, BancoCreateSender>();
            services.AddScoped<IApiNotification, ApiNotification>();
            //options Pattern
            //install Package Microsoft.Extensions.options
            services.Configure<RabbitMqConfiguration>(configuration.GetSection(RabbitMqConfiguration.BaseConfig));

            return services;
        }
    }
}
