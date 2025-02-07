using HealthMed.Auth.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthMed.Auth.Context.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            
            builder.HasKey(u => u.Id);
            
            builder.Property(u => u.Name)
                .IsRequired();
            
            builder.Property(u => u.Email)
                .IsRequired();

            builder.Property(u => u.SecondaryLogin)
                .IsRequired();  

            builder.Property(u => u.Password)
                .IsRequired();
            builder.Property(u => u.UserType)
                .HasColumnType("smallint")
                .IsRequired();

            builder.Property(u => u.CreatedAt)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd()
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            ;
        }
    }
}
