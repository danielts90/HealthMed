using HealthMed.Patients.Interfaces.Services;
using HealthMed.Shared.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Patients.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "RequirePatientRole")]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorsService _doctorsService;

        public DoctorsController(IDoctorsService doctorsService)
        {
            _doctorsService = doctorsService;
        }

        [HttpGet]
        [Route("get-doctors")]
        public async Task<IActionResult> GetDoctors(DoctorMedicalSpeciality speciality)
        {
            try
            {
                var response = await _doctorsService.GetDoctors(speciality);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-doctor-schedule")]
        public async Task<IActionResult> GetDoctorSchedule([FromQuery]int doctorId, [FromQuery]DateTime dateAppointment)
        {
            try
            {
                var response = await _doctorsService.GetDoctorFreeTime(doctorId, dateAppointment);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
