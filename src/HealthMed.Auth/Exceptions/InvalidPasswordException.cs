namespace HealthMed.Auth.Exceptions
{
    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException() : base("Senha inválida.") { }
        public InvalidPasswordException(string message) : base(message) { }
        public InvalidPasswordException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException() : base("Usuário já existe.") { }
        public UserAlreadyExistsException(string message) : base(message) { }
        public UserAlreadyExistsException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class InvalidUserException : Exception
    {
        public InvalidUserException() : base("Usuário inválido.") { }
        public InvalidUserException(string message) : base(message) { }
        public InvalidUserException(string message, Exception innerException) : base(message, innerException) { }
    }
}
