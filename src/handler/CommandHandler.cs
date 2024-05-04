using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;

/// <summary>
/// The interface with all the methods that the CommandHandler needs to implement
/// </summary>
public interface ICommandHandler {
    Task InstallCommandsAsync();
}

/// <summary>
/// The CommandHandler is in charge of installing the commands on the bot
/// </summary>
public class CommandHandler : ICommandHandler {
    private DiscordSocketClient _client;
    private CommandService _commands;
    private ICommandEvents _commandEvents;

    public CommandHandler(DiscordSocketClient client, CommandService commands) {
        _client = client;
        _commands = commands;
        _commandEvents = new CommandEvents(_commands, _client);
    }

    /// <summary>
    /// The InstallCommandsAsync method will install the commands found on the modules folder of the bot
    /// </summary>
    /// <returns>
    /// An asynchronous operation that installs the commands on the bot
    /// </returns>
    public async Task InstallCommandsAsync() {
        _client.MessageReceived += _commandEvents.HandleCommandAsync;

        await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: null);
    }
}
