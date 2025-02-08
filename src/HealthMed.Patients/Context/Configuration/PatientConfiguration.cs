using HealthMed.Patients.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthMed.Patients.Context.Configuration
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.ToTable("Patients");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired();

            builder.Property(p => p.Cpf)
                .IsRequired()
                .ValueGeneratedNever().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Property(p => p.Email)
                .IsRequired()
                .ValueGeneratedNever().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Property(u => u.CreatedAt)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd()
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

        }
    }
}
