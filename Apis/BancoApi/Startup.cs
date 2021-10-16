using BancoApi.Domain.Interfaces.Refit;
using BancoApi.Healthchecks;
using BancoApi.Middlewares;
using BancoApi.Service;
using BancoApi.Service.Configurations;
using BancoApi.Service.Receiver;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Refit;
using System;

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
            //services.AddHealthChecks();
            services.AddOptions();
            services.AddTransient<ErroMiddleware>();

            services.AddRefitClient<IPostalCode>()
                    .ConfigureHttpClient(c =>
                    c.BaseAddress = new Uri("https://viacep.com.br/ws"));
            services.AddHttpContextAccessor();
            services.AddControllers();
            //.AddFluentValidation(fv =>
            //{
            //    fv.RegisterValidatorsFromAssemblyContaining<Startup>();
            //    fv.ValidatorOptions.LanguageManager.Culture = new CultureInfo("pt-BR");
            //});

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BancoApi", Version = "v1" });
            });

            var serviceClientSettingsConfig = Configuration.GetSection("RabbitMqConfiguration");
            var serviceClientSettings = serviceClientSettingsConfig.Get<RabbitMqConfiguration>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var actionExecutingContext =
                        actionContext as ActionExecutingContext;

                    if (actionContext.ModelState.ErrorCount > 0
                        && actionExecutingContext?.ActionArguments.Count == actionContext.ActionDescriptor.Parameters.Count)
                    {
                        return new UnprocessableEntityObjectResult(actionContext.ModelState);
                    }

                    return new BadRequestObjectResult(actionContext.ModelState);
                };
            });

            //services.AddMediatR(typeof(Startup));
            services.AddServiceIoC(Configuration);
            services.AddCustomHealthchecks();

            if (serviceClientSettings.Enable)
            {
                services.AddHostedService<BancoCreateReceiver>();
            }



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
            app.UseMiddleware<ErroMiddleware>();
            app.UseAuthorization();

            app.UseHealthChecks();
            app.UserHealthCheckUi();



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/monitor");
            });
        }
    }
}
