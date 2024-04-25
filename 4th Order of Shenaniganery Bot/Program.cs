using System.Reflection.Metadata.Ecma335;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using DotNetEnv;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;

public class Program {
    private static DiscordSocketClient _client;

    public static async Task Main() {
        // Get Discord TOKEN using .env and DotNetEnv module
        DotNetEnv.Env.Load(@".env");

        // Get token and channel id from .env
        string token = DotNetEnv.Env.GetString("TOKEN");
        ulong applicationId = Convert.ToUInt64(DotNetEnv.Env.GetString("APPLICATION_ID"));
        ulong guildId = Convert.ToUInt64(DotNetEnv.Env.GetString("GUILD_ID"));
        ulong testChannelId = Convert.ToUInt64(DotNetEnv.Env.GetString("TEST_CHANNEL_ID"));
        ulong modChannelId = Convert.ToUInt64(DotNetEnv.Env.GetString("MOD_CHANNEL_ID"));
        ulong welcomeChannelId = Convert.ToUInt64(DotNetEnv.Env.GetString("TEST_CHANNEL_ID"));
        ulong announcementsChannelId = Convert.ToUInt64(DotNetEnv.Env.GetString("ANNOUNCEMENTS_CHANNEL_ID"));
        ulong generalChannelId = Convert.ToUInt64(DotNetEnv.Env.GetString("GENERAL_CHANNEL_ID"));
        ulong testRoleId = Convert.ToUInt64(DotNetEnv.Env.GetString("TEST_ROLE_ID"));
        ulong doofRoleId = Convert.ToUInt64(DotNetEnv.Env.GetString("DOOF_ROLE_ID"));


        var config = new DiscordSocketConfig() {
            GatewayIntents = 
                GatewayIntents.All
        };

        GuildEvents guildEvents =  new GuildEvents(announcementsChannelId, doofRoleId);
        UserEvents userEvents = new UserEvents(welcomeChannelId, modChannelId);

        // Start Discord client and logging of the bot
        _client = new DiscordSocketClient(config);
        _client.Log += Logger.Log;

        // Start sample event
        _client.MessageReceived += MessageEvents.MessageReceived;
        _client.UserJoined += userEvents.UserJoined;
        _client.UserLeft += userEvents.UserLeft;
        _client.UserBanned += userEvents.UserBanned;
        _client.UserUnbanned += userEvents.UserUnbanned;
        _client.GuildScheduledEventCreated += guildEvents.GuildScheduledEventCreated;
        _client.GuildScheduledEventStarted += guildEvents.GuildScheduledEventStarted;

        // Login using the token and start the bot
        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        // Create a function to send a message to a channel when the bot is ready
        _client.Ready += async () => {
            var botChannel = _client.GetChannel(testChannelId) as IMessageChannel;
            var generalChannel = _client.GetChannel(generalChannelId) as IMessageChannel;

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

    // Set logging
    private static Task Log(LogMessage msg) {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}
