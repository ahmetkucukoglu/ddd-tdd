namespace DDDSample.API.Infrastructure
{
    using DDDSample.Application.Infrastructure;
    using Microsoft.AspNetCore.Http;

    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext.Request.Headers["X-User-Id"];

            return userId;
        }
    }
}
