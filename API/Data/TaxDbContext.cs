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

        mb.Entity<TaxBand>()
            .Property(tb => tb.LowerLimit)
            .IsRequired();

        mb.Entity<TaxBand>()
            .Property(tb => tb.Rate)
            .IsRequired();
    }
}