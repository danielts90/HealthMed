using HealthMed.Doctors.Entities;
using HealthMed.Doctors.Interfaces.Services;
using HealthMed.Shared.Dtos;
using MassTransit;

namespace HealthMed.Doctors.Consumers
{
    public class CreateAppointmentConsumer : IConsumer<AppointmentMessage>
    {
        private readonly IAppointmentService _appointmentService;

        public CreateAppointmentConsumer(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public async Task Consume(ConsumeContext<AppointmentMessage> context)
        {
            var appointment = new Appointment
            {
                DoctorId = context.Message.DoctorId,
                PatientId = context.Message.PatientId,
                PatientName = context.Message.PatientName,
                PatientAppointmentId = context.Message.PatientAppointmentId,
                DateAppointment = context.Message.DateAppointment
            };

            await _appointmentService.CreateAppointment(appointment);
        }
    }
}
