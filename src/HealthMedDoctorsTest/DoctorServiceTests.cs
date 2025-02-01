using HealthMed.Doctors.Interfaces.Repositories;
using HealthMed.Doctors.Services;
using Moq;
using Moq.AutoMock;

namespace HealthMedDoctorsTest
{
    [Trait("Category", "Doctors")]
    public class DoctorServiceTests
    {
        private readonly AutoMocker _mocker;
        private readonly DoctorService _doctorsService;
        private readonly Mock<IDoctorRepository> _doctorsRepository;

        public DoctorServiceTests()
        {
            _mocker = new AutoMocker();
            _doctorsService = _mocker.CreateInstance<DoctorService>();
            _doctorsRepository = _mocker.GetMock<IDoctorRepository>();
        }

        [Fact]
        public async Task CreateDoctor_WhenDoctorExists_ShouldThrowException()
        {
            //Arrange
        }

    }
}
