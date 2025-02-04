using HealthMed.Doctors.Entities;
using HealthMed.Doctors.Interfaces.Services;
using HealthMed.Doctors.Services;
using HealthMed.Shared.Enum;
using Moq;
using Moq.AutoMock;

namespace HealthMedDoctorsTest
{
    public class DoctorAvailabilityServiceTest
    {
        private readonly AutoMocker _mocker;
        private readonly DoctorAvailabilityService _service;
        private readonly Mock<IDoctorsWorkTimeService> _workTimeService;
        private readonly Mock<IAppointmentService> _appointmentService;

        public DoctorAvailabilityServiceTest()
        {
            _mocker = new AutoMocker();
            _service = _mocker.CreateInstance<DoctorAvailabilityService>();
            _appointmentService = _mocker.GetMock<IAppointmentService>();
            _workTimeService = _mocker.GetMock<IDoctorsWorkTimeService>();
        }

        [Fact]
        public async Task GetAvailableSlots_DoctorDayOff_ShouldBe_Return_EmptyList()
        {
            //arrange
            var doctorId = 1;
            var dateAppointmet = new DateTime(2024, 11, 11);

            _workTimeService.Setup(wt => wt.GetDoctorWorkTime(doctorId)).ReturnsAsync(new List<DoctorsWorkTime>());

            //act 
            var times = await _service.GetAvailableSlotsAsync(doctorId, dateAppointmet);

            //Assert 
            Assert.NotNull(times);
            Assert.Equal(times.Count(), 0);
        }

        [Fact]
        public async Task GetAvailableSlots_DoctorDay_ShouldBe_Return_List()
        {
            //arrange
            var doctorId = 1;
            var dateAppointmet = new DateTime(2024, 11, 11);
            var doctorWorkTime = new DoctorsWorkTime
            {
                DoctorId = 1,
                WeekDay = 1,
                StartTime = new TimeSpan(8, 0, 0),
                StartInterval = new TimeSpan(12, 0, 0),
                FinishInterval = new TimeSpan(14, 0, 0),
                ExitTime = new TimeSpan(18, 0, 0),
                AppointmentDuration = 60
            };
            var workTimeList = new List<DoctorsWorkTime>
            {
                doctorWorkTime
            };

            var slotsList = new List<TimeSpan>
            {
                new TimeSpan(8, 0, 0),
                new TimeSpan(9, 0, 0),
                new TimeSpan(10, 0, 0),
                new TimeSpan(11, 0, 0),
                new TimeSpan(14, 0, 0),
                new TimeSpan(15, 0, 0),
                new TimeSpan(16, 0, 0),
                new TimeSpan(17, 0, 0)
            };

            _workTimeService.Setup(wt => wt.GetDoctorWorkTime(doctorId)).ReturnsAsync(workTimeList);

            //act 
            var times = await _service.GetAvailableSlotsAsync(doctorId, dateAppointmet);

            //Assert 
            Assert.NotNull(times);
            Assert.Equal(slotsList, times);
        }

        [Fact]
        public async Task GetAvailableSlots_DoctorDay_WithOcuppiedSlots_ShouldBe_Return_List()
        {
            //arrange
            var doctorId = 1;
            var dateAppointmet = new DateTime(2024, 11, 11);
            var doctorWorkTime = new DoctorsWorkTime
            {
                DoctorId = 1,
                WeekDay = 1,
                StartTime = new TimeSpan(8, 0, 0),
                StartInterval = new TimeSpan(12, 0, 0),
                FinishInterval = new TimeSpan(14, 0, 0),
                ExitTime = new TimeSpan(18, 0, 0),
                AppointmentDuration = 60
            };
            var workTimeList = new List<DoctorsWorkTime>
            {
                doctorWorkTime
            };

            var slotsList = new List<TimeSpan>
            {
                new TimeSpan(9, 0, 0),
                new TimeSpan(10, 0, 0),
                new TimeSpan(11, 0, 0),
                new TimeSpan(14, 0, 0),
                new TimeSpan(15, 0, 0),
                new TimeSpan(16, 0, 0),
                new TimeSpan(17, 0, 0)
            };

            var appointments = new List<Appointment>
            {
                new Appointment()
                {
                    DateAppointment = new DateTime(2024,11,11,8,0,0),
                    Status = AppointmentStatus.Created,
                },
                new Appointment()
                {
                    DateAppointment = new DateTime(2024,11,11,9,0,0),
                    Status = AppointmentStatus.Rejected
                }
            };

            _appointmentService.Setup(ap => ap.GetAppointmentsByDoctor(dateAppointmet, doctorId)).ReturnsAsync(appointments);

            _workTimeService.Setup(wt => wt.GetDoctorWorkTime(doctorId)).ReturnsAsync(workTimeList);

            //act 
            var times = await _service.GetAvailableSlotsAsync(doctorId, dateAppointmet);

            //Assert 
            Assert.NotNull(times);
            Assert.Equal(slotsList, times);
        }
    }
}
