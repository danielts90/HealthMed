using HealthMed.Doctors.Context;
using HealthMed.Doctors.Interfaces.Repositories;
using HealthMed.Doctors.Interfaces.UnitOfWork;
using HealthMed.Doctors.Repositories;
using HealthMed.Shared.Repositories.Interfaces;
using HealthMed.Shared.Repositories;
using HealthMed.Shared.Entities;
using HealthMed.Shared.Repos;

namespace HealthMed.Doctors.UnitOfWorks
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly HealthMedDoctorsDbContext _context;
        private readonly Dictionary<Type, object> _repositories;

        public UnitOfWork(HealthMedDoctorsDbContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }

        private IDoctorRepository _doctorRepository;
        private IAppointmentRepository _appointmentRepository;
        private IDoctorsWorkTimeRepository _doctorsWorkTimeRepository;
        
        public IDoctorRepository DoctorRepository { get => _doctorRepository ?? (_doctorRepository = new DoctorRepository(_context)); }
        public IAppointmentRepository AppointmentRepository { get => _appointmentRepository ?? (_appointmentRepository = new AppointmentRepository(_context)); }
        public IDoctorsWorkTimeRepository DoctorsWorkTimeRepository { get => _doctorsWorkTimeRepository ?? (_doctorsWorkTimeRepository = new DoctorsWorkTimeRepository(_context)); }

        /// <summary>
        /// This method avoids the necesseties to create repositories.
        /// It will automate the use of GenericRepo. 
        /// </summary>
        /// <typeparam name="T">EntityBse</typeparam>
        /// <returns></returns>
        public IGenericRepositoryDIO<T> Repository<T>() where T : EntityBase
        {
            if (_repositories.ContainsKey(typeof(T)))
                return (IGenericRepositoryDIO<T>)_repositories[typeof(T)];

            var repository = new GenericRepositoryDIO<T>(_context);
            _repositories.Add(typeof(T), repository);

            return repository;
        }


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
