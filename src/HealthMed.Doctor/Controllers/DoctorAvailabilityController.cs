using HealthMed.Doctors.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Doctors.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorAvailabilityController : ControllerBase
    {
        private readonly IDoctorAvailabilityService _doctorAvailabilityService;

        public DoctorAvailabilityController(IDoctorAvailabilityService doctorAvailabilityService)
        {
            _doctorAvailabilityService = doctorAvailabilityService;
        }

        [HttpGet]
        [Route("slots")]
        [Authorize(Policy = "RequirePatientRole")]
        public async Task<IActionResult> GetAvailableSlots([FromQuery] int doctorId, [FromQuery] DateTime dateAppointment)
        {
            try
            {
                var slots = await _doctorAvailabilityService.GetAvailableSlots(doctorId, dateAppointment);
                return Ok(slots);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
