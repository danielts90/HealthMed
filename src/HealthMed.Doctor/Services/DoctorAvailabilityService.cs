﻿using HealthMed.Doctors.Interfaces.Services;
using HealthMed.Shared.Dtos;
using HealthMed.Shared.Enum;

namespace HealthMed.Doctors.Services
{
    public class DoctorAvailabilityService : IDoctorAvailabilityService
    {
        private readonly IDoctorsWorkTimeService _workTimeService;
        private readonly IAppointmentService _appointmentService;

        public DoctorAvailabilityService(IDoctorsWorkTimeService workTimeService, IAppointmentService appointmentService)
        {
            _workTimeService = workTimeService;
            _appointmentService = appointmentService;
        }

        public async Task<DoctorScheduleDto> GetAvailableSlots(int doctorId, DateTime date)
        {
            var doctorWorkTimes = await _workTimeService.GetDoctorWorkTime(doctorId);
            var workTimeInDateAppointment = doctorWorkTimes.FirstOrDefault(d => d.WeekDay == (int)date.DayOfWeek);

            if (workTimeInDateAppointment == null)
                return new DoctorScheduleDto();

            var appointments = await _appointmentService.GetAppointmentsByDoctor(date, doctorId);

            var occupiedSlots = appointments?.Where(a => a.Status != AppointmentStatus.Rejected)
                                      .Select(a => a.DateAppointment.TimeOfDay)
                                      .ToHashSet() ?? new HashSet<TimeSpan>();



            List<TimeSpan> availableSlots = new List<TimeSpan>();

            TimeSpan startTime = workTimeInDateAppointment.StartTime;
            TimeSpan startInterval = workTimeInDateAppointment.StartInterval;
            TimeSpan finishInterval = workTimeInDateAppointment.FinishInterval;
            TimeSpan exitTime = workTimeInDateAppointment.ExitTime;
            TimeSpan appointmentDuration = TimeSpan.FromMinutes(workTimeInDateAppointment.AppointmentDuration);

            TimeSpan currentSlot = startTime;
            while (currentSlot < startInterval)
            {
                if (!occupiedSlots.Contains(currentSlot))
                    availableSlots.Add(currentSlot);

                currentSlot = currentSlot.Add(appointmentDuration);
            }

            currentSlot = finishInterval;
            while (currentSlot < exitTime)
            {
                if (!occupiedSlots.Contains(currentSlot))
                    availableSlots.Add(currentSlot);

                currentSlot = currentSlot.Add(appointmentDuration);
            }

            return new DoctorScheduleDto { Times  = availableSlots, Price = workTimeInDateAppointment.AppointmentPrice };
        }
    }
}
