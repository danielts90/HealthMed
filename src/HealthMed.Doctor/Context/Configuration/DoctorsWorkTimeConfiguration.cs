using HealthMed.Doctors.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthMed.Doctors.Context.Configuration
{
    public class DoctorsWorkTimeConfiguration : IEntityTypeConfiguration<DoctorsWorkTime>
    {
        public void Configure(EntityTypeBuilder<DoctorsWorkTime> builder)
        {
            builder.ToTable("DoctorsWorkTime");
            
            builder.HasKey(d => d.Id);
            
            builder.Property(d => d.WeekDay)
                .IsRequired();
            
            builder.Property(d => d.StartTime)
                .IsRequired()
                .HasColumnType("time");
            
            builder.Property(d => d.StartInterval)
                .HasColumnType("time");
            
            builder.Property(d => d.FinishInterval)
                .HasColumnType("time");
            
            builder.Property(d => d.ExitTime)
                .IsRequired()
                .HasColumnType("time");
            
            builder.Property(d => d.AppointmentDuration)
                .IsRequired();

            builder.Property(d => d.AppointmentPrice)
                .IsRequired();

            builder.Property(u => u.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedNever()
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);


            builder.HasOne(d => d.Doctor)
                .WithMany()
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
