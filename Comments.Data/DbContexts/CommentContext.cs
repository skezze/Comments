using Comments.Data.Configurations;
using Comments.Domain.Entities;
using Microsoft.EntityFrameworkCore;
public class CommentContext : DbContext
{
    public CommentContext(DbContextOptions<CommentContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) 
    {
        modelBuilder.ApplyConfiguration(new CommentConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }

}
