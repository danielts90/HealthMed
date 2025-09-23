using HealthMed.Doctors.Entities;
using HealthMed.Doctors.Interfaces.Services;
using HealthMed.Doctors.Interfaces.UnitOfWork;
using HealthMed.Shared.Builders;
using HealthMed.Shared.Dtos;
using HealthMed.Shared.Enum;
using HealthMed.Shared.Templates;
using HealthMed.Shared.Util;
using MassTransit;

namespace HealthMed.Doctors.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUserContext _userContext;
        private readonly IDoctorsWorkTimeService _doctorsWorkTimeService;
        private readonly IDoctorService _doctorService;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _uow;


        public AppointmentService(IUserContext userContext,
                                  IDoctorsWorkTimeService doctorsWorkTimeService,
                                  IDoctorService doctorService,
                                  ISendEndpointProvider sendEndpointProvider,
                                  IEmailService emailService,
                                  IUnitOfWork unitOfWork)
        {
            _userContext = userContext;
            _doctorsWorkTimeService = doctorsWorkTimeService;
            _doctorService = doctorService;
            _sendEndpointProvider = sendEndpointProvider;
            _emailService = emailService;
            _uow = unitOfWork;
        }

        public async Task<Appointment> CreateAppointment(Appointment appointment)
        {
            var doctor = await _doctorService.GetDoctorById(appointment.DoctorId);
            await EnsureAppointmentDoesNotExistAsync(appointment);
            await _doctorsWorkTimeService.IsValidWorkTime(appointment.DateAppointment, appointment.DoctorId);

            _uow.AppointmentRepository.Add(appointment);

            _uow.Commit();

            var email = EmailBuilder.New()
                          .To(doctor.Email)
                          .Subject($"Nova consulta marcada para o dia {appointment.DateAppointment.ToString("f")}")
                          .Body(MailTemplates.appointmentUpdated.Replace("{{PACIENTE_NOME}}", appointment.PatientName)
                                                       .Replace("{{DATA_CONSULTA}}", appointment.DateAppointment.ToString("dd/MM/yyyy"))
                                                       .Replace("{{HORA_CONSULTA}}", appointment.DateAppointment.ToString("HH:mm"))
                                                       .Replace("{{MEDICO_NOME}}", doctor.Name))
                          .Build();

            await _emailService.SendMail(email);

            return appointment;
        }

        private async Task EnsureAppointmentDoesNotExistAsync(Appointment appointment)
        {
            var existentAppointment = await _uow.AppointmentRepository.FirstAsync(o => o.DateAppointment == appointment.DateAppointment && o.DoctorId == appointment.DoctorId);
            if (existentAppointment != null)
            {
                await NotifyPatient(appointment, AppointmentStatus.Rejected);
                throw new InvalidOperationException("Já existe uma consulta marcada no dia e horário selecionado.");
            }
        }

        private async Task NotifyPatient(Appointment appointment, AppointmentStatus status)
        {
            var message = new AppointmentDoctorUpdateMessage
                            (
                                appointment.PatientAppointmentId,
                                status
                            );
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:doctor-appointment-queue"));
            await endpoint.Send(message);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctor(DateTime dateAppointments, int? doctorId = null)
        {
            int? IdDoctor = doctorId ?? _userContext.GetUserId();
            var appointments = await _uow.AppointmentRepository.GetDataAsync(o =>
                o.DateAppointment.Date == dateAppointments.Date && o.DoctorId == IdDoctor.Value);
            return appointments;
        }

        private async Task<Appointment> UpdateAppointmentStatus(int appointmentId, AppointmentStatus status)
        {
            var appointment = await _uow.AppointmentRepository.FirstAsync(o => o.PatientAppointmentId == appointmentId)
                              ?? throw new KeyNotFoundException("Consulta não encontrada.");

            await ValidateDoctorPermission(appointment.DoctorId);

            appointment.Status = status;
            var updatedAppointment = _uow.AppointmentRepository.Update(appointment);
            
            _uow.Commit();

            await NotifyPatient(appointment, status);

            return updatedAppointment;
        }

        public async Task<Appointment> AcceptAppointment(int appointmentId)
        {
            return await UpdateAppointmentStatus(appointmentId, AppointmentStatus.Accepted);
        }

        public async Task<Appointment> RejectAppointment(int appointmentId)
        {
            return await UpdateAppointmentStatus(appointmentId, AppointmentStatus.Rejected);
        }

        private async Task ValidateDoctorPermission(int doctorId)
        {
            var doctor = await _doctorService.GetDoctorById(doctorId)
                         ?? throw new KeyNotFoundException("Médico não encontrado.");

            if (doctor.UserId != _userContext.GetUserId())
                throw new InvalidOperationException("Médico não pode alterar consultas de outros médicos.");
        }

        public async Task<Appointment> AppointmentRejectedByPatient(int patientAppointmentId, string cancelReason)
        {
            var appointment = await _uow.AppointmentRepository.FirstAsync(o => o.PatientAppointmentId == patientAppointmentId);
            var doctor = await _doctorService.GetDoctorById(appointment.DoctorId);

            appointment.Status = AppointmentStatus.Rejected;
            appointment.CancelReason = cancelReason;

            _uow.AppointmentRepository.Update(appointment);

            _uow.Commit();

            var email = EmailBuilder.New()
                .To(doctor.Email)
                .Subject($"Consulta do dia {appointment.DateAppointment.ToString("f")} foi cancelada.")
                .Body(MailTemplates.canceledTemplate.Replace("{{PACIENTE_NOME}}", appointment.PatientName)
                                         .Replace("{{DATA_CONSULTA}}", appointment.DateAppointment.ToString("dd/MM/yyyy"))
                                         .Replace("{{HORA_CONSULTA}}", appointment.DateAppointment.ToString("HH:mm"))
                                         .Replace("{{MEDICO_NOME}}", doctor.Name)
                                         .Replace("{{MOTIVO}}", cancelReason))
                .Build();

            await _emailService.SendMail(email);

            return appointment;
        }
    }
}
