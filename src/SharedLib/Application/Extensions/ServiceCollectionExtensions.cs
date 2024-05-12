using Astrum.SharedLib.Application.Behaviours;
using Astrum.SharedLib.Application.Mappings.Resolvers;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.SharedLib.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        var currentAssembly = typeof(PhoneNumberToStringConverter).Assembly;
        services.AddScoped<PhoneNumberToStringConverter>();
        services.AddScoped<StringToPhoneNumberConverter>();

        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(currentAssembly);
            // cfg.AddProfile<SpecificationProfile>();
            // cfg.AddProfile<OrderProfile>();
        });
        services.AddValidatorsFromAssembly(currentAssembly);
        services.AddMediatR(currentAssembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        // services.AddTransient(typeof(SpecificationTypeConverter<,>));
        // services.AddTransient(typeof(SpecificationInterfaceTypeConverter<,>));
    }
}