using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;

public class CommandHandler {
    private DiscordSocketClient _client;
    private CommandService _commands;
    private CommandEvents _commandEvents;

    public CommandHandler(DiscordSocketClient client, CommandService commands) {
        _client = client;
        _commands = commands;
        _commandEvents = new CommandEvents(_commands, _client);
    }

    public async Task InstallCommandsAsync() {
        _client.MessageReceived += _commandEvents.HandleCommandAsync;

        await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: null);
    }
}
