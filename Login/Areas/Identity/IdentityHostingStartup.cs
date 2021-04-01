using Login.Areas.Identity.Data;
using Login.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Login.Areas.Identity.IdentityHostingStartup))]
namespace Login.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<LoginContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("LoginContextConnection")));

                services.AddIdentity<LoginUser, IdentityRole>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    // Default Password settings.
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                })
                .AddEntityFrameworkStores<LoginContext>()
                .AddDefaultTokenProviders()
                .AddRoles<IdentityRole>()
                .AddDefaultUI();
            });
        }
    }
}