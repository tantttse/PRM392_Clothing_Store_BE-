using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shared.Application.Abstractions.DTOs;
using System.Security.Claims;

public class CurrentUserModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext context)
    {
        var user = context.HttpContext.User;

        var userId = Guid.TryParse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : Guid.Empty;
        var email = user.FindFirst(ClaimTypes.Email)?.Value;
        var name = user.FindFirst(ClaimTypes.Name)?.Value;
        var roles = user.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
        var jti = user.FindFirst("jti")?.Value;

        var dto = new CurrentUserDto
        {
            UserId = userId,
            Email = email,
            Name = name,
            Roles = roles,
            Jti = jti
        };

        context.Result = ModelBindingResult.Success(dto);
        return Task.CompletedTask;
    }
}
