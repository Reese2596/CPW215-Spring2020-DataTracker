using System;
using DataTracker.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(DataTracker.Areas.Identity.IdentityHostingStartup))]
namespace DataTracker.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<DataTrackerDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("DataTrackerDbContextConnection")));

                /*NEEDS TO BE SOLVED*/
                //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    //.AddEntityFrameworkStores<DataTrackerDbContext>();
            });
        }
    }
}