using Login.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net;
using Login.Service;
using Login.Data.Interfaces;

namespace Login
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
            

            //server, deployment
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.KnownProxies.Add(IPAddress.Parse("127.0.0.1"));
            });

            //connection to the db 
            services.AddDbContext<ThreadContext>(options => options.UseSqlServer(Configuration.GetConnectionString("LoginContextConnection")));
            services.AddDbContext<ChannelContext>(options => options.UseSqlServer(Configuration.GetConnectionString("LoginContextConnection")));
            services.AddDbContext<AchievementContext>(options => options.UseSqlServer(Configuration.GetConnectionString("LoginContextConnection")));

            //other services
            services.AddControllersWithViews();
            services.AddControllers();
            services.AddRazorPages();

            //services
            services.AddScoped<IThread, ThreadService>();
            services.AddScoped<IApplicationUsers, ApplicationUserService>();
            services.AddScoped<IUpload, UploadService>();
            services.AddScoped<IChannel, ChannelService>();
            services.AddScoped<IAchievement, AchievementService>();
            services.AddScoped<IVision, VisionService>();

            //added the connetion to Azure
            services.AddSingleton(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) { app.UseDeveloperExceptionPage(); /*app.UseDatabaseErrorPage();*/}
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            else { app.UseExceptionHandler("/Home/Error"); app.UseHsts(); }


            //server, deployment
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseHttpsRedirection();
            // makes the app use static files and enables default file maping
            app.UseStaticFiles();
            app.UseDefaultFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseMvc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
