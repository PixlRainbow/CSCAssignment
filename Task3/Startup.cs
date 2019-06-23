using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CSCAssignment.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Logging;

namespace CSCAssignment
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                .AddConfiguration(configuration)
                .AddUserSecrets<Startup>();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            string migrationsAssembly = typeof(Startup).Assembly.GetName().Name;
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString));
            //services.AddDefaultIdentity<IdentityUser>()
            //    .AddDefaultUI(UIFramework.Bootstrap4)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddAspNetIdentity<IdentityUser>()
                .AddConfigurationStore(options => {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlite(connectionString,
                            sqllite => sqllite.MigrationsAssembly(migrationsAssembly));
                }).AddOperationalStore(options => {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlite(connectionString,
                            sqllite => sqllite.MigrationsAssembly(migrationsAssembly));
                });
            
            services.AddAuthentication(options => {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(options => {
                    options.Authority = "http://localhost:5000";
                    options.Audience = "CSCAssignment";
                    options.RequireHttpsMetadata = false;
                })/*.AddIdentityServerAuthentication(options => {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "CSCAssignment";
                    options.ApiSecret = "secret";
                }).AddOpenIdConnect(options => {
                    //options.SignInScheme = "Cookies";

                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;

                    options.ClientId = "mvc";
                    options.ClientSecret = "secret";

                    options.ResponseType = "code id_token";
                    options.Scope.Add("CSCAssignment");
                    options.Scope.Add("offline_access");

                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.SaveTokens = true;
                })*/;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                IdentityModelEventSource.ShowPII = true;
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                //for IdentityServer
                throw new Exception("need to configure key material");
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseIdentityServer();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            InitDatabase.Init(app);
        }
    }
}
