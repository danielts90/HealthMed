using HealthMed.Auth.Entities;
using HealthMed.Shared.Exceptions;
using HealthMed.Auth.Interfaces.Services;
using HealthMed.Auth.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace HealthMed.Auth.Services
{
    public class UserService : IUserService
    {
        private readonly IJwtService _jwtService;
        private readonly IUserRepository _usersRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(IJwtService jwtService, 
                           IUserRepository usersRepository, 
                           IPasswordHasher<User> passwordHasher)
        {
            _jwtService = jwtService;
            _usersRepository = usersRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> CreateUser(User user)
        {
            var existentUser = await _usersRepository.FirstOrDefaultAsync(o => o.Email == user.Email);
            if (existentUser is User)
            {
                throw new UserAlreadyExistsException($"Já existe um usuário com o e-mail {user.Email}");
            }
            user.Password = _passwordHasher.HashPassword(user, user.Password);
            user = await _usersRepository.AddAsync(user);
            return user;
        }

        public async Task<string> Login(string username, string password)
        {
            var existentUser = await _usersRepository.FirstOrDefaultAsync(o => o.Email == username);
            if (existentUser is User)
            {
                var result = _passwordHasher.VerifyHashedPassword(existentUser, existentUser.Password, password);
                if(result == PasswordVerificationResult.Success)
                {
                    var token = _jwtService.GetJwtToken(existentUser);
                    return token;
                }
                throw new InvalidPasswordException();
            }
            throw new InvalidUserException();
        }

        public Task<User> UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
