using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Hosting
{
    internal static class WebHostExtensions
    {
        internal static IWebHost InitializeDbContext<TDbContext>(this IWebHost host) where TDbContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<TDbContext>();

                dbContext.Database.EnsureCreated();
            }

            return host;
        }
    }
}
