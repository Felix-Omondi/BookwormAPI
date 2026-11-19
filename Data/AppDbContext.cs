using BookwormAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookwormAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
}
