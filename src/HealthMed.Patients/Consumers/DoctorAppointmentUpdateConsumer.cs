using HealthMed.Patients.Interfaces.Services;
using HealthMed.Shared.Dtos;
using MassTransit;

namespace HealthMed.Patients.Consumers
{
    public class DoctorAppointmentUpdateConsumer : IConsumer<AppointmentDoctorUpdateMessage>
    {
        private readonly IAppointmentService _appointmentService;

        public DoctorAppointmentUpdateConsumer(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public async Task Consume(ConsumeContext<AppointmentDoctorUpdateMessage> context)
        {
            await _appointmentService.AppointmentUpdatedDoctor(context.Message.PatientAppointmentId, context.Message.Status);
        }
    }
}
