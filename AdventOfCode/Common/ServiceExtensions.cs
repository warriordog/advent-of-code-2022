using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AdventOfCode.Common;

public static class ServiceExtensions
{
    public static T CreateInstance<T>(this IServiceProvider provider, params object[] args) => ActivatorUtilities.CreateInstance<T>(provider, args);
    public static T CreateInstance<T>(this IServiceProvider provider, Type implementationType, params object[] args) => (T)ActivatorUtilities.CreateInstance(provider, implementationType, args);
    public static object CreateInstance(this IServiceProvider provider, Type implementationType, params object[] args) => ActivatorUtilities.CreateInstance(provider, implementationType, args);

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