using Discord;
using Discord.Commands;

/// <summary>
/// Class in charge of executing the Lore command. This command will check if the user is in a voice channel, and play the Lore of the 4th Order of Shenaniganery
/// </summary>
public class LoreModule : ModuleBase<SocketCommandContext>
{
    ISendAudio sendAudio = new SendAudio();

    /// <summary>
    /// This method will do the check if the user is on a voice channel, depending on that, it will send a message to the user if the user is not on a voice channel, if the user is on a voice channel, the bot will join and play a short version of the 4th Order of Shenaniganery lore by calling the SendAsync method of the sendAudio class
    /// </summary>
    /// <param name="voiceChannel">
    /// The parameter voiceChannel is set automatically to null as it will change depending on the state of the user, this also means that the user won't need to send a parameter when using the command
    /// </param>
    /// <returns></returns>
    [Command("lore")]
    public async Task JingleAsync(IVoiceChannel voiceChannel = null)
    {
        voiceChannel = voiceChannel ?? (Context.User as IGuildUser)?.VoiceChannel;

        if (voiceChannel == null)
        {
            await Context.Channel.SendMessageAsync("You need to be in a voice channel to use this command");
        }

        await Context.Channel.SendMessageAsync("So, you want to hear the Gospel of Jerry");
        var audioClient = await voiceChannel.ConnectAsync();

        await sendAudio.SendAsync(audioClient, "audio/lore.mp3");
    }
}