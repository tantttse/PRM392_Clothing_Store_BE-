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

        public string? ProfileImageUrl { get; private set; }
        public string? RefreshToken { get; private set; }
        public DateTime? RefreshTokenExpiry { get; private set; }
        public ICollection<RoleType> Roles { get; private set; } = new List<RoleType>();
        public virtual ICollection<Cart> Carts { get; private set; } = new List<Cart>();

        // EF Core constructor
        private Users() { }

        public static Users Create(
            string? email,
            string userName,
            string passwordHash,
            string? firstName = null,
            string? lastName = null,
            string? phoneNumber = null,
            string? address = null,
            string? profileImageUrl = null,
            RoleType? role = null)
        {
            var user = new Users
            {
                Email = EmailVal.From(email!).Value,
                UserName = userName,
                PasswordHash = passwordHash,
                ProfileImageUrl = profileImageUrl,
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                Address = address,
                IsActive = true,
                Roles = new List<RoleType> { RoleType.Customer }
            };

            // Only add the role if it's provided and not already in the list
            if (role.HasValue && !user.Roles.Contains(role.Value))
                user.Roles.Add(role.Value);

            // Domain event (optional, currently disabled)
            // user.RaiseEvent(new UserRegisteredEvent(user.Id, user.Email!, user.UserName))
            return user;
        }


        // Domain behaviors
        public void ChangeEmail(string newEmail)
        {
            
            Email = newEmail;
            // RaiseEvent(new UserEmailChangedEvent(Id, newEmail));
        }

        public void ChangePassword(string newPasswordHash)
        {
            PasswordHash = newPasswordHash;
            // RaiseEvent(new UserPasswordChangedEvent(Id));
        }

        public void UpdateProfile(string? firstName, string? lastName, string? phone, string? address, string? profileImageUrl)
        {
            FirstName = firstName ?? FirstName;
            LastName = lastName ?? LastName;
            PhoneNumber = phone ?? PhoneNumber;
            Address = address ?? Address;
            ProfileImageUrl = profileImageUrl ?? ProfileImageUrl;
        }


        public void DeactivateUser()
        {
            IsActive = false;
            // RaiseEvent(new UserDeactivatedEvent(Id));
        }

        public void AssignRole(RoleType role)
        {
            if (!Roles.Contains(role))
                Roles.Add(role);
        }

        public void RemoveRole(RoleType role)
        {
            if (Roles.Contains(role))
                Roles.Remove(role);
        }

        public Cart CreateNewCart()
        {
            if (Carts.Any(c => c.Status == "Active"))
                throw new InvalidOperationException("User already has an active cart.");

            var cart = Cart.Create(Id);
            Carts.Add(cart);
            return cart;
        }

        public void SetRefreshToken(string token, DateTime expiry)
        {
            RefreshToken = token;
            RefreshTokenExpiry = expiry;
        }

        // Apply domain events (disabled for now)
        protected override void Apply(IDomainEvent @event)
        {
            // No-op since events are turned off
        }
    }
}
