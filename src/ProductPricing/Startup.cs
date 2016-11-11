using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProductPricing.Data;
using ProductPricing.Models;
using ProductPricing.Services;
using Microsoft.Data.Sqlite;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.FileProviders;

namespace ProductPricing
{
    public class Startup
    {
        IHostingEnvironment _env;
        public Startup(IHostingEnvironment env)
        {
            _env = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();

                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            //SQL Server 
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //SQLite 
            //services.AddDbContext<ABHIPricingDBContext>(options =>
            //    options.UseSqlite(Configuration.GetConnectionString("PricingConnection")));

            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();

            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            //Assembly.GetEntryAssembly()

            //Add Pricing Model DB Connection
            //var connection = @"Server=(localdb)\ProjectsV13;Database=ABHIPricingDB;Trusted_Connection=True;";
            //services.AddDbContext<ABHIPricingDBContext>(options => options.UseSqlServer(connection));

            //Add Pricing Model DB Connection 
            //services.AddDbContext<ABHIPricingDBContext>(options =>
            //     options.UseSqlServer (Configuration.GetConnectionString("PricingConnection")));

            var physicalProvider = _env.ContentRootFileProvider;
            //var embeddedProvider = new EmbeddedFileProvider(Assembly.GetEntryAssembly());
            //var compositeProvider = new CompositeFileProvider(physicalProvider, embeddedProvider);

            // choose one provider to use for the app and register it
            services.AddSingleton<IFileProvider>(physicalProvider);
            //services.AddSingleton<IFileProvider>(embeddedProvider);
            //services.AddSingleton<IFileProvider>(compositeProvider);

            //SQLite DB Connection 
            var PricingConnection = "Data Source=" + _env.ContentRootPath + "\\SQLite\\ABHIPricing.db;Mode=ReadOnly;Cache=Shared";

            services.AddDbContext<ABHIPricingDBContext>(options =>
                 options.UseSqlite(PricingConnection));


            //services.AddDbContext<ABHIPricingDBContext>(options =>
           //      options.UseSqlite(Configuration.GetConnectionString("PricingConnection")));

           // var connection = @"Filename=\\CPTWFP02\UserDocs\wwelbyso\My Documents\Visual Studio 2015\sqlite-tools-win32-x86-3150100\sqlite-tools-win32-x86-3150100\ABHIPricing.db";
           //services.AddDbContext<ABHIPricingDBContext>(options => options.UseSqlite(connection));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();
              
            //app.UseIdentity();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
