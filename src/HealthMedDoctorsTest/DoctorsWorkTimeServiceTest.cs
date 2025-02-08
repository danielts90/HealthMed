using HealthMed.Doctors.Entities;
using HealthMed.Doctors.Interfaces.Repositories;
using HealthMed.Doctors.Interfaces.Services;
using HealthMed.Doctors.Services;
using HealthMed.Shared.Exceptions;
using HealthMed.Shared.Util;
using Moq;
using Moq.AutoMock;
using System.Linq.Expressions;

namespace HealthMedDoctorsTest
{
    [Trait("Category", "DoctorsWorkTime")]
    public class DoctorsWorkTimeServiceTest
    {
        private readonly AutoMocker _mocker;
        private readonly DoctorsWorkTimeService _service;
        private readonly Mock<IUserContext> _userContext;
        private readonly Mock<IDoctorsWorkTimeRepository> _repository;
        private readonly Mock<IDoctorService> _doctorService;

        public DoctorsWorkTimeServiceTest()
        {
            _mocker = new AutoMocker();
            _service = _mocker.CreateInstance<DoctorsWorkTimeService>();
            _repository = _mocker.GetMock<IDoctorsWorkTimeRepository>();
            _userContext = _mocker.GetMock<IUserContext>();
            _doctorService = _mocker.GetMock<IDoctorService>();
        }

        [Fact]
        public async Task Add_WorkTime_NotExistent_Doctor_Should_Return_Exception()
        {
            //arrange 
            var doctorId = 1;
            _doctorService.Setup(ds => ds.GetDoctorById(It.IsAny<int>())).ReturnsAsync((Doctor)null);

            //act 
            var exception = await Assert.ThrowsAsync<RegisterNotFoundException>(() => _service.AddWorkTime(doctorId, new DoctorsWorkTime()));

            //Assert
            Assert.Equal(exception.Message, "Médico não encontrado na base de dados.");
        }

        [Fact]
        public async Task Add_WorkTime_Different_Doctor_Should_Return_Exception()
        {
            //arrange 
            var doctorId = 2;
            CreateUserContextMock();
            _doctorService.Setup(ds => ds.GetDoctorById(It.IsAny<int>())).ReturnsAsync(new Doctor { Id = 2});

            //act 
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.AddWorkTime(doctorId, new DoctorsWorkTime()));

            //Assert
            Assert.Equal(exception.Message, "Não é possível alterar o registro de outro médico.");
        }

        [Fact]
        public async Task Update_WorkTime_Different_Doctor_Should_Return_Exception()
        {
            //arrange 
            var doctorId = 2;
            CreateUserContextMock();
            _doctorService.Setup(ds => ds.GetDoctorById(It.IsAny<int>())).ReturnsAsync(new Doctor { Id = 2 });

            //act 
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateWorkTime(doctorId, new DoctorsWorkTime()));

