using System;
using MediaShareMVC_Core.Areas.Identity.Data;
using MediaShareMVC_Core.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(MediaShareMVC_Core.Areas.Identity.IdentityHostingStartup))]
namespace MediaShareMVC_Core.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<MediaShareDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("MediaShareDbContextConnection")));

                services.AddDefaultIdentity<WebAppUser>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<MediaShareDbContext>();
            });
        }
    }
}