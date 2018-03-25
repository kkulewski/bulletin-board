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
using BulletinBoard.Data.Repositories;
using BulletinBoard.Data.Repositories.Abstract;
using BulletinBoard.Helpers.Filters;
using BulletinBoard.Models;
using BulletinBoard.Services;
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
        
        public IContainer ApplicationContainer { get; private set; }

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
            services.AddAutofac();

            services.AddScoped<ApplicationDbInitializer>();

            services
                .AddMvc(options => { options.Filters.Add(typeof(ExampleActionFilter)); })
                .AddControllersAsServices();

            // Create the container builder.
            var builder = new ContainerBuilder();

            // Register dependencies, populate the services from
            // the collection, and build the container. If you want
            // to dispose of the container at the end of the app,
            // be sure to keep a reference to it as a property or field.
            //
            // Note that Populate is basically a foreach to add things
            // into Autofac that are in the collection. If you register
            // things in Autofac BEFORE Populate then the stuff in the
            // ServiceCollection can override those things; if you register
            // AFTER Populate those registrations can override things
            // in the ServiceCollection. Mix and match as needed.
            builder.RegisterType<Mapper>().As<IMapper>();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<ApplicationUserRepository>().As<IApplicationUserRepository>();
            builder.RegisterType<JobCategoryService>().As<IJobCategoryService>();
            builder.RegisterType<JobTypeRepository>().As<IJobTypeRepository>();
            builder.RegisterType<JobTypeService>().As<IJobTypeService>();
            builder.RegisterType<JobCategoryRepository>().As<IJobCategoryRepository>();
            builder.RegisterType<JobCategoryService>().As<IJobCategoryService>();
            builder.RegisterType<JobOfferRepository>().As<IJobOfferRepository>();
            builder.RegisterType<JobOfferService>().As<IJobOfferService>();
            builder.RegisterType<RoleService>().As<IRoleService>();
            builder.RegisterType<AuthService>().As<IAuthService>();

            builder.Populate(services);
            this.ApplicationContainer = builder.Build();

            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(this.ApplicationContainer);
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
