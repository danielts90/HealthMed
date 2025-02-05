using HealthMed.Patients.Entities;
using HealthMed.Patients.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Patients.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "Patient")]
    public class AppointmentsController : ControllerBase
    { 
        private readonly IAppointmentService _appointmentsService;

        public AppointmentsController(IAppointmentService appointmentsService)
        {
            _appointmentsService = appointmentsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointments()
        {
            try
            {
                var appointments = await _appointmentsService.GetAppointments();
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("create-appointment")]
        public async Task<IActionResult> CreateAppointment([FromBody] Appointment appointment)
        {
            try
            {
                var result = await _appointmentsService.CreateAppointment(appointment);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("cancel-appointment/{id:int}")]
        public async Task<IActionResult> CancelAppointment(int id, [FromBody] string cancelReason)
        {
            try
            {
                var result = await _appointmentsService.CancelAppointment(id, cancelReason);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
