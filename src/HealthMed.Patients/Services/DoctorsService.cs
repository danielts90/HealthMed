using HealthMed.Patients.Interfaces.Services;
using HealthMed.Shared.Dtos;
using HealthMed.Shared.Enum;
using HealthMed.Shared.Extensions;

namespace HealthMed.Patients.Services
{
    public class DoctorsService : IDoctorsService
    {
        private readonly HttpClient _httpClient;
        public DoctorsService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("doctors_api");
        }


        public async Task<IEnumerable<DoctorsDto>> GetDoctors(DoctorMedicalSpeciality speciality)
        {
            var url = $"api/Doctors/speciality/{(int)speciality}";
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<DoctorsDto>>(url);
            return response;
        }

        public async Task<DoctorScheduleDto> GetDoctorFreeTime(int doctorId, DateTime dateAppointment)
        {
            var formattedDate = Uri.EscapeDataString(dateAppointment.ToString("yyyy-MM-dd"));
            var url = $"api/Doctors/get-doctor-schedule?doctorId={doctorId}&dateAppointment={formattedDate}";

            var response = await _httpClient.GetDataFromJsonAsync<DoctorScheduleDto>(url);
            return response;



        }
    }
}
