using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using DataTracker.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.CodeAnalysis.Options;
using DataTracker.Models;

namespace DataTracker
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
            services.AddDbContext<DataTrackerDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>(IdentityHelper.SetIdentityOptions)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DataTrackerDbContext>();

            services.AddControllersWithViews();

            services.AddRazorPages();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            // Create Roles
            IServiceScope serviceProvider = app.ApplicationServices
                                     .GetRequiredService<IServiceProvider>()
                                     .CreateScope();

            IdentityHelper.CreateRoles(serviceProvider.ServiceProvider
                                     , IdentityHelper.ManagementRole
                                     , IdentityHelper.UserRole).Wait();

            // Create Default Admin/Instuctor
            IdentityHelper.CreateDefaultInstructor(serviceProvider.ServiceProvider).Wait();
        }
    }
}
