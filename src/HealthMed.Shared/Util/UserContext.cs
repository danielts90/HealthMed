using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HealthMed.Shared.Util
{
    public interface IUserContext
    {
        string? GetJwtToken();
        string GetName();
        string? GetUserClaim(string claimType);
        string? GetUserEmail();
        int? GetUserId();
    }

    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetJwtToken()
        {
            return _httpContextAccessor.HttpContext?.Request.Headers["Authorization"]
                .FirstOrDefault()?.Replace("Bearer ", "");
        }

        public string GetName()
        {
            return GetUserClaim(ClaimTypes.Name) ?? GetUserClaim("name");
        }

        public string? GetUserClaim(string claimType)
        {
            return _httpContextAccessor.HttpContext?.User.FindFirst(claimType)?.Value;
        }

        public string? GetUserEmail()
        {
            return GetUserClaim(ClaimTypes.Email) ?? GetUserClaim("email");
        }

        public int? GetUserId()
        {
            var userId = GetUserClaim(ClaimTypes.NameIdentifier) ?? GetUserClaim("sub");
            return Convert.ToInt32(userId);
        }
    }
}
