using Discord;
using Discord.Commands;
using Discord.WebSocket;

public class Program {
    private static DiscordSocketClient _client;
    private static CommandService _commands;

    /// <summary>
    /// Main method and entry point of the program, this will create an instance of Program and execute the RunBot() method which will start the bot
    /// </summary>
    public static void Main() => new Program().RunBot().GetAwaiter().GetResult();

    /// <summary>
    /// This method will do the following to start the bot:
    ///     - Load the .env file using DotNetEnv and start an instance of the LoadSecrets class
    ///     - Set the intents and create the discord client
    ///     - Set the event handlers of the bot
    ///     - Set the command service and load the commands
    ///     - Start the bot and start the status event loop
    /// </summary>
    /// <returns>
    /// Task.CompletedTask
    /// </returns>
    public async Task RunBot() {
        string projectDirectory = Path.Combine(AppContext.BaseDirectory, "../../../");
        Environment.CurrentDirectory = projectDirectory;

        // Load the .env file with the path of the file and start an instance of the LoadSecrets class
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
        IGuildEvents guildEvents = new GuildEvents();
        IUserEvents userEvents = new UserEvents();
        MessageEvents messageEvents = new MessageEvents();

        _client.MessageReceived += messageEvents.MessageReceived;
        _client.UserJoined += userEvents.UserJoined;
        _client.UserLeft += userEvents.UserLeft;
        _client.UserBanned += userEvents.UserBanned;
        _client.UserUnbanned += userEvents.UserUnbanned;
        _client.GuildScheduledEventCreated += guildEvents.GuildScheduledEventCreated;
        _client.GuildScheduledEventStarted += guildEvents.GuildScheduledEventStarted;
        _client.Log += Logger.Log;

        // Create the command service specifying the configuration of the command service, then, load all the commands with the command handler class
        var commandConfig = new CommandServiceConfig{
            DefaultRunMode = 
                RunMode.Async
        };
        _commands = new CommandService(commandConfig);

        ICommandHandler commandHandler = new CommandHandler(_client, _commands);
        await commandHandler.InstallCommandsAsync();

        // Login using the token and start the bot and the start event, if errors happen, catch it
        try {
            await _client.LoginAsync(TokenType.Bot, loadSecrets.getToken());
            await _client.StartAsync();
            _client.Ready += new ReadyBot(_client).Ready;
        } catch (Exception e) {
            Console.WriteLine($"Error logging into Discord: {e}");
        }

        // Block the task until the program is closed
        await Task.Delay(-1);
    }
}
