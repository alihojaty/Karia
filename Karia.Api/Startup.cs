using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Karia.Api.DbContexts;
using Karia.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;


namespace Karia.Api
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
            services.AddControllers()
                .AddNewtonsoftJson(setupAction =>
                {
                    setupAction.SerializerSettings.ContractResolver =
                        new CamelCasePropertyNamesContractResolver();
                })
                .ConfigureApiBehaviorOptions(setupAction =>
                {
                    setupAction.InvalidModelStateResponseFactory = context =>
                    {
                        var problemDetailsFactory = context.HttpContext.RequestServices
                            .GetRequiredService<ProblemDetailsFactory>();
                        var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
                            context.HttpContext,
                            context.ModelState);

                        problemDetails.Detail = "See the errors field for details.";
                        problemDetails.Instance = context.HttpContext.Request.Path;

                        var actionExecutingContext =
                            context as Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;

                        if ((context.ModelState.Count > 0) &&
                            (actionExecutingContext?.ActionArguments.Count ==
                             context.ActionDescriptor.Parameters.Count))
                        {
                            problemDetails.Type = "htts://karia.com";
                            problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                            problemDetails.Title = "One or more validation errors occurred.";

                            return new UnprocessableEntityObjectResult(problemDetails)
                            {
                                ContentTypes ={"application/problem+json"}
                            };

                        }

                        problemDetails.Status = StatusCodes.Status400BadRequest;
                        problemDetails.Title = "One or more errors on input occurred.";
                        return new BadRequestObjectResult(problemDetails)
                        {
                            ContentTypes = {"application/problem+json"}
                        };

                    };
                });

            #region DbContext
            services.AddDbContext<KariaDbContext>(option =>
                option.UseSqlServer(Configuration.GetConnectionString("KariaDb")));
            
            #endregion

            #region AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
            #endregion

            #region Services

            services.AddScoped<IKariaRepository, KariaRepository>();

            #endregion

            #region ApiVersioning

            services.AddApiVersioning(setupAction =>
            {
                setupAction.DefaultApiVersion = new ApiVersion(1, 0);
                setupAction.AssumeDefaultVersionWhenUnspecified = true;
                setupAction.ApiVersionReader = new UrlSegmentApiVersionReader();
                setupAction.ReportApiVersions = true;
            });


            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}