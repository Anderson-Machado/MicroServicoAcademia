using BancoApi.Data;
using BancoApi.Domain;
using BancoApi.Service.Command;
using BancoApi.Service.Query;
using BancoApi.Service.Send;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace BancoApi.Service
{
    public static class Startup
    {
        public static IServiceCollection AddServiceIoC(this IServiceCollection services)
        {
            services.AddInfraDataIoC();
            services.AddMediatR(typeof(Startup));
            services.AddTransient<IRequestHandler<CreateBancoMessageCommand, Unit>, CreateBancoMessageHandle>();

            services.AddTransient<IRequestHandler<GetBancoByIdQuery, Banco>, GetBancoByIdQueryHandle>();
            services.AddTransient<IRequestHandler<GetBancoAllQuery, List<Banco>>, GetBancoAllQueryHandle>();

            services.AddTransient<IBancoCreateSender, BancoCreateSender>();


            return services;
        }
    }
}
