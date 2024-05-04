using System.Diagnostics;
using Discord.Audio;

// Interface used to create the SendAudio class
public interface ISendAudio {
    Task SendAsync(IAudioClient audioClient, string path);
}

/// <summary>
/// Class with the methods used to play music or sound in voice channels
/// </summary>
public class SendAudio : ISendAudio {
    /// <summary>
    /// This method will play music or sounds in the voice channels by creating a ffmpeg stream that will be outputted with CreatePCMStream to the voice channel where the user is listening to. When the stream is finished the bot will disconnect from the voice channel. Any exceptions will be logged in the log file 
    /// </summary>
    /// <param name="audioClient">
    /// The client that will allow us to obtain functionality inside VCs
    /// </param>
    /// <param name="path">
    /// The string path to the .mp3 file that will be played in the voice channel
    /// </param>
    /// <returns>
    /// Plays the audio on the VC
    /// </returns>
    public async Task SendAsync(IAudioClient audioClient, string path) {
        try {
            using (var ffmpeg = CreateStream(path))
            using (var output = ffmpeg.StandardOutput.BaseStream)
            using (var discord = audioClient.CreatePCMStream(AudioApplication.Mixed)) {
                try {
                    await output.CopyToAsync(discord);
                } catch (Exception ex) {
                    using (StreamWriter outputFile = File.AppendText("logs/logs.log")) {
                        await outputFile.WriteAsync($"{ex}\n");
                    }
                } finally {
                    await Task.Delay(1500);
                    await audioClient.StopAsync();
                }
            }
        } catch (Exception ex) {
            using (StreamWriter outputFile = File.AppendText("logs/logs.log")) {
                await outputFile.WriteAsync($"{ex}\n");
            }
        }
    }

    /// <summary>
    /// Process to stream the audio using ffmpeg
    /// </summary>
    /// <param name="path">
    /// The path to the .mp3 file
    /// </param>
    /// <returns>
    /// A process object that represents the ffmpeg process
    /// </returns>
    private Process CreateStream(string path)
    {
        return Process.Start(new ProcessStartInfo
        {
            FileName = "ffmpeg",
            Arguments = $@"-loglevel quiet -i ""{path}"" -ac 2 -f s16le -ar 48000 pipe:1",
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
        });
    }
}
