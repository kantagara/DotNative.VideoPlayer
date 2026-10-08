# DotNative.VideoPlayer

Open a native video player with system playback controls:

```csharp
builder.Services.AddVideoPlayer();
await services.GetRequiredService<IVideoPlayer>().OpenAsync(new Uri("https://example.com/movie.mp4"));
```

Android and iOS open the player as a top-level native surface. The renderer does
not yet support inline native views. Supported source schemes are file, HTTP
and HTTPS. This first release has no playlist, subtitles, playback events,
background audio or Windows/macOS/Linux backend. Remote URLs require network
access; iOS app transport security rules still apply.
