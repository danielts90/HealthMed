using HealthMed.Patients.Entities;
using HealthMed.Patients.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Patients.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "Patient")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpPost]
        [Route("create-patient")]
        public async Task<IActionResult> CreatePatient([FromBody]Patient patient)
        {
            try
            {
                var result = _patientService.AddPatient(patient);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("update-patient")]
        public async Task<IActionResult> UpdatePatient([FromBody] Patient patient)
        {
            try
            {
                var result = _patientService.UpdatePatient(patient);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }


}
