using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NinjaTools.FluentMockServer.API
{
    public static class Extensions
    {
        public static async ValueTask<long> ClearAsync<T>(this DbSet<T> dbSet, CancellationToken token = default) where T : class
        {
            var count = await dbSet.LongCountAsync(token);
            dbSet.RemoveRange(dbSet);
            return count;
        }
    }
}
