using HealthMed.Patients.Interfaces.Services;
using HealthMed.Shared.Dtos;
using HealthMed.Shared.Enum;

namespace HealthMed.Patients.Services
{
    public class DoctorsService : IDoctorsService
    {
        private readonly HttpClient _httpClient;
        public DoctorsService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("doctors_api");
        }
        public async Task<DoctorScheduleDto> GetDoctorFreeTime(int doctorId, DateTime dateAppointment)
        {
            var url = $"api/doctorsworktime/get-worktime/{doctorId}";
            var response = await _httpClient.GetFromJsonAsync<DoctorScheduleDto>(url);
            return response;
        }

        public Task<IEnumerable<DoctorsDto>> GetDoctors(DoctorMedicalSpeciality speciality)
        {
            throw new NotImplementedException();
        }
    }
}
