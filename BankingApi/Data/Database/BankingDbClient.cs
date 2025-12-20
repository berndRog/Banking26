using BankingApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace BankingApi.Data.Database;

public sealed class BankingDbContext(
   DbContextOptions<BankingDbContext> options
) : DbContext(options) {
   
   public DbSet<Owner> Owners => Set<Owner>();

   protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<Owner>(b => {
         b.HasKey(o => o.Id);
         b.Property(o => o.FirstName).IsRequired();
         b.Property(o => o.LastName).IsRequired();
         b.Property(o => o.Email).IsRequired();
      });
   }
}