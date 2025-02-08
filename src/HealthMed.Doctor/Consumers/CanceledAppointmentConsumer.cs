using HealthMed.Doctors.Interfaces.Services;
using HealthMed.Shared.Dtos;
using MassTransit;

namespace HealthMed.Doctors.Consumers
{
    public class CanceledAppointmentConsumer : IConsumer<CanceledAppointmentMessage>
    {
        private readonly IAppointmentService _appointmentService;

        public CanceledAppointmentConsumer(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public async Task Consume(ConsumeContext<CanceledAppointmentMessage> context)
        {
            await _appointmentService.AppointmentRejectedByPatient(context.Message.PatientAppointmentId, context.Message.cancelReason);
        }
    }
}
