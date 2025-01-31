using HealthMed.Auth.Entities;
using HealthMed.Auth.Enum;
using HealthMed.Auth.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthMedAuthTest
{
    public class JwtServiceTests
    {
        [Fact]
        public void GetJwtToken_Should_Return_Valid_Token()
        {
            // Arrange
            var configMock = new Mock<IConfiguration>();
            var jwtSettingsSection = new Mock<IConfigurationSection>();

            configMock.Setup(c => c["JwtSettings:Secret"]).Returns("supersecretkey12345678901234567890");
            configMock.Setup(c => c["JwtSettings:Issuer"]).Returns("testIssuer");
            configMock.Setup(c => c["JwtSettings:Audience"]).Returns("testAudience");
            configMock.Setup(c => c["JwtSettings:ExpirationMinutes"]).Returns("60");

            var jwtService = new JwtService(configMock.Object);

            var user = new User
            {
                Id = 1,
                Name = "Test User",
                Email = "test@example.com",
                UserType = UserType.Doctor
            };

            // Act
            var token = jwtService.GetJwtToken(user);

            // Assert
            Assert.NotNull(token);
            Assert.NotEmpty(token);

            // Validar token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("supersecretkey12345678901234567890");

            var validationParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = "testIssuer",
                ValidateAudience = true,
                ValidAudience = "testAudience",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParams, out SecurityToken validatedToken);

            Assert.NotNull(validatedToken);
            Assert.Equal("testIssuer", ((JwtSecurityToken)validatedToken).Issuer);
            Assert.Equal("testAudience", ((JwtSecurityToken)validatedToken).Audiences.First());
            //Assert.Equal(user.Id.ToString(), principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value);
            //Assert.Equal(user.Name, principal.FindFirst(JwtRegisteredClaimNames.Name)?.Value);
            //Assert.Equal(user.Email, principal.FindFirst(JwtRegisteredClaimNames.Email)?.Value);
            //Assert.Equal(user.UserType.ToString(), principal.FindFirst(ClaimTypes.Role)?.Value);

        }
    }
}