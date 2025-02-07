using HealthMed.Doctors.Entities;
using HealthMed.Doctors.Interfaces.Services;
using HealthMed.Shared.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Doctors.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpPost]
        [Authorize(Policy = "RequireDoctorRole")]
        public async Task<IActionResult> CreateDoctor(Doctor doctor) 
        {
            try
            {
                var result = await _doctorService.CreateDoctor(doctor);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _doctorService.GetAllDoctors();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            try
            {
                var result = await _doctorService.GetDoctorById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{speciality:DoctorMedicalSpeciality}")]
        public async Task<IActionResult> GetDoctorBySpeciality(DoctorMedicalSpeciality speciality)
        {
            try
            {
                var result = await _doctorService.GetDoctorBySpeciallity(speciality);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
