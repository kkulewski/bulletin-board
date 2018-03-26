using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BulletinBoard.Data;
using BulletinBoard.Helpers.Filters;
using BulletinBoard.Infrastructure.Autofac;
using BulletinBoard.Models;
using BulletinBoard.Services.Abstract;
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

        public IServiceProvider ConfigureServices(IServiceCollection services)
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

            services.AddRouting(options => options.LowercaseUrls = true);
            
            services.AddAutoMapper();

            services
                .AddMvc(options => { options.Filters.Add(typeof(ExampleActionFilter)); })
                .AddControllersAsServices();

            // Autofac
            var builder = new ContainerBuilder();
            builder.RegisterModule(new InfrastructureHandlerModule());
            builder.RegisterModule(new RepositoryHandlerModule());
            builder.RegisterModule(new ServiceHandlerModule());
            builder.Populate(services);
            return new AutofacServiceProvider(builder.Build());
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMigrationManager migrationManager, ISeedService seedService)
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

            migrationManager.Apply().Wait();
            seedService.Seed().Wait();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=JobOffer}/{action=Popular}/{id?}");
            });
        }
    }
}
