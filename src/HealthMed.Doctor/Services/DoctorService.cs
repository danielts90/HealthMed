using HealthMed.Doctors.Entities;
using HealthMed.Doctors.Interfaces.Services;
using HealthMed.Doctors.Interfaces.UnitOfWork;
using HealthMed.Shared.Enum;
using HealthMed.Shared.Exceptions;
using HealthMed.Shared.Util;

namespace HealthMed.Doctors.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;

        public DoctorService(IUserContext userContext, 
                             IUnitOfWork uow)
        {
            _userContext = userContext;
            _uow = uow;
        }

        public async Task<Doctor> CreateDoctor(Doctor doctor)
        {
            PopulateDoctorFromToken(doctor);
            await CheckExistentDoctor(doctor);

            _uow.DoctorRepository.Add(doctor);

            _uow.Commit();

            return doctor;
        }

        public async Task<IEnumerable<Doctor>> GetAllDoctors()
        {
            return await _uow.DoctorRepository.GetDataAsync();
        }

        public async Task<Doctor?> GetDoctorById(int id)
        {
            return await _uow.DoctorRepository.GetByIdAsync(id);
        }

        private void PopulateDoctorFromToken(Doctor doctor)
        {
            doctor.Name = _userContext.GetName();
            doctor.Email = _userContext.GetUserEmail();
            doctor.UserId = _userContext.GetUserId().Value;
        }

        private async Task CheckExistentDoctor(Doctor doctor)
        {
            var existentDoctor = await _uow.DoctorRepository.FirstAsync(o => o.UserId == doctor.UserId);
            if (existentDoctor is Doctor) throw new RegisterAlreadyExistsException();
        }

        public async Task<IEnumerable<Doctor?>> GetDoctorBySpeciallity(DoctorMedicalSpeciality speciality)
        {
            return await _uow.DoctorRepository.GetDataAsync(o => o.Speciality == speciality);
        }
    }
}
