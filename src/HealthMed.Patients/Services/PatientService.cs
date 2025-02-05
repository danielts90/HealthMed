using HealthMed.Patients.Entities;
using HealthMed.Patients.Interfaces.Repositories;
using HealthMed.Patients.Interfaces.Services;
using HealthMed.Shared.Util;

namespace HealthMed.Patients.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repository;
        private readonly IUserContext _userContext;

        public PatientService(IPatientRepository repository, 
                              IUserContext userContext)
        {
            _repository = repository;
            _userContext = userContext;
        }

        public async Task<Patient> AddPatient(Patient patient)
        {
            var existPatient = await _repository.FirstOrDefaultAsync(p => p.Cpf == patient.Cpf);
            if (existPatient != null) throw new InvalidOperationException("Já existe um paciente cadastrado com este CPF.");

            patient.UserId = _userContext.GetUserId().Value;
            patient.Name = _userContext.GetName();
            patient.Email = _userContext.GetUserEmail();

            return await _repository.AddAsync(patient);
        }

        public async Task<Patient> GetPatientByUserId()
        {
            return await _repository.FirstOrDefaultAsync(o => o.UserId == _userContext.GetUserId());
        }

        public async Task<Patient> UpdatePatient(Patient patient)
        {
            var existPatient = await _repository.FirstOrDefaultAsync(p => p.UserId == _userContext.GetUserId());
            if (existPatient is null) throw new InvalidOperationException("Paciente não encontrado.");

            patient.UserId = _userContext.GetUserId().Value;
            return await _repository.AddAsync(patient);
        }
    }
}