            //Assert
            Assert.Equal(exception.Message, "Não é possível alterar o registro de outro médico.");
        }

        [Fact]
        public async Task Update_WorkTime_Different_Doctor_Should_Be_Ok()
        {
            //arrange 
            var doctor = new Doctor { Id = 1, UserId = 1 };
            var doctorWorkTime = GetValidDoctorWorkTime();
            CreateUserContextMock();

            _doctorService.Setup(ds => ds.GetDoctorById(doctor.Id)).ReturnsAsync(doctor);
            _repository.Setup(repo => repo.UpdateAsync(doctorWorkTime)).ReturnsAsync(doctorWorkTime);


            //act 
            await _service.UpdateWorkTime(doctor.Id, doctorWorkTime);

            //Assert
            _repository.Verify(repo => repo.UpdateAsync(doctorWorkTime), Times.Once());
        }

        [Fact]
        public async Task Add_WorkTime_Existent_Day_Should_Return_Exception()
        {
            //arrange 
            var doctor = new Doctor { Id = 1, UserId = 1 };
            CreateUserContextMock();
            _doctorService.Setup(ds => ds.GetDoctorById(doctor.Id)).ReturnsAsync(doctor);
            _repository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<DoctorsWorkTime, bool>>>())).ReturnsAsync(new DoctorsWorkTime());

            //act 
            var exception = await Assert.ThrowsAsync<RegisterAlreadyExistsException>(() => _service.AddWorkTime(doctor.Id, new DoctorsWorkTime() { WeekDay = 0}));;

            //Assert
            Assert.Equal(exception.Message, "Já existe um registro de horário para Sunday");
        }

        [Fact]
        public async Task Add_WorkTime_Should_Ok()
        {
            //arrange 
            var doctor = new Doctor { Id = 1, UserId = 1 };
            var doctorWorkTime = GetValidDoctorWorkTime();
            CreateUserContextMock();

            _doctorService.Setup(ds => ds.GetDoctorById(doctor.Id)).ReturnsAsync(doctor);
            _repository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<DoctorsWorkTime, bool>>>())).ReturnsAsync((DoctorsWorkTime)null);
            _repository.Setup(repo => repo.AddAsync(doctorWorkTime)).ReturnsAsync(doctorWorkTime);


            //act 
            await _service.AddWorkTime(doctor.Id, doctorWorkTime);

            //Assert
            _repository.Verify(repo => repo.AddAsync(doctorWorkTime), Times.Once());
        }

        [Fact]
        public async Task IsValidWorkTime_ThrowException_WhenDoctorDoesNotWork_OnTheSelectedDay()
        {
            //Arrange
            var dateAppointment = new DateTime(2025, 01, 01);
            var doctorId = 1;

            _repository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<DoctorsWorkTime, bool>>>())).ReturnsAsync((DoctorsWorkTime)null);

            //act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.IsValidWorkTime(dateAppointment, doctorId));

            //Assert
            Assert.Equal(exception.Message, "O médico não atende neste dia da semana.");
        }

        [Fact]
        public async Task IsInvalidWorkTime_Appointment_Out_Doctor_Journey_ThrowException()
        {
            //Arrange
            CreateUserContextMock();
            var doctor = new Doctor { Id = 1, UserId = 1 };
            var doctorWorkTime = GetValidDoctorWorkTime();
            var dateAppointment = new DateTime(2025, 01, 01, 5, 0, 0);

            _repository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<DoctorsWorkTime, bool>>>())).ReturnsAsync(doctorWorkTime);

            //act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.IsValidWorkTime(dateAppointment, doctor.Id));

            //Assert
            Assert.Equal(exception.Message, "O médico atende neste horário.");
        }

        [Fact]
        public async Task IsInvalidWorkTime_Appointment_Inside_Doctor_Interval_ThrowException()
        {
            //Arrange
            CreateUserContextMock();
            var doctor = new Doctor { Id = 1, UserId = 1 };
            var doctorWorkTime = GetValidDoctorWorkTime();
            var dateAppointment = new DateTime(2025, 01, 01, 12, 30, 0);

            _repository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<DoctorsWorkTime, bool>>>())).ReturnsAsync(doctorWorkTime);

            //act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.IsValidWorkTime(dateAppointment, doctor.Id));

            //Assert
            Assert.Equal(exception.Message, "O médico não pode atender no horário de descanso.");
        }

        [Fact]
        public async Task IsInvalidWorkTime_Appointment_Inside_Doctor_Journey_Should_Be_Ok()
        {
            //Arrange
            CreateUserContextMock();
            var doctor = new Doctor { Id = 1, UserId = 1 };
            var doctorWorkTime = GetValidDoctorWorkTime();
            var dateAppointment = new DateTime(2025, 01, 01, 11, 30, 0);

            _repository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<DoctorsWorkTime, bool>>>())).ReturnsAsync(doctorWorkTime);

            //act
            await _service.IsValidWorkTime(dateAppointment, doctor.Id);
        }

        [Fact]
        public async Task GetAllDoctors_ShouldReturnDoctorsList()
        {
            // Arrange
            var workTimes = new List<DoctorsWorkTime>
            {
                GetValidDoctorWorkTime(),
                GetValidDoctorWorkTime() 
            };

            _repository.Setup(repo => repo.FindByAsync(It.IsAny<Expression<Func<DoctorsWorkTime, bool>>>()))
                             .ReturnsAsync(workTimes);

            // Act
            var result = await _service.GetDoctorWorkTime(It.IsAny<int>());

            // Assert
            _repository.Verify(repo => repo.FindByAsync(It.IsAny<Expression<Func<DoctorsWorkTime, bool>>>()), Times.Once);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        private static DoctorsWorkTime GetValidDoctorWorkTime()
        {
            return new DoctorsWorkTime
            {
                DoctorId = 1,
                WeekDay = (int)DayOfWeek.Monday,
                StartTime = new TimeSpan(8, 0, 0),
                StartInterval = new TimeSpan(12, 0, 0),
                FinishInterval = new TimeSpan(13, 0, 0),
                ExitTime = new TimeSpan(17, 0, 0),
                AppointmentDuration = 30
            };
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
