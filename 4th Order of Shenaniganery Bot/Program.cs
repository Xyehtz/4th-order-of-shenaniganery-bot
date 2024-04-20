using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

public class Program {
    private static DiscordSocketClient _client;

    public static async Task Main() {
        // Get Discord TOKEN using .env and DotNetEnv module
        DotNetEnv.Env.Load(@"..\..\..\.env");

        // Start Discord client and logging of the bot
        _client = new DiscordSocketClient();
        _client.Log += Log;

        string token = DotNetEnv.Env.GetString("TOKEN");
        ulong channelId = Convert.ToUInt64(DotNetEnv.Env.GetString("TEST_CHANNEL_ID"));
        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        _client.Ready += async () => {
            var channel = _client.GetChannel(channelId) as IMessageChannel;

            if (channel == null) {
                Console.WriteLine("Error sending message to the channel");
                return;
            }

            await channel.SendMessageAsync("Perry the Platypus!");
        };

        await Task.Delay(-1);
    }

    private static Task Log(LogMessage msg) {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}
