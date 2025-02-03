using HealthMed.Doctors.Entities;
using HealthMed.Doctors.Interfaces.Repositories;
using HealthMed.Doctors.Interfaces.Services;
using HealthMed.Doctors.Services;
using HealthMed.Shared.Exceptions;
using HealthMed.Shared.Util;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace HealthMedDoctorsTest
{
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
            var doctorWorkTime = new DoctorsWorkTime
            {
                DoctorId = 1, 
                WeekDay = (int)DayOfWeek.Monday, 
                StartTime = new TimeSpan(8, 0, 0),
                StartInterval = new TimeSpan(12, 0, 0),
                FinishInterval = new TimeSpan(13, 0, 0),
                ExitTime = new TimeSpan(17, 0, 0), 
                AppointmentDuration = 30 
            };

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
            var dateAppointent = new DateTime(2025, 01, 01);
            var doctorId = 1;

            _repository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<DoctorsWorkTime, bool>>>())).ReturnsAsync((DoctorsWorkTime)null);

            //act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.IsValidWorkTime(dateAppointent, doctorId));

            //Assert
            Assert.Equal(exception.Message, "O médico não atende neste dia da semana.");
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
