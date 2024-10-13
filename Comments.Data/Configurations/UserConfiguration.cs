using Comments.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Comments.Data.Configurations
{
    public class UserConfiguration: IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder) 
        {
            builder.HasIndex(x => x.Id)
                .IsUnique();
            builder.HasIndex(x => x.Email)
                .IsUnique();
            builder.HasIndex(x => x.UserName)
                .IsUnique();
        }
    }
}
