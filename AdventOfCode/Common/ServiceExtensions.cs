using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AdventOfCode.Common;

public static class ServiceExtensions
{
    public static T CreateInstance<T>(this IServiceProvider provider) => ActivatorUtilities.CreateInstance<T>(provider);
    public static T CreateInstance<T>(this IServiceProvider provider, Type implementationType) => (T)ActivatorUtilities.CreateInstance(provider, implementationType);
    public static object CreateInstance(this IServiceProvider provider, Type implementationType) => ActivatorUtilities.CreateInstance(provider, implementationType);

    public static OptionsBuilder<TOptions>? TryAddOptions<TOptions>(this IServiceCollection services)
        where TOptions : class
    {
        if (services.All(d => d.ServiceType != typeof(IConfigureOptions<TOptions>)))
        {
            return services.AddOptions<TOptions>();
        }

        return null;
    }
}