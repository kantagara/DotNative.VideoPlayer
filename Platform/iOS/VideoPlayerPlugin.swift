import AVKit
import Foundation
import UIKit

@MainActor final class VideoPlayerPlugin {
  private static var instance: VideoPlayerPlugin?
  static func register() { if instance == nil { instance = VideoPlayerPlugin() } }
  private var player: AVPlayerViewController?
  private init() {
    let channel = NativeChannels.channel(VideoPlayerChannel)
    channel.onReset = { [weak self] in self?.close() }
    channel.handle("open") { [weak self] args, reply in
      guard let self, let text = args.fields["uri"]?.string, let url = URL(string: text), ["file", "https", "http"].contains(url.scheme?.lowercased() ?? "") else { reply.failure("invalid_uri", "A file, HTTP or HTTPS URI is required"); return }
      guard let presenter = NativeChannels.presenter else { reply.failure("unavailable", "No active view controller is available"); return }
      self.close()
      let controller = AVPlayerViewController(); controller.player = AVPlayer(url: url)
      self.player = controller
      presenter.present(controller, animated: true) { controller.player?.play() }
      reply.success()
    }
    channel.handle("close") { [weak self] _, reply in self?.close(); reply.success() }
  }
  private func close() { player?.player?.pause(); player?.dismiss(animated: true); player = nil }
}
