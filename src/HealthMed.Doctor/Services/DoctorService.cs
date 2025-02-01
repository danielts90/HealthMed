using HealthMed.Doctors.Entities;
using HealthMed.Doctors.Interfaces.Repositories;
using HealthMed.Doctors.Interfaces.Services;
using HealthMed.Shared.Exceptions;
using HealthMed.Shared.Util;

namespace HealthMed.Doctors.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IUserContext _userContext;
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IUserContext userContext, IDoctorRepository doctorRepository)
        {
            _userContext = userContext;
            _doctorRepository = doctorRepository;
        }

        public async Task<Doctor> CreateDoctor(Doctor doctor)
        {
            PopulateDoctorFromToken(doctor);
            await CheckExistentDoctor(doctor);

            return await _doctorRepository.AddAsync(doctor);
        }

        public async Task<IEnumerable<Doctor>> GetAllDoctors()
        {
            return await _doctorRepository.GetAllAsync();
        }

        public async Task<Doctor?> GetDoctorById(int id)
        {
            return await _doctorRepository.GetByIdAsync(id);
        }

        private void PopulateDoctorFromToken(Doctor doctor)
        {
            doctor.Name = _userContext.GetName();
            doctor.Email = _userContext.GetUserEmail();
            doctor.UserId = _userContext.GetUserId().Value;
        }

        private async Task CheckExistentDoctor(Doctor doctor)
        {
            var existentDoctor = await _doctorRepository.FirstOrDefaultAsync(o => o.UserId == doctor.UserId);
            if (existentDoctor is Doctor) throw new RegisterAlreadyExistsException();
        }
    }
}
