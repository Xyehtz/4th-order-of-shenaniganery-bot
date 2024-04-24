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
        ulong channelId = Convert.ToUInt64(DotNetEnv.Env.GetString("TEST_CHANNEL_ID"));

        var config = new DiscordSocketConfig() {
            GatewayIntents = 
                GatewayIntents.All
        };

        // Start Discord client and logging of the bot
        _client = new DiscordSocketClient(config);
        _client.Log += Logger.Log;

        // Start sample event
        _client.MessageReceived += MessageEvents.MessageReceived;
        _client.UserJoined += UserEvents.UserJoined;
        _client.UserLeft += UserEvents.UserLeft;
        _client.UserBanned += UserEvents.UserBanned;
        _client.UserUnbanned += UserEvents.UserUnbanned;
        // _client.GuildStickerCreated
        // _client.GuildScheduledEventCreated
        // _client.Latency

        // Login using the token and start the bot
        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        // Create a function to send a message to a channel when the bot is ready
        _client.Ready += async () => {
            var channel = _client.GetChannel(channelId) as IMessageChannel;

            if (channel == null) {
                Console.WriteLine("Error sending message to the channel");
                return;
            }

            // await channel.SendMessageAsync("Perry the Platypus!");
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
