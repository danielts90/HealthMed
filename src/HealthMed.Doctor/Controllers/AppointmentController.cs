﻿using HealthMed.Doctors.Entities;
using HealthMed.Doctors.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Doctors.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        [Route("doctor-appointments/{dateAppointment}")]
        [Authorize(Policy = "RequireDoctorRole")]
        public async Task<IActionResult> GetDoctorAppointments(DateTime dateAppointment)
        {
            try
            {
                var appointments = await _appointmentService.GetAppointmentsByDoctor(dateAppointment);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("create-appointment")]
        [Authorize(Policy = "RequirePatientRole")]
        public async Task<IActionResult> CreateAppointments([FromBody] Appointment appointment)
        {
            try
            {
                var appointments = await _appointmentService.CreateAppointment(appointment);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
