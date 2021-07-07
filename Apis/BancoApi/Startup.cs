using BancoApi.Data;
using BancoApi.Domain;
using BancoApi.Message;
using BancoApi.Message.Send;
using BancoApi.Service.Command;
using BancoApi.Service.Query;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace BancoApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BancoApi", Version = "v1" });
            });
            
            services.Configure<RabbitMqConfiguration>(Configuration.GetSection("RabbitMq"));
            services.AddMediatR(typeof(Startup));
            services.AddInfraDataIoC();

            services.AddTransient<IBancoCreateSender, BancoCreateSender>();
            services.AddTransient<IRequestHandler<CreateBancoMessageCommand, Unit>,CreateBancoMessageHandle>();
            services.AddTransient<IRequestHandler<GetBancoByIdQuery, Banco>, GetBancoByIdQueryHandle>();
            services.AddTransient<IRequestHandler<GetBancoAllQuery, List<Banco>>, GetBancoAllQueryHandle>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BancoApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
