using Discord;
using Discord.Commands;

/// <summary>
/// Simple command to send a help message to the user
/// </summary>
public class HelpModule : ModuleBase<SocketCommandContext> {
    private readonly ulong _doofAiChannelId = new LoadSecrets().getDoofAiChannelId();
    private readonly ulong _ideasChannelId = new LoadSecrets().getIdeasChannelId();

    /// <summary>
    /// This command will send a simple embed to the server with all the available commands
    /// </summary>
    /// <returns></returns>
    [Command("help")]
    public async Task HelpAsync() {
        string helpMessage = $@"
        ## All commands have the '!' prefix

        - **askDoof**: Ask Doofenshmirtz anything you want, he will answer you with mystique and deviousness. Remember that you must use the <#{_doofAiChannelId}>.
        - **jingle **: Play the Doofenshmirtz Evil Incorporated in the voice channel where you are listening.
        - **kinderlumper **: Play the Der Kinderlumper in the voice channel where you are listening(The Kinderlumper's gonna get ya!).
        - **lore**: Hear the great lore and gospel of Jerry and The 4th Order of Shenaniganery bot.
        - **idea**: If you want to give an idea for a club meeting you can use this command to send a formatted message to the <#{_ideasChannelId}>
        - **ping**: Just to check if the bot is alive
        - **help**: This command will show you all the available commands";

        var embed = new EmbedBuilder {
            Title = "Commands - 4th Order of Shenaniganery bot",
            Description = helpMessage,
            Color = Color.Magenta,
            Timestamp = DateTime.Now,
            ThumbnailUrl = "https://i.ytimg.com/vi/BeSGOfUS-9I/maxresdefault.jpg",
            Footer = new EmbedFooterBuilder {
                Text = $"Requested by {Context.User}",
                IconUrl = Context.User.GetDisplayAvatarUrl()
            }
        };

        await Context.Channel.SendMessageAsync(embed: embed.Build());
    }
}
