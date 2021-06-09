using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Karia.Api.DbContexts;
using Karia.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


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
            services.AddControllers();

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