using HealthMed.Patients.Interfaces.Repositories;

namespace HealthMed.Patients.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        bool Commit();
        IPatientRepository PatientRepository { get; }
        IAppointmentRepository AppointmentRepository { get; }
    }
}
