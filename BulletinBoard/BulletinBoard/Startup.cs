using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BulletinBoard.Data;
using BulletinBoard.Data.Repositories;
using BulletinBoard.Data.Repositories.Abstract;
using BulletinBoard.Models;
using BulletinBoard.Services;
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
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;

            });
            
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en-US");
            });
            
            services.AddScoped<ApplicationDbInitializer>();
            services.AddTransient<IJobCategoryRepository, JobCategoryRepository>();
            services.AddTransient<IJobTypeRepository, JobTypeRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddTransient<IJobCategoryService, JobCategoryService>();

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddAutoMapper();
            services.AddMvc();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbInitializer dbInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/JobOffer/Error");
            }

            app.UseStaticFiles();
            app.UseStatusCodePagesWithReExecute("/JobOffer/Error", "?statusCode={0}");
            app.UseRequestLocalization();
            app.UseAuthentication();
            
            dbInitializer.Seed();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=JobOffer}/{action=Popular}/{id?}");
            });
        }
    }
}
