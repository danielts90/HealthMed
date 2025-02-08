using HealthMed.Doctors.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthMed.Doctors.Context.Configuration
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.ToTable("Doctors");
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                .IsRequired();
            
            builder.Property(d => d.Email)
                .IsRequired()
                .ValueGeneratedNever().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Property(d => d.CRM)
                .IsRequired()
                .ValueGeneratedNever().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Property(d => d.CPF).IsRequired();
            
            builder.Property(d => d.UserId)
                .IsRequired()
                .ValueGeneratedNever().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Property(d => d.Speciality)
                .IsRequired();

            builder.Property(u => u.CreatedAt)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd()
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        }
    }

}
