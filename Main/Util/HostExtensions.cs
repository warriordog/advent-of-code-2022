using Microsoft.Extensions.Hosting;

namespace Main.Util;

public static class HostExtensions
{
    public static IAsyncDisposable AsAsyncDisposable(this IHost host) => (IAsyncDisposable)host;
}