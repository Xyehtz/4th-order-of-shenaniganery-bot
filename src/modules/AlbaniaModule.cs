using Discord;
using Discord.Commands;

/// <summary>
/// Class in charge of executing the alAnthem command. This command will check if the user is in a voice channel, and play the Albanian anthem
/// </summary>
public class AlbaniaModule : ModuleBase<SocketCommandContext> {
    ISendAudio sendAudio = new SendAudio();

    /// <summary>
    /// This method will do the check if the user is on a voice channel, depending on that, it will send a message to the user if the user is not on a voice channel, if the user is on a voice channel, the bot will join and play the Albanian anthem by calling the SendAsync method of the sendAudio class
    /// </summary>
    /// <param name="voiceChannel">
    /// The parameter voiceChannel is set automatically to null as it will change depending on the state of the user, this also means that the user won't need to send a parameter when using the command
    /// </param>
    /// <returns></returns>
    [Command("alAnthem")]
    public async Task JingleAsync(IVoiceChannel voiceChannel = null) {
        voiceChannel = voiceChannel ?? (Context.User as IGuildUser)?.VoiceChannel;

        if (voiceChannel == null) {
            await Context.Channel.SendMessageAsync("You need to be in a voice channel to use this command");
        }

        var audioClient = await voiceChannel.ConnectAsync();

        await Context.Channel.SendMessageAsync("So you want to hear the anthem of my country?");
        await sendAudio.SendAsync(audioClient, "audio/albania_anthem.mp3");
    }
}