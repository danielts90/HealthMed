using HealthMed.Doctors.Entities;
using HealthMed.Doctors.Interfaces.Repositories;
using HealthMed.Doctors.Interfaces.Services;
using HealthMed.Shared.Exceptions;
using HealthMed.Shared.Util;

namespace HealthMed.Doctors.Services
{
    public class DoctorsWorkTimeService : IDoctorsWorkTimeService 
    {
        private readonly IDoctorsWorkTimeRepository _doctorsWorkTimeRepository;
        private readonly IDoctorService _doctorService;
        private readonly IUserContext _userContext;

        public DoctorsWorkTimeService(IDoctorsWorkTimeRepository doctorsWorkTimeRepository,
                                      IUserContext userContext,
                                      IDoctorService doctorService)
        {
            _doctorsWorkTimeRepository = doctorsWorkTimeRepository;
            _userContext = userContext;
            _doctorService = doctorService;
        }

        public async Task<DoctorsWorkTime> AddWorkTime(int doctorId, DoctorsWorkTime doctorWorkTime)
        {
            await CheckDoctor(doctorId);
            await CheckRegister(doctorId, doctorWorkTime);
            return await _doctorsWorkTimeRepository.AddAsync(doctorWorkTime);
        }

        public async Task<DoctorsWorkTime> UpdateWorkTime(int doctorId, DoctorsWorkTime doctorWorkTime)
        {
            await CheckDoctor(doctorId);
            await CheckRegister(doctorId, doctorWorkTime);
            return await _doctorsWorkTimeRepository.UpdateAsync(doctorWorkTime);
        }

        public async Task<IEnumerable<DoctorsWorkTime>> GetDoctorWorkTime(int doctorId)
        {
            return await _doctorsWorkTimeRepository.FindByAsync(o => o.DoctorId == doctorId);
        }

        private async Task CheckRegister(int doctorId, DoctorsWorkTime doctorWorkTime)
        {
            var existentRegister = await _doctorsWorkTimeRepository.FirstOrDefaultAsync(o => o.DoctorId == doctorId && o.WeekDay == doctorWorkTime.WeekDay);
            if (existentRegister is DoctorsWorkTime) throw new RegisterAlreadyExistsException($"Já existe um registro de horário para {(DayOfWeek)doctorWorkTime.WeekDay}");
        }
        private async Task CheckDoctor(int doctorId) 
        {
            var doctor = await _doctorService.GetDoctorById(doctorId);
            if (doctor is null) throw new RegisterNotFoundException("Médico não encontrado na base de dados.");
            if (doctor.UserId != _userContext.GetUserId()) throw new InvalidOperationException("Não é possível alterar o registro de outro médico.");
        }
    }
}
