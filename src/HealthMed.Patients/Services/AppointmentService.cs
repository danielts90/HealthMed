using HealthMed.Patients.Entities;
using HealthMed.Patients.Interfaces.Repositories;
using HealthMed.Patients.Interfaces.Services;
using HealthMed.Shared.Enum;
using HealthMed.Shared.Exceptions;

namespace HealthMed.Patients.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientService _patientService;

        public AppointmentService(IAppointmentRepository appointmentRepository,
                                  IPatientService patientService)
        {
            _appointmentRepository = appointmentRepository;
            _patientService = patientService;
        }

        public async Task<Appointment> CancelAppointment(int appointmentId, string cancelReason)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
            if (appointment == null) throw new KeyNotFoundException("Consulta não encontrada.");

            var patient = await GetPatientAsync();
            if (patient.Id != appointment.PatientId) throw new InvalidUserException("Paciente não pode alterar a consulta e outro.");

            appointment.CancelReason = cancelReason;
            appointment.Status = AppointmentStatus.Rejected;

            await _appointmentRepository.UpdateAsync(appointment);
            //Todo:
            //Notificar o médico; 

            return appointment;
        }

        public async Task<Appointment> CreateAppointment(Appointment appointment)
        {
            var patient = await GetPatientAsync();
            if(patient.Id != appointment.PatientId) throw new InvalidUserException("Paciente não cirar consulta para outro.");

            await _appointmentRepository.AddAsync(appointment);

            //TODO:
            //Notificar doctorsApi

            return appointment;
        }

        public async Task<IEnumerable<Appointment>> GetAppointments()
        {
            var patient = await GetPatientAsync();
            var appointments = await _appointmentRepository.FindByAsync(o => o.PatientId == patient.Id
                                                                        && o.Status != AppointmentStatus.Rejected
                                                                        && o.DateAppointment > DateTime.Now);

            return appointments;
        }

        private async Task<Patient> GetPatientAsync() => await _patientService.GetPatientByUserId();

    }
}
