using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using RoxusWebAPI.Services.Zoho.ZohoProjects;
using RoxusZohoAPI.Contexts;
using RoxusZohoAPI.Repositories;
using RoxusZohoAPI.Services.CompaniesHouse;
using RoxusZohoAPI.Services.Hinets;
using RoxusZohoAPI.Services.MicrosoftGraph;
using RoxusZohoAPI.Services.Nimbus;
using RoxusZohoAPI.Services.ThorneWidgery;
using RoxusZohoAPI.Services.TrenchesReporting;
using RoxusZohoAPI.Services.Zoho;
using RoxusZohoAPI.Services.Zoho.ZohoCRM;
using Syncfusion.Licensing;
using System;

namespace RoxusZohoAPI
{
    public class Startup
    {
        private readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:3000", "https://roxus-ocr.web.app", "https://roxusdocmailapi.azurewebsites.net")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod();
                                  });
            });

            services.AddControllers(options =>
            {
                // Return a 406 when an unsupported media type was requested
                // options.ReturnHttpNotAcceptable = true;

                // Add XML formatters
                // options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                // options.InputFormatters.Add(new XmlSerializerInputFormatter(options));
            }).AddNewtonsoftJson(setupAction =>
            {
                setupAction.SerializerSettings.ContractResolver =
                   new CamelCasePropertyNamesContractResolver();
                setupAction.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            // add support for compressing responses (eg gzip)
            services.AddResponseCompression();

            // suppress automatic model state validation when using the 
            // ApiController attribute (as it will return a 400 Bad Request
            // instead of the more correct 422 Unprocessable Entity when
            // validation errors are encountered)
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            var roxusConnectionString = Configuration["ConnectionStrings:RoxusLogging"];
            var trenchesConnectionString = Configuration["ConnectionStrings:TrenchesReport"];

            services.AddDbContext<RoxusContext>(o => o.UseSqlServer(roxusConnectionString));
            services.AddDbContext<TrenchesContext>(o => o.UseSqlServer(trenchesConnectionString));

            services.AddScoped<IHinetsRepository, HinetsRepository>();
            services.AddScoped<IHinetsCrmService, HinetsCrmService>();
            services.AddScoped<IHinetsProjectsService, HinetsProjectsService>();
            services.AddScoped<IHinetsBooksService, HinetsBooksService>();
            services.AddScoped<IHinetsWriterService, HinetsWriterService>();
            services.AddScoped<IHinetsCustomService, HinetsCustomService>();
            services.AddScoped<IRoxusLoggingRepository, RoxusLoggingRepository>();
            services.AddScoped<ITWRepository, TWRepository>();
            services.AddScoped<ITWService, TWService>();
            services.AddScoped<IZohoCustomService, ZohoCustomService>();
            services.AddScoped<IZohoAuthService, ZohoAuthService>();
            services.AddScoped<IZohoContactService, ZohoContactService>();
            services.AddScoped<IZohoAccountService, ZohoAccountService>();
            services.AddScoped<IZohoUprnService, ZohoUprnService>();
            services.AddScoped<IZohoUsrnService, ZohoUsrnService>();
            services.AddScoped<IZohoTitleService, ZohoTitleService>();
            services.AddScoped<IZohoProjectService, ZohoProjectService>();
            services.AddScoped<IZohoTasklistService, ZohoTasklistService>();
            services.AddScoped<IZohoTaskService, ZohoTaskService>();
            services.AddScoped<INimbusService, NimbusService>();
            services.AddScoped<ITrenchesReportingRepository, TrenchesReportingRepository>();
            services.AddScoped<ITrenchesReportingService, TrenchesReportingService>();
            services.AddScoped<ICompaniesHouseService, CompaniesHouseService>();
            services.AddScoped<IZohoOpenreachService, ZohoOpenreachService>();
            services.AddScoped<IZohoNoteService, ZohoNoteService>();
            services.AddScoped<IMicrosoftGraphAuthService, MicrosoftGraphAuthService>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Roxus Zoho API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBMAY9C3t2VVhjQlFaclhJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxRd0ViW39bdHVWR2JVUU0=");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("swagger/v1/swagger.json", "Roxus Zoho API (v1)");
                    // serve UI at root
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseResponseCompression();

            app.UseSwagger();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
