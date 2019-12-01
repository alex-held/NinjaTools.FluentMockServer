using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NinjaTools.FluentMockServer.Domain.Models;

namespace NinjaTools.FluentMockServer.API.Data
{
    
    public class ExpectationDbContext : DbContext
    {
        public ExpectationDbContext(DbContextOptions<ExpectationDbContext> builder) : base(builder) { }
        
        public DbSet<Expectation> Expectations { get; set; }
//        public DbSet<HttpRequest> Requests { get; set; }
//        public DbSet<HttpForward> Forwards { get; set; }
//        public DbSet<HttpTemplate> Templates { get; set; }
//        public DbSet<HttpError> Errors { get; set; }
//        public DbSet<Times> Times { get; set; }  
//        public DbSet<LifeTime> LifeTimes { get; set; }  
//        public DbSet<Verify> Verifies { get; set; }  
//        

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExpectationDbContext).Assembly);
        }

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        /// <inheritdoc />
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }

}
