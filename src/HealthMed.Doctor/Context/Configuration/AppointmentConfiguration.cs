using HealthMed.Doctors.Entities;
using HealthMed.Shared.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthMed.Doctors.Context.Configuration
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.ToTable("Appointments");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.PatientName)
                .IsRequired();

            builder.Property(p => p.PatientId)
                .IsRequired();

            builder.Property(p => p.DateAppointment)
                .IsRequired();

            builder.Property(p => p.PatientAppointmentId)
                .IsRequired();

            builder.Property(p => p.CancelReason);

            builder.Property(p => p.Status)
                .HasDefaultValue(AppointmentStatus.Created)
                .IsRequired();

            builder.Property(u => u.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedNever()
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.HasOne(p => p.Doctor)
                .WithMany()
                .HasForeignKey(p => p.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
