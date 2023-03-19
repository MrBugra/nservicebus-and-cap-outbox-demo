using Microsoft.EntityFrameworkCore;

namespace CapOutboxSample.Data;

public class WalletDbContext : DbContext
{
    public WalletDbContext(DbContextOptions<WalletDbContext> options)
        : base(options)
    {
    }

    public DbSet<Wallet> Wallet { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        MapModel(modelBuilder);
    }

    private ModelBuilder MapModel(ModelBuilder builder)
    {
        return builder.Entity<Wallet>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasKey(e => e.Id);
        });
    }
}

public class Wallet
{
    public int Id { get; set; }

    public string Username { get; set; }

    public double Balance { get; set; }
}