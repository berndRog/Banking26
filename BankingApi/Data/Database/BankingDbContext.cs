using BankingApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace BankingApi.Data.Database;

public sealed class BankingDbContext(
   DbContextOptions<BankingDbContext> options
) : DbContext(options) {
   
   public DbSet<Owner> Owners => Set<Owner>();
   public DbSet<Account> Accounts => Set<Account>();
   public DbSet<Beneficiary> Beneficiaries => Set<Beneficiary>();
   public DbSet<Transfer> Transfers => Set<Transfer>();
   public DbSet<Transaction> Transactions => Set<Transaction>();


   protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<Owner>(b => {
         b.HasKey(o => o.Id);
         b.Property(o => o.FirstName).IsRequired();
         b.Property(o => o.LastName).IsRequired();
         b.Property(o => o.Email).IsRequired();
      });
      
      modelBuilder.Entity<Account>(builder => {
         builder.HasKey(a => a.Id);
         builder.Property(a => a.Iban).IsRequired();
         builder.Property(a => a.Balance).IsRequired();
         builder.HasOne<Owner>()
            .WithMany()
            .HasForeignKey(a => a.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);
      });
      
      modelBuilder.Entity<Beneficiary>(builder => {
         builder.HasKey(b => b.Id);
         builder.Property(b => b.FirstName).IsRequired();
         builder.Property(b => b.LastName).IsRequired();
         builder.Property(b => b.Iban).IsRequired();
         builder.HasOne<Account>()
            .WithMany()
            .HasForeignKey(b => b.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
      });
      
      // modelBuilder.Entity<Transfer>(builder => {
      //    builder.HasKey(t => t.Id);
      //    builder.Property(t => t.Amount).IsRequired();
      //    builder.Property(t => t.TransferDate).IsRequired();
      //    builder.HasOne<Account>()
      //       .WithMany()
      //       .HasForeignKey(t => t.AccountId)
      //       .OnDelete(DeleteBehavior.Cascade);
      // });
      
   }
   
   
}