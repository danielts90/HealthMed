namespace HealthMed.Shared.Dtos
{
    public class EmailDto
    {
        public string From { get; } = "naoresponder@healthmed.com.br";
        public string To { get; }
        public string Subject { get; }
        public string Body { get; }

        internal EmailDto(string to, string subject, string body)
        {
            To = to;
            Subject = subject;
            Body = body;
        }
    }
}
