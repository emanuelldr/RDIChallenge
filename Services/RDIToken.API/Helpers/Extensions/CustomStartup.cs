using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using RDI.Domain.DataContext;
using RDI.Token.API.Helpers.Defaults;
using RDIToken.API;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using System.Reflection;

namespace RDI.Token.API.Helpers.Extensions
{
    public static class CustomStartup
    {
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TokenContext>(x => x.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }


        public static IServiceCollection AddCustomVersionedApiExplorer(this IServiceCollection services, IConfiguration configuration)
        {
            services
               .AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                });

            return services;
        }



        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(
                opts =>
                {
                    using (var serviceProvider = services.BuildServiceProvider())
                    {
                        var provider = serviceProvider.GetRequiredService<IApiVersionDescriptionProvider>();

                        foreach (var description in provider.ApiVersionDescriptions)
                        {
                            opts.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                        }
                    }

                    opts.OperationFilter<SwaggerDefaultValues>();

                    opts.IncludeXmlComments(XmlCommentsFilePath);
                });

            return services;
        }

        static string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }

        static Info CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new Info()
            {
                Title = $"RDI Token API {description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
                Description = ".NET Core Rest API with SQLITE, EFCore and Self-Documented with Swagger",
                Contact = new Contact() { Name = "RDI/Emanuel Rocha", Email = "" },
                TermsOfService = "Shareware",
                License = new License() { Name = "EMANUEL ROCHA V" + Assembly.GetExecutingAssembly().GetName().Version, Url = "https://github.com/ZXVentures/code-challenge/blob/master/backend.md" }
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}
