using HealthMed.Doctors.Entities;
using HealthMed.Doctors.Interfaces.Repositories;
using HealthMed.Doctors.Interfaces.UnitOfWork;
using HealthMed.Doctors.Services;
using HealthMed.Doctors.UnitOfWorks;
using HealthMed.Shared.Exceptions;
using HealthMed.Shared.Util;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Moq.AutoMock;
using System.Linq.Expressions;

namespace HealthMedDoctorsTest
{
    [Trait("Category", "Doctors")]
    public class DoctorServiceTests
    {
        private readonly AutoMocker _mocker;
        private readonly DoctorService _doctorsService;
        private readonly Mock<IDoctorRepository> _doctorsRepository;
        private readonly Mock<IUserContext> _userContext;
        private readonly Mock<IUnitOfWork> _uow;

        public DoctorServiceTests()
        {
            _mocker = new AutoMocker();
            _doctorsService = _mocker.CreateInstance<DoctorService>();
            _doctorsRepository = _mocker.GetMock<IDoctorRepository>();
            _userContext = _mocker.GetMock<IUserContext>();
            _uow = _mocker.GetMock<IUnitOfWork>();
        }

        [Fact]
        public async Task CreateDoctor_WhenDoctorExists_ShouldThrowException()
        {
            //Arrange
            var doctor = new Doctor();
            CreateUserContextMock();

            _uow.Setup(repo => repo.DoctorRepository.FirstAsync(It.IsAny<Expression<Func<Doctor, bool>>>()))
                .ReturnsAsync(doctor);

            //Act
            var exception = await Assert.ThrowsAsync<RegisterAlreadyExistsException>(() => _doctorsService.CreateDoctor(doctor));
        }

        [Fact]
        public async Task CreateDoctor_WhenDoctorNotExists_ShouldCreate_Doctor()
        {
            //Arrange
            var doctor = new Doctor();
            CreateUserContextMock();

            _uow.Setup(repo => repo.DoctorRepository.FirstAsync(It.IsAny<Expression<Func<Doctor, bool>>>()))
                .ReturnsAsync((Doctor)null);

            _uow.Setup(repo => repo.DoctorRepository.Add(It.IsAny<Doctor>()))
                .Returns(doctor);

            //Act
            var result = await _doctorsService.CreateDoctor(doctor);

            //Assert
            _uow.Verify(repo => repo.DoctorRepository.Add(It.IsAny<Doctor>()), Times.Once);
        }

        [Fact]
        public async Task GetAllDoctors_ShouldReturnDoctorsList()
        {
            // Arrange
            var doctorList = new List<Doctor>
            {
                new Doctor { UserId = 1, Name = "Dr. House" },
                new Doctor { UserId = 2, Name = "Dr. Strange" }
            };

            
            _uow.Setup(repo => repo.DoctorRepository.GetDataAsync(
                It.IsAny<Expression<Func<Doctor, bool>>>(),
                It.IsAny<Func<IQueryable<Doctor>, IIncludableQueryable<Doctor, object>>>(),
                It.IsAny<int?>(),
                It.IsAny<int?>()
            )).ReturnsAsync(doctorList);

            // Act
            var result = await _doctorsService.GetAllDoctors();

            // Assert
            _uow.Verify(repo => repo.DoctorRepository.GetDataAsync(
                It.IsAny<Expression<Func<Doctor, bool>>>(),
                It.IsAny<Func<IQueryable<Doctor>, IIncludableQueryable<Doctor, object>>>(),
                It.IsAny<int?>(),
                It.IsAny<int?>()
            ), Times.Once);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, doctor => doctor.Name == "Dr. House");
            Assert.Contains(result, doctor => doctor.Name == "Dr. Strange");
        }


        [Fact]
        public async Task GetDoctorById_ShouldReturnDoctor()
        {
            // Arrange
            var doctor = new Doctor { UserId = 1, Name = "Dr. House" };

            _uow.Setup(repo => repo.DoctorRepository.GetByIdAsync(1))
                             .ReturnsAsync(doctor);

            // Act
            var result = await _doctorsService.GetDoctorById(1);

            // Assert
            _uow.Verify(repo => repo.DoctorRepository.GetByIdAsync(1), Times.Once);

            Assert.NotNull(result);
            Assert.Equal(result.Name, doctor.Name);
        }

        private void CreateUserContextMock()
        {
            _userContext.Setup(user => user.GetJwtToken()).Returns("fake-jwt");
            _userContext.Setup(user => user.GetName()).Returns("doctor");
            _userContext.Setup(user => user.GetUserEmail()).Returns("doctor@email.com");
            _userContext.Setup(user => user.GetUserId()).Returns(1);
        }
    }
}
