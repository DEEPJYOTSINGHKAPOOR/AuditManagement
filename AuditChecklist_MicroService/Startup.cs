using AuditChecklist_MicroService.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuditChecklist_MicroService.Repository;
using AuditChecklist_MicroService.Repository.IRepository;
using AuditChecklist_MicroService.Provider;
using System.Reflection;
using System.IO;
using AuditChecklist_MicroService.Services;

namespace AuditChecklist_MicroService
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
            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer());
            //services.AddDbContext<ApplicationDbContext>
            //    (options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            services.Configure<MyAppSettings>(Configuration.GetSection(MyAppSettings.SectionName));
            services.AddOptions();


            services.AddScoped<IAuditChecklistProvider, AuditChecklistProvider>();
            services.AddScoped<IAuditChecklistRepository, AuditChecklistRepository>();

            services.AddHttpClient();





            services.AddAutoMapper(typeof(AuditChecklistMappings));
            services.AddSwaggerGen(options => {
                options.SwaggerDoc("AuditCheckListOpenApiSpec",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "AuditCheckList Api",
                        Version = "1",
                        Description = "Audit Checklist will be triggered from Web APP"

                    }
                    );
                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

                options.IncludeXmlComments(cmlCommentsFullPath);
            });

            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint("/swagger/AuditCheckListOpenApiSpec/swagger.json", "AuditCheckListOpenApiSpec");
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
