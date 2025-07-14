using Microsoft.EntityFrameworkCore;
using TicTacToe.Core;

namespace TicTacToe.Api.Data;

public class GameDbContext : DbContext
{
    public DbSet<Game> Games { get; set; } = null!;
    public DbSet<Player> Players { get; set; } = null!;
    public DbSet<Move> Moves { get; set; } = null!;

    public GameDbContext(DbContextOptions<GameDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>()
            .HasKey(g => g.Id);

        modelBuilder.Entity<Player>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<Move>()
            .HasKey(m => m.Id);

        // Связь игроки-игра
        modelBuilder.Entity<Game>()
            .HasOne(g => g.PlayerX);

        modelBuilder.Entity<Game>()
            .HasOne(g => g.PlayerO);

        modelBuilder.Entity<Move>()
            .HasOne<Game>()
            .WithMany()
            .HasForeignKey(m => m.GameId);
    }
}