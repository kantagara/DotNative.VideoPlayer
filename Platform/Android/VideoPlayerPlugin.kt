package com.dotnative.plugins

import android.app.Activity
import android.app.AlertDialog
import android.net.Uri
import android.widget.MediaController
import android.widget.VideoView

class VideoPlayerPlugin(private val activity: Activity) {

    private var dialog: AlertDialog? = null

    init {

        val channel = NativeChannels.channel("dotnative.video-player")
        channel.onReset = {
            NativeChannels.main.post {
                close()
            }
        }
        channel.handle("open") { args, reply ->
            val text = (args as? Map<*, *>)?.get("uri") as? String
            val uri = text?.let {
                runCatching {
                    Uri.parse(it)
                }
                    .getOrNull()
            }
            if (uri == null || uri.scheme !in setOf("file", "http", "https")) {

                reply.failure("invalid_uri", "A file, HTTP or HTTPS URI is required")
                return@handle
            }
            NativeChannels.main.post {
                try {

                    close()
                    val video = VideoView(activity)
                    video.setVideoURI(uri)
                    video.setMediaController(
                        MediaController(activity).also {
                            it.setAnchorView(video)
                        },
                    )
                    dialog =
                        AlertDialog.Builder(activity)
                            .setView(video)
                            .setOnCancelListener {
                                close()
                            }
                            .create()
                    dialog?.setOnDismissListener {
                        dialog = null
                    }
                    dialog?.show()
                    video.start()
                    reply.success()
                } catch (error: Exception) {

                    reply.failure("playback_failed", error.message ?: "Could not open video")
                }
            }
        }
        channel.handle("close") { _, reply ->
            NativeChannels.main.post {
                close()
                reply.success()
            }
        }
    }

    private fun close() {

        dialog?.dismiss()
        dialog = null
    }
}
