namespace HealthMed.Shared.Dtos
{
    public class EmailDto
    {
        public string From { get; set; } = "naoresponder@healthmed.com.br";
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
