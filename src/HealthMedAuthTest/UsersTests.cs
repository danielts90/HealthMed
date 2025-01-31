using HealthMed.Auth.Entities;
using HealthMed.Auth.Exceptions;
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

        public UsersTests()
        {
            _mocker = new AutoMocker();
        }

        private (UserService userService, Mock<IUserRepository> usersRepo, Mock<IPasswordHasher<User>> passwordHasher, Mock<IJwtService> jwtService) SetupUserService()
        {
            var usersRepo = _mocker.GetMock<IUserRepository>();
            var passwordHasher = _mocker.GetMock<IPasswordHasher<User>>();
            var jwtService = _mocker.GetMock<IJwtService>();
            var userService = _mocker.CreateInstance<UserService>();

            return (userService, usersRepo, passwordHasher, jwtService);
        }

        [Fact]
        public async Task CreateUser_WhenUserAlreadyExists_ShouldThrowException()
        {
            // Arrange
            var (userService, usersRepo, _, _) = SetupUserService();
            var testEmail = "teste@teste.com";
            var user = new User { Email = testEmail };

            usersRepo.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(user);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UserAlreadyExistsException>(() => userService.CreateUser(user));

            // Verify
            usersRepo.Verify(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task CreateUser_WhenUserDoesNotExist_ShouldSucceed()
        {
            // Arrange
            var (userService, usersRepo, passwordHasher, _) = SetupUserService();
            var testEmail = "teste@teste.com";
            var testPassword = "AAAAA";
            var user = new User { Email = testEmail, Password = testPassword };

            usersRepo.Setup(repo => repo.AddAsync(It.IsAny<User>())).ReturnsAsync(user);
            passwordHasher.Setup(hasher => hasher.HashPassword(user, user.Password)).Returns("HashedPassword");

            // Act
            var result = await userService.CreateUser(user);

            // Assert
            Assert.Equal("HashedPassword", result.Password);
            passwordHasher.Verify(hasher => hasher.HashPassword(user, testPassword), Times.Once);
            usersRepo.Verify(repo => repo.AddAsync(user), Times.Once);
        }

        [Fact]
        public async Task Login_WhenUserDoesNotExist_ShouldThrowInvalidUserException()
        {
            // Arrange
            var (userService, usersRepo, _, _) = SetupUserService();

            usersRepo.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidUserException>(() => userService.Login("email@teste.com", "senha"));

            // Verify
            usersRepo.Verify(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task Login_WhenPasswordIsIncorrect_ShouldThrowInvalidPasswordException()
        {
            // Arrange
            var (userService, usersRepo, passwordHasher, _) = SetupUserService();
            var testEmail = "teste@teste.com";
            var testPassword = "wrongpassword";
            var hashedPassword = "hashedPassword123";
            var user = new User { Email = testEmail, Password = hashedPassword };

            usersRepo.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(user);

            passwordHasher.Setup(hash => hash.VerifyHashedPassword(user, hashedPassword, testPassword))
                .Returns(PasswordVerificationResult.Failed);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidPasswordException>(() => userService.Login(testEmail, testPassword));

            // Verify
            usersRepo.Verify(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
            passwordHasher.Verify(hash => hash.VerifyHashedPassword(user, hashedPassword, testPassword), Times.Once);
        }

        [Fact]
        public async Task Login_WhenCredentialsAreValid_ShouldReturnToken()
        {
            // Arrange
            var (userService, usersRepo, passwordHasher, jwtService) = SetupUserService();
            var testEmail = "teste@teste.com";
            var testPassword = "password123";
            var hashedPassword = "hashedPassword123";
            var fakeToken = "fake-jwt-token";
            var user = new User { Email = testEmail, Password = hashedPassword };

            usersRepo.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(user);

            passwordHasher.Setup(hash => hash.VerifyHashedPassword(user, hashedPassword, testPassword))
                .Returns(PasswordVerificationResult.Success);

            jwtService.Setup(jwt => jwt.GetJwtToken(user)).Returns(fakeToken);

            // Act
            var token = await userService.Login(testEmail, testPassword);

            // Assert
            Assert.Equal(fakeToken, token);
        }
    }
}