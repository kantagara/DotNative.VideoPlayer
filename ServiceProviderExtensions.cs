using System;
using Microsoft.Extensions.DependencyInjection;

namespace DotNative.VideoPlayer;

public static class VideoPlayerServiceProviderExtensions
{
#if NET10_0_OR_GREATER
    extension(IServiceProvider services)
    {
        /// <summary>Resolves the registered plugin using the provider's DI lifetime.</summary>
        public IVideoPlayer VideoPlayer => services.GetRequiredService<IVideoPlayer>();
    }
#else
    /// <summary>Resolves the registered plugin using the provider's DI lifetime.</summary>
    public static IVideoPlayer VideoPlayer(this IServiceProvider services) =>
        services.GetRequiredService<IVideoPlayer>();
#endif
}
