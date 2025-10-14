using ClothingStore.Domain.Enums;
using ClothingStore.Domain.ValueObjects;
using Shared.Domain.Common.DDD;

namespace ClothingStore.Domain.Entities
{
    public class Users : AggregateRoot<Guid>
    {
        // Identity
        public string? Email { get; private set; } = default!;
        public string UserName { get; private set; } = default!;
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public string PasswordHash { get; private set; } = default!;
        public string? PhoneNumber { get; private set; }
        public string? Address { get; private set; }

        public ICollection<RoleType> Roles { get; private set; } = new List<RoleType>();
        

        // EF Core constructor
        private Users() { }

        // Factory method
        public static Users Create(string? email, string userName, string passwordHash)
        {
            var user = new Users
            {
                Email = EmailVal.From(email!).Value,
                UserName = userName,
                PasswordHash = passwordHash,
                IsActive = true,
                Roles = new List<RoleType> { RoleType.Customer , RoleType.Guest }
            };

            // Domain event disabled for now
            // user.RaiseEvent(new UserRegisteredEvent(user.Id, user.Email!, user.UserName));

            return user;
        }

        // Domain behaviors
        public void ChangeEmail(string newEmail)
        {
            Email = newEmail;
            ModifiedAt = DateTime.UtcNow;
            // RaiseEvent(new UserEmailChangedEvent(Id, newEmail));
        }

        public void ChangePassword(string newPasswordHash)
        {
            PasswordHash = newPasswordHash;
            ModifiedAt = DateTime.UtcNow;
            // RaiseEvent(new UserPasswordChangedEvent(Id));
        }

        public void UpdateProfile(string? firstName, string? lastName, string? phone, string? address)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phone;
            Address = address;
            ModifiedAt = DateTime.UtcNow;
        }

        public void DeactivateUser()
        {
            IsActive = false;
            ModifiedAt = DateTime.UtcNow;
            // RaiseEvent(new UserDeactivatedEvent(Id));
        }

        // Apply domain events (disabled for now)
        protected override void Apply(IDomainEvent @event)
        {
            // No-op since events are turned off
        }
    }
}
