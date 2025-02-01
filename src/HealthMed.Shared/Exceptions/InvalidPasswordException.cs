namespace HealthMed.Shared.Exceptions
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

    public class RegisterAlreadyExistsException : Exception
    {
        public RegisterAlreadyExistsException() : base("Registro já existe na base de dados.") { }
        public RegisterAlreadyExistsException(string message) : base(message) { }
        public RegisterAlreadyExistsException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class RegisterNotFoundException : Exception
    {
        public RegisterNotFoundException() : base("Registro não encontrado na base de dados.") { }
        public RegisterNotFoundException(string message) : base(message) { }
        public RegisterNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
