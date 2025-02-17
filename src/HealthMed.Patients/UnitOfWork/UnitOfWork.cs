using HealthMed.Patients.Context;
using HealthMed.Patients.Interfaces.Repositories;
using HealthMed.Patients.Interfaces.UnitOfWork;
using HealthMed.Patients.Repositories;

namespace HealthMed.Patients.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HealthMedPatientsDbContext _context;

        public UnitOfWork(HealthMedPatientsDbContext context)
        {
            _context = context;
        }

        private IPatientRepository _patientRepository;
        private IAppointmentRepository _appointmentRepository;
        
        public IPatientRepository PatientRepository { get => _patientRepository ?? (_patientRepository = new PatientRepository(_context)); }
        public IAppointmentRepository AppointmentRepository { get => _appointmentRepository ?? (_appointmentRepository = new AppointmentRepository(_context)); }

        public bool Commit()
        {
            return _context.SaveChanges() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
