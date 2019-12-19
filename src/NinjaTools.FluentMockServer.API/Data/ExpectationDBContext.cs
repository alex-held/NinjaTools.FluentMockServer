using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NinjaTools.FluentMockServer.Models;

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
    }

}
