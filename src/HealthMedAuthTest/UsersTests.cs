using HealthMed.Auth.Entities;
using HealthMed.Shared.Exceptions;
using HealthMed.Auth.Interfaces.Services;
using HealthMed.Auth.Repositories.Interfaces;
using HealthMed.Auth.Services;
using Microsoft.AspNetCore.Identity;
using Moq;
using Moq.AutoMock;
using System.Linq.Expressions;

namespace HealthMedAuthTest
{
    [Trait("Category", "Users")]
    public class UsersTests
    {
        private readonly AutoMocker _mocker;
        private readonly UserService _userService;
        private readonly Mock<IUserRepository> _usersRepo;
        private readonly Mock<IPasswordHasher<User>> _passwordHasher;
        private readonly Mock<IJwtService> _jwtService;

        public UsersTests()
        {
            _mocker = new AutoMocker();
            _usersRepo = _mocker.GetMock<IUserRepository>();
            _passwordHasher = _mocker.GetMock<IPasswordHasher<User>>();
            _jwtService = _mocker.GetMock<IJwtService>();
            _userService = _mocker.CreateInstance<UserService>();
        }

        [Fact]
        public async Task CreateUser_WhenUserAlreadyExists_ShouldThrowException()
        {
            // Arrange
            var testEmail = "teste@teste.com";
            var user = new User { Email = testEmail };

            _usersRepo.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(user);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UserAlreadyExistsException>(() => _userService.CreateUser(user));

            // Verify
            _usersRepo.Verify(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task CreateUser_WhenUserDoesNotExist_ShouldSucceed()
        {
            // Arrange
            var testEmail = "teste@teste.com";
            var testPassword = "AAAAA";
            var user = new User { Email = testEmail, Password = testPassword };

            _usersRepo.Setup(repo => repo.AddAsync(It.IsAny<User>())).ReturnsAsync(user);
            _passwordHasher.Setup(hasher => hasher.HashPassword(user, user.Password)).Returns("HashedPassword");

            // Act
            var result = await _userService.CreateUser(user);

            // Assert
            Assert.Equal("HashedPassword", result.Password);
            _passwordHasher.Verify(hasher => hasher.HashPassword(user, testPassword), Times.Once);
            _usersRepo.Verify(repo => repo.AddAsync(user), Times.Once);
        }

        [Fact]
        public async Task Login_WhenUserDoesNotExist_ShouldThrowInvalidUserException()
        {
            // Arrange
            _usersRepo.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidUserException>(() => _userService.Login("email@teste.com", "senha"));

            // Verify
            _usersRepo.Verify(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task Login_WhenPasswordIsIncorrect_ShouldThrowInvalidPasswordException()
        {
            // Arrange
            var testEmail = "teste@teste.com";
            var testPassword = "wrongpassword";
            var hashedPassword = "hashedPassword123";
            var user = new User { Email = testEmail, Password = hashedPassword };

            _usersRepo.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(user);

            _passwordHasher.Setup(hash => hash.VerifyHashedPassword(user, hashedPassword, testPassword))
                .Returns(PasswordVerificationResult.Failed);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidPasswordException>(() => _userService.Login(testEmail, testPassword));

            // Verify
            _usersRepo.Verify(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
            _passwordHasher.Verify(hash => hash.VerifyHashedPassword(user, hashedPassword, testPassword), Times.Once);
        }

        [Fact]
        public async Task Login_WhenCredentialsAreValid_ShouldReturnToken()
        {
            // Arrange
            var testEmail = "teste@teste.com";
            var testPassword = "password123";
            var hashedPassword = "hashedPassword123";
            var fakeToken = "fake-jwt-token";
            var user = new User { Email = testEmail, Password = hashedPassword };

            _usersRepo.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(user);

            _passwordHasher.Setup(hash => hash.VerifyHashedPassword(user, hashedPassword, testPassword))
                .Returns(PasswordVerificationResult.Success);

            _jwtService.Setup(jwt => jwt.GetJwtToken(user)).Returns(fakeToken);

            // Act
            var token = await _userService.Login(testEmail, testPassword);

            // Assert
            Assert.Equal(fakeToken, token);
        }
    }
}