using HealthMed.Doctors.Interfaces.Repositories;
using HealthMed.Shared.Entities;
using HealthMed.Shared.Repositories.Interfaces;

namespace HealthMed.Doctors.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        bool Commit();
        IDoctorRepository DoctorRepository { get; }
        IAppointmentRepository AppointmentRepository { get; }
        IDoctorsWorkTimeRepository DoctorsWorkTimeRepository { get; }
        IGenericRepositoryDIO<T> Repository<T>() where T : EntityBase;

    }
}
