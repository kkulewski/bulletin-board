using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BulletinBoard.Data;
using BulletinBoard.Models;
using Microsoft.AspNetCore.Localization;

namespace BulletinBoard
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // add dbContext with connection string
            // its default location: appsettings.json
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // loosen password rules (simple passwords)
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;

            });

            // use default identity and role system
            // with ApplicationUser serving as a identity user
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // use en-US culture to avoid problems with decimal values (i.e money) validation
            // some countries use comma ',' while others use dot '.' as a decimal separator
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en-US");
            });

            // add dbInitializer that will be used to create + seed database
            services.AddScoped<ApplicationDbInitializer>();

            // use lowercase routing
            services.AddRouting(options => options.LowercaseUrls = true);

            // add automapper - used to map models to viewModels
            services.AddAutoMapper();

            services.AddMvc();
        }

        // Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbInitializer dbInitializer)
        {
            // specify how to handle exceptions
            if (env.IsDevelopment())
            {
                // display rich error info pages when in development mode
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                // custom handler - display error page
                app.UseExceptionHandler("/JobOffer/Error");
            }

            app.UseStaticFiles();

            app.UseRequestLocalization();
            
            // enable authentication
            app.UseAuthentication();

            // create + seed database with default values
            dbInitializer.Seed();

            // re-execute invalid requests (i.e when user tries to access a page that does not exist)
            app.UseStatusCodePagesWithReExecute("/JobOffer/Error", "?statusCode={0}");

            // configure mvc routing
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    // specify route template with default values
                    template: "{controller=JobOffer}/{action=Popular}/{id?}");
            });
        }
    }
}
