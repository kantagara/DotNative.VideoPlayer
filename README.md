# DotNative.VideoPlayer

Open a native video player with system playback controls:

```csharp
builder.Services.AddVideoPlayer();
await services.VideoPlayer.OpenAsync(new Uri("https://example.com/movie.mp4"));
```

Android and iOS open the player as a top-level native surface. The renderer does
not yet support inline native views. Supported source schemes are file, HTTP
and HTTPS. This first release has no playlist, subtitles, playback events,
background audio or Windows/macOS/Linux backend. Remote URLs require network
access; iOS app transport security rules still apply.

## Service access

Import `DotNative.VideoPlayer` to access the plugin through `IServiceProvider`:

```csharp
using DotNative.VideoPlayer;

var plugin = services.VideoPlayer;
```

The getter calls `GetRequiredService<IVideoPlayer>()` on every access, preserving
DI lifetimes and the usual missing-registration error. Register the plugin with
`AddVideoPlayer(...)` before building the provider.

A `net10.0` application uses the property syntax with C# 14 or later. A
`net9.0` application uses only the method equivalent:

```csharp
var plugin = services.VideoPlayer();
```

The package contains separate `net9.0` and `net10.0` assemblies. NuGet selects
the assembly matching the application target framework. `NET10_0_OR_GREATER`
selects the property; the `#else` branch selects the method.

Build and pack both targets with .NET 10 SDK. A source build using .NET 9 SDK
builds only `net9.0`; it does not produce the .NET 10 assembly.
