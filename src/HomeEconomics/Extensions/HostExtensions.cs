using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Microsoft.AspNetCore.Hosting
{
    internal static class HostExtensions
    {
        internal static IHost InitializeDbContext<TDbContext>(this IHost host) where TDbContext : DbContext
        {
            using var scope = host.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<TDbContext>();

            dbContext?.Database.Migrate();

            return host;
        }
    }
}
