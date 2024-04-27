using Discord.Commands;
using Discord.WebSocket;

public class CommandEvents {
    private CommandService _commands;
    private DiscordSocketClient _client;

    public CommandEvents(CommandService commands, DiscordSocketClient client) {
        _commands = commands;
        _client = client;
    }

    public async Task HandleCommandAsync(SocketMessage messageParam) {
        var message = messageParam as SocketUserMessage;
        int argPos = 0;

        if (message == null) return;
        if (message == null || !message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos) || message.Author.IsBot) return;

        var context = new SocketCommandContext(_client, message);

        var result = await _commands.ExecuteAsync(
            context,
            argPos,
            null
        );

        if (!result.IsSuccess) {
            Console.WriteLine(result.ErrorReason, result.Error);
        }
    }
}
