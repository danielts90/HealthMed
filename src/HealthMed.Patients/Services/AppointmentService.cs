using HealthMed.Patients.Entities;
using HealthMed.Patients.Interfaces.Factories;
using HealthMed.Patients.Interfaces.Repositories;
using HealthMed.Patients.Interfaces.Services;
using HealthMed.Patients.Interfaces.UnitOfWork;
using HealthMed.Shared.Builders;
using HealthMed.Shared.Dtos;
using HealthMed.Shared.Enum;
using HealthMed.Shared.Exceptions;
using HealthMed.Shared.Templates;
using HealthMed.Shared.Util;
using MassTransit;

namespace HealthMed.Patients.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IPatientService _patientService;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IEmailService _emailService;
        private readonly ICreateAppointmentMessageFactory _createAppointmentMessageFactory;
        private readonly IUnitOfWork _uow;



        public AppointmentService(IPatientService patientService,
                                  ISendEndpointProvider sendEndpointProvider,
                                  IEmailService emailService,
                                  IUnitOfWork uow,
                                  ICreateAppointmentMessageFactory createAppointmentMessageFactory)

        {
            _patientService = patientService;
            _sendEndpointProvider = sendEndpointProvider;
            _emailService = emailService;
            _uow = uow;
            _createAppointmentMessageFactory = createAppointmentMessageFactory;
        }

        public async Task<Appointment> AppointmentUpdatedDoctor(int apppointmentId, AppointmentStatus status)
        {
            var appointment = await _uow.AppointmentRepository.GetByIdAsync(apppointmentId);
            var patient = await _patientService.GetPatientByPatientId(appointment.PatientId);
            appointment.Status = status;

            _uow.AppointmentRepository.Update(appointment);

            _uow.Commit();

            var statusMessage = status == AppointmentStatus.Accepted ? "<b>Confirmada<b/>" : "<b>Rejeitada<b/>";

            var email = EmailBuilder.New()
                            .To(patient.Email)
                            .Subject($"Consulta marcada para o dia {appointment.DateAppointment.ToString("f")} foi {statusMessage}")
                            .Body(MailTemplates.appointmentUpdated.Replace("{{PACIENTE_NOME}}", patient.Name)
                                                                   .Replace("{{DATA_CONSULTA}}", appointment.DateAppointment.ToString("dd/MM/yyyy"))
                                                                   .Replace("{{HORA_CONSULTA}}", appointment.DateAppointment.ToString("HH:mm"))
                                                                   .Replace("{{MEDICO_NOME}}", appointment.DoctorName)
                                                                   .Replace("{{STATUS_CONSULTA}}", statusMessage))
                            .Build();

            await _emailService.SendMail(email);
            return appointment;
        }

        public async Task<Appointment> CancelAppointment(int appointmentId, string cancelReason)
        {
            var appointment = await _uow.AppointmentRepository.GetByIdAsync(appointmentId);
            if (appointment == null) throw new KeyNotFoundException("Consulta não encontrada.");

            var patient = await GetPatientAsync();
            if (patient.Id != appointment.PatientId) throw new InvalidUserException("Paciente não pode alterar a consulta e outro.");

            appointment.CancelReason = cancelReason;
            appointment.Status = AppointmentStatus.Rejected;

            var canceledAppointment = new CanceledAppointmentMessage(appointmentId, cancelReason);

            _uow.AppointmentRepository.Update(appointment);

            _uow.Commit();

            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:patient-appointment-queue"));
            await endpoint.Send(canceledAppointment);

            return appointment;
        }

        public async Task<Appointment> CreateAppointment(Appointment appointment)
        {
            var patient = await GetPatientAsync();
            if(patient.Id != appointment.PatientId) throw new InvalidUserException("Paciente não pode criar consulta para outro.");

            _uow.AppointmentRepository.Add(appointment);

            _uow.Commit();

            var createdAppointment = _createAppointmentMessageFactory.CreateMessage(appointment, patient);

            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:patient-appointment-queue"));
            await endpoint.Send(createdAppointment);

            return appointment;
        }

        public async Task<IEnumerable<Appointment>> GetAppointments()
        {
            var patient = await GetPatientAsync();
            var appointments = await _uow.AppointmentRepository.GetDataAsync(o => o.PatientId == patient.Id
                                                               && o.Status != AppointmentStatus.Rejected
                                                               && o.DateAppointment > DateTime.Now);

            return appointments;
        }

        private async Task<Patient> GetPatientAsync() => await _patientService.GetPatientByUserId();

    }
}
