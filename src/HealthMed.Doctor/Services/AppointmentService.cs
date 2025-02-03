using HealthMed.Doctors.Entities;
using HealthMed.Doctors.Interfaces.Repositories;
using HealthMed.Doctors.Interfaces.Services;
using HealthMed.Shared.Util;

namespace HealthMed.Doctors.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUserContext _userContext;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorsWorkTimeService _doctorsWorkTimeService;

        public AppointmentService(IUserContext userContext, 
                                  IAppointmentRepository appointmentRepository, 
                                  IDoctorsWorkTimeService doctorsWorkTimeService)
        {
            _userContext = userContext;
            _appointmentRepository = appointmentRepository;
            _doctorsWorkTimeService = doctorsWorkTimeService;
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

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctor(DateTime dateAppointments)
        {
            var appointments = await _appointmentRepository.FindByAsync(o => o.DateAppointment.Date == dateAppointments.Date && o.DoctorId == _userContext.GetUserId());
            return appointments;
        }
    }
}
