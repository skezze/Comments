﻿using Comments.Domain.Models;
using Microsoft.EntityFrameworkCore;
public class CommentContext : DbContext
{
    public CommentContext(DbContextOptions<CommentContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Comment> Comments { get; set; }
}
