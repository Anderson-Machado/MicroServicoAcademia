using BancoApi.Domain;
using BancoApi.Message.Command;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BancoApi.Message
{
    public static class Startup
    {
        public static IServiceCollection AddMessageDataIoC(this IServiceCollection services)
        {
            services.AddTransient<IRequestHandler<CreateBancoCommand, Banco>, CreateBancoCommandHandle>();
            return services;
        }
    }

}
