using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace HealthMed.Shared.Util
{
    public class AuthTokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthTokenHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null && httpContext.Request.Headers.TryGetValue("Authorization", out var token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.ToString().Replace("Bearer ", ""));
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
