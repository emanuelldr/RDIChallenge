using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RDI.ApplicationCore.Services;
using RDI.Domain.ApplicationCore.Interfaces;
using RDI.Domain.Token.Interfaces;
using RDI.Token.API.Helpers.Extensions;
using RDI.Token.Infrastructure.Repositories;

namespace RDIToken.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //Token Db context
            services.AddCustomDbContext(Configuration);

            //Swagger Components
            services
                .AddVersionedApiExplorer()
                .AddApiVersioning(o => { o.ReportApiVersions = true; o.AssumeDefaultVersionWhenUnspecified = true; })
                .AddCustomVersionedApiExplorer(Configuration)
                .AddSwagger(Configuration);

            //DI/Ioc
            services.AddTransient<IAbsoluteDifferenceService, AbsoluteDifferenceService>();
            services.AddTransient<IRotationService, RotationService>();
            services.AddTransient<ICreditCardRepository, CreditCardRepository>();
            services.AddTransient<ITokenService, TokenService>();
        }

        // This method gets called by the runtime.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();


            app
            .UseSwagger()
            .UseSwaggerUI(c =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint(
                            $"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                }

                c.DocumentTitle = "Token Api";
                c.RoutePrefix = string.Empty;
            });

        }
    }
}
