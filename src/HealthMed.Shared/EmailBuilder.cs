using HealthMed.Shared.Dtos;

namespace HealthMed.Shared
{
    public class EmailBuilder
    {
        private string _to;
        private string _subject;
        private string _body;

        internal EmailBuilder()
        {

        }

        public static EmailBuilder New() => new EmailBuilder();


        public EmailBuilder To(string to)
        {
            _to = to;
            return this;
        }

        public EmailBuilder Subject(string subject)
        {
            _subject = subject;
            return this;
        }

        public EmailBuilder Body(string body)
        {
            _body = body;
            return this;
        }

        public EmailDto Build()
        {
            if (string.IsNullOrEmpty(_to))
                throw new ArgumentException("O campo 'To' é obrigatório.");
            if (string.IsNullOrEmpty(_subject))
                throw new ArgumentException("O campo 'Subject' é obrigatório.");
            if (string.IsNullOrEmpty(_body))
                throw new ArgumentException("O campo 'Body' é obrigatório.");

            return new EmailDto(_to, _body, _subject);
        }
    }
}
