namespace Application.Abstractions.Authentication
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? Email { get; }
        string? Name { get; }
        //IReadOnlyList<string> Roles { get; } 
    }
}