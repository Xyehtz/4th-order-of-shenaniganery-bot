using System.Reflection.Metadata.Ecma335;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using DotNetEnv;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;

public class Program {
    private static DiscordSocketClient _client;
    private static CommandService _commands;

    public static void Main() => new Program().RunBot().GetAwaiter().GetResult();

    public async Task RunBot() {
        // Load secrets
        DotNetEnv.Env.Load(@"secrets/.env");
        LoadSecrets loadSecrets = new LoadSecrets();

        // Set bot configuration and create client
        var config = new DiscordSocketConfig()
        {
            GatewayIntents =
                GatewayIntents.All
        };
        _client = new DiscordSocketClient(config);

        // Set event handlers and logger
        GuildEvents guildEvents = new GuildEvents();
        UserEvents userEvents = new UserEvents();

        _client.MessageReceived += new MessageEvents().MessageReceived;
        _client.UserJoined += userEvents.UserJoined;
        _client.UserLeft += userEvents.UserLeft;
        _client.UserBanned += userEvents.UserBanned;
        _client.UserUnbanned += userEvents.UserUnbanned;
        _client.GuildScheduledEventCreated += guildEvents.GuildScheduledEventCreated;
        _client.GuildScheduledEventStarted += guildEvents.GuildScheduledEventStarted;
        _client.Log += Logger.Log;

        // Create command service, start command handler and load commands
        _commands = new CommandService();
        CommandHandler commandHandler = new CommandHandler(_client, _commands);
        await commandHandler.InstallCommandsAsync();

        // Login using the token and start the bot and the start event
        await _client.LoginAsync(TokenType.Bot, loadSecrets.getToken());
        await _client.StartAsync();
        _client.Ready += new ReadyBot(_client).Ready;

        // Block the task until the program is closed
        await Task.Delay(-1);
    }
}
