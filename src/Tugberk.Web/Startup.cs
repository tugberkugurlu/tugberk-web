using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tugberk.Persistance.Abstractions;
using Tugberk.Persistance.InMemory;
using Tugberk.Persistance.SqlServer;
using Tugberk.Web.Controllers;

namespace Tugberk.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BlogDbContext>(options => 
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<BlogDbContext>()
                .AddDefaultTokenProviders();

            services.AddSingleton<IImageStorage, LocalImageStorage>();
            services.AddScoped<IPostsStore, InMemoryPostsStore>();

            services.AddMvc();
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        }

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
