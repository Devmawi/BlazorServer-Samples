using LibVLCSharp.Shared;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorRtspStream
{
    public class RtspBackgroundService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Load native libvlc library
            Core.Initialize();

            // Record in a file "record.ts" located in the bin folder next to the app
            var currentDirectory = Path.GetDirectoryName(AppContext.BaseDirectory);
            var playListFile = Path.Combine(currentDirectory, "wwwroot", "videos", "index.m3u8");
            //var destination2 = Path.Combine(currentDirectory, "mystream.ogg");
            var segmentName = "segment-########.ts";
            var segmentFile = Path.Combine(currentDirectory, "wwwroot", "videos", segmentName);

            using var libVLC = new LibVLC(enableDebugLogs: true);
            using var media = new Media(libVLC, 
                                new Uri("YOUR RSTP ADDRESS")
                                // https://wiki.videolan.org/Documentation:Streaming_HowTo/Streaming_for_the_iPhone/
                                // https://wiki.videolan.org/Documentation:Streaming_HowTo/Command_Line_Examples/#HTTP_streaming
                                , ":sout=#standard{access=livehttp{seglen=10,delsegs=true,numsegs=5,index=" + playListFile + ",index-url=http://localhost:5000/videos//" + segmentName + "},mux=ts{use-key-frames},dst=" + segmentFile + "}"
                                , ":sout-keep"
                                );
            using var mp = new MediaPlayer(media);
            
            libVLC.Log += (sender, e) => Console.WriteLine($"[{e.Level}] {e.Module}:{e.Message}");
            mp.Play(media);
           
            while (!stoppingToken.IsCancellationRequested) { await Task.Delay(3000, stoppingToken); };
            
            await Task.CompletedTask;
        }
    }
}
