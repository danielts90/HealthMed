using HealthMed.Patients.Entities;
using HealthMed.Shared.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthMed.Patients.Context.Configuration
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.ToTable("Appointments");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.DateAppointment)
                .IsRequired()
                .ValueGeneratedNever().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Property(p => p.DoctorId)
                .IsRequired()
                .ValueGeneratedNever().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Property(p => p.Speciality)
                .IsRequired()
                .ValueGeneratedNever().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Property(p => p.Status)
                .HasDefaultValue(AppointmentStatus.Created)
                .IsRequired();

            builder.Property(p => p.PatientId)
                .IsRequired()
                .ValueGeneratedNever().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Property(p => p.DoctorName)
                .IsRequired()
                .ValueGeneratedNever().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Property(p => p.Price)
                .IsRequired()
                .ValueGeneratedNever().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Property(p => p.CancelReason);

            builder.HasOne(d => d.Patient)
                .WithMany()
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Property(u => u.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedNever()
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        }
    }
}
