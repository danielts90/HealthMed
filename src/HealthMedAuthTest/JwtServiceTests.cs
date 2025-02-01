using HealthMed.Auth.Entities;
using HealthMed.Auth.Services;
using HealthMed.Shared.Dtos;
using HealthMed.Shared.Enum;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace HealthMedAuthTest
{
    public class JwtServiceTests
    {
        [Fact]
        public void GetJwtToken_Should_Return_Valid_Token()
        {
            // Arrange
            var jwtSettings = new JwtSettings
            {
                Secret = "supersecretkey12345678901234567890",
                Issuer = "testIssuer",
                Audience = "testAudience",
                ExpirationMinutes = 60
            };

            var settingsMock = Options.Create(jwtSettings);
            var jwtService = new JwtService(settingsMock);

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
            var key = Encoding.UTF8.GetBytes(settingsMock.Value.Secret);

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