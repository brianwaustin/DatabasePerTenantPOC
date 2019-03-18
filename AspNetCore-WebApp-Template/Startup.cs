using DatabasePerTenantPOC.Data;
using DatabasePerTenantPOC.Data.CatalogDB;
using DatabasePerTenantPOC.Interfaces;
using DatabasePerTenantPOC.Services;
using DatabasePerTenantPOC.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DnsClient;
using DatabasePerTenantPOC.Repositories;
using System;
using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;

namespace DatabasePerTenantPOC
{
    public class Startup
    {
        static string errorPage = "/Error";
        static string authorizationFolder = "/Account/Manage";
        static string authorizationPage = "/Account/Logout";
        static bool useInMemory = false;
        private IUtilities _utilities;
        private ICatalogRepository _catalogRepository;
        private ITenantRepository _tenantRepository;
        private ILookupClient _client;

        public static DatabaseConfig DatabaseConfig { get; set; }
        public static CatalogConfig CatalogConfig { get; set; }
        public static TenantServerConfig TenantServerConfig { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            //read config settigs from appsettings.json
            ReadAppConfig();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (useInMemory)
                services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("name"));
            else
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddLogging();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
                        
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder(authorizationFolder);
                    options.Conventions.AuthorizePage(authorizationPage);
                });

            //register catalog DB
            //services.AddDbContext<CatalogDbContext>(options => options.UseSqlServer(GetCatalogConnectionString(CatalogConfig, DatabaseConfig)));
            services.AddDbContext<CatalogDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //Add Application services
            services.AddTransient<ICatalogRepository, CatalogRepository>();
            //services.AddTransient<ITenantRepository, TenantRepository>();
            //services.AddSingleton<ITenantRepository>(p => new TenantRepository(GetBasicSqlConnectionString()));
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<ILookupClient>(p => new LookupClient());

            // Register no-op EmailSender used by account confirmation and password reset during development
            // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
            services.AddSingleton<IEmailSender, EmailSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        { 
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler(errorPage);
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        /// <summary>
        ///  Gets the catalog connection string using the app settings
        /// </summary>
        /// <param name="catalogConfig">The catalog configuration.</param>
        /// <param name="databaseConfig">The database configuration.</param>
        /// <returns></returns>
        private string GetCatalogConnectionString(CatalogConfig catalogConfig, DatabaseConfig databaseConfig)
        {
            return
                $"Server=tcp:{catalogConfig.CatalogServer},1433;Database={catalogConfig.CatalogDatabase};User ID={databaseConfig.DatabaseUser};Password={databaseConfig.DatabasePassword};Trusted_Connection=False;Encrypt=True;";
        }

        /// <summary>
        /// Reads the application settings from appsettings.json
        /// </summary>
        private void ReadAppConfig()
        {
            DatabaseConfig = new DatabaseConfig
            {
                DatabasePassword = Configuration["DatabasePassword"],
                DatabaseUser = Configuration["DatabaseUser"],
                DatabaseServerPort = Convert.ToInt32(Configuration["DatabaseServerPort"]),
                SqlProtocol = SqlProtocol.Tcp,
                ConnectionTimeOut = Convert.ToInt32(Configuration["ConnectionTimeOut"]),
                LearnHowFooterUrl = Configuration["LearnHowFooterUrl"]
            };

            CatalogConfig = new CatalogConfig
            {
                ServicePlan = Configuration["ServicePlan"],
                CatalogDatabase = Configuration["CatalogDatabase"],
                CatalogServer = Configuration["CatalogServer"] + ".database.windows.net",
                CatalogLocation = Configuration["APP_REGION"]
            };

            TenantServerConfig = new TenantServerConfig
            {
                TenantServer = Configuration["TenantServer"] + ".database.windows.net"
            };

            bool isResetEventDatesEnabled = false;
            if (bool.TryParse(Configuration["ResetEventDates"], out isResetEventDatesEnabled))
            {
                TenantServerConfig.ResetEventDates = isResetEventDatesEnabled;
            }
        }
    }
}
