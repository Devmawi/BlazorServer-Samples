// https://github.com/video-dev/hls.js/blob/master/docs/API.md#first-step-setup-and-support
export function createVideo(id) {
    if (Hls.isSupported()) {
        var video = document.getElementById(id);
        var hls = new Hls({ autoStartLoad: true, levelLoadingMaxRetry: 4 });
        // bind them together
        hls.attachMedia(video);
        // MEDIA_ATTACHED event is fired by hls object once MediaSource is ready
        hls.on(Hls.Events.MEDIA_ATTACHED, function () {
            console.log('video and hls.js are now bound together !');
            hls.loadSource('http://localhost:5000/videos/index.m3u8');
            hls.on(Hls.Events.MANIFEST_PARSED, function (event, data) {
                console.log(
                    'manifest loaded, found ' + data.levels.length + ' quality level'
                );
                video.play();
            });
        });
    }
}