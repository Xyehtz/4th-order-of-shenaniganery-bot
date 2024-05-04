using Discord.Commands;
using Discord.WebSocket;

// Interface that will set the methods that must be implemented in CommandEvents
public interface ICommandEvents {
    Task HandleCommandAsync(SocketMessage messageParam);
}

/// <summary>
/// CommandEvent class implements the methods defined in ICommandEvents. Inside this class we will handle and execute the commands
/// </summary>
public class CommandEvents : ICommandEvents {
    private CommandService _commands;
    private DiscordSocketClient _client;
    private string[] commands = { "!idea", "!ping", "!test", "!askDoof", "!jingle" };

    /// <summary>
    /// Constructor of the CommandEvents class that will obtain the CommandService and DiscordSocketClient from the Program.cs file to be used in the HandleCommandAsync() method to execute the commands
    /// </summary>
    /// <param name="commands"></param>
    /// <param name="client"></param>
    public CommandEvents(CommandService commands, DiscordSocketClient client) {
        _commands = commands;
        _client = client;
    }

    /// <summary>
    /// This method will handle the commands based on the messages received by the bot. The handler will check a series of conditions to determine if the command is valid and the author is not a bot. Following the conditions, the command will be executed, if the command is not successfully executed it will print the error
    /// </summary>
    /// <param name="messageParam">
    /// The parameters that the user passes with the command such as a text that the command will use to give a response to the user
    /// </param>
    /// <returns></returns>
    public async Task HandleCommandAsync(SocketMessage messageParam) {
        var message = messageParam as SocketUserMessage;
        int argPos = 0;

        // Conditions to check if the command is valid
        bool isMessageNull = message == null;
        bool hasPrefix = !message.HasCharPrefix('!', ref argPos);
        bool hasMentionPrefix = message.HasMentionPrefix(_client.CurrentUser, ref argPos);
        bool isBot = message.Author.IsBot;

        // Check if the command sent by the user is a command or it has a typo, if so it will send an error message to the user with a sticker of Jerry
        if (commandWithTypoReceived(message)) {
            await message.Channel.SendMessageAsync("This command does not exist or it has a typo");
            await message.Channel.SendMessageAsync("Mad Jerry face should go here");
            return;
        }
        if (isMessageNull) return;
        if (isMessageNull || hasPrefix || hasMentionPrefix || isBot) return;

        // Create a command context and execute the command by passing the context, argPos and services
        var context = new SocketCommandContext(_client, message);
        var result = await _commands.ExecuteAsync(
            context,
            argPos,
            null
        );

        // Write a message to the commands_log.log file it the command is not successfully executed
        if (!result.IsSuccess) {
            using (StreamWriter logFile = File.AppendText("logs/commands_log.log")) {
                await logFile.WriteAsync($"[{result.ErrorReason}] {result.Error}\n");
            }
        }
    }

    /// <summary>
    /// This method will check if the message starts with the prefix (!) and if so, it will check if the command typed by the user has a typo or not, this is done to make commands case-sensitive
    /// </summary>
    /// <param name="message">
    /// The message sent by the user
    /// </param>
    /// <returns>
    /// Either true or false based on the result of the conditions
    /// </returns>
    private bool commandWithTypoReceived(SocketMessage message)
    {
        bool startsWithPrefix = message.Content.StartsWith("!");
        bool isCommand = !commands.Contains(message.Content.Split(" ")[0]);

        return startsWithPrefix && isCommand;
    }
}
