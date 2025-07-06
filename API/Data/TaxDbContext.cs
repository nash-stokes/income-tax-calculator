using API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class TaxDbContext : DbContext
{
    public TaxDbContext(DbContextOptions<TaxDbContext> options)
        : base(options)
    {
    }

    public DbSet<TaxBand> TaxBands { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<TaxBand>()
            .HasKey(tb => tb.Id);

        // Configure auto-generated ID
        mb.Entity<TaxBand>()
            .Property(tb => tb.Id)
            .UseIdentityColumn(); // This sets up auto-increment behavior

        mb.Entity<TaxBand>()
            .Property(tb => tb.LowerLimit)
            .IsRequired();

        mb.Entity<TaxBand>()
            .Property(tb => tb.Rate)
            .IsRequired();
    }

}