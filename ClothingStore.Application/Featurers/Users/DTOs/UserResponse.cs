using ClothingStore.Domain.Enums;
using Shared.Application.Abstractions.DTOs;

namespace ClothingStore.Application.Features.User.Dtos
{
    public class UserResponse : BaseDto<Guid>
    {
        public string? Email { get; set; }
        public string UserName { get; set; } = default!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public List<RoleType> Roles { get; set; } = new();
    }
}
