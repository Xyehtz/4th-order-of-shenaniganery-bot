using Discord;
using Discord.WebSocket;

public class Status {
    private DiscordSocketClient _client;
    private string[] _games = {"Test", "Test2", "Test3", "Test4", "Test5", "Test6", "Test7", "Test8", "Test9", "Test10"};

    public Status(DiscordSocketClient client) {
        _client = client;
    }

    public async Task SetGameAsync() {
        var timer = new System.Timers.Timer();
        timer.Interval = TimeSpan.FromMinutes(0.1).TotalMilliseconds;
        timer.AutoReset = true;
        timer.Elapsed += async (sender, e) => {
            await changeStatus();
        };
        timer.Start();

        await changeStatus();
    }

    private async Task changeStatus() {
        try {
            await _client.SetGameAsync(_games[new Random().Next(0, _games.Length - 1)]);
        } catch (Exception e) {
            Console.WriteLine($"Error setting the status: {e.Message}");
        }
    }
}
