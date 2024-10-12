using Comments.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Comments.Data.Configurations
{
    public class CommentConfiguration: IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            // Define indexes
            builder.HasIndex(c => c.Id)
                   .IsUnique();

            // Define relationships
            builder.HasOne(u => u.User)
                   .WithMany(c => c.Comments)
                   .HasForeignKey(c => c.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
