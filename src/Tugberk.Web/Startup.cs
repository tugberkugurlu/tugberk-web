using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
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

            services.AddHttpClient(NamedHttpClients.GoogleReCaptchaClient, client =>
            {
                client.BaseAddress = new Uri("https://www.google.com/recaptcha/");
            });

            services.AddSingleton<IEmailSender, NoOpEmailSender>();
            services.AddScoped<IPostsRepository, PostsSqlServerRepository>();
            services.AddScoped<ITagsRepository, TagsSqlServerRepository>();
            services.Configure<BlogSettings>(_configuration.GetSection("BlogSettings"));
            services.Configure<GoogleReCaptchaSettings>(_configuration.GetSection("GoogleReCaptcha"));
            services.Configure<GoogleAnalyticsSettings>(_configuration.GetSection("GoogleAnalytics"));
            services.Configure<AzureBlobStorageSettings>(_configuration.GetSection("ImageStorage").GetSection("AzureBlobStorage"));

            services.AddScoped<IImageStorage>(sp => 
            {
                var blobStorageSettings = sp.GetService<IOptions<AzureBlobStorageSettings>>();

                return blobStorageSettings.Value.IsAzureBlobStorageEnabled()
                    ? new AzureBlobStorageImageStorage(CloudStorageAccount.Parse(blobStorageSettings.Value.ConnectionString).CreateCloudBlobClient()) as IImageStorage
                    : new LocalImageStorage(sp.GetService<ILogger<LocalImageStorage>>());
            });

            services.AddMvc();
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        }

        [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Invoked by ASP.NET Core hosting layer")]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
#if RELEASE
            app.UseRewriter(new RewriteOptions().AddRedirectToWwwPermanent());
#endif
            UseLegacyBlogImagesRedirector(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
            
            MigrateAndEnsureDataIsSeeded(app);
        }

        private static void UseLegacyBlogImagesRedirector(IApplicationBuilder app)
        {
            app.Use((context, next) =>
            {
                const string legacyImagesRootPath = "/content/images";
                var isMatch = context.Request.Path.StartsWithSegments(new PathString(legacyImagesRootPath),
                    StringComparison.InvariantCultureIgnoreCase);

                if (isMatch)
                {
                    var fullPath = context.Request.Path.ToUriComponent();
                    var path = fullPath.Substring(legacyImagesRootPath.Length,
                        fullPath.Length - legacyImagesRootPath.Length);

                    Console.WriteLine(path);

                    const string newUriPrefix = "https://tugberkugurlu.blob.core.windows.net/bloggyimages/legacy-blog-images/images";
                    var newUri = $"{newUriPrefix}{path}";
                    context.Response.Redirect(newUri, true);

                    return Task.CompletedTask;
                }

                return next.Invoke();
            });
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
