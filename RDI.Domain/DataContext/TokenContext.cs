using Microsoft.EntityFrameworkCore;
using RDI.Domain.Token.Entities;

namespace RDI.Domain.DataContext
{
    public class TokenContext : DbContext
    {
        public TokenContext(DbContextOptions<TokenContext> options) : base(options) { }

        public virtual DbSet<CreditCard> CreditCards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CreditCard>().ToTable("CreditCard");
        }
    }
}
