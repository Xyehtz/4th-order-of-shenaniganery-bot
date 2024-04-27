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

    public static async Task Main() {
        DotNetEnv.Env.Load(@"secrets/.env");
        LoadSecrets loadSecrets = new LoadSecrets();

        var config = new DiscordSocketConfig() {
            GatewayIntents = 
                GatewayIntents.All
        };

        GuildEvents guildEvents =  new GuildEvents(loadSecrets.getAnnouncementsChannelId(), loadSecrets.getDoofRoleId());
        UserEvents userEvents = new UserEvents(loadSecrets.getWelcomeChannelId(), loadSecrets.getModChannelId());
        MessageEvents messageEvents = new MessageEvents(loadSecrets.getIdeasChannelId());

        // Start Discord client and logging of the bot
        _client = new DiscordSocketClient(config);
        _client.Log += Logger.Log;

        // Start sample event
        _client.MessageReceived += messageEvents.MessageReceived;
        _client.UserJoined += userEvents.UserJoined;
        _client.UserLeft += userEvents.UserLeft;
        _client.UserBanned += userEvents.UserBanned;
        _client.UserUnbanned += userEvents.UserUnbanned;
        _client.GuildScheduledEventCreated += guildEvents.GuildScheduledEventCreated;
        _client.GuildScheduledEventStarted += guildEvents.GuildScheduledEventStarted;

        _commands = new CommandService();
        CommandHandler commandHandler = new CommandHandler(_client, _commands);
        await commandHandler.InstallCommandsAsync();

        // Login using the token and start the bot
        await _client.LoginAsync(TokenType.Bot, loadSecrets.getToken());
        await _client.StartAsync();

        // Create a function to send a message to a channel when the bot is ready
        _client.Ready += async () => {
            var botChannel = _client.GetChannel(loadSecrets.getTestChannelId()) as IMessageChannel;
            var generalChannel = _client.GetChannel(loadSecrets.getGeneralChannelId()) as IMessageChannel;

            if (botChannel == null || generalChannel == null) {
                Console.WriteLine("Error sending message to the channel(s)");
                return;
            }

            // await botChannel.SendMessageAsync($"Successfully connected to the server (Ping: {_client.Latency} ms)");
            // await generalChannel.SendMessageAsync("Perry the Platypus!");
        };

        // Block the task until the program is closed
        await Task.Delay(-1);
    }
}
