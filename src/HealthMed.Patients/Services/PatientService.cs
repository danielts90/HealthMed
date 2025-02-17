using HealthMed.Patients.Entities;
using HealthMed.Patients.Interfaces.Repositories;
using HealthMed.Patients.Interfaces.Services;
using HealthMed.Patients.Interfaces.UnitOfWork;
using HealthMed.Shared.Util;

namespace HealthMed.Patients.Services
{
    public class PatientService : IPatientService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;

        public PatientService(IUserContext userContext,
                              IUnitOfWork uow)
        {
            _userContext = userContext;
            _uow = uow;
        }

        public async Task<Patient> AddPatient(Patient patient)
        {
            var existPatient = await _uow.PatientRepository.FirstAsync(p => p.Cpf == patient.Cpf);
            if (existPatient != null) throw new InvalidOperationException("Já existe um paciente cadastrado com este CPF.");

            patient.UserId = _userContext.GetUserId().Value;
            patient.Name = _userContext.GetName();
            patient.Email = _userContext.GetUserEmail();

            _uow.PatientRepository.Add(patient);

            _uow.Commit();

            return patient;
        }

        public async Task<Patient> GetPatientByPatientId(int PatientId)
        {
            return await _uow.PatientRepository.FirstAsync(o => o.Id == PatientId);
        }

        public async Task<Patient> GetPatientByUserId()
        {
            return await _uow.PatientRepository.FirstAsync(o => o.UserId == _userContext.GetUserId());
        }

        public async Task<Patient> UpdatePatient(Patient patient)
        {
            var existPatient = await _uow.PatientRepository.FirstAsync(p => p.UserId == _userContext.GetUserId());
            if (existPatient is null) throw new InvalidOperationException("Paciente não encontrado.");

            patient.UserId = _userContext.GetUserId().Value;
            _uow.PatientRepository.Update(patient);

            _uow.Commit();

            return patient;
        }
    }
}
