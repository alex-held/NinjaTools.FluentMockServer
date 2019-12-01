using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NinjaTools.FluentMockServer.Domain.Models;

namespace NinjaTools.FluentMockServer.API.Data
{
    
    public class ExpectationDbContext : DbContext
    {
        public ExpectationDbContext([NotNull] DbContextOptions builder) : base(builder) { }
        
        public DbSet<Expectation> Expectations { get; set; }


        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExpectationDbContext).Assembly);
        }
        
        /// <inheritdoc />
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }

}
