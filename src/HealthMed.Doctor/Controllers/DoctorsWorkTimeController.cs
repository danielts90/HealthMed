using HealthMed.Doctors.Entities;
using HealthMed.Doctors.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Doctors.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "RequireDoctorRole")]
    public class DoctorsWorkTimeController : ControllerBase
    {
        private readonly IDoctorsWorkTimeService _doctorsWorkTimeService;

        public DoctorsWorkTimeController(IDoctorsWorkTimeService doctorsWorkTimeService)
        {
            _doctorsWorkTimeService = doctorsWorkTimeService;
        }

        [HttpPost]
        [Route("add-worktime/{doctorId:int}")]
        public async Task<IActionResult> AddWorkTime(int doctorId,[FromBody]DoctorsWorkTime workTime)
        {
            try
            {
                var result = await _doctorsWorkTimeService.AddWorkTime(doctorId, workTime);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [Route("update-worktime/{doctorId:int}")]
        public async Task<IActionResult> UpdateWorkTime(int doctorId, [FromBody] DoctorsWorkTime workTime)
        {
            try
            {
                var result = await _doctorsWorkTimeService.UpdateWorkTime(doctorId, workTime);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-worktime/{doctorId:int}")]
        public async Task<IActionResult> GetWorkTimeByDoctor(int doctorId)
        {
            try
            {
                var workTime = await _doctorsWorkTimeService.GetDoctorWorkTime(doctorId);
                return Ok(workTime);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
