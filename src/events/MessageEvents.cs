using Discord;
using Discord.WebSocket;

/// <summary>
/// This class is in charge of processing the messages received in the server, the MessageReceived task holds multiple steps that the bot will execute which each message it receives
/// </summary>
public class MessageEvents {
    private Emoji hiEmoji = new Emoji("👋");
    private Emoji saluteEmoji = new Emoji("🫡");
    private Random randNum = new Random();
    private readonly ulong _doofRoleId = new LoadSecrets().getDoofRoleId();
    private readonly ulong _modRoleId = new LoadSecrets().getModRoleId();

    /// <summary>
    /// The MessageReceived method is responsible for processing all the messages received in the server where the bot is a member, the bot messages will not be processed, therefore only the users messages are
    /// </summary>
    /// <param name="message">
    /// The message received by the bot from the user
    /// </param>
    /// <returns>
    /// Answers back with either a message or a reaction
    /// </returns>
    public async Task MessageReceived(SocketMessage message) {
        // Guard clause against null and bot messages
        if (message.Author.IsBot || message == null) {
            return;
        }

        // This block of code will check if the message contains either:
        //  - an unknown command (if this happens it will send a sticker)
        //  - a message containing a tag to everyone or to the Doof role sent by the moderators (if this happens it will react with the salute emoji)
        if (isModTaggingEveryone(message)) {
            await message.AddReactionAsync(saluteEmoji);
        }

        // Case switch to respond to different messages
        switch (message.Content.ToLower()) {
            case "hello":
                await message.Channel.SendMessageAsync($"Hello {message.Author.Mention}");
                await message.AddReactionAsync(hiEmoji);
                break;

            case "hi":
                await message.Channel.SendMessageAsync($"Hello {message.Author.Mention}");
                await message.AddReactionAsync(hiEmoji);
                break;

            case "hey":
                await message.Channel.SendMessageAsync($"Hello {message.Author.Mention}");
                await message.AddReactionAsync(hiEmoji);
                break;

            case "random number":
                await message.Channel.SendMessageAsync($"Your random number is: {randNum.Next(0, 1000)}");
                break;

            case "rand num":
                await message.Channel.SendMessageAsync($"Your random number is: {randNum.Next(0, 1000)}");
                break;

            case "rand":
                await message.Channel.SendMessageAsync($"Your random number is: {randNum.Next(0, 1000)}");
                break;
        }
    }

    /// <summary>
    /// This method will check the following:
    ///     - If the message contains either a tag to the Doof role or to everyone
    ///     - If the user that sent the message has mod (inator) roles
    /// </summary>
    /// <param name="message">
    /// Message sent by the user
    /// </param>
    /// <returns>
    /// Either true of false based on the result of the conditions below
    /// </returns>
    private bool isModTaggingEveryone(SocketMessage message) {
        var user = message.Author as SocketGuildUser;

        if (user == null) {
            return false;
        }

        bool hasModRole = false;
        bool containsDoofTag = message.Content.ToLower().Contains($"<@&{_doofRoleId}>");
        bool containsEveryoneTag = message.Content.ToLower().Contains("@everyone");

        if (user.Roles.Any(role => role.Id.Equals(_modRoleId))) {
            hasModRole = true;
        }

        return hasModRole && (containsDoofTag || containsEveryoneTag);
    }
}
