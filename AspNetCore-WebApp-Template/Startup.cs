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
using System.Data.SqlClient;
using DatabasePerTenantPOC.Data.TenantDB;

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
        private ICustomerRepository _customerRepository;
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

            ///* Temporary code to allow EF Core to apply code first changes */           
            //services.AddDbContext<CustomerDbContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("TenantDbConnection2")));

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
            services.AddDbContext<CatalogDbContext>(options => options.UseSqlServer(GetCatalogConnectionString(CatalogConfig, DatabaseConfig)));
            
            //Add Application services
            services.AddTransient<ICatalogRepository, CatalogRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddSingleton<ICustomerRepository>(p => new CustomerRepository(GetBasicSqlConnectionString()));
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<ILookupClient>(p => new LookupClient());

            //create instance of utilities class
            services.AddTransient<IUtilities, Utilities.Utilities>();
            var provider = services.BuildServiceProvider();
            _utilities = provider.GetService<IUtilities>();
            _catalogRepository = provider.GetService<ICatalogRepository>();
            _customerRepository = provider.GetService<ICustomerRepository>();
            _client = provider.GetService<ILookupClient>();

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

            //shard management
            InitialiseShardMapManager();
            _utilities.RegisterTenantShard(TenantServerConfig, DatabaseConfig, CatalogConfig, TenantServerConfig.ResetEventDates);
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
                $"Server=tcp:{catalogConfig.CatalogServer},{databaseConfig.DatabaseServerPort};Database={catalogConfig.CatalogDatabase};User ID={databaseConfig.DatabaseUser};Password={databaseConfig.DatabasePassword};Trusted_Connection=True;Encrypt=False;";                
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
                ConnectionTimeOut = Convert.ToInt32(Configuration["ConnectionTimeOut"])
            };

            CatalogConfig = new CatalogConfig
            {
                ServicePlan = Configuration["ServicePlan"],
                CatalogDatabase = Configuration["CatalogDatabase"],
                CatalogServer = Configuration["CatalogServer"],
                CatalogLocation = Configuration["APP_REGION"]
            };

            TenantServerConfig = new TenantServerConfig
            {
                TenantServer = Configuration["TenantServer"]
            };

            bool isResetEventDatesEnabled = false;
            if (bool.TryParse(Configuration["ResetEventDates"], out isResetEventDatesEnabled))
            {
                TenantServerConfig.ResetEventDates = isResetEventDatesEnabled;
            }
        }

        /// <summary>
        /// Initialises the shard map manager and shard map 
        /// <para>Also does all tasks related to sharding</para>
        /// </summary>
        private void InitialiseShardMapManager()
        {
            var basicConnectionString = GetBasicSqlConnectionString();
            SqlConnectionStringBuilder connectionString = new SqlConnectionStringBuilder(basicConnectionString)
            {
                DataSource = DatabaseConfig.SqlProtocol + ":" + CatalogConfig.CatalogServer + "," + DatabaseConfig.DatabaseServerPort,                            
                InitialCatalog = CatalogConfig.CatalogDatabase
            };

            var sharding = new Sharding(CatalogConfig.CatalogDatabase, connectionString.ConnectionString, _catalogRepository, _customerRepository, _utilities);
        }

        /// <summary>
        /// Gets the basic SQL connection string.
        /// </summary>
        /// <returns></returns>
        private string GetBasicSqlConnectionString()
        {
            var connStrBldr = new SqlConnectionStringBuilder
            {
                UserID = DatabaseConfig.DatabaseUser,
                Password = DatabaseConfig.DatabasePassword,
                ApplicationName = Configuration["ApplicationName"],
                ConnectTimeout = DatabaseConfig.ConnectionTimeOut,
                LoadBalanceTimeout = 15
            };

            return connStrBldr.ConnectionString;
        }
    }
}
