using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shared.Application.Abstractions.DTOs;

public class CurrentUserModelBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.ModelType == typeof(CurrentUserDto) &&
            context.BindingInfo.BindingSource == BindingSource.Custom)
        {
            return new CurrentUserModelBinder();
        }

        return null!;
    }
}
