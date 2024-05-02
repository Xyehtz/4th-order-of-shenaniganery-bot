using Discord;
using Discord.Commands;

/// <summary>
/// Class in charge of the !ideas command, this command will get a message from the user, said message will be formatted as an embed and sent to the ideas channel on the server.
/// The bot will automatically add a cross and tick reaction to the message. 
/// The original message will be deleted
/// </summary>
public class IdeasModule : ModuleBase<SocketCommandContext> {
    private ulong ideasChannelId = new LoadSecrets().getIdeasChannelId();
    private Emoji tickEmoji = new Emoji("✅");
    private Emoji crossEmoji = new Emoji("❌");

    /// <summary>
    /// This method will get the command of the user along with the parameters, in this case the parameter will be used as the description inside of the embed, other elements such as title and color are automatically set.
    /// It will add reactions to the formatted message and deleted the original message
    /// </summary>
    /// <param name="ideaText">
    /// A string array parameter of variable size (depending on the length of the message)
    /// </param>
    /// <returns>
    /// A message operation that will sent the formatted message on the ideas channel
    /// </returns>
    [Command("idea")]
    public async Task IdeasAsync(params string[] ideaText) {
        string idea = string.Join(" ", ideaText);

        // Creation of the embed
        var embed = new EmbedBuilder() {
            Title = "New idea just dropped",
            Description = idea,
            Color = Color.Purple,
            Timestamp = DateTime.Now
        };
        embed.WithAuthor(Context.User);
        
        // Verification of the ideas channel
        var channel = Context.Guild.GetChannel(ideasChannelId) as IMessageChannel;
        if (channel == null) {
            return;
        }

        // Deletion of the original message with the message operation of the embed and the reactions to it
        await Context.Message.DeleteAsync();
        var message = await channel.SendMessageAsync(embed: embed.Build());
        await message.AddReactionAsync(tickEmoji);
        await message.AddReactionAsync(crossEmoji);
    }
}