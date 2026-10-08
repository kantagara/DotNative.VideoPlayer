using DotNative.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DotNative.VideoPlayer;

public interface IVideoPlayer
{
    Task OpenAsync(Uri source, CancellationToken cancellationToken = default);
    Task CloseAsync(CancellationToken cancellationToken = default);
}

public static class VideoPlayerServices
{
    public static IServiceCollection AddVideoPlayer(this IServiceCollection services)
    {
        services.TryAddSingleton<IVideoPlayer, NativeVideoPlayer>();
        return services;
    }
}

internal sealed class NativeVideoPlayer(IPlatformChannels channels) : IVideoPlayer
{
    public Task OpenAsync(Uri source, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (source.Scheme is not ("http" or "https" or "file"))
            throw new ArgumentException(
                "Only HTTP, HTTPS and file URIs are supported.",
                nameof(source)
            );
        return channels
            .Get("dotnative.video-player")
            .InvokeAsync(
                "open",
                new Dictionary<string, object?> { ["uri"] = source.AbsoluteUri },
                cancellationToken
            );
    }

    public Task CloseAsync(CancellationToken cancellationToken = default) =>
        channels
            .Get("dotnative.video-player")
            .InvokeAsync("close", cancellationToken: cancellationToken);
}
