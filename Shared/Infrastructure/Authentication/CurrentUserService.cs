using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Application.Abstractions.Authentication;

namespace Infrastructure.Authentication
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? UserId =>
            _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public string? Email =>
            _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

        public string? Name =>
            _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);

        // public IReadOnlyList<string> Roles =>
        //     _httpContextAccessor.HttpContext?.User?
        //         .FindAll(ClaimTypes.Role)
        //         .Select(r => r.Value)
        //         .ToList() ?? new List<string>();
    }
}
