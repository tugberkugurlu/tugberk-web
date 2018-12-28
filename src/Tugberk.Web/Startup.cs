using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tugberk.Persistance.Abstractions;
using Tugberk.Persistance.SqlServer;
using Tugberk.Persistance.SqlServer.Stores;
using Tugberk.Web.MediaStorage;
using Tugberk.Web.Services;

namespace Tugberk.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Invoked by ASP.NET Core hosting layer")]
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Invoked by ASP.NET Core hosting layer")]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BlogDbContext>(options => 
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>(options => 
                {
                    options.ClaimsIdentity.UserNameClaimType = ClaimTypes.Email;
                }).AddEntityFrameworkStores<BlogDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options => 
            {
                options.LoginPath = "/portal/account/login";
                options.LogoutPath = "/portal/account/logout";
                options.AccessDeniedPath = "/portal/account/accessDenied";
            });

            services.AddSingleton<IEmailSender, NoOpEmailSender>();
            services.AddSingleton<IImageStorage, LocalImageStorage>();
            services.AddScoped<IPostsStore, PostsSqlServerStore>();
            services.AddScoped<ITagsStore, TagsSqlServerStore>();
            services.Configure<BlogSettings>(_configuration.GetSection("BlogSettings"));
            services.Configure<GoogleReCaptchaSettings>(_configuration.GetSection("GoogleReCaptcha"));

            services.AddMvc();
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        }

        [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Invoked by ASP.NET Core hosting layer")]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();

            MigrateAndEnsureDataIsSeeded(app);
        }

        private void MigrateAndEnsureDataIsSeeded(IApplicationBuilder app)
        {
            // https://blogs.msdn.microsoft.com/dotnet/2016/09/29/implementing-seeding-custom-conventions-and-interceptors-in-ef-core-1-0/

            using(var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope()) 
            {
                var context = serviceScope.ServiceProvider.GetService<BlogDbContext>();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<IdentityUser>>();

                context.Database.Migrate();
                context.EnsureSeedData(userManager);
            }
        }
    }
}
