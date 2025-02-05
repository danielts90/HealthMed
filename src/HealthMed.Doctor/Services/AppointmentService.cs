using HealthMed.Doctors.Entities;
using HealthMed.Doctors.Interfaces.Repositories;
using HealthMed.Doctors.Interfaces.Services;
using HealthMed.Shared.Enum;
using HealthMed.Shared.Util;

namespace HealthMed.Doctors.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUserContext _userContext;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorsWorkTimeService _doctorsWorkTimeService;
        private readonly IDoctorService _doctorService;

        public AppointmentService(IUserContext userContext,
                                  IAppointmentRepository appointmentRepository,
                                  IDoctorsWorkTimeService doctorsWorkTimeService,
                                  IDoctorService doctorService)
        {
            _userContext = userContext;
            _appointmentRepository = appointmentRepository;
            _doctorsWorkTimeService = doctorsWorkTimeService;
            _doctorService = doctorService;
        }

        public async Task<Appointment> CreateAppointment(Appointment appointment)
        {
            await EnsureAppointmentDoesNotExistAsync(appointment);
            await _doctorsWorkTimeService.IsValidWorkTime(appointment.DateAppointment, appointment.DoctorId);

            return await _appointmentRepository.AddAsync(appointment);
            //Deve enviar um e-mail para o médico...
        }

        private async Task EnsureAppointmentDoesNotExistAsync(Appointment appointment)
        {
            var existentAppointment = await _appointmentRepository.FirstOrDefaultAsync(o => o.DateAppointment == appointment.DateAppointment && o.DoctorId == appointment.DoctorId);
            if (existentAppointment != null) throw new InvalidOperationException("Já existe uma consulta marcada no dia e horário selecionado.");
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctor(DateTime dateAppointments, int? doctorId = null)
        {
            int? IdDoctor = doctorId ?? _userContext.GetUserId();
            var appointments = await _appointmentRepository.FindByAsync(o =>
                o.DateAppointment.Date.ToUniversalTime() == dateAppointments.Date.ToUniversalTime() && o.DoctorId == IdDoctor.Value);
            return appointments;
        }

        private async Task<Appointment> UpdateAppointmentStatus(int appointmentId, AppointmentStatus status)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId)
                              ?? throw new KeyNotFoundException("Consulta não encontrada.");

            ValidateDoctorPermission(appointment.DoctorId);

            appointment.Status = status;
            var updatedAppointment = await _appointmentRepository.UpdateAsync(appointment);

           // Enviar mensagem para a fila
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

        private async void ValidateDoctorPermission(int doctorId)
        {
            var doctor = await _doctorService.GetDoctorById(doctorId)
                         ?? throw new KeyNotFoundException("Médico não encontrado.");

            if (doctor.UserId != _userContext.GetUserId())
                throw new InvalidOperationException("Médico não pode alterar consultas de outros médicos.");
        }
    }
}
