using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tugberk.Persistance.SqlServer.Tests
{
    public class BlogDbContextProvider : IDisposable
    {
        private readonly DbContextOptions<BlogDbContext> _options;

        private BlogDbContextProvider()
        {
            var databaseName = $"TugberkWeb_{Path.GetRandomFileName()}";
            var connectionString = $"Server=localhost;Database={databaseName};User ID=sa;Password=Passw0rd;MultipleActiveResultSets=true";

            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<BlogDbContext>();
            builder.UseSqlServer(connectionString)
                .UseInternalServiceProvider(serviceProvider);

            _options = builder.Options;
        }

        public BlogDbContext CreateContext() => new BlogDbContext(_options);

        public void Dispose()
        {
            using(var ctx = CreateContext())
            {
                ctx.Database.EnsureDeleted();
            }
        }

        public static BlogDbContextProvider Create()
        {
            var provider = new BlogDbContextProvider();
            using(var ctx = provider.CreateContext())
            {
                ctx.Database.Migrate();
            }

            return provider;
        }
    }
}
