using AuditSeverity_MicroService.Logger;
using AuditSeverity_MicroService.Repository;
using AuditSeverity_MicroService.Repository.IRepository;
using AuditSeverity_MicroService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AuditSeverity_MicroService
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

            services.AddScoped<IAuditSeverityRepository, AuditSeverityRepository>();


            services.AddOptions();

            services.Configure<MyAppSettings>(Configuration.GetSection(MyAppSettings.SectionName));

            services.AddHttpClient();

            services.AddAutoMapper(typeof(AuditSeverityMappings));
            services.AddControllers();
            services.AddHttpClient();

            services.AddSwaggerGen(options=> {
                options.SwaggerDoc("AuditSeverityOpenApiSpec",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title ="AuditSeverity Api",
                        Version = "1",
                        Description = "Audit Severity will be triggered from Web APP"

                    }
                    );
                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

                options.IncludeXmlComments(cmlCommentsFullPath);
            });
            

            //services.AddSession(options =>
            //{
            //    // Set a short timeout for easy testing.
            //    options.IdleTimeout = TimeSpan.FromMinutes(10);
            //    options.Cookie.HttpOnly = true;
            //    // Make the session cookie essential
            //    options.Cookie.IsEssential = true;
            //});

             services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<ILoggerManager, LoggerManager>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env ,ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();



            app.UseHttpsRedirection();


            app.UseSwagger();
            app.UseSwaggerUI(options=> {
                options.SwaggerEndpoint("/swagger/AuditSeverityOpenApiSpec/swagger.json","AuditSeverityApi");
                options.RoutePrefix = "";
            });



            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
