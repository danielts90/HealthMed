using HealthMed.Patients.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Patients.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "RequirePatientRole")]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorsService doctorsService;

        public DoctorsController(IDoctorsService doctorsService)
        {
            this.doctorsService = doctorsService;
        }
    }
}
